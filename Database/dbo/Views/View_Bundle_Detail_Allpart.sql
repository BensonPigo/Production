CREATE view [dbo].[View_Bundle_Detail_Allpart]
as 

SELECT [ID], [Patterncode], [PatternDesc], [parts], [Ukey], [IsPair], [Location]
FROM Bundle_Detail_Allpart WITH(NOLOCK)

union ALL

SELECT [ID], [Patterncode], [PatternDesc], [parts], [Ukey], [IsPair], [Location]
FROM Bundle_Detail_Allpart_History WITH(NOLOCK)

GO
