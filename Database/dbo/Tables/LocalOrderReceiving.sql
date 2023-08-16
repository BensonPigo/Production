CREATE TABLE [dbo].[LocalOrderReceiving]
(
	[ID]            VARCHAR(13)                                                                         NOT NULL, 
    [MDivisionID]   VARCHAR(8)      CONSTRAINT [DF_LocalOrderReceiving_MDivisionID]     DEFAULT ((''))  NOT NULL, 
    [FactoryID]     VARCHAR(8)      CONSTRAINT [DF_LocalOrderReceiving_FactoryID]       DEFAULT ((''))  NOT NULL, 
    [WhseArrival]   DATE                                                                                NULL, 
    [Remark]        NVARCHAR(100)   CONSTRAINT [DF_LocalOrderReceiving_Remark]          DEFAULT ((''))  NOT NULL, 
    [Status]        VARCHAR(15)     CONSTRAINT [DF_LocalOrderReceiving_Status]          DEFAULT ((''))  NOT NULL, 
    [Packages]      NUMERIC(5)                                                                          NULL, 
    [AddName]       VARCHAR(10)     CONSTRAINT [DF_LocalOrderReceiving_AddName]         DEFAULT ((''))  NOT NULL, 
    [AddDate]       DATETIME                                                                            NULL, 
    [EditName]      VARCHAR(10)     CONSTRAINT [DF_LocalOrderReceiving_EditName]        DEFAULT ((''))  NOT NULL, 
    [EditDate]      DATETIME                                                                            NULL, 
    CONSTRAINT [PK_LocalOrderReceiving] PRIMARY KEY ([ID])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'收料單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠代',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'收料到倉日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'WhseArrival'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單據狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'包裹數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'Packages'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderReceiving',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'