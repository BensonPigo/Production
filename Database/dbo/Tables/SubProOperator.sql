CREATE TABLE [dbo].[SubProOperator]
(
	[EmployeeID] nvarchar(8) NOT NULL, 
    [FirstName] NVARCHAR(30) CONSTRAINT [DF_SubProOperator_FirstName] DEFAULT ('') NOT NULL, 
    [LastName] NVARCHAR(30) CONSTRAINT [DF_SubProOperator_LastName] DEFAULT ('') NOT NULL, 
    [SubprocessID] VARCHAR(15) CONSTRAINT [DF_SubProOperator_SubprocessID] DEFAULT ('') NOT NULL, 
    [FactoryID] VARCHAR(8) CONSTRAINT [DF_SubProOperator_FactoryID] DEFAULT ('') NOT NULL, 
    [Junk] BIT CONSTRAINT [DF_SubProOperator_Junk] DEFAULT (0) NOT NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_SubProOperator_AddName] DEFAULT ('') NOT NULL, 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SubProOperator_EditName] DEFAULT ('') NOT NULL, 
    [EditDate] DATETIME NULL, 
    CONSTRAINT [PK_SubProOperator] PRIMARY KEY CLUSTERED ([EmployeeID], [SubprocessID] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'員工編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'EmployeeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'員工名稱First Name',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'FirstName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'員工名稱Last Name',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'LastName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'SubprocessID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否Junk',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'Junk'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新建日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProOperator',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'