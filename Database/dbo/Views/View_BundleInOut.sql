CREATE view [dbo].[View_BundleInOut]
as 

SELECT [BundleNo], [SubProcessId], [InComing], [OutGoing], [AddDate], [EditDate]
	, [SewingLineID], [LocationID], [RFIDProcessLocationID], [PanelNo], [CutCellID]
FROM BundleInOut WITH(NOLOCK)

union ALL

SELECT [BundleNo], [SubProcessId], [InComing], [OutGoing], [AddDate], [EditDate]
	, [SewingLineID], [LocationID], [RFIDProcessLocationID], [PanelNo], [CutCellID]
FROM BundleInOut_History WITH(NOLOCK)

GO