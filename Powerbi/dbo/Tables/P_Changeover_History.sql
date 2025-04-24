CREATE TABLE [dbo].[P_Changeover_History]
(
	[Ukey] BIGINT NOT NULL IDENTITY(1,1), 
    [TransferDate] DATE NULL, 
    [FactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_Changeover_History] PRIMARY KEY ([Ukey])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Changeover_History',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'資料轉換當日的日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Changeover_History',
    @level2type = N'COLUMN',
    @level2name = N'TransferDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Changeover_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Changeover_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'