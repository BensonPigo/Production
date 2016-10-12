CREATE TABLE [dbo].[SuppLevel] (
    [ID]       VARCHAR (1)    CONSTRAINT [DF_SuppLevel_ID] DEFAULT ('') NOT NULL,
    [Type]     VARCHAR (1)    CONSTRAINT [DF_SuppLevel_Type] DEFAULT ('') NOT NULL,
    [Junk]     BIT            CONSTRAINT [DF_SuppLevel_Junk] DEFAULT ((0)) NULL,
    [Range1]   NUMERIC (5, 2) CONSTRAINT [DF_SuppLevel_Range1] DEFAULT ((0)) NOT NULL,
    [Range2]   NUMERIC (5, 2) CONSTRAINT [DF_SuppLevel_Range2] DEFAULT ((0)) NOT NULL,
    [Result]   VARCHAR (1)    CONSTRAINT [DF_SuppLevel_Result] DEFAULT ('') NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_SuppLevel_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   CONSTRAINT [DF_SuppLevel_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME       NULL,
    CONSTRAINT [PK_SuppLevel] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The Level of Supplier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Level ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'比率範圍上限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'Range1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'比率範圍下限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'Range2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SuppLevel', @level2type = N'COLUMN', @level2name = N'EditDate';

