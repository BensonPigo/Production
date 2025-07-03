CREATE TABLE [dbo].[P_DailyAccuCPULoading](
	[Year] [varchar](4) NOT NULL,
	[Month] [varchar](2) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[TTLCPULoaded] [int] NOT NULL,
	[UnfinishedLastMonth] [int] NOT NULL,
	[FinishedLastMonth] [int] NOT NULL,
	[CanceledStillNeedProd] [int] NOT NULL,
	[SubconToSisFactory] [int] NOT NULL,
	[SubconFromSisterFactory] [int] NOT NULL,
	[PullForwardFromNextMonths] [int] NOT NULL,
	[LoadingDelayFromThisMonth] [int] NOT NULL,
	[LocalSubconInCPU] [int] NOT NULL,
	[LocalSubconOutCPU] [int] NOT NULL,
	[RemainCPUThisMonth] [int] NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[Date] [varchar](5) NOT NULL,
	[WeekDay] [varchar](3) NOT NULL,
	[DailyCPULoading] [int] NOT NULL,
	[NewTarget] [int] NOT NULL,
	[ActCPUPerformed] [decimal](8, 3) NOT NULL,
	[DailyCPUVarience] [int] NOT NULL,
	[AccuLoading] [int] NOT NULL,
	[AccuActCPUPerformed] [int] NOT NULL,
	[AccuCPUVariance] [int] NOT NULL,
	[LeftWorkDays] [int] NOT NULL,
	[AvgWorkhours] [decimal](8, 2) NOT NULL,
	[PPH] [decimal](8, 2) NOT NULL,
	[Direct] [int] NOT NULL,
	[Active] [int] NOT NULL,
	[VPH] [decimal](8, 2) NOT NULL,
	[ManpowerRatio] [decimal](8, 2) NOT NULL,
	[LineNo] [int] NOT NULL,
	[LineManpower] [int] NOT NULL,
	[GPH] [decimal](8, 2) NOT NULL,
	[SPH] [decimal](8, 2) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_DailyAccuCPULoading] PRIMARY KEY CLUSTERED 
(
	[Year] ASC,
	[Month] ASC,
	[FactoryID] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ('') FOR [Year]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ('') FOR [Month]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [TTLCPULoaded]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [UnfinishedLastMonth]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [FinishedLastMonth]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [CanceledStillNeedProd]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [SubconToSisFactory]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [SubconFromSisterFactory]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [PullForwardFromNextMonths]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [LoadingDelayFromThisMonth]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [LocalSubconInCPU]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [LocalSubconOutCPU]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [RemainCPUThisMonth]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ('') FOR [Date]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ('') FOR [WeekDay]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [DailyCPULoading]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [NewTarget]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [ActCPUPerformed]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [DailyCPUVarience]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [AccuLoading]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [AccuActCPUPerformed]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [AccuCPUVariance]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [LeftWorkDays]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [AvgWorkhours]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [PPH]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [Direct]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [Active]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [VPH]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [ManpowerRatio]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [LineNo]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [LineManpower]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [GPH]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  DEFAULT ((0)) FOR [SPH]
GO

ALTER TABLE [dbo].[P_DailyAccuCPULoading] ADD  CONSTRAINT [DF_P_DailyAccuCPULoading_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO