CREATE TABLE [dbo].[MtlFactor] (
    [Type]        VARCHAR (1)    CONSTRAINT [DF_MtlFactor_Type] DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (3)    CONSTRAINT [DF_MtlFactor_ID] DEFAULT ('') NOT NULL,
    [Pattern]     VARCHAR (30)   CONSTRAINT [DF_MtlFactor_Pattern] DEFAULT ('') NULL,
    [PatternCode] VARCHAR (1)    CONSTRAINT [DF_MtlFactor_PatternCode] DEFAULT ('') NULL,
    [Drape]       VARCHAR (30)   CONSTRAINT [DF_MtlFactor_Drape] DEFAULT ('') NULL,
    [DrapeCode]   VARCHAR (1)    CONSTRAINT [DF_MtlFactor_DrapeCode] DEFAULT ('') NULL,
    [Color]       VARCHAR (30)   CONSTRAINT [DF_MtlFactor_Color] DEFAULT ('') NULL,
    [ColorCode]   VARCHAR (1)    CONSTRAINT [DF_MtlFactor_ColorCode] DEFAULT ('') NULL,
    [Rate]        NUMERIC (5, 2) CONSTRAINT [DF_MtlFactor_Rate] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_MtlFactor_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_MtlFactor_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_MtlFactor] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabirc/Cutting Factor 基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'對條對格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'Pattern';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'厚薄手感', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'Drape';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'DrapeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'面料顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'Color';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'ColorCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'比例', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'Rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlFactor', @level2type = N'COLUMN', @level2name = N'EditDate';

