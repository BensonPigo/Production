CREATE PROCEDURE dbo.Juki_H2J_Operator
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,  -- �C��B�z����
        @Remaining INT;

    -- 3. �M�z�Ȧs��
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 4. ���X�Ҧ��ݦP�B�� Operator ��ƨ�Ȧs��A�ña�W Ukey �H�K�����s
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

    -- 5. �إ߻E�L���ޡA�[�t JOIN/ORDER/DELETE
    CREATE CLUSTERED INDEX IX_FilterData_Ukey 
        ON #FilterData(Ukey);

    -- 6. �}�l����j��
    SELECT @Remaining = COUNT(*) FROM #FilterData;
    WHILE (@Remaining > 0)
    BEGIN
        -- 6.1 �����Ʒh�� #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY
            -- 6.2 OPENQUERY �@���e�X����Ҧ���
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

            -- 6.3 ���\�G��s���a�P�B�ɶ�
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_Operator AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey;

            -- 6.4 �R���w�B�z���A�H�K�U�@��
            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END TRY
        BEGIN CATCH
            -- 6.5 ���奢�ѡG�O�����~�T���ò����A�H�K����
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

        -- 6.6 �p��Ѿl���ơA�M�w�O�_�~��
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- 7. �M�z�Ȧs��
    DROP TABLE #FilterData;
END
GO