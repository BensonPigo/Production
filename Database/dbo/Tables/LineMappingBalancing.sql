CREATE TABLE [dbo].[LineMappingBalancing]
(
	[ID] INT NOT NULL IDENTITY, 
    [AutomatedLineMappingID] INT NOT NULL, 
    [StyleUKey] BIGINT CONSTRAINT [DF_LineMappingBalancing_StyleUKey] DEFAULT (0) NOT NULL, 
    [Phase] VARCHAR(7) CONSTRAINT [DF_LineMappingBalancing_Phase] DEFAULT ('') NOT NULL,  
    [Version] TINYINT CONSTRAINT [DF_LineMappingBalancing_Version] DEFAULT (0) NOT NULL,  
    [FactoryID] VARCHAR(8) CONSTRAINT [DF_LineMappingBalancing_FactoryID] DEFAULT ('') NOT NULL,  
    [StyleID] VARCHAR(15) CONSTRAINT [DF_LineMappingBalancing_StyleID] DEFAULT ('') NOT NULL,  
    [SeasonID] VARCHAR(10) CONSTRAINT [DF_LineMappingBalancing_SeasonID] DEFAULT ('') NOT NULL,  
    [BrandID] VARCHAR(8) CONSTRAINT [DF_LineMappingBalancing_BrandID] DEFAULT ('') NOT NULL,  
    [ComboType] VARCHAR CONSTRAINT [DF_LineMappingBalancing_ComboType] DEFAULT ('') NOT NULL,  
    [StyleCPU] NUMERIC(5, 3) CONSTRAINT [DF_LineMappingBalancing_StyleCPU] DEFAULT (0) NOT NULL,  
    [SewingLineID] VARCHAR(8) CONSTRAINT [DF_LineMappingBalancing_SewingLineID] DEFAULT ('') NOT NULL,
    [Team] VARCHAR(8) CONSTRAINT [DF_LineMappingBalancing_Team] DEFAULT ('') NOT NULL,
    [SewerManpower] TINYINT CONSTRAINT [DF_LineMappingBalancing_SewerManpower] DEFAULT (0) NOT NULL,  
    [PackerManpower] TINYINT CONSTRAINT [DF_LineMappingBalancing_PackerManpower] DEFAULT (0) NOT NULL,  
    [PresserManpower] TINYINT CONSTRAINT [DF_LineMappingBalancing_PresserManpower] DEFAULT (0) NOT NULL,  
    [TotalGSDTime] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_TotalGSDTime] DEFAULT (0) NOT NULL,  
    [HighestGSDTime] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_HighestGSDTime] DEFAULT (0) NOT NULL,  
    [TotalCycleTime] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_TotalCycleTime] DEFAULT (0) NOT NULL,  
    [HighestCycleTime] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_HighestCycleTime] DEFAULT (0) NOT NULL,  
    [TimeStudyID] BIGINT CONSTRAINT [DF_LineMappingBalancing_TimeStudyID] DEFAULT (0) NOT NULL,  
    [TimeStudyStatus] VARCHAR(15) CONSTRAINT [DF_LineMappingBalancing_TimeStudyStatus] DEFAULT ('') NOT NULL,  
    [TimeStudyVersion] VARCHAR(2) CONSTRAINT [DF_LineMappingBalancing_TimeStudyVersion] DEFAULT ('') NOT NULL,  
    [WorkHour] NUMERIC(3, 1) CONSTRAINT [DF_LineMappingBalancing_WorkHour] DEFAULT (0) NOT NULL,  
    [Status] VARCHAR(9) CONSTRAINT [DF_LineMappingBalancing_Status] DEFAULT ('') NOT NULL,  
    [CFMName] VARCHAR(10) CONSTRAINT [DF_LineMappingBalancing_CFMName] DEFAULT ('') NOT NULL,  
    [CFMDate] DATETIME  NULL,  
    [AddName] VARCHAR(10) CONSTRAINT [DF_LineMappingBalancing_AddName] DEFAULT ('') NOT NULL,  
    [AddDate] DATETIME  NULL,  
    [EditName] VARCHAR(10) CONSTRAINT [DF_LineMappingBalancing_EditName] DEFAULT ('') NOT NULL,  
    [EditDate] DATETIME NULL, 
    [OriNoNumber] INT    CONSTRAINT [DF_LineMappingBalancing_OriNoNumber] DEFAULT (0) NOT NULL , 
    [Reason] VARCHAR(5) CONSTRAINT [DF_LineMappingBalancing_Reason] DEFAULT ('') NOT NULL, 
    [OriTotalGSDTime] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_OriTotalGSDTime] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_LineMappingBalancing] PRIMARY KEY CLUSTERED ([ID] ASC)
)

GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LM單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM階段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'Phase'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM版號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車工人力',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'SewerManpower'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'包裝人力',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'PackerManpower'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'整燙人力',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'PresserManpower'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總GSD時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'TotalGSDTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最高的GSD時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'HighestGSDTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD版號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyVersion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'每站工時',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'WorkHour'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Line Mapping狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'AutomatedLineMappingID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'紀錄從P05轉來時的站台數量，插入/刪除超過五站後，Save時會需要填表頭Reason',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'OriNoNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'插入/刪除超過五站後，Save時需要填Reason，IEReason.Type = LN',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'Reason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'P05的總GSD時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing',
    @level2type = N'COLUMN',
    @level2name = N'OriTotalGSDTime'