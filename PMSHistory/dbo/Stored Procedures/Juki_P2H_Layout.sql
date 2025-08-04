CREATE PROCEDURE Juki_P2H_Layout
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Production.dbo.System WHERE JukiExchangeDBActive = 0) RETURN;
    DECLARE @MaxDate DATETIME = (
		SELECT MAX(
			CASE Flag
				WHEN 3 THEN DATEADD(ms, 6, AddDate)  -- 還原 -6ms
				WHEN 2 THEN DATEADD(ms, 3, AddDate)  -- 還原 -3ms
				ELSE AddDate
			END)
		FROM PMSHistory.dbo.Juki_T_Layout
    );
	/* -----------------------------
		AddDate 依 Flag 調整
	   1. 寫入：
		  - Flag = 3  =>  AddDate = DATEADD(ms,-6,GETDATE())
		  - Flag = 2  =>  AddDate = DATEADD(ms,-3,GETDATE())  (目前腳本未用到，但先備好)
	   2. 取 @MaxDate 與比較 AddDate 時還原毫秒：
		  - Flag = 3  =>  +6 ms
		  - Flag = 2  =>  +3 ms
	   其餘邏輯保持不變。
	--------------------------------*/

    SELECT
            --By ID 欄位
            TableName = 'LineMapping'
            ,lm.ID
            ,lm.EditDate
            ,lm.SeasonID
            ,lm.StyleID
            ,lm.ComboType
            ,lm.SewingLineID
            ,lm.FactoryID
            ,lm.LMMachineConfirmDate
            ,b.SampleGroup
            --表身展開欄位
            ,lmd.OperationID
            ,lmd.MachineID
            ,[No] = ISNULL(lmd.[No], '')
            ,MachineTypeID = ISNULL(lmd.MachineTypeID, '')
            ,lmd.MasterPlusGroup
            ,Attachment = ISNULL(lmd.Attachment, '')
            ,lmd.SewingMachineAttachmentID
            ,Template = ISNULL(lmd.Template, '')
            ,lmd.SewerDiffPercentage
            ,OneShot = ISNULL(lmd.OneShot, 0)
            ,GSD = lmd.GSD
            ,[ProcessCode_Before] = dbo.RemoveLeadingTrailingZeros(OriNO)
            ,ForProcessSeq = ROW_NUMBER() OVER(PARTITION BY lm.ID ORDER BY
                            CASE WHEN lmd.No = '' THEN 1
	                        WHEN left(lmd.No, 1) = 'P' THEN 2
	                        ELSE 3 
	                        END, 
                            lmd.GroupKey)
    INTO #tmp
    FROM Production.dbo.LineMapping lm WITH(NOLOCK)
    INNER JOIN Production.dbo.LineMapping_Detail lmd WITH(NOLOCK) ON lmd.ID = lm.ID
    INNER JOIN Production.dbo.Brand b WITH(NOLOCK) ON lm.BrandID = b.ID
    WHERE lm.JukiIoTDataExchange = 1
    AND lm.Status = 'Confirmed'
    AND (@MaxDate IS NULL OR lm.EditDate > @MaxDate OR LM.LMMachineConfirmDate > @MaxDate)
    AND lmd.MachineCount = 1
    AND lmd.MachineTypeID <> ''
    AND lmd.MasterPlusGroup <>''
    AND lmd.No <> ''
    AND lmd.MachineID <> ''
    --AND lm.LMMachineConfirmDate <> ''
    
    UNION ALL
    SELECT
            --By ID 欄位
            TableName = 'LineMappingBalancing'
            ,lm.ID
            ,lm.EditDate
            ,lm.SeasonID
            ,lm.StyleID
            ,lm.ComboType
            ,lm.SewingLineID
            ,lm.FactoryID
            ,lm.LMMachineConfirmDate
            ,b.SampleGroup
            --表身展開欄位
            ,lmd.OperationID
            ,lmd.MachineID
            ,lmd.[No]
            ,lmd.MachineTypeID
            ,lmd.MasterPlusGroup
            ,lmd.Attachment
            ,lmd.SewingMachineAttachmentID
            ,lmd.Template
            ,lmd.SewerDiffPercentage
            ,OneShot = ISNULL(lmd.OneShot, 0)
            ,GSD = lmd.GSD
            ,[ProcessCode_Before] = dbo.RemoveLeadingTrailingZeros(lmd.TimeStudySeq)
            ,ForProcessSeq = ROW_NUMBER() OVER(PARTITION BY lm.ID ORDER BY IIF(No = '', 'ZZ', No), Seq)
    FROM Production.dbo.LineMappingBalancing lm WITH(NOLOCK)
    INNER JOIN Production.dbo.LineMappingBalancing_Detail lmd WITH(NOLOCK) ON lmd.ID = lm.ID
    INNER JOIN Production.dbo.Brand b WITH(NOLOCK) ON lm.BrandID = b.ID
    WHERE lm.JukiIoTDataExchange = 1
    AND lm.Status = 'Confirmed'
    AND (@MaxDate IS NULL OR lm.EditDate > @MaxDate OR LM.LMMachineConfirmDate > @MaxDate)
    AND lmd.PPA <> 'C'
    AND lmd.IsNonSewingLine = 0
    AND lmd.MachineTypeID NOT LIKE 'MM%'
    AND lmd.No <> ''
    AND lmd.MachineID <> ''
    --AND lm.LMMachineConfirmDate <> ''

    --Group 不同單
    SELECT FactoryID, SewingLineID, SampleGroup, StyleID, ComboType, SeasonID
          ,EditDate = MAX(EditDate) -- P03 + P06 可能會有相同欄位組合(Group By 那些欄位), 取最大 Confirm 時間
    INTO #tmpGroup
    FROM #tmp
    GROUP BY FactoryID, SewingLineID, SampleGroup, StyleID, ComboType, SeasonID--表頭
    
    --以表頭 Key 挑出最新的那張單, (含了展開 _Detail)
    SELECT t.*
        ,GroupForProcessOneShot1 = DENSE_RANK() OVER(
            ORDER BY -- 這些欄位值相同 → 一樣的數字
                ID
                ,TableName
                ,MachineID
                ,[No]
                ,MachineTypeID
                ,MasterPlusGroup
                ,Attachment
                ,SewingMachineAttachmentID
                ,Template
                ,OneShot)
    INTO #tmpbyKeyOnly1Header
    FROM #tmpGroup g
    OUTER APPLY(
        SELECT *
        FROM #tmp t
        WHERE t.SampleGroup = g.SampleGroup
          AND t.StyleID = g.StyleID
          AND t.ComboType = g.ComboType
          AND t.SeasonID = g.SeasonID
          AND t.FactoryID = g.FactoryID
          AND t.SewingLineID = g.SewingLineID
          AND t.EditDate = g.EditDate -- 最後那筆
    ) t
    
    /*
    編碼 ProcessSeq
    以下欄位相同群組, 先稱為(GK)
    ID
    ,TableName
    ,MachineID
    ,[No]
    ,MachineTypeID
    ,MasterPlusGroup
    ,Attachment
    ,SewingMachineAttachmentID
    ,Template
    編碼 ProcessSeq 從 1 開始
    排序用 ForProcessSeq

    但以上欄位(GK) + 欄位OneShot 有多筆一樣時, 這幾筆的 ProcessSeq 會編碼一樣的數字

    舉例,排序後資料
    第1筆 (GK):A, OneShot:0, ProcessSeq:1
    第2筆 (GK):A, OneShot:1, ProcessSeq:2
    第3筆 (GK):A, OneShot:0, ProcessSeq:3
    第4筆 (GK):A, OneShot:1, ProcessSeq:2(因為與第2筆(GK)+OneShot=1一樣,編碼相同為2)
    第5筆 (GK):A, OneShot:0, ProcessSeq:4

    且只有 oneShot = 1 時會出現合併資料情況
    
    同時處理
    ProcessCode
    SewerDiffPercentage
    */
    --先把 OneShot = 1 的去重複
    --組合
    
    SELECT ID, TableName, FactoryID, SewingLineID, SampleGroup, StyleID, SeasonID, ComboType, LMMachineConfirmDate
           ,MachineID, [No], MachineTypeID, MasterPlusGroup, Attachment, SewingMachineAttachmentID, Template, ForProcessSeq
          ,SewerDiffPercentage = IIF(SewerDiffPercentage2 = 0, 0, SewerDiffPercentage1 / SewerDiffPercentage2)
          ,[ProcessCode] = STUFF((SELECT CONCAT('_', [ProcessCode_Before]) FROM #tmpbyKeyOnly1Header t WHERE t.GroupForProcessOneShot1 = g.GroupForProcessOneShot1 ORDER BY [ProcessCode_Before] FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')
                         +(SELECT CONCAT('_', OperationID) FROM #tmpbyKeyOnly1Header t WHERE t.GroupForProcessOneShot1 = g.GroupForProcessOneShot1 ORDER BY [ProcessCode_Before] FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)')
    INTO #tmpProcessSeq1
    FROM(
        SELECT ID, TableName, FactoryID, SewingLineID, SampleGroup, StyleID, SeasonID, ComboType, LMMachineConfirmDate--表頭欄位
               ,MachineID, [No], MachineTypeID, MasterPlusGroup, Attachment, SewingMachineAttachmentID, Template--表身要聚合的欄位
               ,GroupForProcessOneShot1
               ,ForProcessSeq = MIN(ForProcessSeq)
               ,SewerDiffPercentage1 = SUM(GSD * SewerDiffPercentage)
               ,SewerDiffPercentage2 = SUM(GSD)        
        FROM #tmpbyKeyOnly1Header g
        WHERE OneShot = 1
        GROUP BY ID, TableName, FactoryID, SewingLineID, SampleGroup, StyleID, SeasonID, ComboType, LMMachineConfirmDate
                ,MachineID, [No], MachineTypeID, MasterPlusGroup, Attachment, SewingMachineAttachmentID, Template
                ,GroupForProcessOneShot1
    ) g

    UNION ALL
    SELECT ID, TableName, FactoryID, SewingLineID, SampleGroup, StyleID, SeasonID, ComboType, LMMachineConfirmDate
           ,MachineID, [No], MachineTypeID, MasterPlusGroup, Attachment, SewingMachineAttachmentID, Template, ForProcessSeq
           ,SewerDiffPercentage = IIF(GSD = 0, 0, GSD * SewerDiffPercentage / GSD)
           ,[ProcessCode] = [ProcessCode_Before] + CONCAT('_', OperationID)
    FROM #tmpbyKeyOnly1Header
    WHERE ISNULL(OneShot, 0) = 0

    --編碼 ProcessSeq
    SELECT *
        ,ProcessSeq =
            ROW_NUMBER() OVER (
                PARTITION BY ID, TableName, MachineID, [No], MachineTypeID, MasterPlusGroup, Attachment, SewingMachineAttachmentID, Template
                ORDER BY ForProcessSeq
            )
    INTO #tmpProcessSeq
    FROM #tmpProcessSeq1

    --編碼 MachineSeq 註: No排序後, 連續相同 MachineID 編碼相同 MachineSeq
    ;WITH CTE AS (
        SELECT *
            ,PrevMachineID = LAG(MachineID) OVER (PARTITION BY TableName, ID ORDER BY No)
        FROM #tmpProcessSeq
    )
    ,CTE2 AS (
        SELECT *
            ,ChangeFlag = CASE WHEN MachineID = PrevMachineID THEN 0 ELSE 1 END
        FROM CTE
    )
    ,CTE3 AS (
        SELECT *
            ,MachineSeq = SUM(ChangeFlag) OVER (PARTITION BY TableName, ID ORDER BY No ROWS UNBOUNDED PRECEDING)
        FROM CTE2
    )
    SELECT *
    INTO #tmpFINAL
    FROM CTE3
    WHERE MachineSeq IS NOT NULL

    --比較並準備資訊
    SELECT *
    INTO #tmpExistsbyHeader
    FROM PMSHistory.dbo.Juki_T_Layout j
    WHERE j.Flag <> 3
    AND EXISTS (
            SELECT 1 FROM #tmpFINAL
            WHERE SampleGroup = j.SampleGroup
            AND StyleID = j.StyleID
            AND ComboType = j.ComboType
            AND SeasonID = j.SeasonID
            AND FactoryID = j.FactoryID
            AND SewingLineID = j.SewingLineID)

    --準備 Flag = 3 (Juki 需要的 Delete) 資訊, 重複資訊, by Key 取最大時間
    SELECT t.*
    INTO #tmpFlag3byKey
    FROM(
        SELECT FactoryID, SewingLineID, SampleGroup, StyleID, ComboType, SeasonID, AddDate = MAX(AddDate) -- P03 + P06 可能會有相同組合, 取最大 Confirm 時間
        FROM #tmpExistsbyHeader
        GROUP BY FactoryID, SewingLineID, SampleGroup, StyleID, ComboType, SeasonID
    )m
    INNER JOIN #tmpExistsbyHeader t
           ON t.SampleGroup = m.SampleGroup
          AND t.StyleID = m.StyleID
          AND t.ComboType = m.ComboType
          AND t.SeasonID = m.SeasonID
          AND t.FactoryID = m.FactoryID
          AND t.SewingLineID = m.SewingLineID
          AND t.AddDate = m.AddDate

    BEGIN TRANSACTION--開啟交易
    BEGIN TRY
        INSERT INTO PMSHistory.dbo.Juki_T_Layout
                   (TableName
                   ,lmID
                   ,FactoryID
                   ,SewingLineID
                   ,SampleGroup
                   ,StyleID
                   ,SeasonID
                   ,ComboType
                   ,MachineSeq
                   ,MachineID
                   ,ProcessSeq
                   ,ProcessCode
                   ,SewerDiffPercentage
                   ,Flag
                   ,AddDate)
        SELECT
            TableName
            ,lmID
            ,FactoryID
            ,SewingLineID
            ,SampleGroup
            ,StyleID
            ,SeasonID
            ,ComboType
            ,MachineSeq
            ,MachineID
            ,ProcessSeq
            ,ProcessCode
            ,SewerDiffPercentage
            ,Flag = 3
            ,DATEADD(ms, -6, GETDATE())
        FROM #tmpFlag3byKey t

        INSERT INTO PMSHistory.dbo.Juki_T_Layout
                   (TableName
                   ,lmID
                   ,FactoryID
                   ,SewingLineID
                   ,SampleGroup
                   ,StyleID
                   ,SeasonID
                   ,ComboType
                   ,MachineSeq
                   ,MachineID
                   ,ProcessSeq
                   ,ProcessCode
                   ,SewerDiffPercentage
                   ,Flag
                   ,AddDate)
        SELECT
            TableName
            ,ID
            ,FactoryID
            ,SewingLineID
            ,SampleGroup
            ,StyleID
            ,SeasonID
            ,ComboType
            ,MachineSeq
            ,MachineID
            ,ProcessSeq
            ,ProcessCode
            ,SewerDiffPercentage
            ,Flag = 1
            ,GETDATE()
        FROM #tmpFINAL t

        --更新 Production 時間
        UPDATE Production.dbo.LineMapping
        SET JukiLayoutDataSubmitDate = GETDATE() 
        WHERE EXISTS (SELECT 1 FROM #tmp WHERE ID = LineMapping.ID AND TableName = 'LineMapping')

        UPDATE Production.dbo.LineMappingBalancing
        SET JukiLayoutDataSubmitDate = GETDATE() 
        WHERE EXISTS (SELECT 1 FROM #tmp WHERE ID = LineMappingBalancing.ID AND TableName = 'LineMappingBalancing')
            
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
    
    DROP TABLE #tmp, #tmpGroup, #tmpbyKeyOnly1Header, #tmpFINAL, #tmpExistsbyHeader, #tmpFlag3byKey, #tmpProcessSeq1, #tmpProcessSeq

END
GO
