CREATE TABLE [dbo].[Season] (
    [ID]          VARCHAR (10) CONSTRAINT [DF_Season_ID] DEFAULT ('') NOT NULL,
    [BrandID]     VARCHAR (8)  CONSTRAINT [DF_Season_BrandID] DEFAULT ('') NOT NULL,
    [CostRatio]   TINYINT      CONSTRAINT [DF_Season_CostRatio] DEFAULT ((0)) NULL,
    [SeasonSCIID] VARCHAR (10) CONSTRAINT [DF_Season_SeasonSCIID] DEFAULT ('') NULL,
    [Month]       VARCHAR (7)  CONSTRAINT [DF_Season_Month] DEFAULT ('') NULL,
    [Junk]        BIT          CONSTRAINT [DF_Season_Junk] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10) CONSTRAINT [DF_Season_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (10) CONSTRAINT [DF_Season_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME     NULL,
    [SeasonForDisplay] VARCHAR(20) NULL, 
    CONSTRAINT [PK_Season] PRIMARY KEY CLUSTERED ([ID] ASC, [BrandID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各Brand的季別基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand 的季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Material Alarm Cost%', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'CostRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sci Season', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'SeasonSCIID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand Season 的起始年月', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'Month';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Season', @level2type = N'COLUMN', @level2name = N'EditDate';

