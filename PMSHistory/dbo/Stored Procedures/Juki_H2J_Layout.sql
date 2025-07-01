CREATE PROCEDURE dbo.Juki_H2J_Layout
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,  -- 每批筆數
        @Remaining INT;

    -- 1. 清理暫存表
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 2. 全量撈出未同步的資料到 #FilterData，並帶上 Ukey 以便後續更新
    SELECT
        CONCAT(SampleGroup, '_', StyleID, '_', ComboType, '_', SeasonID) AS LayoutKey,
        CONCAT(FactoryID, '_', SewingLineID)                       AS FactoryLine,
        MachineID,
        MachineSeq,
        ProcessSeq,
        ProcessCode,
        SewerDiffPercentage * 100.0                             AS TarPer,
        CONCAT(SampleGroup, '_', StyleID, '_', ComboType)       AS KeyWord1,
        SeasonID                                               AS KeyWord2,
        Flag,
        AddDate                                                AS Systime,
        Ukey
    INTO #FilterData
    FROM Juki_T_Layout
    WHERE TransferToJukiExchangeDBDate IS NULL;

    -- 3. 在暫存表上建立索引，加速 JOIN／DELETE／ORDER
    CREATE CLUSTERED INDEX IX_FilterData_Ukey ON #FilterData(Ukey);

    -- 4. 開始分批迴圈
    SELECT @Remaining = COUNT(*) FROM #FilterData;

    WHILE (@Remaining > 0)
    BEGIN
        -- 取本批資料到 #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY
            -- 5. OPENQUERY 一次送出本批所有筆
            INSERT INTO JukiInstance.ExchangeDB.dbo.VW_Layout(LayoutName,LineName,MachCode,MachSeqNo,ProSeqNo,ProcessCode,TarPer,KeyWord1,KeyWord2,Flag,Systime)
            SELECT
                LayoutKey      AS LayoutName,
                FactoryLine    AS LineName,
                MachineID      AS MachCode,
                MachineSeq     AS MachSeqNo,
                ProcessSeq      AS ProSeqNo,
                ProcessCode,
                TarPer,
                KeyWord1,
                KeyWord2,
                Flag,
                Systime
            FROM #Batch;

            -- 6. 成功：更新本地同步時間
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_Layout AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey;

            -- 7. 刪除已處理的暫存筆，以便下一輪
            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END TRY
        BEGIN CATCH
            -- 8. 本批失敗：記錄錯誤訊息，並移除本批避免重複
            UPDATE tgt
            SET TransferToJukiExchangeDBErrorMsg = ERROR_MESSAGE()
            FROM Juki_T_Layout AS tgt
            INNER JOIN #Batch     AS b
                ON tgt.Ukey = b.Ukey;

            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END CATCH;

        -- 9. 計算剩餘筆數，決定是否繼續
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- 10. 清理暫存表
    DROP TABLE #FilterData;
END
GO
