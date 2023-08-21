CREATE TABLE [dbo].[LocalOrderMaterial]
(
	[POID]                  VARCHAR(13)                                                                     NOT NULL, 
    [Seq1]                  VARCHAR(3)                                                                      NOT NULL, 
    [Seq2]                  VARCHAR(2)                                                                      NOT NULL, 
    [FabricType]            VARCHAR(1)      CONSTRAINT[DF_LocalOrderMaterial_FabricType]    DEFAULT ((''))  NOT NULL, 
    [Refno]                 VARCHAR(36)     CONSTRAINT[DF_LocalOrderMaterial_Refno]         DEFAULT ((''))  NOT NULL, 
    [Desc]                  NVARCHAR(MAX)   CONSTRAINT[DF_LocalOrderMaterial_Desc]          DEFAULT ((''))  NOT NULL, 
    [Color]                 VARCHAR(500)    CONSTRAINT[DF_LocalOrderMaterial_Color]         DEFAULT ((''))  NOT NULL, 
    [SizeCode]              VARCHAR(15)     CONSTRAINT[DF_LocalOrderMaterial_SizeCode]      DEFAULT ((''))  NOT NULL, 
    [WeaveType]             VARCHAR(20)     CONSTRAINT[DF_LocalOrderMaterial_WeaveType]     DEFAULT ((''))  NOT NULL, 
    [MtlType]               VARCHAR(20)     CONSTRAINT[DF_LocalOrderMaterial_MtlType]       DEFAULT ((''))  NOT NULL, 
    [Unit]                  VARCHAR(8)      CONSTRAINT[DF_LocalOrderMaterial_Unit]          DEFAULT ((''))  NOT NULL, 
    [AddName]               VARCHAR(10)     CONSTRAINT[DF_LocalOrderMaterial_AddName]       DEFAULT ((''))  NOT NULL, 
    [AddDate]               DATETIME                                                                            NULL, 
    [EditName]              VARCHAR(10)     CONSTRAINT[DF_LocalOrderMaterial_EditName]      DEFAULT ((''))  NOT NULL, 
    [EditDate]              DATETIME                                                                            NULL, 
    CONSTRAINT [PK_LocalOrderMaterial] PRIMARY KEY ([POID], [Seq2], [Seq1]), 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'主料 F 還是輔料 A',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'FabricType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'料號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'Refno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'明細',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'Desc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'Color'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'尺寸',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'SizeCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'織法',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'WeaveType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物料種類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'MtlType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'Unit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderMaterial',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'