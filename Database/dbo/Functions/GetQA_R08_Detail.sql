CREATE FUNCTION [dbo].[GetQA_R08_Detail]
(
	@InspectionDateFrom date,
	@InspectionDateTo date,
	@Inspectors varchar(200),
	@POIDFrom varchar(13),
	@POIDTo varchar(13),
	@RefNoFrom varchar(36),
	@RefNoTo varchar(36),
	@BrandIDs varchar(200),
    @EditDateFrom date,
	@EditDateTo date
)
RETURNS @returntable TABLE
(
	InspDate date,
    Inspector varchar(10),
    InspectorName nvarchar(30),
    BrandID varchar(8),
    Factory varchar(8),
    StyleID varchar(15),
    POID varchar(13),
    SEQ varchar(6),
    StockType nvarchar(10),
    Wkno varchar(13),
    SuppID varchar(6),
    SuppName varchar(12),
    ATA date,
    Roll varchar(8),
    Dyelot varchar(8),
    RefNo varchar(36),
    Color varchar(500),
    ArrivedYDS numeric(11, 2),
    ActualYDS numeric(8, 2),
    LthOfDiff numeric(8, 2),
    TransactionID varchar(30),
    QCIssueQty numeric(11, 2),
    QCIssueTransactionID varchar(13),
    CutWidth numeric(5, 2),
    ActualWidth numeric(5, 2),
    Speed numeric(10, 2),
    TotalDefectPoints numeric(6, 0),
	PointRatePerRoll numeric(8,2),
    Grade varchar(10),
    ActInspTimeStart datetime,
    CalculatedInspTimeStartFirstTime datetime,
    ActInspTimeFinish datetime,
    InspTimeFinishFirstTime datetime,
    QCMachineStopTime int,
    QCMachineRunTime int,
    Remark nvarchar(60),
    MCHandle varchar(45),
    WeaveType varchar(20),
	ReceivingID varchar(13),
    AddDate datetime,
    EditDate datetime
)
AS
BEGIN

    Declare @InspectorWhere table(
        Inspector  varchar(10)
    )

    Declare @BrandWhere table(
        BrandID  varchar(8)
    )

    insert into @InspectorWhere
    select  Data
    from SplitString(@Inspectors, ',')

    insert into @BrandWhere
    select  Data
    from SplitString(@BrandIDs, ',')

	insert into @returntable( InspDate
                             ,Inspector
                             ,InspectorName
                             ,BrandID
                             ,Factory
                             ,StyleID
                             ,POID
                             ,SEQ
                             ,StockType
                             ,Wkno
                             ,SuppID
                             ,SuppName
                             ,ATA
                             ,Roll
                             ,Dyelot
                             ,RefNo
                             ,Color
                             ,ArrivedYDS
                             ,ActualYDS
                             ,LthOfDiff
                             ,TransactionID
                             ,QCIssueQty
                             ,QCIssueTransactionID
                             ,CutWidth
                             ,ActualWidth
                             ,Speed
                             ,TotalDefectPoints
							 ,PointRatePerRoll
                             ,Grade
                             ,ActInspTimeStart
                             ,CalculatedInspTimeStartFirstTime
                             ,ActInspTimeFinish
                             ,InspTimeFinishFirstTime
                             ,QCMachineStopTime
                             ,QCMachineRunTime
                             ,Remark
                             ,MCHandle
                             ,WeaveType
							 ,ReceivingID
                             ,AddDate
                             ,EditDate
)
    SELECT   FP.InspDate
            ,FP.Inspector
            ,isnull(Pass1.Name, '')
            ,isnull(o.brandID  , '')
            ,isnull(o.FtyGroup , '')
            ,isnull(o.StyleID  , '')
            ,F.POID
            ,concat(RTRIM(F.SEQ1) ,'-',F.SEQ2)
            ,[StockType]=iif(isnull(rd. StockType, '') = '', '', (select Name from DropDownList ddl  where ddl.id like '%'+rd. StockType+'%' and ddl.Type = 'Pms_StockType'))
            ,[Wkno] = isnull(rd.ExportID, '')
	        ,f.SuppID
	        ,[SuppName]=(select AbbEN from Supp where id = f.SuppID)
	        ,rd.WhseArrival
            ,fp.Roll
            ,fp.Dyelot
	        ,RTRIM(isnull(p.RefNo, ''))
	        ,[Color]= isnull(dbo.GetColorMultipleID(o.BrandID,isnull(psdsC.SpecValue ,'')), '')
            ,[ArrivedYDS] = isnull(RD.StockQty, 0)
            ,[ActualYDS] = FP.ActualYds
            ,[LthOfDiff] = FP.ActualYds - FP.TicketYds
	        ,isnull(FP.TransactionID, '')
	        ,isnull(isd.Qty, 0)
	        ,[QCIssueTransactionID] = isnull(isd.Id, '')
            ,[CutWidth] = isnull(Fabric.width, 0)
            ,[ActualWidth] = FP.ActualWidth
            ,[Speed] = convert(numeric(10,2), IIF((FP.QCTime- System.QCMachineDelayTime * FP.QCStopQty) <= 0, 0,
	                     Round(FP.ActualYds/((FP.QCTime- System.QCMachineDelayTime * FP.QCStopQty)/60),2)))
	        ,FP.TotalPoint
			,[PointRatePerRoll] = FP.PointRate
            ,isnull(FP.Grade, '')
            ,[ActualInspectionTimeStart] = FP.StartTime
	        ,[CalculatedInspTimeStartFirstTime] = DATEADD(second, FP.QCTime*-1, FP.AddDate)
            ,[ActualInspectionTimeFinish] = fp.EditDate
	        ,[InspTimeFinishFirstTime] = FP.AddDate
            ,[QCMachineStopTime] = case when fp.AddDate is null or fp.StartTime is null then fp.QCTime
		        						   else DATEDIFF(SECOND,fp.StartTime,fp.AddDate) - fp.QCTime end
	        ,[QCMachineRunTime] = fp.QCTime
            ,[Remark]=FP.Remark
            ,[MCHandle]= isnull(dbo.getPass1_ExtNo(o.MCHandle), '')
            ,isnull(Fabric.WeaveTypeID, '')
			,[ReceivingID] = isnull(RD.ID,'')
            ,fp.AddDate
	        ,fp.EditDate
    FROM System, FIR_Physical AS FP
    inner JOIN FIR AS F ON FP.ID=F.ID
    LEFT JOIN View_AllReceivingDetail RD ON RD.PoId= F.POID AND RD.Seq1 = F.SEQ1 AND RD.Seq2 = F.SEQ2
    								AND RD.Roll = FP.Roll AND RD.Dyelot = FP.Dyelot
    LEFT join PO_Supp_Detail p on p.ID = f.poid and p.seq1 = f.seq1 and p.seq2 = f.seq2
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = p.id and psdsC.seq1 = p.seq1 and psdsC.seq2 = p.seq2 and psdsC.SpecColumnID = 'Color'
    LEFT join orders o on o.id=f.POID
    LEFT JOIN Fabric on Fabric.SCIRefno  = f.SCIRefno
    LEFT JOIN Issue_Detail isd on FP.Issue_DetailUkey = isd.ukey and isd.IsQMS = 1
    LEFT JOIN Pass1 on Pass1.ID = FP.Inspector
    WHERE   (FP.InspDate >= @InspectionDateFrom or @InspectionDateFrom is null) and
            (FP.InspDate <= @InspectionDateTo or @InspectionDateTo is null) and
            (FP.EditDate >= @EditDateFrom or FP.AddDate >= @EditDateFrom or @EditDateFrom is null) and
            (FP.EditDate <= @EditDateTo or FP.AddDate <= @EditDateTo or @EditDateTo is null) and
            (F.POID >= @POIDFrom or @POIDFrom = '') and
            (F.POID <= @POIDTo or @POIDTo = '') and
            (p.RefNo >= @RefNoFrom or @RefNoFrom = '') and
            (p.RefNo <= @RefNoTo or @RefNoTo = '') and
            (FP.Inspector in (select Inspector from @InspectorWhere) or @Inspectors = '') and
            (o.brandID in (select BrandID from @BrandWhere) or @BrandIDs = '') 
    ORDER BY FP.InspDate, FP.Inspector, F.POID, F.SEQ1, F.SEQ2, fp.Roll, fp.Dyelot


	RETURN
END
go
