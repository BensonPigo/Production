CREATE TABLE [dbo].[Style_MiAdidasColorCombo] (
    [StyleUkey]  BIGINT       CONSTRAINT [DF_Style_MiAdidasColorCombo_StyleUkey] DEFAULT ((0)) NOT NULL,
    [LectraCode] VARCHAR (2)  CONSTRAINT [DF_Style_MiAdidasColorCombo_LectraCode] DEFAULT ('') NOT NULL,
    [SetupID]    VARCHAR (3)  CONSTRAINT [DF_Style_MiAdidasColorCombo_SetupID] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_Style_MiAdidasColorCombo_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_Style_MiAdidasColorCombo_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME     NULL,
    [Ukey_old] VARCHAR(10) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_Style_MiAdidasColorCombo] PRIMARY KEY CLUSTERED ([StyleUkey], [LectraCode], [Ukey_old])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式基本檔- MI ADIDAS 配色設定表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MiAdidasColorCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連接STYLE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MiAdidasColorCombo', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LectraCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MiAdidasColorCombo', @level2type = N'COLUMN', @level2name = N'LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SetupID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MiAdidasColorCombo', @level2type = N'COLUMN', @level2name = N'SetupID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MiAdidasColorCombo', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MiAdidasColorCombo', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MiAdidasColorCombo', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MiAdidasColorCombo', @level2type = N'COLUMN', @level2name = N'EditDate';

