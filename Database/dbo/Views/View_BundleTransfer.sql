CREATE view [dbo].[View_BundleTransfer]
as 

SELECT [Sid], [RFIDReaderId], [Type], [SubProcessId], [TagId], [BundleNo], [TransferDate], [AddDate]
	, [LocationID], [RFIDProcessLocationID], [PanelNo], [CutCellID], [SewingLineID]
FROM BundleTransfer WITH(NOLOCK)

union ALL

SELECT [Sid], [RFIDReaderId], [Type], [SubProcessId], [TagId], [BundleNo], [TransferDate], [AddDate]
	, [LocationID], [RFIDProcessLocationID], [PanelNo], [CutCellID], [SewingLineID]
FROM BundleTransfer_History WITH(NOLOCK)

GO