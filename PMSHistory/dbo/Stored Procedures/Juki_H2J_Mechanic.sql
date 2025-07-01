CREATE PROCEDURE dbo.Juki_H2J_Mechanic
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,  -- 每批處理筆數
        @Remaining INT;

    -- 1. 清理暫存表
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 2. 全量撈出待同步的機台人員資料到暫存表，並帶上 Pkey欄位 以便後續更新
    SELECT
        EmployeeID    AS MechanicCode,
        EmployeeName  AS MechanicName,
        RFIDNo,
        Flag,
		/* ==== The Systime is determined by the Flag to maintain the order ==== */
		CASE 
			WHEN Flag = 3 THEN DATEADD(millisecond, -6, AddDate) -- -6 ms
			WHEN Flag = 2 THEN DATEADD(millisecond, -3, AddDate) -- -3 ms
			WHEN Flag = 1 THEN DATEADD(millisecond,  0, AddDate) -- +0 ms
			ELSE AddDate
		END                                                        AS Systime,
        Ukey
    INTO #FilterData
    FROM Juki_T_Mechanic
    WHERE TransferToJukiExchangeDBDate IS NULL;

    -- 3. 在暫存表上建立索引，加速後續 JOIN/ORDER/DELETE
    CREATE CLUSTERED INDEX IX_FilterData_Ukey ON #FilterData(Ukey);

    -- 4. 計算總筆數，開始分批處理
    SELECT @Remaining = COUNT(*) FROM #FilterData;
    WHILE (@Remaining > 0)
    BEGIN
        -- 4.1 取本批資料到 #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY
            -- 5. OPENQUERY 一次送出本批所有筆
            INSERT INTO JukiInstance.ExchangeDB.dbo.VW_Mechanic(MechanicCode,MechanicName,RFIDNo,Flag,Systime)
            SELECT
                MechanicCode,
                MechanicName,
                RFIDNo,
                Flag,
                Systime
            FROM #Batch;

            -- 6. 成功：更新本地同步時間
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_Mechanic AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey AND tgt.EmployeeID = b.MechanicCode;

            -- 7. 刪除已處理資料，進行下一批
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
            FROM Juki_T_Mechanic AS tgt
            INNER JOIN #Batch     AS b
                ON tgt.Ukey = b.Ukey AND tgt.EmployeeID = b.MechanicCode;

            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey AND tgt.EmployeeID = b.MechanicCode;

            DROP TABLE #Batch;
        END CATCH;

        -- 9. 計算剩餘筆數，決定是否繼續
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- 10. 清理暫存表
    DROP TABLE #FilterData;
END
GO
