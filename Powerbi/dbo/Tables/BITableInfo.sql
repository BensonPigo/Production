CREATE TABLE [dbo].[BITableInfo]
(
	[Id] VARCHAR(50) NOT NULL  DEFAULT (''), 
    [TransferDate] DATETIME NULL, 
    [IS_Trans] BIT Not Null CONSTRAINT [DF_BITableInfo_IS_Trans] DEFAULT (0), 
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
    @level2name = N'TransferDate'
GO
	EXEC sys.sp_addextendedproperty 
		@name=N'MS_Description', @value=N'是否要重轉資料' , 
		@level0type=N'SCHEMA', @level0name=N'dbo', 
		@level1type=N'TABLE', @level1name=N'BITableInfo', 
		@level2type=N'COLUMN', @level2name=N'IS_Trans'
GO