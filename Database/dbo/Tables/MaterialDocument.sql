CREATE TABLE [dbo].[MaterialDocument]
(
	[DocumentName]      VARCHAR(100)    CONSTRAINT [DF_MaterialDocument_DocumentName]       DEFAULT ('')    NOT NULL, 
    [BrandID]           VARCHAR(8)      CONSTRAINT [DF_MaterialDocument_BrandID]            DEFAULT ('')    NOT NULL, 
    [Description]       NVARCHAR(500)   CONSTRAINT [DF_MaterialDocument_Description]        DEFAULT ('')    NULL , 
    [FabricType]        VARCHAR(50)     CONSTRAINT [DF_MaterialDocument_FabricType]         DEFAULT ('')    NOT NULL, 
    [Target]            VARCHAR(50)     CONSTRAINT [DF_MaterialDocument_Target]             DEFAULT ('')    NOT NULL, 
    [FileRule]          VARCHAR(50)     CONSTRAINT [DF_MaterialDocument_FileRule]           DEFAULT ('')    NOT NULL, 
    [Expiration]        NUMERIC(3)      CONSTRAINT [DF_MaterialDocument_Expiration]         DEFAULT ((0))   NULL, 
    [Filepath]          NVARCHAR(500)   CONSTRAINT [DF_MaterialDocument_Filepath]           DEFAULT ('')    NULL, 
    [ActiveSeason]      VARCHAR(10)     CONSTRAINT [DF_MaterialDocument_ActiveSeason]       DEFAULT ('')    NOT NULL, 
    [EndSeason]         VARCHAR(10)     CONSTRAINT [DF_MaterialDocument_EndSeason]          DEFAULT ('')    NULL, 
    [Responsibility]    VARCHAR(50)     CONSTRAINT [DF_MaterialDocument_Responsibility]     DEFAULT ('')    NOT NULL, 
    [Category]          VARCHAR(50)     CONSTRAINT [DF_MaterialDocument_Category]           DEFAULT ('')    NULL , 
    [ExcludeProgram]    VARCHAR(1000)   CONSTRAINT [DF_MaterialDocument_ExcludeProgram]     DEFAULT ('')    NULL , 
    [ExcludeReplace]    BIT             CONSTRAINT [DF_MaterialDocument_ExcludeReplace]     DEFAULT((0))    NOT NULL , 
    [ExcludeStock]      BIT             CONSTRAINT [DF_MaterialDocument_ExcludeStock]       DEFAULT((0))    NOT NULL , 
    [MtlTypeClude]      VARCHAR(50)     CONSTRAINT [DF_MaterialDocument_MtlTypeClude]       DEFAULT ('')    NULL , 
    [SupplierClude]     VARCHAR(50)     CONSTRAINT [DF_MaterialDocument_SupplierClude]      DEFAULT ('')    NULL , 
    [Junk]              BIT             CONSTRAINT [DF_MaterialDocument_Junk]	            DEFAULT ((0))   NULL , 
    AddDate				DateTime											                                NULL,
	AddName			    varchar(10)		CONSTRAINT [DF_MaterialDocument_AddName]			DEFAULT ('')	NULL,
	EditDate		    DateTime											                                NULL,
	Editname		    varchar(10)		CONSTRAINT [DF_MaterialDocument_Editname]			DEFAULT ('')	NULL, 
    CONSTRAINT [PK_MaterialDocument] PRIMARY KEY ([DocumentName], [BrandID]) ,
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'測試報告文件名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'DocumentName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'簡介',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物料類型',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'FabricType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'文件規則',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'FileRule'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'開始的季節',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'ActiveSeason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'結束的季節',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'EndSeason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'責任歸屬',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'Responsibility'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'類別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Junk',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'Junk'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument',
    @level2type = N'COLUMN',
    @level2name = N'Editname'