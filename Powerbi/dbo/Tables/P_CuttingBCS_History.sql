CREATE TABLE [dbo].[P_CuttingBCS_History]
(
	[HistoryUkey] BIGINT NOT NULL IDENTITY(1,1), 
    [OrderID] VARCHAR(13) CONSTRAINT [DF_P_CuttingBCS_History_OrderID]  NOT NULL DEFAULT (('')), 

    [SewingLineID] VARCHAR(5) CONSTRAINT [DF_P_CuttingBCS_History_SewingLineID] NOT NULL DEFAULT (('')), 
    [RequestDate] DATE NULL, 
    [BIFactoryID] VARCHAR(8) CONSTRAINT [DF_P_CuttingBCS_History_BIFactoryID] NOT NULL, 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_CuttingBCS_History] PRIMARY KEY ([HistoryUkey]), 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'子單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CuttingBCS_History',
    @level2type = N'COLUMN',
    @level2name = N'OrderID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產線號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CuttingBCS_History',
    @level2type = N'COLUMN',
    @level2name = N'SewingLineID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預計的SewingDate',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CuttingBCS_History',
    @level2type = N'COLUMN',
    @level2name = N'RequestDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CuttingBCS_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CuttingBCS_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'