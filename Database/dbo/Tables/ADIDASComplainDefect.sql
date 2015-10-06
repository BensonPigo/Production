CREATE TABLE [dbo].[ADIDASComplainDefect] (
    [ID]       VARCHAR (2)    CONSTRAINT [DF_ADIDASComplainDefect_ID] DEFAULT ('') NOT NULL,
    [Name]     NVARCHAR (250) CONSTRAINT [DF_ADIDASComplainDefect_Name] DEFAULT ('') NOT NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_ADIDASComplainDefect_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   CONSTRAINT [DF_ADIDASComplainDefect_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME       NULL,
    CONSTRAINT [PK_ADIDASComplainDefect] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adidas Complain Defect 代碼主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Main ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect', @level2type = N'COLUMN', @level2name = N'EditDate';

