CREATE PROCEDURE dbo.Juki_H2J_Operator
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,  -- 每批處理筆數
        @Remaining INT;

    -- 3. 清理暫存表
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 4. 撈出所有待同步的 Operator 資料到暫存表，並帶上 Ukey 以便後續更新
    SELECT
        EmployeeID    AS OperatorCode,
        EmployeeName  AS OperatorName,
        RFIDNo,
        FactoryID     AS KeyWord1,
        CONCAT(FactoryID, '_', SewingLineID) AS KeyWord2,
        Flag,
        AddDate       AS Systime,
        Ukey
    INTO #FilterData
    FROM Juki_T_Operator
    WHERE TransferToJukiExchangeDBDate IS NULL;

    -- 5. 建立聚簇索引，加速 JOIN/ORDER/DELETE
    CREATE CLUSTERED INDEX IX_FilterData_Ukey 
        ON #FilterData(Ukey);

    -- 6. 開始分批迴圈
    SELECT @Remaining = COUNT(*) FROM #FilterData;
    WHILE (@Remaining > 0)
    BEGIN
        -- 6.1 本批資料搬到 #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY
            -- 6.2 OPENQUERY 一次送出本批所有筆
            INSERT INTO JukiInstance.ExchangeDB.dbo.VW_Operator(OperatorCode,OperatorName,RFIDNo,KeyWord1,KeyWord2,Flag,Systime)
            SELECT
                OperatorCode,
                OperatorName,
                RFIDNo,
                KeyWord1,
                KeyWord2,
                Flag,
                Systime
            FROM #Batch;

            -- 6.3 成功：更新本地同步時間
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_Operator AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey;

            -- 6.4 刪除已處理筆，以便下一輪
            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END TRY
        BEGIN CATCH
            -- 6.5 本批失敗：記錄錯誤訊息並移除，以免重複
            UPDATE tgt
            SET TransferToJukiExchangeDBErrorMsg = ERROR_MESSAGE()
            FROM Juki_T_Operator AS tgt
            INNER JOIN #Batch     AS b
                ON tgt.Ukey = b.Ukey;

            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END CATCH;

        -- 6.6 計算剩餘筆數，決定是否繼續
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- 7. 清理暫存表
    DROP TABLE #FilterData;
END
GO