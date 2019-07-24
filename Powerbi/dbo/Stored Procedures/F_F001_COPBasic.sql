-- =============================================
-- Author:		<Eden.Wang>
-- Create date: <2019/05/30>
-- Description:	<Import FinanceTW MDivision,Process,ProcessDetail>
-- =============================================
CREATE PROCEDURE [dbo].[F_F001_COPBasic]

AS
BEGIN
	SET NOCOUNT ON;

	-- CREATE TABLE [dbo].[F_F001_MDivision]
	If Exists(Select * From PBIReportData.sys.tables Where Name = 'F_F001_MDivision')
	Begin
		Drop Table PBIReportData.dbo.F_F001_MDivision;
	End;
	CREATE TABLE [dbo].[F_F001_MDivision] (
		[MdivisionSeq] [int] NOT NULL,
		[Mdivision] [nvarchar](5) NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[MdivisionSeq] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	-- CREATE TABLE [dbo].[F_F001_Process]
	If Exists(Select * From PBIReportData.sys.tables Where Name = 'F_F001_Process')
	Begin
		Drop Table PBIReportData.dbo.F_F001_Process;
	End;
	CREATE TABLE [dbo].[F_F001_Process] (
		[ProcessSeq] [int] NOT NULL,
		[Process] [nvarchar](100) NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[ProcessSeq] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	-- CREATE TABLE [dbo].[F_F001_ProcessDetail]
	If Exists(Select * From PBIReportData.sys.tables Where Name = 'F_F001_ProcessDetail')
	Begin
		Drop Table PBIReportData.dbo.F_F001_ProcessDetail;
	End;
	CREATE TABLE [dbo].[F_F001_ProcessDetail] (
		[ProcessDetailSeq] [numeric](18, 2) NOT NULL,
		[ProcessSeq] [int] NOT NULL,
		[ProcessDetailSort] [int] NOT NULL,
		[ProcessDetail] [nvarchar](100) NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[ProcessDetailSeq] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	IF OBJECT_ID('tempdb.dbo.#OUMDivision') IS NOT NULL 
	BEGIN
		DROP TABLE #OUMDivision
	END
	
	IF OBJECT_ID('tempdb.dbo.#COPGroup') IS NOT NULL 
	BEGIN
		DROP TABLE #COPGroup
	END

	IF OBJECT_ID('tempdb.dbo.#COPBasic') IS NOT NULL 
	BEGIN
		DROP TABLE #COPBasic
	END
	
	SELECT * into #OUMDivision
	FROM OPENQUERY ([TRADEDB], 
	'select distinct MdivisionSeq = ReportOrder,Mdivision = OUID
	 from FinanceTW.dbo.DivisionMapping dm
	 left join FinanceTW.dbo.OU on dm.OUID = OU.ID
	 WHERE DivisionID <> '''' and IsSampleOU = 0 and ReportOrder <> 0')
	
	SELECT * into #COPGroup
	FROM OPENQUERY ([TRADEDB], 
	'select ProcessSeq = c.ReportOrder,
		    Process = c.Description,
		    ProcessDetailSeq = cast(c.ReportOrder as varchar(2)) + cast(cast(cd.SerialNumber/100.0 as numeric(18,2)) as varchar(6)),
		    ProcessDetailSort = cd.SerialNumber,
		    ProcessDetail = iif(cd.Details in(''Revenue'',''Expense'',''Profit / (Loss)''),''Sub '' + cd.Details,cd.Details)
	 from FinanceTW.dbo.COPGroup c
	 inner join FinanceTW.dbo.COPGroup_ReportDetail cd on c.ID = cd.ID
	 where ReportOrder > 0
	 order by ReportOrder,SerialNumber')

	 select * into #COPBasic
	 from #OUMDivision
	 inner join #COPGroup on 1= 1 
	 order by MdivisionSeq,ProcessSeq,ProcessDetailSort

	 insert into F_F001_MDivision
	 select distinct MdivisionSeq,Mdivision from #COPBasic order by MdivisionSeq

	 insert into F_F001_Process
	 select distinct ProcessSeq,Process from #COPBasic order by ProcessSeq

	 insert into F_F001_ProcessDetail
	 select distinct ProcessDetailSeq,ProcessSeq,ProcessDetailSort,ProcessDetail from #COPBasic order by ProcessDetailSeq
END