CREATE TABLE [dbo].[LocalOrderAdjust]
(
	[ID]            VARCHAR(13)                                                                     NOT NULL , 
    [MDivisionID]   VARCHAR(8)      CONSTRAINT [DF_LocalOrderAdjust_MDivisionID]    DEFAULT ((''))  NOT NULL , 
    [FactoryID]     VARCHAR(8)      CONSTRAINT [DF_LocalOrderAdjust_FactoryID]      DEFAULT ((''))  NOT NULL , 
    [IssueDate]     DATE                                                                            NULL, 
    [Remark]        NVARCHAR(100)   CONSTRAINT [DF_LocalOrderAdjust_Remark]         DEFAULT ((''))  NOT NULL , 
    [Status]        VARCHAR(15)     CONSTRAINT [DF_LocalOrderAdjust_Status]         DEFAULT ((''))  NOT NULL , 
    [IsFromWMS]     BIT             CONSTRAINT [DF_LocalOrderAdjust_IsFromWMS]      DEFAULT ((0))   NOT NULL , 
    [AddName]       VARCHAR(10)     CONSTRAINT [DF_LocalOrderAdjust_AddName]        DEFAULT ((''))  NOT NULL , 
    [AddDate]       DATETIME                                                                        NULL, 
    [EditName]      VARCHAR(10)     CONSTRAINT [DF_LocalOrderAdjust_EditName]       DEFAULT ((''))  NOT NULL , 
    [EditDate]      DATETIME                                                                        NULL, 
    CONSTRAINT [PK_LocalOrderAdjust] PRIMARY KEY ([ID])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠代',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'IssueDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單據狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WMS 建立的調整單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'IsFromWMS'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderAdjust',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'