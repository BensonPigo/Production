CREATE PROCEDURE [dbo].[exp_SUNRISE]
	@StartDate date,
	@EndDate date
AS
begin
Set NoCount On;
declare @ToAddress nvarchar(max)
declare @CcAddress nvarchar(max)
declare @RgCode nvarchar(255)
select @ToAddress = ToAddress ,@CcAddress = CcAddress from MailTo where id = '016';
select @RgCode = RgCode from system;
begin try
--抓出時間區間內SewingSchedule的orderID
select distinct OrderID
into #SrcOrderID
from dbo.SewingSchedule with (nolock) 
where inline between @StartDate and @EndDate or 
	  Offline between @StartDate and @EndDate or
	  (inline <= @StartDate and Offline >= @EndDate)


--tMODCS(制單顏色尺碼錶)
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

--tMOSeqD(制單工序明細表)
select
[MONo] = so.OrderID + '-' + sl.Location,
[OrderID] = so.OrderID,
[Location] = sl.Location,
[Version] = ts.Version,
[SeqNo] = tsd.Seq,
[SeqCode] = tsd.OperationID,
[SeqName] = iif(isnull(op.Annotation,'')='',op.DescEN,op.Annotation),
[MachineTypeID] = tsd.MachineTypeID,
[SAM] = Round(tsd.SMV/60,7)
into #tMOSeqD
from #SrcOrderID so with (nolock)
inner join orders o with (nolock) on so.OrderID = o.id
inner join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey
inner join TimeStudy ts with (nolock) on o.StyleID = ts.StyleID and o.SeasonID = ts.SeasonID and o.BrandID = ts.BrandID and ts.ComboType = sl.Location
inner join TimeStudy_Detail tsd with (nolock) on tsd.ID = ts.ID and ISNUMERIC(tsd.Seq) = 1 and tsd.SMV<>0
left join Operation op  with (nolock) on tsd.OperationID = op.ID 

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
inner join orders o with (nolock) on so.OrderID = o.id
inner join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey
inner join TimeStudy ts with (nolock) on o.StyleID = ts.StyleID and o.SeasonID = ts.SeasonID and o.BrandID = ts.BrandID and ts.ComboType = sl.Location
outer apply (select sum(SAM) as [Value] from #tMOSeqD where OrderID = so.OrderID and Location = sl.Location) as SAMTotal

--tMOM(制單主表)
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

--tMachineInfo(衣車信息表)
select 
[MachineCode] = m.ID,
[TypeCode] = m.MachineGroupID,
[TypeName] =mg.Description,
[Model] = m.Model,
[Brand] = m.MachineBrandID,
[Insertor] = m.AddName,
[IsUsing] = iif(m.Junk = 0,1,0)
into #tMachineInfo
from Machine.dbo.Machine m with (nolock)
inner join Machine.dbo.MachineGroup mg with (nolock) on m.MachineGroupID = mg.id
where exists (select distinct MachineGroupID from Production.dbo.MachineType
				inner join #tMOSeqD tM on tm.MachineTypeID = MachineType.ID
				where ArtworkTypeID = 'SEWING' and MachineType.MachineGroupID = m.MachineGroupID)

--tSeqBase(基本工序表)
select 
[SeqCode] = ID,
[SeqName] = DescEN,
[SAM] = SMV
into #tSeqBase
from Operation with (nolock)
where exists (select 1 from #tMOSeqD where SeqCode = Operation.ID)



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
T.SeqNo = S.SeqNo collate SQL_Latin1_General_CP1_CI_AS

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD(MONo,Version,SeqNo,SeqCode ,SeqName,SAM,CmdType,CmdTime,InterfaceTime)
select S.MONo,S.Version,S.SeqNo,S.SeqCode ,S.SeqName,S.SAM,'insert',GetDate(),null from #tMOSeqD S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD T where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS  and 
T.Version = S.Version collate SQL_Latin1_General_CP1_CI_AS and 
T.SeqNo = S.SeqNo collate SQL_Latin1_General_CP1_CI_AS)

-- delete 
update T set CmdType = 'delete',
			 CmdTime = GetDate(),
			 InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMOSeqD T
where exists (select 1 from #tMOSeqD S where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS ) and  
not exists (select 1 from #tMOSeqD S where T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS and
T.Version = S.Version collate SQL_Latin1_General_CP1_CI_AS and 
T.SeqNo = S.SeqNo collate SQL_Latin1_General_CP1_CI_AS)

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
				InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMOM T
inner join #tMOM S on T.MONo = S.MONo collate SQL_Latin1_General_CP1_CI_AS

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMOM(MONo,ProNoticeNo,Styleno,Qty ,CustName,SAMTotal,Insertor,DeliveryDate,CmdType,CmdTime,InterfaceTime)
select S.MONo,S.ProNoticeNo,S.Styleno,S.Qty ,S.CustName,S.SAMTotal,S.Insertor,S.DeliveryDate,'insert',GetDate(),null from #tMOM S 
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
				InterfaceTime = null
from [SUNRISE].SUNRISEEXCH.dbo.tMachineInfo T
inner join #tMachineInfo S on T.MachineCode = S.MachineCode collate SQL_Latin1_General_CP1_CI_AS

--insert 
insert into [SUNRISE].SUNRISEEXCH.dbo.tMachineInfo(MachineCode,TypeCode,TypeName,Model,Brand,Insertor,IsUsing,CardNo,CmdType,CmdTime,InterfaceTime)
select S.MachineCode,S.TypeCode,S.TypeName,S.Model,S.Brand,S.Insertor,S.IsUsing,0,'insert',GetDate(),null from #tMachineInfo S 
where not exists(select 1 from [SUNRISE].SUNRISEEXCH.dbo.tMachineInfo T where T.MachineCode = S.MachineCode collate SQL_Latin1_General_CP1_CI_AS)

drop table #SrcOrderID,#tMODCS,#tMOSeqD,#tMOSeqM,#tMOM,#tSeqBase,#tMachineInfo

-- mail 通知信
declare @subject nvarchar(255) = concat('SUNRISE Daily transfer-',Format(getdate(),'yyyy/MM/dd'),'-',@RgCode)
EXEC msdb.dbo.sp_send_dbmail  
    @profile_name = 'SUNRISEmailnotice',  
    @recipients = @ToAddress,
	@copy_recipients= @CcAddress,  
    @body = 'SUNRISE Daily transfer successfully',  
    @subject = @subject; 
	
end try
begin catch
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorLine int;
	DECLARE @ErrorBody NVARCHAR(4000);
    SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorLine = Error_Line();

	set @ErrorBody = N'Error Line: ' + CAST(@ErrorLine AS nvarchar(100)) + CHAR(13)+CHAR(10) + N'Error Message: ' + @ErrorMessage;

	declare @err_subject nvarchar(255) = concat('SUNRISE Daily transfer failure-',Format(getdate(),'yyyy/MM/dd'),'-',@RgCode)
	EXEC msdb.dbo.sp_send_dbmail  
    @profile_name = 'SUNRISEmailnotice',  
    @recipients = @ToAddress,
	@copy_recipients= @CcAddress,  
    @body = @ErrorBody,  
    @subject = @err_subject; 

end catch

end
