CREATE TABLE [dbo].[P_Import_MtltoFTYAnalysis_History]
(
	[HistoryUkey] bigint NOT NULL  IDENTITY(1,1), 
    [OrderID] VARCHAR(13) NOT NULL  DEFAULT (''), 
    [Seq1] VARCHAR(3) NULL DEFAULT (''), 
    [Seq2] VARCHAR(2) NULL DEFAULT (''), 
    [WKID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] NCHAR(10) NULL, 
    CONSTRAINT [PK_P_Import_MtltoFTYAnalysis_History] PRIMARY KEY ([HistoryUkey])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Import_MtltoFTYAnalysis_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Import_MtltoFTYAnalysis_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Import_MtltoFTYAnalysis_History',
    @level2type = N'COLUMN',
    @level2name = N'OrderID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Import_MtltoFTYAnalysis_History',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Import_MtltoFTYAnalysis_History',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'