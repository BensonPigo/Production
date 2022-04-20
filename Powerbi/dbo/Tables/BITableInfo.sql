CREATE TABLE [dbo].[BITableInfo]
(
	[Id] VARCHAR(50) NOT NULL  DEFAULT (''), 
    [TrasferDate] DATETIME NULL, 
    CONSTRAINT [PK_BITableInfo] PRIMARY KEY ([Id])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'BI Table Name',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BITableInfo',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後轉入時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'BITableInfo',
    @level2type = N'COLUMN',
    @level2name = N'TrasferDate'