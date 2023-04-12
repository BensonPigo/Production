CREATE TABLE [dbo].[Export_Container]
(
	[ID]        VARCHAR(13)     CONSTRAINT [DF_Export_Container_ID]         DEFAULT ('')    NOT NULL, 
    [Seq]       VARCHAR(2)      CONSTRAINT [DF_Export_Container_Seq]        DEFAULT ('')    NOT NULL , 
    [Type]      VARCHAR(2)      CONSTRAINT [DF_Export_Container_Type]       DEFAULT ('')    NOT NULL, 
    [Container] VARCHAR(20)     CONSTRAINT [DF_Export_Container_Container]  DEFAULT ('')    NOT NULL, 
    [CartonQty] NUMERIC(5)      CONSTRAINT [DF_Export_Container_CartonQty]  DEFAULT ((0))   NOT NULL, 
    [WeightKg]  NUMERIC(9, 2)   CONSTRAINT [DF_Export_Container_WeightKg]   DEFAULT ((0))   NOT NULL, 
    [AddName]   VARCHAR(10)     CONSTRAINT [DF_Export_Container_AddName] NULL, 
    [AddDate]   DATETIME        CONSTRAINT [DF_Export_Container_AddDate] NULL, 
    [EditName]  VARCHAR(10)     CONSTRAINT [DF_Export_Container_EditName] NULL, 
    [EditDate]  DATETIME        CONSTRAINT [DF_Export_Container_EditDate] NULL, 
    CONSTRAINT [PK_Export_Container] PRIMARY KEY ([ID],[Container])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WK#',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'序號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'Seq'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'櫃型 (20,40,HQ)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'Type'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'櫃號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'Container'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'櫃內的件數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'CartonQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'重量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'WeightKg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Container',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'