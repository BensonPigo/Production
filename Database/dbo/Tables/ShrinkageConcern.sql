CREATE TABLE [dbo].[ShrinkageConcern] (
    [RefNo]    VARCHAR (36) CONSTRAINT [DF_ShrinkageConcern_RefNo] NOT NULL,
    [Junk]     BIT          NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_ShrinkageConcern_AddName] NULL DEFAULT (''),
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_ShrinkageConcern_EditName] NULL DEFAULT (''),
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_ShrinkageConcern] PRIMARY KEY CLUSTERED ([RefNo] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShrinkageConcern', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShrinkageConcern', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShrinkageConcern', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShrinkageConcern', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShrinkageConcern', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShrinkageConcern', @level2type = N'COLUMN', @level2name = N'AddDate';

