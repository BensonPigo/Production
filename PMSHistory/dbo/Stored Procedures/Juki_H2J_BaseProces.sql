CREATE PROCEDURE dbo.Juki_H2J_BaseProces
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,      -- 每批筆數
        @Remaining INT;

    -- 清理舊暫存表
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 3. 把所有待同步資料一次撈到 #FilterData，並在 Ukey 上建索引
    SELECT
        ProcessCode,
        ProcessName,
        LEFT(ProcessName, 16)                             AS ProcessDisp,
        TargetTime,
        CONCAT(SampleGroup,'_',StyleID,'_',ComboType)     AS KeyWord1,
        SeasonID                                         AS KeyWord2,
        CONCAT(ProductType,'_',Construction)              AS KeyWord3,
        Flag,
        AddDate                                          AS Systime,
        Ukey
    INTO #FilterData
    FROM Juki_T_BaseProcess
    WHERE TransferToJukiExchangeDBDate IS NULL;

    CREATE CLUSTERED INDEX IX_FilterData_Ukey 
        ON #FilterData(Ukey);

    -- 4. 分批迴圈
    SELECT @Remaining = COUNT(*) FROM #FilterData;
    WHILE (@Remaining > 0)
    BEGIN
        -- 每輪先把 TOP N 筆搬到 #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY

            -- 5. 一次送出本批所有筆（減少 round-trip）
            INSERT INTO JukiInstance.ExchangeDB.dbo.VW_BaseProcess( ProcessCode,ProcessName,ProcessDisp,TargetTime,KeyWord1,KeyWord2,KeyWord3,Flag,Systime)
            SELECT
                ProcessCode,
                ProcessName,
                ProcessDisp,
                TargetTime,
                KeyWord1,
                KeyWord2,
                KeyWord3,
                Flag,
                Systime
            FROM #Batch;

            -- 6. 成功後更新本地同步時間
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_BaseProcess AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey;

            -- 7. 刪除已處理筆，進行下一輪
            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END TRY
        BEGIN CATCH
            -- 8. 本批失敗則記錯誤訊息，並同樣移出以免重複
            UPDATE tgt
            SET TransferToJukiExchangeDBErrorMsg = ERROR_MESSAGE()
            FROM Juki_T_BaseProcess AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey;

            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END CATCH;

        -- 計算剩餘筆數
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- 清理
    DROP TABLE #FilterData;
END
GO