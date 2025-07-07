CREATE PROCEDURE dbo.Juki_H2J_BaseProces
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,      -- �C�嵧��
        @Remaining INT;

    -- �M�z�¼Ȧs��
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 3. ��Ҧ��ݦP�B��Ƥ@������ #FilterData�A�æb Ukey �W�د���
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

    -- 4. ����j��
    SELECT @Remaining = COUNT(*) FROM #FilterData;
    WHILE (@Remaining > 0)
    BEGIN
        -- �C������ TOP N ���h�� #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY

            -- 5. �@���e�X����Ҧ����]��� round-trip�^
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

            -- 6. ���\���s���a�P�B�ɶ�
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_BaseProcess AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey;

            -- 7. �R���w�B�z���A�i��U�@��
            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END TRY
        BEGIN CATCH
            -- 8. ���奢�ѫh�O���~�T���A�æP�˲��X�H�K����
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

        -- �p��Ѿl����
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- �M�z
    DROP TABLE #FilterData;
END
GO