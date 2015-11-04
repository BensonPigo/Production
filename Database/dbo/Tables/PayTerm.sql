CREATE TABLE [dbo].[PayTerm] (
    [ID]       VARCHAR (6)   CONSTRAINT [DF_PayTerm_ID] DEFAULT ('') NOT NULL,
    [Name]     NVARCHAR (60) CONSTRAINT [DF_PayTerm_Name] DEFAULT ('') NOT NULL,
    [Junk]     BIT           CONSTRAINT [DF_PayTerm_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10)  CONSTRAINT [DF_PayTerm_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME      NULL,
    [EditName] VARCHAR (10)  CONSTRAINT [DF_PayTerm_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME      NULL,
    CONSTRAINT [PK_PayTerm] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Payment Term', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTerm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTerm', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTerm', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTerm', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTerm', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTerm', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTerm', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTerm', @level2type = N'COLUMN', @level2name = N'EditDate';

