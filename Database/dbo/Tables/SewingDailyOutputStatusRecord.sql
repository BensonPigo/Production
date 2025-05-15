CREATE TABLE [dbo].[SewingDailyOutputStatusRecord](
	[SewingLineID] [varchar](5) NOT NULL,
	[SewingOutputDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[SewingInLine] [datetime] NULL,
	[SewingOffLine] [datetime] NULL,
	[StandardOutputPerDay] [int] NOT NULL,
	[CuttingRemark] [varchar](50) NOT NULL,
	[LoadingRemark] [varchar](50) NOT NULL,
	[LoadingExclusion] [bit] NOT NULL,
	[ATRemark] [varchar](50) NOT NULL,
	[ATExclusion] [bit] NOT NULL,
	[AUTRemark] [varchar](50) NOT NULL,
	[AUTExclusion] [bit] NOT NULL,
	[HTRemark] [varchar](50) NOT NULL,
	[HTExclusion] [bit] NOT NULL,
	[BORemark] [varchar](50) NOT NULL,
	[BOExclusion] [bit] NOT NULL,
	[FMRemark] [varchar](50) NOT NULL,
	[FMExclusion] [bit] NOT NULL,
	[PRTRemark] [varchar](50) NOT NULL,
	[PRTExclusion] [bit] NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[PADPRTRemark] [varchar](50) NOT NULL,
	[PADPRTExclusion] [bit] NOT NULL,
	[EMBRemark] [varchar](50) NOT NULL,
	[EMBExclusion] [bit] NOT NULL,
	[FIRemark] [varchar](50) NOT NULL,
	[FIExclusion] [bit] NOT NULL,
 CONSTRAINT [PK_SewingDailyOutputStatusRecord] PRIMARY KEY CLUSTERED 
(
	[SewingLineID] ASC,
	[SewingOutputDate] ASC,
	[OrderID] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_StandardOutputPerDay]  DEFAULT ((0)) FOR [StandardOutputPerDay]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_CuttingRemark]  DEFAULT ('') FOR [CuttingRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_LoadingRemark]  DEFAULT ('') FOR [LoadingRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_LoadingExclusion]  DEFAULT ((0)) FOR [LoadingExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_ATRemark]  DEFAULT ('') FOR [ATRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_ATExclusion]  DEFAULT ((0)) FOR [ATExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_AUTRemark]  DEFAULT ('') FOR [AUTRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_AUTExclusion]  DEFAULT ((0)) FOR [AUTExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_HTRemark]  DEFAULT ('') FOR [HTRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_HTExclusion]  DEFAULT ((0)) FOR [HTExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_BORemark]  DEFAULT ('') FOR [BORemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_BOExclusion]  DEFAULT ((0)) FOR [BOExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_FMRemark]  DEFAULT ('') FOR [FMRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_FMExclusion]  DEFAULT ((0)) FOR [FMExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_PRTRemark]  DEFAULT ('') FOR [PRTRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_PRTExclusion]  DEFAULT ((0)) FOR [PRTExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_PADPRTRemark]  DEFAULT ('') FOR [PADPRTRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_PADPRTExclusion]  DEFAULT ((0)) FOR [PADPRTExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_EMBRemark]  DEFAULT ('') FOR [EMBRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_EMBExclusion]  DEFAULT ((0)) FOR [EMBExclusion]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_FIRemark]  DEFAULT ('') FOR [FIRemark]
GO

ALTER TABLE [dbo].[SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_SewingDailyOutputStatusRecord_FIExclusion]  DEFAULT ((0)) FOR [FIExclusion]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingOutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排程開始日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排程結束日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingOffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每日標準產出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'StandardOutputPerDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段Cutting供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'CuttingRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段Loading供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'LoadingRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段Loading完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'LoadingExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段AT供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ATRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段AT完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ATExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段AUT供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AUTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段AUT完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AUTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段HT供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'HTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段HT完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'HTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段BO供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BORemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段BO完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BOExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FM供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FMRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FM完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FMExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PRT供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PRTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PRT完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PRTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PAD-PRT供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PADPRTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PAD-PRT完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PADPRTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段EMB供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'EMBRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段EMB完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'EMBExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FI供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FIRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FI完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FIExclusion'
GO