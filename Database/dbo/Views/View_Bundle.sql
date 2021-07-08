CREATE view [dbo].[View_Bundle]
as 

SELECT [ID], [POID], [MDivisionid], [Sizecode], [Colorid], [Article], [PatternPanel], [Cutno], [Cdate]
	, [Orderid], [Sewinglineid], [Item], [SewingCell], [Ratio], [Startno], [Qty], [PrintDate], [AllPart], [CutRef]
	, [AddName], [AddDate], [EditName], [EditDate], [oldid], [FabricPanelCode], [IsEXCESS], [ByToneGenerate], [SubCutNo]
FROM Bundle WITH(NOLOCK)

union ALL

SELECT [ID], [POID], [MDivisionid], [Sizecode], [Colorid], [Article], [PatternPanel], [Cutno], [Cdate]
	, [Orderid], [Sewinglineid], [Item], [SewingCell], [Ratio], [Startno], [Qty], [PrintDate], [AllPart], [CutRef]
	, [AddName], [AddDate], [EditName], [EditDate], [oldid], [FabricPanelCode], [IsEXCESS], [ByToneGenerate], [SubCutNo]
FROM Bundle_History WITH(NOLOCK)

GO