CREATE TABLE [dbo].[SewerGroupRange]
(
	[Seq] tinyint CONSTRAINT [DF_SewerGroupRange_Seq] DEFAULT (0) NOT NULL, 
    [Sewer] TINYINT CONSTRAINT [DF_SewerGroupRange_Sewer] DEFAULT (0) NOT NULL, 
    [Condition1] VARCHAR(2) CONSTRAINT [DF_SewerGroupRange_Condition1] DEFAULT ('') NOT NULL, 
    [Range1] NUMERIC(3, 2) CONSTRAINT [DF_SewerGroupRange_Range1] DEFAULT (0) NOT NULL, 
    [Condition2] VARCHAR(2) CONSTRAINT [DF_SewerGroupRange_Condition2] DEFAULT ('') NOT NULL, 
    [Range2] NUMERIC(3, 2) CONSTRAINT [DF_SewerGroupRange_Range2] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_SewerGroupRange] PRIMARY KEY CLUSTERED ([Sewer] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'排序順序',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewerGroupRange',
    @level2type = N'COLUMN',
    @level2name = N'Seq'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'分配車工人數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewerGroupRange',
    @level2type = N'COLUMN',
    @level2name = N'Sewer'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Range1判斷條件',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewerGroupRange',
    @level2type = N'COLUMN',
    @level2name = N'Condition1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Condition1的值',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewerGroupRange',
    @level2type = N'COLUMN',
    @level2name = N'Range1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Range2判斷條件',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewerGroupRange',
    @level2type = N'COLUMN',
    @level2name = N'Condition2'