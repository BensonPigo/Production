CREATE TABLE [dbo].[P_LineMapping_Detail_History]
(
	[HistoryUkey] bigint NOT NULL IDENTITY(1,1), 
    [Ukey] BIGINT NULL, 
    [IsFrom] VARCHAR(6) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_LineMapping_Detail_History] PRIMARY KEY ([HistoryUkey])


)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'資料來源為IE P03或IE P06',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'IsFrom'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'