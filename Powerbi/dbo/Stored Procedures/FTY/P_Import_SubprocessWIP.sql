Create PROCEDURE [dbo].[P_Import_SubprocessWIP]
	@StartDate date = null,
	@FirstDate date = null
As
Begin

Set NoCount On;
Declare @ExecSQL1 NVarChar(MAX);

Declare @EditDateFrom varchar(12) = case when isnull(@StartDate,'') = '' then '' else Format(@StartDate, 'yyyyMMdd') end
Declare @FirstDateFrom varchar(12) = case when isnull(@FirstDate,'') = '' then '' else Format(@FirstDate, 'yyyyMMdd') end

select *
into #tmpP_SubprocessWIP
from P_SubprocessWIP
where 1=0

set @ExecSQL1 = N'
insert into #tmpP_SubprocessWIP(
	[Bundleno]
      ,[RFIDProcessLocationID]
      ,[EXCESS]
      ,[FabricKind]
      ,[CutRef]
      ,[Sp]
      ,[MasterSP]
      ,[M]
      ,[Factory]
      ,[Category]
      ,[Program]
      ,[Style]
      ,[Season]
      ,[Brand]
      ,[Comb]
      ,[CutNo]
      ,[FabPanelCode]
      ,[Article]
      ,[Color]
      ,[ScheduledLineID]
      ,[ScannedLineID]
      ,[Cell]
      ,[Pattern]
      ,[PtnDesc]
      ,[Group]
      ,[Size]
      ,[Artwork]
      ,[Qty]
      ,[SubprocessID]
      ,[PostSewingSubProcess]
      ,[NoBundleCardAfterSubprocess]
      ,[Location]
      ,[BundleCreateDate]
      ,[BuyerDeliveryDate]
      ,[SewingInline]
      ,[SubprocessQAInspectionDate]
      ,[InTime]
      ,[OutTime]
      ,[POSupplier]
      ,[AllocatedSubcon]
      ,[AvgTime]
      ,[TimeRange]
      ,[EstimatedCutDate]
      ,[CuttingOutputDate]
      ,[Item]
      ,[PanelNo]
      ,[CutCellID]
      ,[SpreadingNo]
      ,[LastSewDate]
      ,[SewQty]
)
select 
	[Bundleno],
	[RFIDProcessLocationID] ,
	[EXCESS] ,
	[FabricKind] ,
	[Cut Ref#] ,
	[SP#],
	[Master SP#] ,
    [M] ,
    [Factory] ,
	[Category],
	[Program] ,
    [Style] ,
    [Season],
    [Brand] ,
    [Comb] ,
    Cutno ,
	[Fab_Panel Code] ,
    [Article] ,
    [Color] ,
    [Line] ,
    SewingLineID ,
    [Cell] ,
    [Pattern] ,
    [PtnDesc] ,
    [Group] ,
    [Size] ,
    [Artwork] ,
    [Qty] ,
    [Sub-process] ,
    [Post Sewing SubProcess] ,
    [No Bundle Card After Subprocess] ,
    LocationID ,
    Cdate ,
    BuyerDelivery ,
    SewInLine ,
	[InspectionDate] ,
    [InComing] ,
    [Out (Time)],
    [POSupplier] ,
    [AllocatedSubcon] ,
	AvgTime ,
	TimeRange ,
	EstCutDate,
	CuttingOutputDate ,
	Item ,
	PanelNo ,
	CutCellID ,
	[SpreadingNo] ,
	[LastSewDate] ,
	[SewQty]
from OPENQUERY([MainServer], '' SET NOCOUNT ON; select * from Production.dbo.Get_SubCon_R41_Report('''''+ @EditDateFrom +''''','''''+ @FirstDateFrom +''''') '')
'

EXEC sp_executesql @ExecSQL1

Merge P_SubprocessWIP t
using( select * from #tmpP_SubprocessWIP) as s
	on t.Bundleno = s.Bundleno 
	and t.RFIDProcessLocationID = s.RFIDProcessLocationID 
	and t.Sp = s.Sp 
	and t.Pattern = s.Pattern
	and t.SubprocessID = s.SubprocessID
WHEN MATCHED then UPDATE SET
      t.[EXCESS] = s.[EXCESS]
      ,t.[FabricKind] = s.[FabricKind]
      ,t.[CutRef] = s.[CutRef]
      ,t.[Sp] = s.[Sp]
      ,t.[MasterSP] = s.[MasterSP]
      ,t.[M] = s.[M]
      ,t.[Factory] = s.[Factory]
      ,t.[Category] = s.[Category]
      ,t.[Program] = s.[Program]
      ,t.[Style] = s.[Style]
      ,t.[Season] = s.[Season]
      ,t.[Brand] = s.[Brand]
      ,t.[Comb] = s.[Comb]
      ,t.[CutNo] = s.[CutNo]
      ,t.[FabPanelCode] = s.[FabPanelCode]
      ,t.[Article] = s.[Article]
      ,t.[Color] = s.[Color]
      ,t.[ScheduledLineID] = s.[ScheduledLineID]
      ,t.[ScannedLineID] = s.[ScannedLineID]
      ,t.[Cell] = s.[Cell]
      ,t.[Pattern] = s.[Pattern]
      ,t.[PtnDesc] = s.[PtnDesc]
      ,t.[Group] = s.[Group]
      ,t.[Size] = s.[Size]
      ,t.[Artwork] = s.[Artwork]
      ,t.[Qty] = s.[Qty]
      ,t.[SubprocessID] = s.[SubprocessID]
      ,t.[PostSewingSubProcess] = s.[PostSewingSubProcess]
      ,t.[NoBundleCardAfterSubprocess] = s.[NoBundleCardAfterSubprocess]
      ,t.[Location] = s.[Location]
      ,t.[BundleCreateDate] = s.[BundleCreateDate]
      ,t.[BuyerDeliveryDate] = s.[BuyerDeliveryDate]
      ,t.[SewingInline] = s.[SewingInline]
      ,t.[SubprocessQAInspectionDate] = s.[SubprocessQAInspectionDate]
      ,t.[InTime] = s.[InTime]
      ,t.[OutTime] = s.[OutTime]
      ,t.[POSupplier] = s.[POSupplier]
      ,t.[AllocatedSubcon] = s.[AllocatedSubcon] 
      ,t.[AvgTime] = s.AvgTime
      ,t.[TimeRange] = s.[TimeRange]
      ,t.[EstimatedCutDate] = s.[EstimatedCutDate]
      ,t.[CuttingOutputDate] = s.CuttingOutputDate
      ,t.[Item] = s.Item
      ,t.[PanelNo] = s.PanelNo
      ,t.[CutCellID] = s.CutCellID
      ,t.[SpreadingNo] = s.SpreadingNo
      ,t.[LastSewDate] = s.LastSewDate
      ,t.[SewQty] = s.SewQty
WHEN NOT MATCHED BY TARGET THEN
 insert([Bundleno]   ,[RFIDProcessLocationID]   ,[EXCESS]  ,[FabricKind]  ,[CutRef]  ,[Sp]  ,[MasterSP]
      ,[M] ,[Factory],[Category],[Program],[Style],[Season],[Brand],[Comb],[CutNo],[FabPanelCode],[Article]
      ,[Color],[ScheduledLineID],[ScannedLineID],[Cell],[Pattern],[PtnDesc],[Group],[Size],[Artwork]
      ,[Qty],[SubprocessID],[PostSewingSubProcess],[NoBundleCardAfterSubprocess],[Location],[BundleCreateDate]
      ,[BuyerDeliveryDate],[SewingInline],[SubprocessQAInspectionDate],[InTime],[OutTime],[POSupplier]
      ,[AllocatedSubcon],[AvgTime],[TimeRange],[EstimatedCutDate],[CuttingOutputDate]
      ,[Item],[PanelNo],[CutCellID],[SpreadingNo],[LastSewDate],[SewQty])
	  values(    s.[Bundleno] ,s.[RFIDProcessLocationID] ,s.[EXCESS] ,s.[FabricKind] ,s.[CutRef] ,
    s.[Sp] ,s.[MasterSP] ,s.[M] ,s.[Factory] ,s.[Category],s.[Program],s.[Style] ,s.[Season],
    s.[Brand],s.[Comb] ,s.[Cutno],s.[FabPanelCode] ,s.[Article] ,s.[Color] ,s.[ScheduledLineID] ,s.[ScannedLineID] ,
    s.[Cell] ,s.[Pattern] ,s.[PtnDesc] ,s.[Group] ,s.[Size] ,s.[Artwork] ,s.[Qty] ,s.[SubprocessID] ,
    s.[PostSewingSubProcess] ,s.[NoBundleCardAfterSubprocess] ,s.[Location] ,s.[BundleCreateDate],
    s.[BuyerDeliveryDate],s.[SewingInline],s.[SubprocessQAInspectionDate],s.[InTime],s.[OutTime],s.[POSupplier] ,
    s.[AllocatedSubcon] ,s.AvgTime ,s.[TimeRange],s.[EstimatedCutDate],s.CuttingOutputDate,	s.Item 
	,s.PanelNo	,s.CutCellID     ,s.SpreadingNo     ,s.[LastSewDate]     ,s.[SewQty] );


if exists (select 1 from BITableInfo b where b.id = 'P_SubprocessWIP')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_SubprocessWIP'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_SubprocessWIP', getdate())
end


END