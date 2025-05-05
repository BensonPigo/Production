CREATE TABLE [dbo].[P_CartonStatusTrackingList_History]
(
	[Ukey] BIGINT NOT NULL PRIMARY KEY, 
    [FactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [SP] VARCHAR(13) NOT NULL DEFAULT (''), 
    [SeqNo] VARCHAR(2) NOT NULL DEFAULT (''), 
    [PackingListID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [CtnNo] VARCHAR(6) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Order No',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'SP'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Shipment Seq',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'SeqNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Packing List #',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'PackingListID'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Carton#',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Order Factory',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Carton#',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'CtnNo'