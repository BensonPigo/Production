CREATE PROCEDURE dbo.Juki_H2J_Mechanic
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,  -- �C��B�z����
        @Remaining INT;

    -- 1. �M�z�Ȧs��
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 2. ���q���X�ݦP�B�����x�H����ƨ�Ȧs��A�ña�W Pkey��� �H�K�����s
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

    -- 3. �b�Ȧs��W�إ߯��ޡA�[�t���� JOIN/ORDER/DELETE
    CREATE CLUSTERED INDEX IX_FilterData_Ukey ON #FilterData(Ukey);

    -- 4. �p���`���ơA�}�l����B�z
    SELECT @Remaining = COUNT(*) FROM #FilterData;
    WHILE (@Remaining > 0)
    BEGIN
        -- 4.1 �������ƨ� #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY
            -- 5. OPENQUERY �@���e�X����Ҧ���
            INSERT INTO JukiInstance.ExchangeDB.dbo.VW_Mechanic(MechanicCode,MechanicName,RFIDNo,Flag,Systime)
            SELECT
                MechanicCode,
                MechanicName,
                RFIDNo,
                Flag,
                Systime
            FROM #Batch;

            -- 6. ���\�G��s���a�P�B�ɶ�
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_Mechanic AS tgt
            INNER JOIN #Batch AS b
                ON tgt.Ukey = b.Ukey AND tgt.EmployeeID = b.MechanicCode;

            -- 7. �R���w�B�z��ơA�i��U�@��
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
            FROM Juki_T_Mechanic AS tgt
            INNER JOIN #Batch     AS b
                ON tgt.Ukey = b.Ukey AND tgt.EmployeeID = b.MechanicCode;

            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey AND tgt.EmployeeID = b.MechanicCode;

            DROP TABLE #Batch;
        END CATCH;

        -- 9. �p��Ѿl���ơA�M�w�O�_�~��
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- 10. �M�z�Ȧs��
    DROP TABLE #FilterData;
END
GO
