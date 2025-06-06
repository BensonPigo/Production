CREATE PROCEDURE Juki_P2H_BaseProcess
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Production.dbo.System WHERE JukiExchangeDBActive = 0) RETURN;
    
    DECLARE @MaxDate DATETIME = (SELECT MAX(AddDate) FROM PMSHistory.dbo.Juki_T_BaseProcess)
    
    SELECT
            --By ID 欄位
            TableName = 'LineMapping'
            ,lmd.ID
            ,lm.EditDate
            ,lm.SeasonID
            ,lm.StyleID
            ,lm.ComboType
            ,b.SampleGroup
            ,ProductType = r.Name
            ,Construction = dd.Name
            --表身展開欄位
            ,lmd.OperationID
            ,lmd.MachineID
            ,[No] = ISNULL(lmd.[No], '')
            ,MachineTypeID = ISNULL(lmd.MachineTypeID, '')
            ,lmd.MasterPlusGroup
            ,Attachment = ISNULL(lmd.Attachment, '')
            ,lmd.SewingMachineAttachmentID
            ,Template = ISNULL(lmd.Template, '')
            ,OneShot = ISNULL(lmd.OneShot, 0)
            ,[ProcessCode_Before] = dbo.RemoveLeadingTrailingZeros(OriNO)
            ,[ProcessName] = 
                CASE
                    WHEN f.CountryID <> 'KH' THEN lmd.Annotation
                    ELSE dbo.GetFirstPartBeforeSemicolon(lmd.Annotation)
                END        
            ,[TargetTime] = lmd.GSD
    INTO #tmp
    FROM Production.dbo.LineMapping lm WITH(NOLOCK)
    INNER JOIN Production.dbo.LineMapping_Detail lmd WITH(NOLOCK) ON lmd.ID = lm.ID
    INNER JOIN Production.dbo.Factory f WITH(NOLOCK) ON f.ID = lm.FactoryID
    INNER JOIN Production.dbo.Style s WITH(NOLOCK) ON s.Ukey = lm.StyleUKey
    INNER JOIN Production.dbo.Brand b WITH(NOLOCK) ON lm.BrandID = b.ID
    LEFT JOIN Production.dbo.Reason r WITH(NOLOCK) ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type'
    LEFT JOIN Production.dbo.DropDownList dd WITH(NOLOCK) ON dd.ID = s.Construction AND dd.Type = 'StyleConstruction'
    WHERE lm.JukiIoTDataExchange = 1
    AND lm.Status = 'Confirmed'
    AND (@MaxDate IS NULL OR lm.EditDate > @MaxDate)
    AND lmd.MachineCount = 1
    AND lmd.MachineTypeID <> ''
    AND lmd.MasterPlusGroup <>''
    AND lmd.No <> ''
    
    UNION ALL
    SELECT
            --By ID 欄位
            TableName = 'LineMappingBalancing'
            ,lm.ID
            ,lm.EditDate
            ,lm.SeasonID
            ,lm.StyleID
            ,lm.ComboType
            ,b.SampleGroup
            ,ProductType = r.Name
            ,Construction = dd.Name
            --表身展開欄位
            ,lmd.OperationID
            ,lmd.MachineID
            ,lmd.[No]
            ,lmd.MachineTypeID
            ,lmd.MasterPlusGroup
            ,lmd.Attachment
            ,lmd.SewingMachineAttachmentID
            ,lmd.Template
            ,OneShot = ISNULL(lmd.OneShot, 0)
            ,[ProcessCode_Before] = dbo.RemoveLeadingTrailingZeros(lmd.TimeStudySeq)
            ,[ProcessName] = 
                CASE
                    WHEN f.CountryID <> 'KH' THEN lmd.Annotation
                    ELSE dbo.GetFirstPartBeforeSemicolon(lmd.Annotation)
                END        
            ,[TargetTime] = lmd.GSD
    FROM Production.dbo.LineMappingBalancing lm WITH(NOLOCK)
    INNER JOIN Production.dbo.LineMappingBalancing_Detail lmd WITH(NOLOCK) ON lmd.ID = lm.ID
    INNER JOIN Production.dbo.Factory f WITH(NOLOCK) ON f.ID = lm.FactoryID
    INNER JOIN Production.dbo.Style s WITH(NOLOCK) ON s.Ukey = lm.StyleUKey
    INNER JOIN Production.dbo.Brand b WITH(NOLOCK) ON lm.BrandID = b.ID
    LEFT JOIN Production.dbo.Reason r WITH(NOLOCK) ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type'
    LEFT JOIN Production.dbo.DropDownList dd WITH(NOLOCK) ON dd.ID = s.Construction AND dd.Type = 'StyleConstruction'
    WHERE lm.JukiIoTDataExchange = 1
    AND lm.Status = 'Confirmed'
    AND (@MaxDate IS NULL OR lm.EditDate > @MaxDate)
    AND lmd.PPA <> 'C'
    AND lmd.IsNonSewingLine = 0
    AND lmd.MachineTypeID NOT LIKE 'MM%'
    AND lmd.No <> ''
        
    SELECT SampleGroup, StyleID, ComboType, SeasonID, EditDate = MAX(EditDate)
    INTO #tmpGroup
    FROM #tmp
    GROUP BY SampleGroup, StyleID, ComboType, SeasonID

    --by Key 挑出最新的那筆
    SELECT t.*
        ,GroupProcessCode =  DENSE_RANK() OVER(
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
    FROM #tmpGroup m
    OUTER APPLY(
        SELECT *
        FROM #tmp t
        WHERE t.SampleGroup = m.SampleGroup
          AND t.StyleID = m.StyleID
          AND t.ComboType = m.ComboType
          AND t.SeasonID = m.SeasonID
          AND t.EditDate = m.EditDate
    ) t
          
    SELECT
         TableName
        ,ID
        ,[SampleGroup] = ISNULL([SampleGroup], '')
        ,[StyleID] = ISNULL([StyleID], '')
        ,[SeasonID]= ISNULL([SeasonID], '')
        ,[ComboType] = ISNULL([ComboType], '')
        ,[ProductType] = ISNULL(ProductType, '')
        ,[Construction] = ISNULL(Construction, '')
        ,[ProcessCode] = STUFF((SELECT CONCAT('_', [ProcessCode_Before]) FROM #tmpbyKeyOnly1Header t WHERE t.GroupProcessCode = g.GroupProcessCode ORDER BY [ProcessCode_Before] FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')
                        + (SELECT CONCAT('_', OperationID) FROM #tmpbyKeyOnly1Header t WHERE t.GroupProcessCode = g.GroupProcessCode ORDER BY [ProcessCode_Before] FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)')
        ,[ProcessName] =  STUFF((SELECT CONCAT(N'_', [ProcessName]) FROM #tmpbyKeyOnly1Header t WHERE t.GroupProcessCode = G.GroupProcessCode ORDER BY [ProcessCode_Before] FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')
        ,TargetTime = (SELECT SUM(TargetTime) FROM #tmpbyKeyOnly1Header t WHERE t.GroupProcessCode = G.GroupProcessCode)
    INTO #tmpFINAL
    FROM (SELECT DISTINCT TableName, ID, SampleGroup, StyleID, ComboType, SeasonID, ProductType, Construction, GroupProcessCode FROM #tmpbyKeyOnly1Header )g
    
    SELECT t.*
    INTO #tmpExistsLast
    FROM(
        SELECT s.SampleGroup, s.StyleID, s.ComboType, s.SeasonID, s.ProcessCode, AddDate = MAX(s.AddDate)
        FROM #tmpFINAL t
        INNER JOIN PMSHistory.dbo.Juki_T_BaseProcess s ON s.SampleGroup = t.SampleGroup AND s.StyleID = t.StyleID AND s.ComboType = t.ComboType AND s.SeasonID = t.SeasonID AND s.ProcessCode = t.ProcessCode
        GROUP BY s.SampleGroup, s.StyleID, s.ComboType, s.SeasonID, s.ProcessCode--Juki 需要的 Key
    )m
    OUTER APPLY(
        SELECT TOP 1 *
        FROM PMSHistory.dbo.Juki_T_BaseProcess t
        WHERE t.SampleGroup = m.SampleGroup
          AND t.StyleID = m.StyleID
          AND t.ComboType = m.ComboType
          AND t.SeasonID = m.SeasonID
          AND t.ProcessCode = m.ProcessCode
          AND t.AddDate = m.AddDate
    ) t
    
    SELECT t.*
    INTO #tmpFlag2
    FROM #tmpFINAL t
    INNER JOIN #tmpExistsLast s ON s.SampleGroup = t.SampleGroup AND s.StyleID = t.StyleID AND s.ComboType = t.ComboType AND s.SeasonID = t.SeasonID AND s.ProcessCode = t.ProcessCode
    WHERE s.Flag <> 3
    AND (t.ProductType <> s.ProductType
        OR t.Construction <> s.Construction
        OR t.ProcessName <> s.ProcessName
        OR t.TargetTime <> s.TargetTime)
        
    SELECT t.*
    INTO #tmpFlag1
    FROM #tmpFINAL t
    WHERE NOT EXISTS (SELECT 1 FROM PMSHistory.dbo.Juki_T_BaseProcess WHERE SampleGroup = t.SampleGroup AND StyleID = t.StyleID AND ComboType = t.ComboType AND SeasonID = t.SeasonID AND ProcessCode = t.ProcessCode)
    OR EXISTS (SELECT 1 FROM #tmpExistsLast WHERE SampleGroup = t.SampleGroup AND StyleID = t.StyleID AND ComboType = t.ComboType AND SeasonID = t.SeasonID AND ProcessCode = t.ProcessCode AND Flag = 3)

    BEGIN TRANSACTION
    BEGIN TRY
        INSERT INTO PMSHistory.dbo.Juki_T_BaseProcess
            (TableName,lmID,SampleGroup,StyleID,SeasonID,ComboType,ProductType,Construction,ProcessCode,ProcessName,TargetTime,Flag,AddDate)
        SELECT
            TableName,ID,SampleGroup,StyleID,SeasonID,ComboType
            ,ISNULL(ProductType, '')
            ,ISNULL(Construction, '')
            ,ISNULL(ProcessCode, '')
            ,ISNULL(ProcessName, '')
            ,TargetTime
            ,Flag = 1
            ,AddDate = GETDATE()
        FROM #tmpFlag1 t
    
        INSERT INTO PMSHistory.dbo.Juki_T_BaseProcess
            (TableName,lmID,SampleGroup,StyleID,SeasonID,ComboType,ProductType,Construction,ProcessCode,ProcessName,TargetTime,Flag,AddDate)
        SELECT
            TableName,ID,SampleGroup,StyleID,SeasonID,ComboType
            ,ISNULL(ProductType, '')
            ,ISNULL(Construction, '')
            ,ISNULL(ProcessCode, '')
            ,ISNULL(ProcessName, '')
            ,TargetTime
            ,Flag = 2
            ,AddDate = GETDATE()
        FROM #tmpFlag2 t

        UPDATE Production.dbo.LineMapping_Detail
        SET JukiBaseProcessDataSubmitDate = GETDATE() 
        WHERE EXISTS (SELECT 1 FROM #tmp WHERE ID = LineMapping_Detail.ID AND TableName = 'LineMapping')
        AND MachineCount = 1
        AND MachineTypeID <> ''
        AND MasterPlusGroup <>''
        AND No <> ''
        AND JukiBaseProcessDataSubmitDate IS NULL

        UPDATE Production.dbo.LineMappingBalancing_Detail
        SET JukiBaseProcessDataSubmitDate = GETDATE() 
        WHERE EXISTS (SELECT 1 FROM #tmp WHERE ID = LineMappingBalancing_Detail.ID AND TableName = 'LineMappingBalancing')
        AND PPA <> 'C'
        AND IsNonSewingLine = 0
        AND MachineTypeID NOT LIKE 'MM%'
        AND No <> ''
        AND JukiBaseProcessDataSubmitDate IS NULL
                
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
    
    DROP TABLE #tmp, #tmpbyKeyOnly1Header, #tmpFINAL
END