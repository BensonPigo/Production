CREATE TABLE [dbo].[SubProRestTime]
(
	[FactoryID] VARCHAR(8) NOT NULL, 
    [StartDate] DATE NOT NULL, 
    [Shift] VARCHAR NOT NULL, 
    [SubprocessID] VARCHAR(15) NOT NULL, 
    [Rest1Start] TIME NULL, 
    [Rest1End] TIME NULL, 
    [Rest2Start] TIME NULL, 
    [Rest2End] TIME NULL, 
    [Junk] BIT CONSTRAINT [DF_SubProRestTime_Junk] DEFAULT (0) NOT NULL,  
    [AddName] VARCHAR(10) CONSTRAINT [DF_SubProRestTime_AddName] DEFAULT ('') NOT NULL,  
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SubProRestTime_EditName] DEFAULT ('') NOT NULL,  
    [EditDate] DATETIME NULL, 
    CONSTRAINT [PK_SubProRestTime] PRIMARY KEY CLUSTERED ([FactoryID], [StartDate], [Shift], [SubprocessID] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProRestTime',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工作起始日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProRestTime',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'班別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProRestTime',
    @level2type = N'COLUMN',
    @level2name = N'Shift'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProRestTime',
    @level2type = N'COLUMN',
    @level2name = N'SubprocessID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'休息起始時間1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProRestTime',
    @level2type = N'COLUMN',
    @level2name = N'Rest1Start'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'休息結束時間1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProRestTime',
    @level2type = N'COLUMN',
    @level2name = N'Rest1End'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'休息起始時間2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProRestTime',
    @level2type = N'COLUMN',
    @level2name = N'Rest2Start'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'休息結束時間2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProRestTime',
    @level2type = N'COLUMN',
    @level2name = N'Rest2End'