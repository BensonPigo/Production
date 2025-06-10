CREATE PROCEDURE [dbo].[MovePhysicalInspectionToHistory]
(
    @FIR_PhysicalDetailUkey BIGINT,        -- Physical Inspection ������Ӫ� Pkey
    @ReInspection BIT,                     -- 0: ��Ʋ��� (������J���v��); 1: ���� (������ƫO�d�b������)
    @Inspector VARCHAR(10) = NULL          -- �u�� QMS P01 �~�ݭn�ǤJ����������H�� ID
)
AS
BEGIN
SET NOCOUNT ON;
    
    DECLARE @FIRID BIGINT;
    DECLARE @Roll NVARCHAR(50);
    DECLARE @Dyelot NVARCHAR(50);
    DECLARE @InspSeq INT;
    DECLARE @POID VARCHAR(13);
    DECLARE @Seq1 VARCHAR(3);
    DECLARE @Seq2 VARCHAR(2);
    DECLARE @ReceivingID VARCHAR(13);

	-- 1. ���X FIR_Physical.InspSeq �ά�����T
	SELECT
		@FIRID = p.ID
	   ,@Roll = p.Roll
	   ,@Dyelot = p.Dyelot
	   ,@InspSeq = p.InspSeq
	FROM [dbo].[FIR_Physical] p
	WHERE p.DetailUkey = @FIR_PhysicalDetailUkey;
	
	-- �q FIR ��X�������: POID, Seq1, Seq2, ReceivingID
	SELECT
		@POID = f.POID
	   ,@Seq1 = f.SEQ1
	   ,@Seq2 = f.SEQ2
	   ,@ReceivingID = f.ReceivingID
	FROM [dbo].[FIR] f
	WHERE f.ID = @FIRID;
	
	-- 2. ��J���v��: FIR_Physical -> FIR_Physical_His
    INSERT INTO [dbo].[FIR_Physical_His] (
    DetailUkey, 
    InspSeq, 
    ID, 
    Roll, 
    Dyelot, 
    POID, 
    Seq1, 
    Seq2, 
    ReceivingID,
    TicketYds, 
    ActualYds, 
    FullWidth, 
    ActualWidth, 
    TotalPoint, 
    PointRate, 
    Grade,
    Result, 
    Remark, 
    InspDate, 
    Inspector, 
    AddName, 
    AddDate, 
    EditName, 
    EditDate,
    QCTime, 
    IsQMS, 
    Issue_DetailUkey, 
    QMSMachineID, 
    ColorToneCheck, 
    GrandCcanUseReason,
    StartTime, 
    TransactionID, 
    QCstopQty, 
    Moisture, 
    IsGrandCCanUse
    )
		SELECT
			p.DetailUkey
		   ,@InspSeq
		   ,p.ID
		   ,p.Roll
		   ,p.Dyelot
		   ,@POID
		   ,@Seq1
		   ,@Seq2
		   ,@ReceivingID
		   ,p.TicketYds
		   ,p.ActualYds
		   ,p.FullWidth
		   ,p.ActualWidth
		   ,p.TotalPoint
		   ,p.PointRate
		   ,p.Grade
		   ,p.Result
		   ,p.Remark
		   ,p.InspDate
		   ,p.Inspector
		   ,p.AddName
		   ,p.AddDate
		   ,p.EditName
		   ,p.EditDate
		   ,p.QCTime
		   ,p.IsQMS
		   ,p.Issue_DetailUkey
		   ,p.QMSMachineID
		   ,p.ColorToneCheck
		   ,p.GrandCCanUseReason
		   ,p.StartTime
		   ,p.TransactionID
		   ,p.QCStopQty
		   ,p.Moisture
		   ,p.IsGrandCCanUse
		FROM [dbo].[FIR_Physical] p
		WHERE p.DetailUkey = @FIR_PhysicalDetailUkey;
	
	-- 3. ��J���v��: FIR_Physical_Defect -> FIR_Physical_Defect_His
	INSERT INTO [dbo].[FIR_Physical_Defect_His] (FIR_PhysicalDetailUKey, DefectLocation, ID, InspSeq, DefectRecord, Point, RealTimeInsert)
		SELECT
			FIR_PhysicalDetailUKey
		   ,DefectLocation
		   ,ID
		   ,@InspSeq
		   ,DefectRecord
		   ,Point
		   ,RealTimeInsert
		FROM [dbo].[FIR_Physical_Defect]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
	
	-- 4. ��J���v��: FIR_Physical_Defect_Realtime -> FIR_Physical_Defect_Realtime_His
	INSERT INTO [dbo].[FIR_Physical_Defect_Realtime_His] (Id, InspSeq, FIR_PhysicalDetailUKey, Yards, FabricdefectID, AddDate, T2, MachineIoTUkey)
		SELECT
			Id
		   ,@InspSeq
		   ,FIR_PhysicalDetailUKey
		   ,Yards
		   ,FabricdefectID
		   ,AddDate
		   ,T2
		   ,MachineIoTUkey
		FROM [dbo].[FIR_Physical_Defect_Realtime]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
	
	-- 5. ��J���v��: FIR_Physical_QCTime -> FIR_Physical_OCTime_His (�`�N��W�ٮt��)
	INSERT INTO [dbo].[FIR_Physical_OCTime_His] (Ukey, InspSeq, FIR_PhysicalDetailUKey, MachineIoTUkey, IsFirstInspection,
	StartTime, EndTime, StartYards, EndYards)
		SELECT
			Ukey
		   ,@InspSeq
		   ,FIR_PhysicalDetailUKey
		   ,MachineIoTUkey
		   ,IsFirstInspection
		   ,StartTime
		   ,EndTime
		   ,StartYards
		   ,EndYards
		FROM [dbo].[FIR_Physical_QCTime]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
	
	-- ���o�U�@������Ǹ�
	SET @InspSeq = dbo.GetNextInspectionSeq(@FIRID, @Roll, @Dyelot);
	
	-- �ھ� ReInspection ��ܫ���ʧ@
	IF @ReInspection = 0 -- ��Ʋ���
	BEGIN
		-- �R�� FIR_Physical_QCTime
		DELETE FROM [dbo].[FIR_Physical_QCTime]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
		
		-- �R�� FIR_Physical_Defect_Realtime
		DELETE FROM [dbo].[FIR_Physical_Defect_Realtime]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
		
		-- �R�� FIR_Physical_Defect
		DELETE FROM [dbo].[FIR_Physical_Defect]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
		
		-- �R�� FIR_Physical
		DELETE FROM [dbo].[FIR_Physical]
		WHERE DetailUkey = @FIR_PhysicalDetailUkey;
	END
	ELSE IF @ReInspection = 1 -- ����
	BEGIN
		-- ��s FIR_Physical
		UPDATE [dbo].[FIR_Physical]
		SET InspDate = GETDATE()
		   ,Inspector = @Inspector
		   ,StartTime = GETDATE()
		   ,EditName = @Inspector
		   ,EditDate = GETDATE()
		   ,QCTime = 0
		   ,QCStopQty = 0
		   ,InspSeq = @InspSeq
		WHERE DetailUkey = @FIR_PhysicalDetailUkey;
		
		-- ��s FIR_Physical_Defect_Realtime
		UPDATE [dbo].[FIR_Physical_Defect_Realtime]
		SET AddDate = NULL
		   ,MachineIoTUkey = NULL
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
		
		-- �R�� FIR_Physical_QCTime
		DELETE FROM [dbo].[FIR_Physical_QCTime]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
	END

END;
GO
