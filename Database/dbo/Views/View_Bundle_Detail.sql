CREATE view [dbo].[View_Bundle_Detail]
as 

SELECT [BundleNo], [Id], [BundleGroup], [Patterncode], [PatternDesc], [SizeCode], [Qty], [Parts]
	, [Farmin], [FarmOut], [PrintDate], [IsPair], [Location], [RFUID], [Tone], [RFPrintDate], [PrintGroup], [RFIDScan]
FROM Bundle_Detail WITH(NOLOCK)

union ALL

SELECT [BundleNo], [Id], [BundleGroup], [Patterncode], [PatternDesc], [SizeCode], [Qty], [Parts]
	, [Farmin], [FarmOut], [PrintDate], [IsPair], [Location], [RFUID], [Tone], [RFPrintDate], [PrintGroup], [RFIDScan]
FROM Bundle_Detail_History WITH(NOLOCK)

GO