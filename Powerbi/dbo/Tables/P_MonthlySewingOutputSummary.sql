CREATE TABLE [dbo].[P_MonthlySewingOutputSummary](
	[Fty] [varchar](8) NOT NULL,
	[Period] [varchar](6) NOT NULL,
	[LastDatePerMonth] [date] NULL,
	[TtlQtyExclSubconOut] [int] NOT NULL,
	[TtlCPUInclSubconIn] [numeric](10, 3) NOT NULL,
	[SubconInTtlCPU] [numeric](10, 3) NOT NULL,
	[SubconOutTtlCPU] [numeric](10, 3) NOT NULL,
	[PPH] [numeric](4, 2) NOT NULL,
	[AvgWorkHr] [numeric](4, 2) NOT NULL,
	[TtlManpower] [int] NOT NULL,
	[TtlManhours] [numeric](8, 1) NOT NULL,
	[Eff] [numeric](5, 2) NOT NULL,
	[AvgWorkHrPAMS] [numeric](4, 2) NOT NULL,
	[TtlManpowerPAMS] [int] NOT NULL,
	[TtlManhoursPAMS] [numeric](11, 4) NOT NULL,
	[EffPAMS] [numeric](5, 2) NOT NULL,
	[TransferManpowerPAMS] [int] NOT NULL,
	[TransferManhoursPAMS] [numeric](11, 4) NOT NULL,
	[TtlRevenue] [numeric](10, 3) NOT NULL,
	[TtlWorkDay] [tinyint] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_MonthlySewingOutputSummary] PRIMARY KEY CLUSTERED 
(
	[Fty] ASC,
	[Period] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_Fty]  DEFAULT ('') FOR [Fty]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_Period]  DEFAULT ('') FOR [Period]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlQtyExclSubconOut]  DEFAULT ((0)) FOR [TtlQtyExclSubconOut]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlCPUInclSubconIn]  DEFAULT ((0)) FOR [TtlCPUInclSubconIn]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_SubconInTtlCPU]  DEFAULT ((0)) FOR [SubconInTtlCPU]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_SubconOutTtlCPU]  DEFAULT ((0)) FOR [SubconOutTtlCPU]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_PPH]  DEFAULT ((0)) FOR [PPH]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_AvgWorkHr]  DEFAULT ((0)) FOR [AvgWorkHr]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlManpower]  DEFAULT ((0)) FOR [TtlManpower]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlManhours]  DEFAULT ((0)) FOR [TtlManhours]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_Eff]  DEFAULT ((0)) FOR [Eff]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_AvgWorkHrPAMS]  DEFAULT ((0)) FOR [AvgWorkHrPAMS]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlManpowerPAMS]  DEFAULT ((0)) FOR [TtlManpowerPAMS]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlManhoursPAMS]  DEFAULT ((0)) FOR [TtlManhoursPAMS]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_EffPAMS]  DEFAULT ((0)) FOR [EffPAMS]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TransferManpowerPAMS]  DEFAULT ((0)) FOR [TransferManpowerPAMS]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TransferManhoursPAMS]  DEFAULT ((0)) FOR [TransferManhoursPAMS]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlRevenue]  DEFAULT ((0)) FOR [TtlRevenue]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlWorkDay]  DEFAULT ((0)) FOR [TtlWorkDay]
GO

ALTER TABLE [dbo].[P_MonthlySewingOutputSummary] ADD  CONSTRAINT [DF_P_MonthlySewingOutputSummary_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Output Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'Fty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LastDatePerMonth的年月(YYYYMM)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'Period'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每月月底日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'LastDatePerMonth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Output(Qty) Exclude subcon-out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlQtyExclSubconOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total CPU Included Subcon-In' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlCPUInclSubconIn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subcon-In Total CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'SubconInTtlCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subcon-Out Total CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'SubconOutTtlCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPU/Sewer/HR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'PPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Average Working Hour' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'AvgWorkHr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Manpower' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlManpower'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Manhours' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlManhours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Eff%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'Eff'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Average Working Hour(PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'AvgWorkHrPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Manpower (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlManpowerPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Manhours (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlManhoursPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Eff% (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'EffPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Transfer Manpower (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TransferManpowerPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Transfer Manhours (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TransferManhoursPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Revenue (US$)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlRevenue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total work day' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlWorkDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO