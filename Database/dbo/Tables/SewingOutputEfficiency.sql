CREATE TABLE [dbo].[SewingOutputEfficiency]
(
	[Ukey] bigint NOT NULL IDENTITY, 
    [FactoryID] varchar(8) NOT NULL, 
    [StyleUkey] bigint NOT NULL, 
    [StyleID] varchar(15) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_StyleID] DEFAULT (''), 
    [BrandID] varchar(8) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_BrandID] DEFAULT (''), 
    [SeasonID] varchar(10) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_SeasonID] DEFAULT (''), 
    [Efficiency] numeric(5,2) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_Efficiency] DEFAULT (0), 
    [Junk] bit NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_Junk] DEFAULT (0), 
    [AddName] varchar(10) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_AddName] DEFAULT (''), 
    [AddDate] datetime NOT NULL, 
    [EditName] varchar(10) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_EditName] DEFAULT (''), 
    [EditDate] datetime NULL, 
    CONSTRAINT [PK_SewingOutputEfficiency] PRIMARY KEY CLUSTERED ([FactoryID],[StyleUkey] ASC)
)
