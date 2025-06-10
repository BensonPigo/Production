CREATE PROCEDURE Juki_P2H_Style
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Production.dbo.System WHERE JukiExchangeDBActive = 0) RETURN;

    DECLARE @MaxDate DATETIME = (SELECT MAX(AddDate) FROM PMSHistory.dbo.Juki_T_Style)
    
    --撈出要更新資訊
    SELECT
            TableName = 'LineMapping'
            ,lm.ID
            ,lm.EditDate
            ,lm.SeasonID
            ,lm.StyleID
            ,lm.ComboType
            ,b.SampleGroup
            ,s.StyleName
            ,ProductType = r.Name
            ,Construction = dd.Name
    INTO #tmp
    FROM Production.dbo.LineMapping lm WITH(NOLOCK)
    INNER JOIN Production.dbo.Brand b ON lm.BrandID = b.ID
    INNER JOIN Production.dbo.Style s ON s.Ukey = lm.StyleUKey
    LEFT JOIN Production.dbo.Reason r ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type'
    LEFT JOIN Production.dbo.DropDownList dd ON dd.ID = s.Construction AND dd.Type = 'StyleConstruction'
    WHERE lm.JukiIoTDataExchange = 1
    AND lm.Status = 'Confirmed'
    AND (@MaxDate IS NULL OR lm.EditDate > @MaxDate OR s.JukiStyleDataLastEditDate > @MaxDate)    

    UNION ALL
    SELECT
            TableName = 'LineMappingBalancing'
            ,lm.ID
            ,lm.EditDate
            ,lm.SeasonID
            ,lm.StyleID
            ,lm.ComboType
            ,b.SampleGroup
            ,s.StyleName
            ,ProductType = r.Name
            ,Construction = dd.Name
    FROM Production.dbo.LineMappingBalancing lm WITH(NOLOCK)
    INNER JOIN Production.dbo.Brand b ON lm.BrandID = b.ID
    INNER JOIN Production.dbo.Style s ON s.Ukey = lm.StyleUKey
    LEFT JOIN Production.dbo.Reason r ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type'
    LEFT JOIN Production.dbo.DropDownList dd ON dd.ID = s.Construction AND dd.Type = 'StyleConstruction'
    WHERE lm.JukiIoTDataExchange = 1
    AND lm.Status = 'Confirmed'
    AND (@MaxDate IS NULL OR lm.EditDate > @MaxDate OR s.JukiStyleDataLastEditDate > @MaxDate)

    --依據 Juki 需要的 Key 挑出最新的那筆
    SELECT t.*
    INTO #tmpbyKeyOnly1Row
    FROM(
        SELECT SampleGroup, StyleID, ComboType, EditDate = MAX(EditDate)
        FROM #tmp
        GROUP BY SampleGroup, StyleID, ComboType--Juki 需要的 Key
    )m
    OUTER APPLY(
        SELECT TOP 1 *
        FROM #tmp t
        WHERE t.SampleGroup = m.SampleGroup
          AND t.StyleID = m.StyleID
          AND t.ComboType = m.ComboType
          AND t.EditDate = m.EditDate
    ) t

    SELECT t.*
    INTO #tmpExistsLast
    FROM(
        SELECT s.SampleGroup, s.StyleID, s.ComboType, AddDate = MAX(s.AddDate)
        FROM #tmpbyKeyOnly1Row t
        INNER JOIN PMSHistory.dbo.Juki_T_Style s ON s.SampleGroup = t.SampleGroup AND s.StyleID = t.StyleID AND s.ComboType = t.ComboType
        GROUP BY s.SampleGroup, s.StyleID, s.ComboType--Juki 需要的 Key
    )m
    OUTER APPLY(
        SELECT TOP 1 *
        FROM PMSHistory.dbo.Juki_T_Style t
        WHERE t.SampleGroup = m.SampleGroup
          AND t.StyleID = m.StyleID
          AND t.ComboType = m.ComboType
          AND t.AddDate = m.AddDate
    ) t

    SELECT t.*
    INTO #tmpFlag2
    FROM #tmpbyKeyOnly1Row t
    INNER JOIN #tmpExistsLast s ON s.SampleGroup = t.SampleGroup AND s.StyleID = t.StyleID AND s.ComboType = t.ComboType
    WHERE s.Flag <> 3
    AND (t.StyleName <> s.StyleName
        OR t.SeasonID <> s.SeasonID
        OR t.ProductType <> s.ProductType
        OR t.Construction <> s.Construction)

    SELECT t.*
    INTO #tmpFlag1
    FROM #tmpbyKeyOnly1Row t
    WHERE NOT EXISTS (SELECT 1 FROM PMSHistory.dbo.Juki_T_Style WHERE SampleGroup = t.SampleGroup AND StyleID = t.StyleID AND ComboType = t.ComboType)
    OR EXISTS (SELECT 1 FROM #tmpExistsLast WHERE SampleGroup = t.SampleGroup AND StyleID = t.StyleID AND ComboType = t.ComboType AND Flag = 3)

    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO PMSHistory.dbo.Juki_T_Style (
            TableName
            ,lmID
            ,SampleGroup
            ,StyleID
            ,StyleName
            ,SeasonID
            ,ComboType
            ,ProductType
            ,Construction
            ,Flag
            ,AddDate
        )
        SELECT
            TableName
            ,ID
            ,ISNULL(SampleGroup, '')
            ,ISNULL(StyleID, '')
            ,ISNULL(StyleName, '')
            ,ISNULL(SeasonID, '')
            ,ISNULL(ComboType, '')
            ,ISNULL(ProductType, '')
            ,ISNULL(Construction, '')
            ,Flag = 1
            ,AddDate = GETDATE()
        FROM #tmpFlag1 t

        INSERT INTO PMSHistory.dbo.Juki_T_Style (
            TableName
            ,lmID
            ,SampleGroup
            ,StyleID
            ,StyleName
            ,SeasonID
            ,ComboType
            ,ProductType
            ,Construction
            ,Flag
            ,AddDate
        )
        SELECT
            TableName
            ,ID
            ,ISNULL(SampleGroup, '')
            ,ISNULL(StyleID, '')
            ,ISNULL(StyleName, '')
            ,ISNULL(SeasonID, '')
            ,ISNULL(ComboType, '')
            ,ISNULL(ProductType, '')
            ,ISNULL(Construction, '')
            ,Flag = 2
            ,AddDate = GETDATE()
        FROM #tmpFlag2 t
                
        --#tmp 是這次有更新的所有資訊
        UPDATE Production.dbo.LineMapping
        SET JukiStyleDataSubmitDate = GETDATE()
        WHERE EXISTS (SELECT 1 FROM #tmp WHERE ID = LineMapping.ID AND TableName = 'LineMapping')

        UPDATE Production.dbo.LineMappingBalancing
        SET JukiStyleDataSubmitDate = GETDATE()
        WHERE EXISTS (SELECT 1 FROM #tmp WHERE ID = LineMappingBalancing.ID AND TableName = 'LineMappingBalancing')

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    DROP TABLE #tmp, #tmpbyKeyOnly1Row
END