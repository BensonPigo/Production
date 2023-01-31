CREATE TABLE [dbo].[Country] (
    [ID]        VARCHAR (2)   CONSTRAINT [DF_Country_ID] DEFAULT ('') NOT NULL,
    [NameCH]    NVARCHAR (40) CONSTRAINT [DF_Country_NameCH] DEFAULT ('') NULL,
    [NameEN]    NVARCHAR (30) CONSTRAINT [DF_Country_NameEN] DEFAULT ('') NULL,
    [Alias]     VARCHAR (30)  CONSTRAINT [DF_Country_Alias] DEFAULT ('') NULL,
    [Junk]      BIT           CONSTRAINT [DF_Country_Junk] DEFAULT ((0)) NULL,
    [MtlFormA]  BIT           CONSTRAINT [DF_Country_MtlFormA] DEFAULT ((0)) NULL,
    [Continent] VARCHAR (2)   CONSTRAINT [DF_Country_Continent] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)  CONSTRAINT [DF_Country_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME      NULL,
    [EditName]  VARCHAR (10)  CONSTRAINT [DF_Country_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME      NULL,
    [SpecificDestination] BIT CONSTRAINT [DF_Country_SpecificDestination] DEFAULT ((0)) NOT NULL, 
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Country', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'別名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'Alias';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作廢', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料生產國別是否提供FormA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'MtlFormA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'洲別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'Continent';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Country', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉物物料轉出時需要特別注意 Tone/Grp',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Country',
    @level2type = N'COLUMN',
    @level2name = N'SpecificDestination'