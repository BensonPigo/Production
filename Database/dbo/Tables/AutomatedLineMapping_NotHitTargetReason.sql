CREATE TABLE [dbo].[AutomatedLineMapping_NotHitTargetReason]
(
	[ID] INT CONSTRAINT [DF_AutomatedLineMapping_NotHitTargetReason_ID] DEFAULT (0) NOT NULL, 
    [No] VARCHAR(2) CONSTRAINT [DF_AutomatedLineMapping_NotHitTargetReason_No] DEFAULT ('') NOT NULL, 
    [TotalGSDTimeAuto] NUMERIC(12, 2) CONSTRAINT [DF_AutomatedLineMapping_NotHitTargetReason_TotalGSDTimeAuto] DEFAULT (0) NOT NULL, 
    [TotalGSDTimeFinal] NUMERIC(12, 2) CONSTRAINT [DF_AutomatedLineMapping_NotHitTargetReason_TotalGSDTimeFinal] DEFAULT (0) NOT NULL, 
    [SewerLoadingAuto] NUMERIC(12, 2) CONSTRAINT [DF_AutomatedLineMapping_NotHitTargetReason_SewerLoadingAuto] DEFAULT (0) NOT NULL, 
    [SewerLoadingFinal] NUMERIC(12, 2) CONSTRAINT [DF_AutomatedLineMapping_NotHitTargetReason_SewerLoadingFinal] DEFAULT (0) NOT NULL, 
    [IEReasonID] VARCHAR(5) CONSTRAINT [DF_AutomatedLineMapping_NotHitTargetReason_IEReasonID] DEFAULT ('') NOT NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMapping_NotHitTargetReason_EditName] DEFAULT ('') NOT NULL, 
    [EditDate] DATETIME NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'站位編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'No'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該站位系統原本排的秒數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'TotalGSDTimeAuto'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該站位系統, 工廠手動排的秒數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'TotalGSDTimeFinal'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該站系統算出來該車工的Loading',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'SewerLoadingAuto'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該站手動調整後車工的Loading',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'SewerLoadingFinal'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'沒達目標的原因',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_NotHitTargetReason',
    @level2type = N'COLUMN',
    @level2name = N'IEReasonID'