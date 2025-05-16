CREATE PROCEDURE [dbo].[exp_SUNRISE]
	@StartDate date,
	@EndDate date
AS
begin
Set NoCount On;
declare @ToAddress nvarchar(max)
declare @CcAddress nvarchar(max)
declare @RgCode nvarchar(255)
declare @StartDate1 datetime, @EndDate1 datetime
select @ToAddress = ToAddress ,@CcAddress = CcAddress from MailTo where id = '016';
select @RgCode = RgCode from system;
set @StartDate1 = cast(convert(nvarchar(10),@StartDate,112) + ' 00:00:00'  as datetime)
set @EndDate1 = cast(convert(nvarchar(10),dateadd(dd,1,@EndDate),112) + ' 00:00:00'  as datetime)

--抓出時間區間內SewingSchedule的orderID
select distinct ss.OrderID,o.StyleID,o.SeasonID,o.BrandID,o.StyleUKey,o.Qty,o.AddName,o.SCIDelivery ,o.CustPONo
into #SrcOrderID
from dbo.SewingSchedule ss with (nolock)
inner join orders o with (nolock) on  ss.OrderID = o.ID
where (inline >= @StartDate1 and inline < @EndDate1) 
	  or (Offline >= @StartDate1 and Offline < @EndDate1) 
	  or (inline <= @StartDate1 and Offline >= @EndDate1)



--tMODCS(制單顏色尺碼錶)
select 
[MONo] = so.OrderID + '-' + sl.Location,
[ColorNo] = oq.Article,
[ColorName] = oq.Article,
[SizeName] = oq.SizeCode,
[qty] = oq.Qty
into #tMODCS
from #SrcOrderID so with (nolock)
inner join Order_Qty oq with (nolock) on so.OrderID = oq.ID
inner join Style_Location sl with (nolock) on sl.StyleUkey = so.StyleUkey

--tMOSeqD(制單工序明細表)
select
[MONo] = so.OrderID + '-' + sl.Location,
[OrderID] = so.OrderID,
[Location] = sl.Location,
[Version] = ts.Version,
[SeqNo] = cast(tsd.Seq as int),
[SeqCode] = tsd.Seq,
[SeqName] = tsd.Annotation,
[MachineTypeID] = tsd.MachineTypeID,
[SAM] = Round(tsd.SMV/60,7)
into #tMOSeqD
from #SrcOrderID so with (nolock)
inner join Style_Location sl with (nolock) on sl.StyleUkey = so.StyleUkey
inner join TimeStudy ts with (nolock) on so.StyleID = ts.StyleID and so.SeasonID = ts.SeasonID and so.BrandID = ts.BrandID and ts.ComboType = sl.Location
inner join TimeStudy_Detail tsd with (nolock) on tsd.ID = ts.ID and ISNUMERIC(tsd.Seq) = 1 and tsd.SMV<>0

--tMOSeqM(制單工序主表) 
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
inner join Style_Location sl with (nolock) on sl.StyleUkey = so.StyleUkey
inner join TimeStudy ts with (nolock) on so.StyleID = ts.StyleID and so.SeasonID = ts.SeasonID and so.BrandID = ts.BrandID and ts.ComboType = sl.Location
outer apply (select sum(SAM) as [Value] from #tMOSeqD where OrderID = so.OrderID and Location = sl.Location) as SAMTotal

--tMOM(制單主表)
select 
[MONo] = so.OrderID + '-' + sl.Location,
[ProNoticeNo] = so.OrderID,
[Styleno] = so.StyleID,
[Qty] = so.Qty,
[CustName] = so.BrandID,
[SAMTotal] = tMM.SAMTotal,
[Insertor] = so.AddName,
[DeliveryDate] = so.SCIDelivery,
[CustPONo] = so.CustPONo
into #tMOM
from #SrcOrderID so with (nolock)
inner join Style_Location sl with (nolock) on sl.StyleUkey = so.StyleUkey
left join #tMOSeqM tMM on tMM.OrderID = so.OrderID and tMM.Location = sl.Location

--tMachineInfo(衣車信息表)
select 
[MachineCode] = m.ID,
[TypeCode] = m.MasterGroupID,
[TypeName] =mg.Description,
[Model] = m.Model,
[Brand] = m.MachineBrandID,
[Insertor] = m.AddName,
[IsUsing] = iif(m.Junk = 0,1,0),
[CardNo] = isnull(m.RFIDCardNo,0)
into #tMachineInfo
from SciMachine_Machine m with (nolock)
inner join SciMachine_MachineGroup mg with (nolock) on m.MachineGroupID = mg.id and m.MasterGroupID = mg.MasterGroupID

--tSeqBase(基本工序表)
select 
[SeqCode] = ID,
[SeqName] = DescEN,
[SAM] = SMV
into #tSeqBase
from Operation with (nolock)
where exists (select 1 from #tMOSeqD where SeqCode = Operation.ID)

--同步資料至SUNRISE db
--tMOM(制單主表)
--update
update T set	ProNoticeNo = S.ProNoticeNo,
				Styleno = S.Styleno,
				Qty = S.Qty,
				CustName = S.CustName,
				SAMTotal = S.SAMTotal,
				Insertor = S.Insertor,
				DeliveryDate = S.DeliveryDate,
				CmdType = iif(InterfaceTime is not null,'update',CmdType),
				CmdTime = GetDate(),
				InterfaceTime = null,
				CustPoNo = S.CustPONo
from [SUNRISE].SUNRISEEXCH.dbo.tMOM T
inner join #tMOM S on T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMOM
(
	MONo
	,ProNoticeNo
	,Styleno
	,Qty 
	,CustName
	,SAMTotal
	,Insertor
	,DeliveryDate
	,CmdType
	,CmdTime
	,InterfaceTime
	,CustPONo
)
select 
	S.MONo
	,S.ProNoticeNo
	,S.Styleno
	,S.Qty 
	,S.CustName
	,S.SAMTotal
	,S.Insertor
	,S.DeliveryDate
	,'insert'
	,GetDate()
	,null 
	,S.CustPONo
from #tMOM S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMOM T where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS)

--tMODCS(制單顏色尺碼錶)
--update
update T set	ColorName = S.ColorName,
				Qty = S.qty,
				CmdType = iif(InterfaceTime is not null,'update',CmdType),
				CmdTime = GetDate(),
				InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMODCS T
inner join #tMODCS S on T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS  and T.ColorNo = S.ColorNo collate SQL_Latin1_General_CP1_CI_AS and T.SizeName = S.SizeName collate SQL_Latin1_General_CP1_CI_AS

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMODCS(MONo,ColorNo,ColorName,SizeName ,Qty,CmdType,CmdTime,InterfaceTime)
select S.MONo,S.ColorNo,S.ColorName,S.SizeName ,S.qty,'insert',GetDate(),null from #tMODCS S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMODCS T where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS and 
T.ColorNo = S.ColorNo collate SQL_Latin1_General_CP1_CI_AS and 
T.SizeName = S.SizeName collate SQL_Latin1_General_CP1_CI_AS)

-- delete 
update T set CmdType ='delete',
			 CmdTime = GetDate(),
			 InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMODCS T
where exists (select 1 from #tMODCS S where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS  ) and  
not exists (select 1 from #tMODCS S where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS and 
T.ColorNo = S.ColorNo collate SQL_Latin1_General_CP1_CI_AS and
 T.SizeName = S.SizeName collate SQL_Latin1_General_CP1_CI_AS)

--tMOSeqD(制單工序明細表) SeqNo 兩邊型別不同
--update
update T set	SeqCode = S.SeqCode,
				SeqName = S.SeqName,
				SAM = S.SAM,
				CmdType = iif(InterfaceTime is not null,'update',CmdType),
				CmdTime = GetDate(),
				InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD T
inner join #tMOSeqD S on T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS and 
T.Version = S.Version collate SQL_Latin1_General_CP1_CI_AS and 
T.SeqNo = S.SeqNo

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD(MONo,Version,SeqNo,SeqCode ,SeqName,SAM,CmdType,CmdTime,InterfaceTime)
select S.MONo,S.Version,S.SeqNo,S.SeqCode ,S.SeqName,S.SAM,'insert',GetDate(),null from #tMOSeqD S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD T where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS  and 
T.Version = S.Version collate SQL_Latin1_General_CP1_CI_AS and T.SeqNo = S.SeqNo )

-- delete 
update T set CmdType = 'delete',
			 CmdTime = GetDate(),
			 InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD T
where exists (select 1 from #tMOSeqD S where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS ) and  
  not exists (select 1 from #tMOSeqD S where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS and
				T.Version = S.Version collate SQL_Latin1_General_CP1_CI_AS and 
				T.SeqNo = S.SeqNo)
  and (T.CmdType <> 'delete' )  -- add by Roger 05.09

--select * from #tMOSeqD where mono = '18052464GGS-B' and Version = '01' and SeqNo = '0620'

--tMOSeqM(制單工序主表) 
--update
update T set	CMSAMTotal = S.CMSAMTotal,
				SAMTotal = S.SAMTotal,
				EffDate = S.EffDate,
				insertor = S.insertor,
				CmdType = iif(InterfaceTime is not null,'update',CmdType),
				CmdTime = GetDate(),
				InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqM T
inner join #tMOSeqM S on T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS and T.Version = S.Version collate SQL_Latin1_General_CP1_CI_AS

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMOSeqM(MONo,Version,CMSAMTotal,SAMTotal ,EffDate,insertor,CmdType,CmdTime,InterfaceTime)
select S.MONo,S.Version,S.CMSAMTotal,S.SAMTotal ,S.EffDate,S.insertor,'insert',GetDate(),null from #tMOSeqM S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqM T where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS  and 
T.Version = S.Version collate SQL_Latin1_General_CP1_CI_AS)

-- delete 
update T set CmdType = 'delete',
			 CmdTime = GetDate(),
			 InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqM T
where exists (select 1 from #tMOSeqM S where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS ) and  
not exists (select 1 from #tMOSeqM S where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS and 
T.Version = S.Version collate SQL_Latin1_General_CP1_CI_AS)

--tSeqBase(基本工序表)
--update
update T set	SeqName = S.SeqName,
				SAM = S.SAM,
				CmdType = iif(InterfaceTime is not null,'update',CmdType),
				CmdTime = GetDate(),
				InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tSeqBase T
inner join #tSeqBase S on T.SeqCode = S.SeqCode collate SQL_Latin1_General_CP1_CI_AS 

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tSeqBase(SeqCode,SeqName,SAM,Price,CmdType,CmdTime,InterfaceTime)
select S.SeqCode,S.SeqName,S.SAM,0,'insert',GetDate(),null from #tSeqBase S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tSeqBase T where T.SeqCode = S.SeqCode collate SQL_Latin1_General_CP1_CI_AS)

--tMachineInfo(衣車信息表)
--update
update T set	TypeCode = S.TypeCode,
				TypeName = S.TypeName,
				Model = S.Model,
				Brand = S.Brand,
				Insertor = S.Insertor,
				CmdType = iif(InterfaceTime is not null,'update',CmdType),
				CmdTime = GetDate(),
				InterfaceTime = null,
				CardNo = S.CardNo
from [SUNRISE].SUNRISEEXCH.dbo.tMachineInfo T
inner join #tMachineInfo S on T.MachineCode = S.MachineCode collate SQL_Latin1_General_CP1_CI_AS

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMachineInfo(MachineCode,TypeCode,TypeName,Model,Brand,Insertor,IsUsing,CardNo,CmdType,CmdTime,InterfaceTime,CardNo)
select S.MachineCode,S.TypeCode,S.TypeName,S.Model,S.Brand,S.Insertor,S.IsUsing,0,'insert',GetDate(),null,S.CardNo from #tMachineInfo S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMachineInfo T where T.MachineCode = S.MachineCode collate SQL_Latin1_General_CP1_CI_AS)

drop table #SrcOrderID,#tMODCS,#tMOSeqD,#tMOSeqM,#tMOM,#tSeqBase,#tMachineInfo


end