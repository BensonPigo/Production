CREATE TABLE [dbo].[ADIDASMiSetup_ColorComb] (
    [ID]          VARCHAR (3)   CONSTRAINT [DF_ADIDASMiSetup_ColorComb_ID] DEFAULT ('') NOT NULL,
    [ExcelName]   NVARCHAR (50) CONSTRAINT [DF_ADIDASMiSetup_ColorComb_ExcelName] DEFAULT ('') NULL,
    [ExcelColumn] VARCHAR (2)   CONSTRAINT [DF_ADIDASMiSetup_ColorComb_ExcelColumn] DEFAULT ('') NULL,
    [isArtwork]   BIT           CONSTRAINT [DF_ADIDASMiSetup_ColorComb_isArtwork] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_ADIDASMiSetup_ColorComb_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_ADIDASMiSetup_ColorComb_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_ADIDASMiSetup_ColorComb] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MI Adidas 的配色設定檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Field Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb', @level2type = N'COLUMN', @level2name = N'ExcelName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Excel Column', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb', @level2type = N'COLUMN', @level2name = N'ExcelColumn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'is Artwork', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb', @level2type = N'COLUMN', @level2name = N'isArtwork';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASMiSetup_ColorComb', @level2type = N'COLUMN', @level2name = N'EditDate';

