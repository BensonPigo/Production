CREATE PROCEDURE Juki_P2H_ProdPlan
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Production.dbo.System WHERE JukiExchangeDBActive = 0) RETURN;
    
    DECLARE @MaxDate DATETIME = (SELECT MAX(AddDate) FROM PMSHistory.dbo.Juki_T_ProdPlan)
    
    --撈出要更新資訊
    SELECT
            TableName = 'LineMapping'
            ,lm.ID
            ,lm.EditDate
            ,lm.FactoryID
            ,lm.SewingLineID
            ,lm.StyleID
            ,lm.SeasonID
            ,lm.ComboType
            ,b.SampleGroup
    INTO #tmp
    FROM Production.dbo.LineMapping lm WITH(NOLOCK)
    INNER JOIN Production.dbo.Brand b ON lm.BrandID = b.ID
    WHERE lm.JukiIoTDataExchange = 1
    AND lm.Status = 'Confirmed'
    AND (@MaxDate IS NULL OR lm.EditDate > @MaxDate)

    UNION ALL
    SELECT
            TableName = 'LineMappingBalancing'
            ,lm.ID
            ,lm.EditDate
            ,lm.FactoryID
            ,lm.SewingLineID
            ,lm.StyleID
            ,lm.SeasonID
            ,lm.ComboType
            ,b.SampleGroup
    FROM Production.dbo.LineMappingBalancing lm WITH(NOLOCK)
    INNER JOIN Production.dbo.Brand b ON lm.BrandID = b.ID
    WHERE lm.JukiIoTDataExchange = 1
    AND lm.Status = 'Confirmed'
    AND (@MaxDate IS NULL OR lm.EditDate > @MaxDate)
    
    --依據 Juki 需要的 Key 挑出最新的那筆
    SELECT t.*
    INTO #tmpbyKeyOnly1Row
    FROM(
        SELECT SampleGroup, StyleID, ComboType, SeasonID, SewingLineID, EditDate = MAX(EditDate)
        FROM #tmp
        GROUP BY SampleGroup, StyleID, ComboType, SeasonID, SewingLineID--Juki 需要的 Key
    )m
    OUTER APPLY(
        SELECT TOP 1 *
        FROM #tmp t
        WHERE t.SampleGroup = m.SampleGroup
          AND t.StyleID = m.StyleID
          AND t.ComboType = m.ComboType
          AND t.SeasonID = m.SeasonID
          AND t.SewingLineID = m.SewingLineID
          AND t.EditDate = m.EditDate
    ) t
    
    SELECT t.*
    INTO #tmpExistsLast
    FROM(
        SELECT s.SampleGroup, s.StyleID, s.ComboType, s.SeasonID, s.SewingLineID, AddDate = MAX(s.AddDate)
        FROM #tmpbyKeyOnly1Row t
        INNER JOIN PMSHistory.dbo.Juki_T_ProdPlan s
            ON t.SampleGroup = s.SampleGroup
           AND t.StyleID = s.StyleID
           AND t.ComboType = s.ComboType
           AND t.SeasonID = s.SeasonID
           AND t.SewingLineID = s.SewingLineID
        GROUP BY s.SampleGroup, s.StyleID, s.ComboType, s.SeasonID, s.SewingLineID--Juki 需要的 Key
    )m
    OUTER APPLY(
        SELECT TOP 1 *
        FROM PMSHistory.dbo.Juki_T_ProdPlan t
        WHERE t.SampleGroup = m.SampleGroup
          AND t.StyleID = m.StyleID
          AND t.ComboType = m.ComboType
          AND t.SeasonID = m.SeasonID
          AND t.SewingLineID = m.SewingLineID
          AND t.AddDate = m.AddDate
    ) t
    
    SELECT t.*
    INTO #tmpFlag1
    FROM #tmpbyKeyOnly1Row t
    WHERE NOT EXISTS (SELECT 1 FROM PMSHistory.dbo.Juki_T_ProdPlan WHERE SampleGroup = t.SampleGroup AND StyleID = t.StyleID AND ComboType = t.ComboType AND SeasonID = t.SeasonID AND SewingLineID = t.SewingLineID)
    OR EXISTS (SELECT 1 FROM #tmpExistsLast WHERE SampleGroup = t.SampleGroup AND StyleID = t.StyleID AND ComboType = t.ComboType AND SeasonID = t.SeasonID AND SewingLineID = t.SewingLineID AND Flag = 3)

    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO PMSHistory.dbo.Juki_T_ProdPlan (
            TableName
            ,lmID
            ,[FactoryID]
            ,[SewingLineID]
            ,[SampleGroup]
            ,[StyleID]
            ,[SeasonID]
            ,[ComboType]
            ,[Flag]
            ,[AddDate])
        SELECT
            TableName
            ,ID
            ,[FactoryID]
            ,[SewingLineID]
            ,[SampleGroup]
            ,[StyleID]
            ,[SeasonID]
            ,[ComboType]
            ,1
            ,GETDATE()
        FROM #tmpFlag1 t
        
        --#tmp 是這次有更新的所有資訊
        UPDATE Production.dbo.LineMapping
        SET JukiProdPlanDataSubmitDate = GETDATE()
        WHERE EXISTS (SELECT 1 FROM #tmp WHERE ID = LineMapping.ID AND TableName = 'LineMapping')

        UPDATE Production.dbo.LineMappingBalancing
        SET JukiProdPlanDataSubmitDate = GETDATE()
        WHERE EXISTS (SELECT 1 FROM #tmp WHERE ID = LineMappingBalancing.ID AND TableName = 'LineMappingBalancing')
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
    
    DROP TABLE #tmp, #tmpbyKeyOnly1Row
END