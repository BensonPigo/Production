CREATE TABLE [dbo].[MailGroup] (
    [Code]      VARCHAR (3)    NOT NULL,
    [FactoryID] VARCHAR (8)    NOT NULL,
    [ToAddress] NVARCHAR (MAX) CONSTRAINT [DF_MailGroup_ToAddress] DEFAULT ('') NOT NULL,
    [CCAddress] NVARCHAR (MAX) CONSTRAINT [DF_MailGroup_CCAddress] DEFAULT ('') NOT NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_MailGroup_AddName] DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_MailGroup_EditName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_MailGroup] PRIMARY KEY CLUSTERED ([Code] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'副本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailGroup', @level2type = N'COLUMN', @level2name = N'CCAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailGroup', @level2type = N'COLUMN', @level2name = N'ToAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠代', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailGroup', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'此部分與 MailTo.ID 相對應', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MailGroup', @level2type = N'COLUMN', @level2name = N'Code';

