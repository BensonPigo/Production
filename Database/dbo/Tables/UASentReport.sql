
	CREATE TABLE [dbo].[UASentReport] (
    [BrandRefno]         VARCHAR (50)   CONSTRAINT [DF_UASentReport_BrandRefno] DEFAULT ('') NOT NULL,
    [ColorID]            VARCHAR (6)    CONSTRAINT [DF_UASentReport_ColorID] DEFAULT ('') NOT NULL,
    [SuppID]             VARCHAR (6)    CONSTRAINT [DF_UASentReport_SuppID] DEFAULT ('') NOT NULL,
    [DocumentName]       VARCHAR (100)  CONSTRAINT [DF_UASentReport_DocumentName] DEFAULT ('') NOT NULL,
    [BrandID]            VARCHAR (8)    CONSTRAINT [DF_UASentReport_BrandID] DEFAULT ('') NOT NULL,
    [TestSeasonID]       VARCHAR (8)    CONSTRAINT [DF_UASentReport_TestSeasonID] DEFAULT ('') NOT NULL,
    [DueSeason]          VARCHAR (8)    CONSTRAINT [DF_UASentReport_DueSeason] DEFAULT ('') NOT NULL,
    [DueDate]            DATE           NULL,
    [Ukey]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [TestReport]         DATE           NULL,
    [FTYReceivedReport]  DATE           NULL,
    [TestReportTestDate] DATE           NULL,
    [AddDate]            DATETIME       NULL,
    [AddName]            VARCHAR (10)   CONSTRAINT [DF_UASentReport_AddName] DEFAULT ('') NOT NULL,
    [EditDate]           DATETIME       NULL,
    [Editname]           VARCHAR (10)   CONSTRAINT [DF_UASentReport_Editname] DEFAULT ('') NOT NULL,
    [UniqueKey]          NVARCHAR (200) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_UASentReport] PRIMARY KEY CLUSTERED ([BrandRefno] ASC, [ColorID] ASC, [SuppID] ASC, [DocumentName] ASC, [BrandID] ASC)
);


	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'BrandRefno',
	@name = N'MS_Description',@value = N'主料';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'ColorID',
	@name = N'MS_Description',@value = N'顏色ID';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'SuppID',
	@name = N'MS_Description',@value = N'供應商ID';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'DocumentName',
	@name = N'MS_Description',@value = N'測試報告文件名稱';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'BrandID',
	@name = N'MS_Description',@value = N'品牌ID';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'TestSeasonID',
	@name = N'MS_Description',@value = N'測試報告季節';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'DueSeason',
	@name = N'MS_Description',@value = N'應季';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'DueDate',
	@name = N'MS_Description',@value = N'應期';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'Ukey',
	@name = N'MS_Description',@value = N'Ukey';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'TestReport',
	@name = N'MS_Description',@value = N'上傳測試報告日期';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'FTYReceivedReport',
	@name = N'MS_Description',@value = N'工廠接收測試報告的日期';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'TestReportTestDate',
	@name = N'MS_Description',@value = N'測試報告的檢驗日期';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'AddDate',
	@name = N'MS_Description',@value = N'新建日期';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'AddName',
	@name = N'MS_Description',@value = N'新建人員';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'EditDate',
	@name = N'MS_Description',@value = N'編輯日期';
	go
	EXECUTE sp_addextendedproperty
	@level0type = N'SCHEMA',@level0name = N'dbo', 
	@level1type = N'TABLE',@level1name = N'UASentReport',
	@level2type = N'COLUMN',@level2name = N'Editname',
	@name = N'MS_Description',@value = N'編輯人名';
	go
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將Pkey分別依序用_組成文字串寫入當作檔案識別唯一值,用來跟GASAClip串接條件避免跟Trade key重複 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UASentReport', @level2type = N'COLUMN', @level2name = N'UniqueKey';

