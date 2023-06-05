CREATE TABLE [dbo].[WHIssueToPlace]
(
	[ID]                VARCHAR(15)     CONSTRAINT[DF_WHIssueToPlace_ID]            DEFAULT('')     NOT NULL, 
    [Description]       NVARCHAR(100)   CONSTRAINT[DF_WHIssueToPlace_Description]   DEFAULT ('')    NOT NULL, 
    [Junk]              BIT             CONSTRAINT[DF_WHIssueToPlace_Junk]          DEFAULT (0)     NOT NULL, 
    [AddName]           VARCHAR(10)     CONSTRAINT[DF_WHIssueToPlace_AddName]       DEFAULT ('')    NOT NULL, 
    [AddDate]           DATETIME        CONSTRAINT[DF_WHIssueToPlace_AddDate]                           NULL, 
    [EditName]          VARCHAR(10)     CONSTRAINT[DF_WHIssueToPlace_EditName]      DEFAULT ('')    NOT NULL, 
    [EditDate]          DATETIME        CONSTRAINT[DF_WHIssueToPlace_EditDate]                          NULL, 
    CONSTRAINT [PK_WHIssueToPlace] PRIMARY KEY ([ID]), 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'部門',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WHIssueToPlace',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WHIssueToPlace',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'標註不再使用',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WHIssueToPlace',
    @level2type = N'COLUMN',
    @level2name = N'Junk'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WHIssueToPlace',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WHIssueToPlace',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WHIssueToPlace',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WHIssueToPlace',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'