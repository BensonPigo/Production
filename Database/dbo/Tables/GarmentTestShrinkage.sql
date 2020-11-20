CREATE TABLE [dbo].[GarmentTestShrinkage] (
    [BrandID]      VARCHAR (8)   CONSTRAINT [DF_GarmentTestShrinkage_BrandID] DEFAULT ('') NULL,
    [LocationGroup] VARCHAR (4)   CONSTRAINT [DF_GarmentTestShrinkage_LocationGoup] DEFAULT ('') NULL,
    [Location]     VARCHAR (1)   CONSTRAINT [DF_GarmentTestShrinkage_Location] DEFAULT ('') NULL,
    [Seq]          INT           CONSTRAINT [DF_GarmentTestShrinkage_Seq] DEFAULT ((0)) NULL,
    [Type]         VARCHAR (150) CONSTRAINT [DF_GarmentTestShrinkage_Type] DEFAULT ('') NULL,
    [Ukey]         BIGINT        IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_GarmentTestShrinkage] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測量點', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTestShrinkage', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統畫面 / 報表排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTestShrinkage', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各測量點對應的部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTestShrinkage', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'判斷款式是否為套裝 (T+B) / 其他種類的套裝 / PCS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTestShrinkage', @level2type = N'COLUMN', @level2name = N'LocationGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shrinkage 測量點清單是否有指定品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTestShrinkage', @level2type = N'COLUMN', @level2name = N'BrandID';

