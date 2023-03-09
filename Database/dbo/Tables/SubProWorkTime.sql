CREATE TABLE [dbo].[SubProWorkTime]
(
	[ID] bigint NOT NULL IDENTITY , 
    [FactoryID] VARCHAR(8) NOT NULL, 
    [Shift] VARCHAR NOT NULL, 
    [SubprocessID] VARCHAR(15) NOT NULL, 
    [StartDate] DATE NOT NULL, 
    [BeginTime] TIME NULL, 
    [EndTime] TIME NULL, 
    [Junk] BIT CONSTRAINT [DF_SubProWorkTime_Junk] DEFAULT (0) NOT NULL,  
    [AddName] VARCHAR(10) CONSTRAINT [DF_SubProWorkTime_AddName] DEFAULT ('') NOT NULL,  
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SubProWorkTime_EditName] DEFAULT ('') NOT NULL,  
    [EditDate] DATETIME NULL, 
    CONSTRAINT [PK_SubProWorkTime] PRIMARY KEY CLUSTERED ([FactoryID], [StartDate], [Shift], [SubprocessID] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProWorkTime',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'班別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProWorkTime',
    @level2type = N'COLUMN',
    @level2name = N'Shift'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProWorkTime',
    @level2type = N'COLUMN',
    @level2name = N'SubprocessID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工作起始日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProWorkTime',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'開始時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProWorkTime',
    @level2type = N'COLUMN',
    @level2name = N'BeginTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'結束時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProWorkTime',
    @level2type = N'COLUMN',
    @level2name = N'EndTime'