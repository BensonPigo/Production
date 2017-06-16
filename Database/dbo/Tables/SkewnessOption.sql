CREATE TABLE [dbo].[SkewnessOption] (
    [BrandID]  VARCHAR (8)  NOT NULL,
    [OptionID] VARCHAR (1)  CONSTRAINT [DF_SkewnessOption_OptionID] DEFAULT ((1)) NOT NULL,
    [Junk]     BIT          CONSTRAINT [DF_SkewnessOption_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10) NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) NULL,
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_SkewnessOption_1] PRIMARY KEY CLUSTERED ([BrandID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SkewnessOption', @level2type = N'COLUMN', @level2name = N'BrandID';

