CREATE TABLE [dbo].[MailTo] (
    [ID]          VARCHAR (3)    CONSTRAINT [DF_MailTo_ID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (60)  CONSTRAINT [DF_MailTo_Description] DEFAULT ('') NOT NULL,
    [ToAddress]   NVARCHAR (MAX) CONSTRAINT [DF_MailTo_ToAddress] DEFAULT ('') NULL,
    [CcAddress]   NVARCHAR (MAX) CONSTRAINT [DF_MailTo_CcAddress] DEFAULT ('') NULL,
    [Subject]     NVARCHAR (100) CONSTRAINT [DF_MailTo_Subject] DEFAULT ('') NOT NULL,
    [Content]     NVARCHAR (MAX) CONSTRAINT [DF_MailTo_Content] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_MailTo_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_MailTo_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_MailTo] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Email 設定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'ToAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'副本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'CcAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主旨', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'Subject';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'內文', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'Content';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailTo', @level2type = N'COLUMN', @level2name = N'EditDate';

