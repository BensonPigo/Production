CREATE TABLE [dbo].[P_LoadingProductionOutput_History]
(
	[HistoryUkey] bigint NOT NULL IDENTITY(1,1), 
    [FactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [Ukey] BIGINT NULL, 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_LoadingProductionOutput_History] PRIMARY KEY ([HistoryUkey]), 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingProductionOutput_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingProductionOutput_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'