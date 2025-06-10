CREATE PROCEDURE [dbo].[MovePhysicalInspectionToHistory]
(
    @FIR_PhysicalDetailUkey BIGINT,        -- Physical Inspection 檢驗明細的 Pkey
    @ReInspection BIT,                     -- 0: 資料移除 (直接轉入歷史區); 1: 複驗 (部分資料保留在正式區)
    @Inspector VARCHAR(10) = NULL          -- 只有 QMS P01 才需要傳入此次的檢驗人員 ID
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

	-- 1. 取出 FIR_Physical.InspSeq 及相關資訊
	SELECT
		@FIRID = p.ID
	   ,@Roll = p.Roll
	   ,@Dyelot = p.Dyelot
	   ,@InspSeq = p.InspSeq
	FROM [dbo].[FIR_Physical] p
	WHERE p.DetailUkey = @FIR_PhysicalDetailUkey;
	
	-- 從 FIR 找出相關欄位: POID, Seq1, Seq2, ReceivingID
	SELECT
		@POID = f.POID
	   ,@Seq1 = f.SEQ1
	   ,@Seq2 = f.SEQ2
	   ,@ReceivingID = f.ReceivingID
	FROM [dbo].[FIR] f
	WHERE f.ID = @FIRID;
	
	-- 2. 轉入歷史區: FIR_Physical -> FIR_Physical_His
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
	
	-- 3. 轉入歷史區: FIR_Physical_Defect -> FIR_Physical_Defect_His
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
	
	-- 4. 轉入歷史區: FIR_Physical_Defect_Realtime -> FIR_Physical_Defect_Realtime_His
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
	
	-- 5. 轉入歷史區: FIR_Physical_QCTime -> FIR_Physical_OCTime_His (注意表名稱差異)
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
	
	-- 取得下一個檢驗序號
	SET @InspSeq = dbo.GetNextInspectionSeq(@FIRID, @Roll, @Dyelot);
	
	-- 根據 ReInspection 選擇後續動作
	IF @ReInspection = 0 -- 資料移除
	BEGIN
		-- 刪除 FIR_Physical_QCTime
		DELETE FROM [dbo].[FIR_Physical_QCTime]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
		
		-- 刪除 FIR_Physical_Defect_Realtime
		DELETE FROM [dbo].[FIR_Physical_Defect_Realtime]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
		
		-- 刪除 FIR_Physical_Defect
		DELETE FROM [dbo].[FIR_Physical_Defect]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
		
		-- 刪除 FIR_Physical
		DELETE FROM [dbo].[FIR_Physical]
		WHERE DetailUkey = @FIR_PhysicalDetailUkey;
	END
	ELSE IF @ReInspection = 1 -- 複驗
	BEGIN
		-- 更新 FIR_Physical
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
		
		-- 更新 FIR_Physical_Defect_Realtime
		UPDATE [dbo].[FIR_Physical_Defect_Realtime]
		SET AddDate = NULL
		   ,MachineIoTUkey = NULL
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
		
		-- 刪除 FIR_Physical_QCTime
		DELETE FROM [dbo].[FIR_Physical_QCTime]
		WHERE FIR_PhysicalDetailUKey = @FIR_PhysicalDetailUkey;
	END

END;
GO
