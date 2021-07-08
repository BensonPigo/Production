CREATE view [dbo].[View_Bundle_Detail_Art]
as 

SELECT [Bundleno], [SubprocessId], [PatternCode], [ID], [Ukey], [PostSewingSubProcess], [NoBundleCardAfterSubprocess]
FROM Bundle_Detail_Art WITH(NOLOCK)

union ALL

SELECT [Bundleno], [SubprocessId], [PatternCode], [ID], [Ukey], [PostSewingSubProcess], [NoBundleCardAfterSubprocess]
FROM Bundle_Detail_Art_History WITH(NOLOCK)

GO