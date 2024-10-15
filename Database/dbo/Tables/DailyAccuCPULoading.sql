CREATE TABLE [dbo].[DailyAccuCPULoading]
(
	 [ukey]							bigint				CONSTRAINT [DF_DailyAccuCPULoading_ukey]						NOT NULL IDENTITY(1,1)
	,[Year]							varchar(4)			CONSTRAINT [DF_DailyAccuCPULoading_Year]						NOT NULL DEFAULT ((''))
	,[Month]						varchar(2)			CONSTRAINT [DF_DailyAccuCPULoading_Month]						NOT NULL DEFAULT ((''))
	,[FactoryID]					varchar(8)			CONSTRAINT [DF_DailyAccuCPULoading_FactoryID]					NOT NULL DEFAULT (('')) 
	,[TTLCPULoaded]					decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_TTLCPULoaded]				NOT NULL DEFAULT ((0))
	,[UnfinishedLastMonth]			decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_UnfinishedLastMonth]			NOT NULL DEFAULT ((0))
	,[FinishedLastMonth]			decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_FinishedLastMonth]			NOT NULL DEFAULT ((0))
	,[CanceledStillNeedProd]		decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_CanceledStillNeedProd]		NOT NULL DEFAULT ((0))
	,[SubconToSisFactory]			decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_SubconToSisFactory]			NOT NULL DEFAULT ((0))
	,[SubconFromSisterFactory]		decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_SubconFromSisterFactory]		NOT NULL DEFAULT ((0))
	,[PullForwardFromNextMonths]	decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_PullForwardFromNextMonths]	NOT NULL DEFAULT ((0))
	,[LoadingDelayFromThisMonth]	decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_LoadingDelayFromThisMonth]	NOT NULL DEFAULT ((0))
	,[LocalSubconInCPU]				decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_LocalSubconInCPU]			NOT NULL DEFAULT ((0))
	,[LocalSubconOutCPU]			decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_LocalSubconOutCPU]			NOT NULL DEFAULT ((0))
	,[RemainCPUThisMonth]			decimal(10,3)		CONSTRAINT [DF_DailyAccuCPULoading_RemainCPUThisMonth]			NOT NULL DEFAULT ((0))
	,[AddDate]						varchar(10)			CONSTRAINT [DF_DailyAccuCPULoading_AddDate]						NULL 
	,[AddName]						datetime			CONSTRAINT [DF_DailyAccuCPULoading_AddName]						NOT NULL DEFAULT ((''))
	,[EditDate]						varchar(10)			CONSTRAINT [DF_DailyAccuCPULoading_EditDate]					NULL 
	,[EditName]						datetime			CONSTRAINT [DF_DailyAccuCPULoading_EditName]					NOT NULL DEFAULT (('')), 
    CONSTRAINT [PK_DailyAccuCPULoading] PRIMARY KEY ([Year],[Month],[FactoryID])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'統計年分',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'Year'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'統計月份',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'Month'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Loaded by Taipei planning by SCIDelivery',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'TTLCPULoaded'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Unfinished CPU in last month',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'UnfinishedLastMonth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'End of the month should check the volume for next month record',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'FinishedLastMonth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Cenceled but still need production loading by SCI',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'CanceledStillNeedProd'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Deduct the volume subcon by sister factory',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'SubconToSisFactory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Add the volume subcon from sister factory',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'SubconFromSisterFactory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Add Factory expect to advance from next months',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'PullForwardFromNextMonths'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Deduct Factory expect to delay to next months',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'LoadingDelayFromThisMonth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Add the volume that take from local subcon',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'LocalSubconInCPU'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Deduct the volume that take from local subcon',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'LocalSubconOutCPU'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The volume should be balanced into daily',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DailyAccuCPULoading',
    @level2type = N'COLUMN',
    @level2name = N'RemainCPUThisMonth'