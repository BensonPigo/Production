CREATE TABLE [dbo].[LineMappingBalancing_NotHitTargetReason]
(
	[ID] INT CONSTRAINT [DF_LineMappingBalancing_NotHitTargetReason_ID] DEFAULT (0) NOT NULL, 
    [No] VARCHAR(2) CONSTRAINT [DF_LineMappingBalancing_NotHitTargetReason_No] DEFAULT ('') NOT NULL, 
    [TotalCycleTimeAuto] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_NotHitTargetReason_TotalCycleTimeAuto] DEFAULT (0) NOT NULL, 
    [TotalCycleTimeFinal] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_NotHitTargetReason_TotalCycleTimeFinal] DEFAULT (0) NOT NULL, 
    [SewerLoadingAuto] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_NotHitTargetReason_SewerLoadingAuto] DEFAULT (0) NOT NULL, 
    [SewerLoadingFinal] NUMERIC(12, 2) CONSTRAINT [DF_LineMappingBalancing_NotHitTargetReason_SewerLoadingFinal] DEFAULT (0) NOT NULL, 
    [IEReasonID] VARCHAR(5) CONSTRAINT [DF_LineMappingBalancing_NotHitTargetReason_IEReasonID] DEFAULT ('') NOT NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_LineMappingBalancing_NotHitTargetReason_EditName] DEFAULT ('') NOT NULL, 
    [EditDate] DATETIME NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該站位系統原本排的秒數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'TotalCycleTimeAuto'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該站位系統, 工廠手動排的秒數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'TotalCycleTimeFinal'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該站系統算出來該車工的Loading',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'SewerLoadingAuto'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該站手動調整後車工的Loading',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'SewerLoadingFinal'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'沒達目標的原因',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMappingBalancing_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'IEReasonID'