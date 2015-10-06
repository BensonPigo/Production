CREATE TABLE [dbo].[ADIDASComplainTarget] (
    [Year]     VARCHAR (4)    CONSTRAINT [DF_ADIDASComplainTarget_Year] DEFAULT ('') NOT NULL,
    [Target]   NUMERIC (5, 4) CONSTRAINT [DF_ADIDASComplainTarget_Target] DEFAULT ((0)) NOT NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_ADIDASComplainTarget_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   CONSTRAINT [DF_ADIDASComplainTarget_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME       NULL,
    CONSTRAINT [PK_ADIDASComplainTarget] PRIMARY KEY CLUSTERED ([Year] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adidas Complain Target檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainTarget';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Year', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainTarget', @level2type = N'COLUMN', @level2name = N'Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Target', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainTarget', @level2type = N'COLUMN', @level2name = N'Target';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainTarget', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainTarget', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainTarget', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainTarget', @level2type = N'COLUMN', @level2name = N'EditDate';

