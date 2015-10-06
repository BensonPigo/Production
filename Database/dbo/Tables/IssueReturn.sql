CREATE TABLE [dbo].[IssueReturn] (
    [Id]        VARCHAR (13)  CONSTRAINT [DF_IssueReturn_Id] DEFAULT ('') NOT NULL,
    [IssueDate] DATE          NULL,
    [FactoryId] VARCHAR (8)   CONSTRAINT [DF_IssueReturn_FactoryId] DEFAULT ('') NULL,
    [Status]    VARCHAR (15)  CONSTRAINT [DF_IssueReturn_Status] DEFAULT ('') NULL,
    [IssueId]   VARCHAR (13)  CONSTRAINT [DF_IssueReturn_IssueId] DEFAULT ('') NULL,
    [Remark]    NVARCHAR (60) CONSTRAINT [DF_IssueReturn_Remark] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)  CONSTRAINT [DF_IssueReturn_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME      NULL,
    [EditName]  VARCHAR (10)  CONSTRAINT [DF_IssueReturn_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME      NULL,
    CONSTRAINT [PK_IssueReturn] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料退回主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料退回單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'IssueId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn', @level2type = N'COLUMN', @level2name = N'EditDate';

