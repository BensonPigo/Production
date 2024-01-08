CREATE TABLE [dbo].[FabricNotch]
(
	[BrandID]   VARCHAR(8)      CONSTRAINT [DF_FabricNotch_BrandID]     DEFAULT ((''))  NOT NULL, 
    [SeasonID]  VARCHAR(10)     CONSTRAINT [DF_FabricNotch_SeasonID]    DEFAULT ((''))  NOT NULL, 
    [Refno]     VARCHAR(36)     CONSTRAINT [DF_FabricNotch_Refno]       DEFAULT ((''))  NOT NULL, 
    [NoNotch]   BIT             CONSTRAINT [DF_FabricNotch_NoNotch]     DEFAULT ((0))   NOT NULL, 
    [UKEY]      BIGINT          IDENTITY(1,1)                           DEFAULT ((0))   NOT NULL , 
    [AddName]   VARCHAR(10)     CONSTRAINT [DF_FabricNotch_AddName]     DEFAULT ((''))  NOT NULL, 
    [AddDate]   DATETIME        CONSTRAINT [DF_FabricNotch_AddDate]                     NULL    ,
    [EditName]  VARCHAR(10)     CONSTRAINT [DF_FabricNotch_EditName]    DEFAULT ((''))  NULL    , 
    [EditDate]  DATETIME        CONSTRAINT [DF_FabricNotch_EditDate]                    NULL, 
    CONSTRAINT [PK_FabricNotch] PRIMARY KEY ([UKEY])

)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricNotch',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'季節',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricNotch',
    @level2type = N'COLUMN',
    @level2name = N'SeasonID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'誤料編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricNotch',
    @level2type = N'COLUMN',
    @level2name = N'Refno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'牙口有無',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricNotch',
    @level2type = N'COLUMN',
    @level2name = N'NoNotch'