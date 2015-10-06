CREATE TABLE [dbo].[Style_HSCode] (
    [StyleUkey] BIGINT       CONSTRAINT [DF_Style_HSCode_StyleUkey] DEFAULT ((0)) NULL,
    [UKEY]      BIGINT       CONSTRAINT [DF_Style_HSCode_UKEY] DEFAULT ((0)) NOT NULL,
    [Article]   VARCHAR (8)  CONSTRAINT [DF_Style_HSCode_Article] DEFAULT ('') NULL,
    [CountryID] VARCHAR (2)  CONSTRAINT [DF_Style_HSCode_CountryID] DEFAULT ('') NULL,
    [Continent] VARCHAR (2)  CONSTRAINT [DF_Style_HSCode_Continent] DEFAULT ('') NULL,
    [HSCode1]   VARCHAR (14) CONSTRAINT [DF_Style_HSCode_HSCode1] DEFAULT ('') NULL,
    [HSCode2]   VARCHAR (14) CONSTRAINT [DF_Style_HSCode_HSCode2] DEFAULT ('') NULL,
    [CATNo1]    VARCHAR (3)  CONSTRAINT [DF_Style_HSCode_CATNo1] DEFAULT ('') NULL,
    [CATNo2]    VARCHAR (3)  CONSTRAINT [DF_Style_HSCode_CATNo2] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_Style_HSCode_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_Style_HSCode_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME     NULL,
    CONSTRAINT [PK_Style_HSCode] PRIMARY KEY CLUSTERED ([UKEY] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style_HSCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'UKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'洲別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'Continent';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際商品統一分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'HSCode1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際商品統一分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'HSCode2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cat No1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'CATNo1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cat No2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'CATNo2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_HSCode', @level2type = N'COLUMN', @level2name = N'EditDate';

