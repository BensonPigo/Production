CREATE TABLE [dbo].[NewSentReport] (
    [ExportID]             VARCHAR (13)    CONSTRAINT [DF_NewSentReport_ExportID] DEFAULT ('') NOT NULL,
    [PoID]                 VARCHAR (13)    CONSTRAINT [DF_NewSentReport_PoID] DEFAULT ('') NOT NULL,
    [Seq1]                 VARCHAR (3)     CONSTRAINT [DF_NewSentReport_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]                 VARCHAR (2)     CONSTRAINT [DF_NewSentReport_Seq2] DEFAULT ('') NOT NULL,
    [DocumentName]         VARCHAR (100)   CONSTRAINT [DF_NewSentReport_DocumentName] DEFAULT ('') NOT NULL,
    [BrandID]              VARCHAR (8)     CONSTRAINT [DF_NewSentReport_BrandID] DEFAULT ('') NOT NULL,
    [ReportDate]           DATE            NULL,
    [FTYReceivedReport]    DATE            NULL,
    [AWBno]                VARCHAR (30)    CONSTRAINT [DF_NewSentReport_AWBno] DEFAULT ('') NOT NULL,
    [T2InspYds]            DECIMAL (10, 2) CONSTRAINT [DF_NewSentReport_T2InspYds] DEFAULT ((0)) NOT NULL,
    [T2DefectPoint]        DECIMAL (5)     CONSTRAINT [DF_NewSentReport_T2DefectPoint] DEFAULT ((0)) NOT NULL,
    [T2Grade]              VARCHAR (1)     CONSTRAINT [DF_NewSentReport_T2Grade] DEFAULT ('') NOT NULL,
    [TestReportTestDate]   DATE            NULL,
    [Ukey]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [AddDate]              DATETIME        NULL,
    [AddName]              VARCHAR (10)    CONSTRAINT [DF_NewSentReport_AddName] DEFAULT ('') NOT NULL,
    [EditDate]             DATETIME        NULL,
    [Editname]             VARCHAR (10)    CONSTRAINT [DF_NewSentReport_Editname] DEFAULT ('') NOT NULL,
    [TestReportCheckClima] BIT             DEFAULT ((0)) NOT NULL,
    [UniqueKey]            VARCHAR (200)   DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_NewSentReport] PRIMARY KEY CLUSTERED ([DocumentName] ASC, [ExportID] ASC, [PoID] ASC, [Seq1] ASC, [Seq2] ASC, [BrandID] ASC)
);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WK No',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'ExportID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'PoID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'測試報告文件名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'DocumentName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'上傳測試報告日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'ReportDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠接收測試報告的日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'FTYReceivedReport'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'主料檢驗的尺碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'T2InspYds'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'主料瑕疵點數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'T2DefectPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'主料的分級',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'T2Grade'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'測試報告的檢驗日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'TestReportTestDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UKEY',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NewSentReport',
    @level2type = N'COLUMN',
    @level2name = N'Editname'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將Pkey分別依序用_組成文字串寫入當作檔案識別唯一值,用來跟GASAClip串接條件避免跟Trade key重複 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NewSentReport', @level2type = N'COLUMN', @level2name = N'UniqueKey';

