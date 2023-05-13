CREATE TABLE [dbo].[MaterialDocument_MtlType]
(
	[DocumentName]  varchar(100)    CONSTRAINT [DF_MaterialDocument_MtlType_DocumentName]   NOT NULL DEFAULT (''), 
    [BrandID]       VARCHAR(8)      CONSTRAINT [DF_MaterialDocument_MtlType_BrandID]        NOT NULL DEFAULT (''), 
    [MtltypeId]     VARCHAR(20)     CONSTRAINT [DF_MaterialDocument_MtlType_MtltypeId]      NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_MaterialDocument_MtlType] PRIMARY KEY ([DocumentName], [BrandID], [MtltypeId]) ,
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'測試報告文件名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_MtlType',
    @level2type = N'COLUMN',
    @level2name = N'DocumentName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_MtlType',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物料的總類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_MtlType',
    @level2type = N'COLUMN',
    @level2name = N'MtltypeId'