CREATE TABLE [dbo].[AutomatedLineMapping]
(
	[ID] INT NOT NULL IDENTITY, 
    [StyleUKey] BIGINT CONSTRAINT [DF_AutomatedLineMapping_StyleUKey] DEFAULT (0) NOT NULL, 
    [Phase] VARCHAR(7) CONSTRAINT [DF_AutomatedLineMapping_Phase] DEFAULT ('') NOT NULL,  
    [Version] TINYINT CONSTRAINT [DF_AutomatedLineMapping_Version] DEFAULT (0) NOT NULL,  
    [FactoryID] VARCHAR(8) CONSTRAINT [DF_AutomatedLineMapping_FactoryID] DEFAULT ('') NOT NULL,  
    [StyleID] VARCHAR(15) CONSTRAINT [DF_AutomatedLineMapping_StyleID] DEFAULT ('') NOT NULL,  
    [SeasonID] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMapping_SeasonID] DEFAULT ('') NOT NULL,  
    [BrandID] VARCHAR(8) CONSTRAINT [DF_AutomatedLineMapping_BrandID] DEFAULT ('') NOT NULL,  
    [ComboType] VARCHAR CONSTRAINT [DF_AutomatedLineMapping_ComboType] DEFAULT ('') NOT NULL,  
    [StyleCPU] NUMERIC(5, 3) CONSTRAINT [DF_AutomatedLineMapping_StyleCPU] DEFAULT (0) NOT NULL,  
    [SewerManpower] TINYINT CONSTRAINT [DF_AutomatedLineMapping_SewerManpower] DEFAULT (0) NOT NULL,  
    [OriSewerManpower] TINYINT CONSTRAINT [DF_AutomatedLineMapping_OriSewerManpower] DEFAULT (0) NOT NULL,  
    [PackerManpower] TINYINT CONSTRAINT [DF_AutomatedLineMapping_PackerManpower] DEFAULT (0) NOT NULL,  
    [PresserManpower] TINYINT CONSTRAINT [DF_AutomatedLineMapping_PresserManpower] DEFAULT (0) NOT NULL,  
    [TotalGSDTime] NUMERIC(12, 2) CONSTRAINT [DF_AutomatedLineMapping_TotalGSDTime] DEFAULT (0) NOT NULL,  
    [HighestGSDTime] NUMERIC(12, 2) CONSTRAINT [DF_AutomatedLineMapping_HighestGSDTime] DEFAULT (0) NOT NULL,  
    [TimeStudyID] BIGINT CONSTRAINT [DF_AutomatedLineMapping_TimeStudyID] DEFAULT (0) NOT NULL,  
    [TimeStudyStatus] VARCHAR(15) CONSTRAINT [DF_AutomatedLineMapping_TimeStudyStatus] DEFAULT ('') NOT NULL,  
    [TimeStudyVersion] VARCHAR(2) CONSTRAINT [DF_AutomatedLineMapping_TimeStudyVersion] DEFAULT ('') NOT NULL,  
    [WorkHour] NUMERIC(3, 1) CONSTRAINT [DF_AutomatedLineMapping_WorkHour] DEFAULT (0) NOT NULL,  
    [Status] VARCHAR(9) CONSTRAINT [DF_AutomatedLineMapping_Status] DEFAULT ('') NOT NULL,  
    [CFMName] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMapping_CFMName] DEFAULT ('') NOT NULL,  
    [CFMDate] DATETIME  NULL,  
    [AddName] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMapping_AddName] DEFAULT ('') NOT NULL,  
    [AddDate] DATETIME  NULL,  
    [EditName] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMapping_EditName] DEFAULT ('') NOT NULL,  
    [EditDate] DATETIME NULL, 
    CONSTRAINT [PK_AutomatedLineMapping] PRIMARY KEY CLUSTERED ([ID] ASC)
)

GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM階段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'Phase'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM版號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車工人力',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'SewerManpower'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'原始車工人力',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'OriSewerManpower'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'包裝人力',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'PackerManpower'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'整燙人力',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'PresserManpower'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總GSD時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'TotalGSDTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最高的GSD時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'HighestGSDTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD版號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'每站工時',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'WorkHour'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Line Mapping狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping',
    @level2type = N'COLUMN',
    @level2name = N'Status'