CREATE TABLE [dbo].[ExportRefnoSentReport] (
    [ExportID]          VARCHAR (13)   CONSTRAINT [DF_ExportRefnoSentReport_ExportID] DEFAULT ('') NOT NULL,
    [BrandRefno]        VARCHAR (50)   CONSTRAINT [DF_ExportRefnoSentReport_BrandRefno] DEFAULT ('') NOT NULL,
    [ColorID]           VARCHAR (13)   CONSTRAINT [DF_ExportRefnoSentReport_ColorID] DEFAULT ('') NOT NULL,
    [DocumentName]      VARCHAR (100)  CONSTRAINT [DF_ExportRefnoSentReport_DocumentName] DEFAULT ('') NOT NULL,
    [BrandID]           VARCHAR (8)    CONSTRAINT [DF_ExportRefnoSentReport_BrandID] DEFAULT ('') NOT NULL,
    [ReportDate]        DATE           NULL,
    [FTYReceivedReport] DATE           NULL,
    [AWBno]             VARCHAR (30)   CONSTRAINT [DF_ExportRefnoSentReport_AWBno] DEFAULT ('') NOT NULL,
    [Ukey]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [AddDate]           DATETIME       NULL,
    [AddName]           VARCHAR (10)   CONSTRAINT [DF_ExportRefnoSentReport_AddName] DEFAULT ('') NOT NULL,
    [EditDate]          DATETIME       NULL,
    [Editname]          VARCHAR (10)   CONSTRAINT [DF_ExportRefnoSentReport_Editname] DEFAULT ('') NOT NULL,
    [UniqueKey]         NVARCHAR (200) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ExportRefnoSentReport] PRIMARY KEY CLUSTERED ([ExportID] ASC, [BrandRefno] ASC, [ColorID] ASC, [DocumentName] ASC, [BrandID] ASC)
);



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
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將Pkey分別依序用_組成文字串寫入當作檔案識別唯一值,用來跟GASAClip串接條件避免跟Trade key重複 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExportRefnoSentReport', @level2type = N'COLUMN', @level2name = N'UniqueKey';

