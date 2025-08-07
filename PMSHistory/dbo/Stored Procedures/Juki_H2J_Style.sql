CREATE PROCEDURE dbo.Juki_H2J_Style
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @BatchSize INT = 5000,  -- �C��B�z����
        @Remaining INT;

    -- 3. �M�z�Ȧs��
    IF OBJECT_ID('tempdb..#FilterData') IS NOT NULL
        DROP TABLE #FilterData;

    -- 4. ���X�Ҧ��|���P�B�� Style ��ƨ�Ȧs��A�ëO�d Ukey �H�K�����s
    SELECT
        SeasonID                                             AS Season,
        CONCAT(SampleGroup,'_',StyleID,'_',ComboType)        AS StyleNo,
        CONCAT(StyleID,'_',StyleName)						 AS StyleName,
        CONCAT(SampleGroup,'_',StyleID,'_',ComboType)        AS KeyWord1,
        SeasonID                                             AS KeyWord2,
        CONCAT(ProductType,'_',Construction)                 AS KeyWord3,
        Flag,
        AddDate                                              AS Systime,
        Ukey
    INTO #FilterData
    FROM Juki_T_Style
    WHERE TransferToJukiExchangeDBDate IS NULL;

    -- 5. �إ߻E�L���ޡA�[�t���� ORDER BY/ JOIN/ DELETE
    CREATE CLUSTERED INDEX IX_FilterData_Ukey 
        ON #FilterData(Ukey);

    -- 6. �p���`���ơA�}�l����j��
    SELECT @Remaining = COUNT(*) FROM #FilterData;
    WHILE (@Remaining > 0)
    BEGIN
        -- 6.1 �N�����Ʒh�� #Batch
        IF OBJECT_ID('tempdb..#Batch') IS NOT NULL
            DROP TABLE #Batch;

        SELECT TOP (@BatchSize) *
        INTO #Batch
        FROM #FilterData
        ORDER BY Ukey;

        BEGIN TRY
            -- 6.2 OPENQUERY �@�����]�e�X����Ҧ���
            INSERT INTO JukiInstance.ExchangeDB.dbo.VW_Style(Season,StyleNo,StyleName,KeyWord1,KeyWord2,KeyWord3,Flag,Systime)
            SELECT
                Season,
                StyleNo,
                StyleName,
                KeyWord1,
                KeyWord2,
                KeyWord3,
                Flag,
                Systime
            FROM #Batch;

            -- 6.3 ���\�G��s���a�P�B�ɶ�
            UPDATE tgt
            SET TransferToJukiExchangeDBDate = GETDATE()
            FROM Juki_T_Style AS tgt
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
            FROM Juki_T_Style AS tgt
            INNER JOIN #Batch     AS b
                ON tgt.Ukey = b.Ukey;

            DELETE f
            FROM #FilterData AS f
            INNER JOIN #Batch     AS b
                ON f.Ukey = b.Ukey;

            DROP TABLE #Batch;
        END CATCH;

        -- 6.6 ��s�Ѿl���ơA�M�w�O�_�~��
        SELECT @Remaining = COUNT(*) FROM #FilterData;
    END

    -- 7. �M�z�Ȧs��
    DROP TABLE #FilterData;
END
GO
