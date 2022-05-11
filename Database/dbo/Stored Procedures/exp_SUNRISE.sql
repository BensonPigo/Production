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

--tRoute(加工方案信息表)
select 
[guid] = NEWID(),
[MONo] = so.OrderID + '-' + lm.ComboType,
[RouteName] = so.OrderID + '-' + lm.ComboType + '-' + CAST(lm.Version as varchar)
into #tRoute
from #SrcOrderID so with (nolock)
inner join LineMapping lm with (nolock) on so.StyleUkey = lm.StyleUKey and so.BrandID = lm.BrandID and so.SeasonID = lm.SeasonID
where lm.Status = 'Confirmed' 
	and 1=2 -- add by Roger 04.27

--tRouteLine(加工方案與生產線關係表)
select 
[guid] = NEWID(),
[RouteName] = so.OrderID  + '-' + lm.ComboType + '-' + CAST(lm.Version as varchar),
[LineID] = cast(ss.SewingLineID as int)
into #tRouteLine
from #SrcOrderID so with (nolock)
inner join LineMapping lm with (nolock) on so.StyleUkey = lm.StyleUKey and so.BrandID = lm.BrandID and so.SeasonID = lm.SeasonID
cross apply (select top 1 SewingLineID from SewingSchedule  with (nolock) where OrderID = so.OrderID and ISNUMERIC(SewingLineID) = 1 ) ss 
where lm.Status = 'Confirmed'
	and 1=2 -- add by Roger 04.27

--tSeqAssign(加工方案-工序安排)
;with tSeqAssignTmp as
(
select 
[RouteName] = so.OrderID + '-' + lm.ComboType + '-' + CAST(lm.Version as varchar),
[SeqNo] = cast(lmd.OriNO as int),
[IsOutputSeq] = 0,
[Station] = MIN(cast(lmd.No as int)),
[StyleUkey] = so.StyleUkey,
[BrandID] = so.BrandID,
[SeasonID] = so.SeasonID,
[OriNO] = lmd.OriNO
from #SrcOrderID so with (nolock)
inner join LineMapping lm with (nolock) on so.StyleUkey = lm.StyleUKey and so.BrandID = lm.BrandID and so.SeasonID = lm.SeasonID
inner join LineMapping_Detail lmd with (nolock) on lm.ID = lmd.ID and lmd.No <> ''
where lm.Status = 'Confirmed' and ISNUMERIC(lmd.OriNO) = 1
group by lmd.OriNO,so.OrderID,lm.Version,so.StyleUkey,so.BrandID,so.SeasonID,lm.ComboType)
select 
[guid] = NEWID(),
RouteName,
[SeqOrder] = DENSE_RANK() OVER (PARTITION BY RouteName ORDER BY SeqNo),
[bMerge] = LAG(1,1,0) OVER (PARTITION BY RouteName,Station ORDER BY SeqNo),
SeqNo,
Station,
IsOutputSeq,
StyleUkey,
BrandID,
SeasonID,
OriNO
into #tSeqAssign
from tSeqAssignTmp
	where 1=2  -- add by Roger 04.27

--如果原本tSeqAssign已傳至SunRise將guid更新回本次要傳的資料中
select * into #tSeqAssignSunRise from [SUNRISE].SUNRISEEXCH.dbo.tSeqAssign
update t set t.guid = s.guid
from #tSeqAssign t
inner join #tSeqAssignSunRise s on t.RouteName = s.RouteName collate SQL_Latin1_General_CP1_CI_AS and t.SeqNo = s.SeqNo

--tStAssign(加工方案-工作站安排)
select 
[guid] = NEWID(),
[RouteName] = tsa.RouteName,
[SeqAssign_guid] = tsa.guid,
[StationID] = cast(lmd.No as int),
[StFunc] = 0
into #tStAssign
from #tSeqAssign tsa
inner join LineMapping lm with (nolock) on tsa.StyleUkey = lm.StyleUKey and tsa.BrandID = lm.BrandID and tsa.SeasonID = lm.SeasonID
inner join LineMapping_Detail lmd with (nolock) on lm.ID = lmd.ID and lmd.No <> '' and lmd.OriNO = tsa.OriNO
where tsa.bMerge = 0 and lm.Status = 'Confirmed' and ISNUMERIC(lmd.OriNO) = 1
	and 1=2 -- add by Roger 04.27


--同步資料至SUNRISE db
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


--tRoute(加工方案信息表)
insert into [SUNRISE].SUNRISEEXCH.dbo.tRoute(guid,MONo,RouteName,CmdType,CmdTime)
select S.guid,S.MONo,S.RouteName,'insert',GetDate() from #tRoute S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tRoute T where T.RouteName = S.RouteName collate SQL_Latin1_General_CP1_CI_AS)

--tRouteLine(加工方案與生產線關係表)
insert into [SUNRISE].SUNRISEEXCH.dbo.tRouteLine(guid,RouteName,LineID,CmdType,CmdTime)
select S.guid,S.RouteName,S.LineID,'insert',GetDate() from #tRouteLine S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tRouteLine T where T.RouteName = S.RouteName collate SQL_Latin1_General_CP1_CI_AS and T.LineID = S.LineID )

--tSeqAssign(加工方案-工序安排)
update T set	SeqOrder = S.SeqOrder,
				bMerge = S.bMerge,
				IsOutputSeq = S.IsOutputSeq,
				CmdType = 'update',
				CmdTime = GetDate()
from [SUNRISE].SUNRISEEXCH.dbo.tSeqAssign T
inner join #tSeqAssign S on T.RouteName = S.RouteName collate SQL_Latin1_General_CP1_CI_AS and T.SeqNo = S.SeqNo 

insert into [SUNRISE].SUNRISEEXCH.dbo.tSeqAssign(guid,RouteName,SeqOrder,bMerge,SeqNo,IsOutputSeq,CmdType,CmdTime)
select S.guid,S.RouteName,S.SeqOrder,S.bMerge,S.SeqNo,S.IsOutputSeq,'insert',GetDate() from #tSeqAssign S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tSeqAssign T where T.RouteName = S.RouteName collate SQL_Latin1_General_CP1_CI_AS and T.SeqNo = S.SeqNo )

--tStAssign(加工方案-工作站安排)
insert into [SUNRISE].SUNRISEEXCH.dbo.tStAssign(guid,RouteName,SeqAssign_guid,StationID,StFunc,CmdType,CmdTime)
select S.guid,S.RouteName,S.SeqAssign_guid,S.StationID,S.StFunc,'insert',GetDate() from #tStAssign S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tStAssign T where T.SeqAssign_guid = S.SeqAssign_guid and T.StationID = S.StationID and T.RouteName=s.RouteName collate SQL_Latin1_General_CP1_CI_AS)

update s set CmdType = 'delete', CmdTime = GetDate()
from [SUNRISE].SUNRISEEXCH.dbo.tStAssign s
where exists(select 1 from #tStAssign where SeqAssign_guid = s.SeqAssign_guid) and
not exists(select 1 from #tStAssign where SeqAssign_guid = s.SeqAssign_guid and StationID = s.StationID and RouteName=s.RouteName collate SQL_Latin1_General_CP1_CI_AS)

drop table #SrcOrderID,#tMODCS,#tMOSeqD,#tMOSeqM,#tMOM,#tSeqBase,#tMachineInfo,#tRoute,#tRouteLine,#tSeqAssign,#tStAssign,#tSeqAssignSunRise


end