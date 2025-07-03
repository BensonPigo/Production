CREATE TABLE [dbo].[P_ReplacementReport](
	[ID] [varchar](13) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SPNo] [varchar](13) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[Season] [varchar](10) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[Status] [varchar](15) NOT NULL,
	[Cdate] [date] NULL,
	[FtyApvDate] [date] NULL,
	[CompleteDate] [date] NULL,
	[LockDate] [date] NULL,
	[Responsibility] [nvarchar](100) NOT NULL,
	[TtlEstReplacementAMT] [numeric](20, 2) NOT NULL,
	[RMtlUS] [numeric](11, 2) NOT NULL,
	[ActFreightUS] [numeric](10, 2) NOT NULL,
	[EstFreightUS] [numeric](10, 2) NOT NULL,
	[SurchargeUS] [numeric](10, 2) NOT NULL,
	[TotalUS] [numeric](12, 2) NOT NULL,
	[ResponsibilityFty] [varchar](8) NOT NULL,
	[ResponsibilityDept] [varchar](8) NOT NULL,
	[ResponsibilityPercent] [numeric](5, 2) NOT NULL,
	[ShareAmount] [numeric](11, 2) NOT NULL,
	[VoucherNo] [varchar](16) NOT NULL,
	[VoucherDate] [date] NULL,
	[POSMR] [varchar](45) NOT NULL,
	[POHandle] [varchar](45) NOT NULL,
	[PCSMR] [varchar](45) NOT NULL,
	[PCHandle] [varchar](45) NOT NULL,
	[Prepared] [varchar](45) NOT NULL,
	[PPIC/Factory mgr] [varchar](45) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_ReplacementReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[FactoryID] ASC,
	[ResponsibilityFty] ASC,
	[ResponsibilityDept] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_SPNo]  DEFAULT ('') FOR [SPNo]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_Brand]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_Status]  DEFAULT ('') FOR [Status]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_Responsibility]  DEFAULT ('') FOR [Responsibility]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_TtlEstReplacementAMT]  DEFAULT ((0)) FOR [TtlEstReplacementAMT]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_RMtlUS]  DEFAULT ((0)) FOR [RMtlUS]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_ActFreightUS]  DEFAULT ((0)) FOR [ActFreightUS]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_EstFreightUS]  DEFAULT ((0)) FOR [EstFreightUS]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_SurchargeUS]  DEFAULT ((0)) FOR [SurchargeUS]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_TotalUS]  DEFAULT ((0)) FOR [TotalUS]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_ResponsibilityFty]  DEFAULT ('') FOR [ResponsibilityFty]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_ResponsibilityDept]  DEFAULT ('') FOR [ResponsibilityDept]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_ResponsibilityPercent]  DEFAULT ((0)) FOR [ResponsibilityPercent]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_ShareAmount]  DEFAULT ((0)) FOR [ShareAmount]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_VoucherNo]  DEFAULT ('') FOR [VoucherNo]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_POSMR]  DEFAULT ('') FOR [POSMR]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_POHandle]  DEFAULT ('') FOR [POHandle]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_PCSMR]  DEFAULT ('') FOR [PCSMR]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_PCHandle]  DEFAULT ('') FOR [PCHandle]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_Prepared]  DEFAULT ('') FOR [Prepared]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_PPIC/Factory mgr]  DEFAULT ('') FOR [PPIC/Factory mgr]
GO

ALTER TABLE [dbo].[P_ReplacementReport] ADD  CONSTRAINT [DF_P_ReplacementReport_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ReplacementReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ReplacementReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO