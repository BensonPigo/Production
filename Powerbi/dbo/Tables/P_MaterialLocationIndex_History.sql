CREATE TABLE [dbo].[P_MaterialLocationIndex_History]
(
	[HistoryUkey] INT NOT NULL IDENTITY(1,1), 
    [ID] VARCHAR(20) NOT NULL DEFAULT (''), 
    [StockType] VARCHAR(10) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_MaterialLocationIndex_History] PRIMARY KEY ([HistoryUkey]) 
)

GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MaterialLocationIndex_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'儲位ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MaterialLocationIndex_History',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MaterialLocationIndex_History',
    @level2type = N'COLUMN',
    @level2name = N'StockType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MaterialLocationIndex_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'