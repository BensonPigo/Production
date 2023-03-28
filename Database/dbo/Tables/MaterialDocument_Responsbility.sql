CREATE TABLE [dbo].[MaterialDocument_Responsbility]
(
	[DocumentName]      varchar(100)    CONSTRAINT [DF_MaterialDocument_Responsbility_DocumentName]     DEFAULT ('') NOT NULL, 
    [BrandID]           VARCHAR(8)      CONSTRAINT [DF_MaterialDocument_Responsbility_BrandID]          DEFAULT ('') NOT NULL , 
    [SuppID]            VARCHAR(8)      CONSTRAINT [DF_MaterialDocument_Responsbility_SuppID]           DEFAULT ('') NOT NULL , 
    [Responsibility]    VARCHAR(50)     CONSTRAINT [DF_MaterialDocument_Responsbility_Responsibility]   DEFAULT ('') NOT NULL , 
    CONSTRAINT [PK_MaterialDocument_Responsbility] PRIMARY KEY ([DocumentName], [BrandID], [SuppID]) ,
)


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'責任歸屬(編輯用)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_Responsbility',
    @level2type = N'COLUMN',
    @level2name = N'Responsibility'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'供應商ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_Responsbility',
    @level2type = N'COLUMN',
    @level2name = N'SuppID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_Responsbility',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'測試報告文件名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_Responsbility',
    @level2type = N'COLUMN',
    @level2name = N'DocumentName'