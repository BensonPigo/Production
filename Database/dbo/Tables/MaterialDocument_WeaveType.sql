CREATE TABLE [dbo].[MaterialDocument_WeaveType]
(
	[DocumentName]      varchar(100)    CONSTRAINT [DF_MaterialDocument_WeaveType_DocumentName]         DEFAULT ('') NOT NULL, 
    [BrandID]           VARCHAR(8)      CONSTRAINT [DF_MaterialDocument_WeaveType_BrandID]              DEFAULT ('') NOT NULL , 
    [WeaveTypeId]       VARCHAR(20)      CONSTRAINT [DF_MaterialDocument_WeaveType_WeaveTypeId]          DEFAULT ('') NOT NULL , 
    CONSTRAINT [PK_MaterialDocument_WeaveType] PRIMARY KEY ([DocumentName], [BrandID], [WeaveTypeId]) ,
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'測試報告文件名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_WeaveType',
    @level2type = N'COLUMN',
    @level2name = N'DocumentName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_WeaveType',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編織總類 KNIT/WOVEN',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MaterialDocument_WeaveType',
    @level2type = N'COLUMN',
    @level2name = N'WeaveTypeId'