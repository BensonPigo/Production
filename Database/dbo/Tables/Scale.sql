CREATE TABLE [dbo].[Scale] (
    [ID]       VARCHAR (5)  CONSTRAINT [DF_Scale_ID] DEFAULT ('') NOT NULL,
    [Junk]     BIT          CONSTRAINT [DF_Scale_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_Scale_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_Scale_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_Scale] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grey Scale 基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Scale 代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scale', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scale', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scale', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scale', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scale', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scale', @level2type = N'COLUMN', @level2name = N'EditDate';

