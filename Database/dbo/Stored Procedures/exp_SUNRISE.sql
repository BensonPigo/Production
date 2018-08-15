CREATE PROCEDURE [dbo].[exp_SUNRISE]
	@StartDate date,
	@EndDate date
AS
begin
Set NoCount On;
--抓出時間區間內SewingSchedule的orderID
select distinct OrderID
into #SrcOrderID
from dbo.SewingSchedule with (nolock) 
where inline between @StartDate and @EndDate or Offline between @StartDate and @EndDate


--tMODCS（制单颜色尺码表）
select 
[MONo] = so.OrderID + '-' + sl.Location,
[ColorNo] = oq.Article,
[ColorName] = oq.Article,
[SizeName] = oq.SizeCode,
[qty] = oq.Qty
into #tMODCS
from #SrcOrderID so with (nolock)
inner join orders o with (nolock) on so.OrderID = o.id
inner join Order_Qty oq with (nolock) on o.id = oq.ID
inner join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey

--tMOSeqD(制单工序明细表)
select
[MONo] = so.OrderID + '-' + sl.Location,
[OrderID] = so.OrderID,
[Location] = sl.Location,
[Version] = ts.Version,
[SeqNo] = tsd.Seq,
[SeqCode] = tsd.OperationID,
[SeqName] = op.DescEN,
[SAM] = Round(tsd.SMV/60,7)
into #tMOSeqD
from #SrcOrderID so with (nolock)
inner join orders o with (nolock) on so.OrderID = o.id
inner join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey
inner join TimeStudy ts with (nolock) on o.StyleID = ts.StyleID and o.SeasonID = ts.SeasonID and o.BrandID = ts.BrandID and ts.ComboType = sl.Location
inner join TimeStudy_Detail tsd with (nolock) on tsd.ID = ts.ID and ISNUMERIC(tsd.Seq) = 1
inner join Operation op  with (nolock) on tsd.OperationID = op.ID 

--tMOSeqM（制单工序主表） group by tMOSeqD **
select 
[MONo] = so.OrderID + '-' + sl.Location,
[OrderID] = so.OrderID,
[Location] = sl.Location,
[CMSAMTotal] = SAMTotal.Value,
[SAMTotal] = SAMTotal.Value,
[Version] = ts.Version,
[EffDate] = ts.AddDate,
[insertor] = ts.AddName
into #tMOSeqM
from #SrcOrderID so with (nolock)
inner join orders o with (nolock) on so.OrderID = o.id
inner join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey
inner join TimeStudy ts with (nolock) on o.StyleID = ts.StyleID and o.SeasonID = ts.SeasonID and o.BrandID = ts.BrandID and ts.ComboType = sl.Location
outer apply (select sum(SAM) as [Value] from #tMOSeqD where OrderID = so.OrderID and Location = sl.Location) as SAMTotal

--tMOM（制单主表）
select 
[MONo] = so.OrderID + '-' + sl.Location,
[ProNoticeNo] = so.OrderID,
[Styleno] = o.StyleID,
[Qty] = o.Qty,
[CustName] = o.BrandID,
[SAMTotal] = tMM.SAMTotal,
[Insertor] = o.AddName,
[DeliveryDate] = o.SCIDelivery
into #tMOM
from #SrcOrderID so with (nolock)
inner join orders o with (nolock) on so.OrderID = o.id
inner join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey
left join #tMOSeqM tMM on tMM.OrderID = so.OrderID and tMM.Location = sl.Location



--tSeqBase（基本工序表）
select 
[SeqCode] = ID,
[SeqName] = DescEN,
[SAM] = SMV
into #tSeqBase
from Operation with (nolock)



--同步資料至SUNRISE db

--tMODCS（制单颜色尺码表）
--update
update T set	ColorName = S.ColorName,
				Qty = S.qty,
				CmdType = 'update',
				CmdTime = GetDate(),
				SunriseUpdated = 0
from [SUNRISE].SUNRISEEXCH.dbo.tMODCS T
inner join #tMODCS S on T.MONo = S.MONo  and T.ColorNo = S.ColorNo and T.SizeName = S.SizeName

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMODCS(MONo,ColorNo,ColorName,SizeName ,Qty,CmdType,CmdTime,SunriseUpdated)
select S.MONo,S.ColorNo,S.ColorName,S.SizeName ,S.qty,'insert',GetDate(),0 from #tMODCS S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMODCS T where T.MONo = S.MONo  and T.ColorNo = S.ColorNo and T.SizeName = S.SizeName)

-- delete 
update T set CmdType = 'delete',
			 CmdTime = GetDate(),
			 SunriseUpdated = 0
from [SUNRISE].SUNRISEEXCH.dbo.tMODCS T
where exists (select 1 from #tMODCS S where T.MONo = S.MONo  ) and  
not exists (select 1 from #tMODCS S where T.MONo = S.MONo  and T.ColorNo = S.ColorNo and T.SizeName = S.SizeName)

--tMOSeqD(制单工序明细表) SeqNo 兩邊型別不同
--update
update T set	SeqCode = S.SeqCode,
				SeqName = S.SeqName,
				SAM = S.SAM,
				CmdType = 'update',
				CmdTime = GetDate(),
				SunriseUpdated = 0
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD T
inner join #tMOSeqD S on T.MONo = S.MONo  and T.Version = S.Version and T.SeqNo = S.SeqNo

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD(MONo,Version,SeqNo,SeqCode ,SeqName,SAM,CmdType,CmdTime,SunriseUpdated)
select S.MONo,S.Version,S.SeqNo,S.SeqCode ,S.SeqName,S.SAM,'insert',GetDate(),0 from #tMOSeqD S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD T where T.MONo = S.MONo  and T.Version = S.Version and T.SeqNo = S.SeqNo)

-- delete 
update T set CmdType = 'delete',
			 CmdTime = GetDate(),
			 SunriseUpdated = 0
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD T
where exists (select 1 from #tMOSeqD S where T.MONo = S.MONo  ) and  
not exists (select 1 from #tMOSeqD S where T.MONo = S.MONo  and T.Version = S.Version and T.SeqNo = S.SeqNo)

--select * from #tMOSeqD where mono = '18052464GGS-B' and Version = '01' and SeqNo = '0620'

--tMOSeqM（制单工序主表）
--update
update T set	CMSAMTotal = S.CMSAMTotal,
				SAMTotal = S.SAMTotal,
				EffDate = S.EffDate,
				insertor = S.insertor,
				CmdType = 'update',
				CmdTime = GetDate(),
				SunriseUpdated = 0
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqM T
inner join #tMOSeqM S on T.MONo = S.MONo  and T.Version = S.Version 

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMOSeqM(MONo,Version,CMSAMTotal,SAMTotal ,EffDate,insertor,CmdType,CmdTime,SunriseUpdated)
select S.MONo,S.Version,S.CMSAMTotal,S.SAMTotal ,S.EffDate,S.insertor,'insert',GetDate(),0 from #tMOSeqM S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqM T where T.MONo = S.MONo  and T.Version = S.Version)

-- delete 
update T set CmdType = 'delete',
			 CmdTime = GetDate(),
			 SunriseUpdated = 0
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqM T
where exists (select 1 from #tMOSeqM S where T.MONo = S.MONo  ) and  
not exists (select 1 from #tMOSeqM S where T.MONo = S.MONo  and T.Version = S.Version )

--tMOM（制单主表）
--update
update T set	ProNoticeNo = S.ProNoticeNo,
				Styleno = S.Styleno,
				Qty = S.Qty,
				CustName = S.CustName,
				SAMTotal = S.SAMTotal,
				Insertor = S.Insertor,
				DeliveryDate = S.DeliveryDate,
				CmdType = 'update',
				CmdTime = GetDate(),
				SunriseUpdated = 0
from [SUNRISE].SUNRISEEXCH.dbo.tMOM T
inner join #tMOM S on T.MONo = S.MONo 

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMOM(MONo,ProNoticeNo,Styleno,Qty ,CustName,SAMTotal,Insertor,DeliveryDate,CmdType,CmdTime,SunriseUpdated)
select S.MONo,S.ProNoticeNo,S.Styleno,S.Qty ,S.CustName,S.SAMTotal,S.Insertor,S.DeliveryDate,'insert',GetDate(),0 from #tMOM S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMOM T where T.MONo = S.MONo )

--tSeqBase（基本工序表）
--update
update T set	SeqName = S.SeqName,
				SAM = S.SAM,
				CmdType = 'update',
				CmdTime = GetDate(),
				SunriseUpdated = 0
from [SUNRISE].SUNRISEEXCH.dbo.tSeqBase T
inner join #tSeqBase S on T.SeqCode = S.SeqCode 

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tSeqBase(SeqCode,SeqName,SAM,Price,CmdType,CmdTime,SunriseUpdated)
select S.SeqCode,S.SeqName,S.SAM,0,'insert',GetDate(),0 from #tSeqBase S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tSeqBase T where T.SeqCode = S.SeqCode )



drop table #SrcOrderID,#tMODCS,#tMOSeqD,#tMOSeqM,#tMOM,#tSeqBase

end
