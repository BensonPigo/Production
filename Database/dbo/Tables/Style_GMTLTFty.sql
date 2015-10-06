CREATE TABLE [dbo].[Style_GMTLTFty] (
    [StyleUkey] BIGINT       CONSTRAINT [DF_Style_GMTLTFty_StyleUkey] DEFAULT ((0)) NOT NULL,
    [FactoryID] VARCHAR (8)  CONSTRAINT [DF_Style_GMTLTFty_FactoryID] DEFAULT ('') NOT NULL,
    [GMTLT]     SMALLINT     CONSTRAINT [DF_Style_GMTLTFty_GMTLT] DEFAULT ((0)) NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_Style_GMTLTFty_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_Style_GMTLTFty_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME     NULL,
    CONSTRAINT [PK_Style_GMTLTFty] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GarmentList-LT(成衣LEADTIME)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_GMTLTFty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連接STYLE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_GMTLTFty', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_GMTLTFty', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣LEADTIME', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_GMTLTFty', @level2type = N'COLUMN', @level2name = N'GMTLT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_GMTLTFty', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_GMTLTFty', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_GMTLTFty', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_GMTLTFty', @level2type = N'COLUMN', @level2name = N'EditDate';

