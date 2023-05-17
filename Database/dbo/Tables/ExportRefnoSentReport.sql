CREATE TABLE [dbo].[ExportRefnoSentReport]
(
    [ExportID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [BrandRefno] VARCHAR(50) NOT NULL DEFAULT (''), 
    [ColorID] VARCHAR(3) NOT NULL DEFAULT (''), 
    [DocumentName] VARCHAR(100) NOT NULL DEFAULT (''), 
    [BrandID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [ReportDate] DATE NULL, 
    [FTYReceivedReport] DATE NULL, 
    [AWBno] VARCHAR(30) NULL DEFAULT (''), 
    [UKEY] BIGINT NOT NULL IDENTITY, 
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    [Editname] VARCHAR(10) NULL DEFAULT (''),     
    CONSTRAINT [PK_ExportRefnoSentReport] PRIMARY KEY ([ExportID], [BrandRefno], [ColorID], [DocumentName], [BrandID])
)

GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌料號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'BrandRefno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'ColorID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'測試報告文件名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'DocumentName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'上傳測試報告日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'ReportDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠接收測試報告的日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'FTYReceivedReport'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'AWBno',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'AWBno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UKEY',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'UKEY'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'Editname'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WK No',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ExportRefnoSentReport',
    @level2type = N'COLUMN',
    @level2name = N'ExportID'