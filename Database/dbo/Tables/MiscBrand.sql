CREATE TABLE [dbo].[MiscBrand] (
    [ID]       VARCHAR (10)  CONSTRAINT [DF_MiscBrand_ID] DEFAULT ('') NOT NULL,
    [Name]     NVARCHAR (50) CONSTRAINT [DF_MiscBrand_Name] DEFAULT ('') NULL,
    [Junk]     BIT           CONSTRAINT [DF_MiscBrand_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10)  CONSTRAINT [DF_MiscBrand_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME      NULL,
    [EditName] VARCHAR (10)  CONSTRAINT [DF_MiscBrand_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME      NULL,
    [Local]    BIT           CONSTRAINT [DF_MiscBrand_Local] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_MiscBrand] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠牌ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠牌全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地廠牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscBrand', @level2type = N'COLUMN', @level2name = N'Local';

