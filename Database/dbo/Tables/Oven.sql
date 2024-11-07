CREATE TABLE [dbo].[Oven] (
    [ID]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [POID]        VARCHAR (13)   CONSTRAINT [DF_Oven_POID] DEFAULT ('') NOT NULL,
    [TestNo]      NUMERIC (2)    CONSTRAINT [DF_Oven_TestNo] DEFAULT ((0)) NOT NULL,
    [InspDate]    DATE           NOT NULL,
    [Article]     VARCHAR (8)    CONSTRAINT [DF_Oven_Article] DEFAULT ('') NOT NULL,
    [Result]      VARCHAR (15)   CONSTRAINT [DF_Oven_Result] DEFAULT ('') NOT NULL,
    [Status]      VARCHAR (15)   CONSTRAINT [DF_Oven_Status] DEFAULT ('') NULL,
    [Inspector]   VARCHAR (10)   CONSTRAINT [DF_Oven_Inspector] DEFAULT ('') NOT NULL,
    [Remark]      NVARCHAR (120) CONSTRAINT [DF_Oven_Remark] DEFAULT ('') NULL,
    [addName]     VARCHAR (10)   CONSTRAINT [DF_Oven_addName] DEFAULT ('') NULL,
    [addDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_Oven_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [Temperature] INT            DEFAULT ((0)) NOT NULL,
    [Time]        INT            DEFAULT ((0)) NOT NULL,
	ReportNo varchar(14) not null CONSTRAINT [DF_Oven_ReportNo] default '',
    [Approver] VARCHAR(10) NOT NULL  CONSTRAINT [DF_Oven_Approver] DEFAULT '', 
    [ReportDate] DATE NULL, 
    CONSTRAINT [PK_Oven] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Oven Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗順序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'TestNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'InspDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'addName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'addDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
CREATE NONCLUSTERED INDEX [Index_Oven_GetFirQaRecord]
    ON [dbo].[Oven]([POID] ASC);

GO

