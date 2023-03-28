CREATE TABLE [dbo].[BrandRelation]
(
	[BrandID]       VARCHAR(8)      CONSTRAINT [DF_BrandRelation_BrandID]       DEFAULT ('')    NOT NULL, 
    [SuppGroup]     VARCHAR(6)      CONSTRAINT [DF_BrandRelation_SuppGroup]     DEFAULT ('')    NOT NULL, 
    [SuppID]        VARCHAR(6)      CONSTRAINT [DF_BrandRelation_SuppID]        DEFAULT ('')    NOT NULL, 
    [AddDate]       DATETIME                                                                    NOT NULL, 
    [AddName]       VARCHAR(10)     CONSTRAINT [DF_BrandRelation_AddName]       DEFAULT ('')    NOT NULL, 
    [EditDate]      DATETIME                                                                    NULL, 
    [Editname]      VARCHAR(10)     CONSTRAINT [DF_BrandRelation_Editname]      DEFAULT ('')    NULL, 
    CONSTRAINT [PK_BrandRelation] PRIMARY KEY ([BrandID], [SuppGroup], [SuppID]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BrandRelation',
    @level2type = N'COLUMN',
    @level2name = N'Editname'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BrandRelation',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BrandRelation',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'供應商群組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BrandRelation',
    @level2type = N'COLUMN',
    @level2name = N'SuppGroup'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'供應商ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BrandRelation',
    @level2type = N'COLUMN',
    @level2name = N'SuppID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BrandRelation',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BrandRelation',
    @level2type = N'COLUMN',
    @level2name = N'AddName'