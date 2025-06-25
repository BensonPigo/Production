CREATE PROCEDURE dbo.Juki_H2J_Layout
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,  -- �C�嵧��
        @Remaining INT;

    -- 1. �M�z�Ȧs��
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 2. ���q���X���P�B����ƨ� #FilterData�A�ña�W Ukey �H�K�����s
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

    -- 3. �b�Ȧs��W�إ߯��ޡA�[�t JOIN��DELETE��ORDER
    CREATE CLUSTERED INDEX IX_FilterData_Ukey ON #FilterData(Ukey);

    -- 4. �}�l����j��
    SELECT @Remaining = COUNT(*) FROM #FilterData;

    WHILE (@Remaining > 0)
    BEGIN
        -- �������ƨ� #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY
            -- 5. OPENQUERY �@���e�X����Ҧ���
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

            -- 6. ���\�G��s���a�P�B�ɶ�
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_Layout AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey;

            -- 7. �R���w�B�z���Ȧs���A�H�K�U�@��
            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END TRY
        BEGIN CATCH
            -- 8. ���奢�ѡG�O�����~�T���A�ò��������קK����
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

        -- 9. �p��Ѿl���ơA�M�w�O�_�~��
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- 10. �M�z�Ȧs��
    DROP TABLE #FilterData;
END
GO
