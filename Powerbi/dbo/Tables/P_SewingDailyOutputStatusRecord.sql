CREATE TABLE [dbo].[P_SewingDailyOutputStatusRecord](
	[SewingLineID] [varchar](5) NOT NULL,
	[SewingOutputDate] [date] NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SPNo] [varchar](13) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[CDCodeNew] [varchar](5) NOT NULL,
	[Article] [varchar](200) NOT NULL,
	[POID] [varchar](13) NOT NULL,
	[Category] [varchar](1) NOT NULL,
	[SCIDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[OrderQty] [int] NOT NULL,
	[AlloQty] [int] NOT NULL,
	[Artwork] [varchar](200) NOT NULL,
	[JITDate] [date] NULL,
	[BCSDate] [date] NULL,
	[SewingInLine] [date] NULL,
	[ReadyDate] [date] NULL,
	[SewingOffLine] [date] NULL,
	[StardardOutputPerDay] [int] NOT NULL,
	[WorkHourPerDay] [numeric](11, 6) NOT NULL,
	[CuttingOutput] [int] NOT NULL,
	[CuttingRemark] [nvarchar](50) NOT NULL,
	[Consumption] [numeric](18, 4) NOT NULL,
	[ActConsOutput] [numeric](18, 4) NOT NULL,
	[LoadingOutput] [int] NOT NULL,
	[LoadingRemark] [varchar](50) NOT NULL,
	[LoadingExclusion] [bit] NOT NULL,
	[ATOutput] [int] NOT NULL,
	[ATRemark] [varchar](50) NOT NULL,
	[ATExclusion] [bit] NOT NULL,
	[AUTOutput] [int] NOT NULL,
	[AUTRemark] [varchar](50) NOT NULL,
	[AUTExclusion] [bit] NOT NULL,
	[HTOutput] [int] NOT NULL,
	[HTRemark] [varchar](50) NOT NULL,
	[HTExclusion] [bit] NOT NULL,
	[BOOutput] [int] NOT NULL,
	[BORemark] [varchar](50) NOT NULL,
	[BOExclusion] [bit] NOT NULL,
	[FMOutput] [int] NOT NULL,
	[FMRemark] [varchar](50) NOT NULL,
	[FMExclusion] [bit] NOT NULL,
	[PRTOutput] [int] NOT NULL,
	[PRTRemark] [varchar](50) NOT NULL,
	[PRTExclusion] [bit] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
	[PADPRTOutput] [int] NOT NULL,
	[PADPRTRemark] [varchar](50) NOT NULL,
	[PADPRTExclusion] [bit] NOT NULL,
	[EMBOutput] [int] NOT NULL,
	[EMBRemark] [varchar](50) NOT NULL,
	[EMBExclusion] [bit] NOT NULL,
	[FIOutput] [int] NOT NULL,
	[FIRemark] [varchar](50) NOT NULL,
	[FIExclusion] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SewingLineID] ASC,
	[SewingOutputDate] ASC,
	[SPNo] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SPNo]  DEFAULT ('') FOR [SPNo]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CDCodeNew]  DEFAULT ('') FOR [CDCodeNew]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AlloQty]  DEFAULT ((0)) FOR [AlloQty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Artwork]  DEFAULT ('') FOR [Artwork]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_StardardOutputPerDay]  DEFAULT ((0)) FOR [StardardOutputPerDay]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_WorkHourPerDay]  DEFAULT ((0)) FOR [WorkHourPerDay]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CuttingOutput]  DEFAULT ((0)) FOR [CuttingOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CuttingRemark]  DEFAULT ('') FOR [CuttingRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Consumption]  DEFAULT ((0)) FOR [Consumption]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ActConsOutput]  DEFAULT ((0)) FOR [ActConsOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingOutput]  DEFAULT ((0)) FOR [LoadingOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingRemark]  DEFAULT ('') FOR [LoadingRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingExclusion]  DEFAULT ((0)) FOR [LoadingExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATOutput]  DEFAULT ((0)) FOR [ATOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATRemark]  DEFAULT ('') FOR [ATRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATExclusion]  DEFAULT ((0)) FOR [ATExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTOutput]  DEFAULT ((0)) FOR [AUTOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTRemark]  DEFAULT ('') FOR [AUTRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTExclusion]  DEFAULT ((0)) FOR [AUTExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTOutput]  DEFAULT ((0)) FOR [HTOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTRemark]  DEFAULT ('') FOR [HTRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTExclusion]  DEFAULT ((0)) FOR [HTExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BOOutput]  DEFAULT ((0)) FOR [BOOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BORemark]  DEFAULT ('') FOR [BORemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BOExclusion]  DEFAULT ((0)) FOR [BOExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMOutput]  DEFAULT ((0)) FOR [FMOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMRemark]  DEFAULT ('') FOR [FMRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMExclusion]  DEFAULT ((0)) FOR [FMExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTOutput]  DEFAULT ((0)) FOR [PRTOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTRemark]  DEFAULT ('') FOR [PRTRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTExclusion]  DEFAULT ((0)) FOR [PRTExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PADPRTOutput]  DEFAULT ((0)) FOR [PADPRTOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PADPRTRemark]  DEFAULT ('') FOR [PADPRTRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PADPRTExclusion]  DEFAULT ((0)) FOR [PADPRTExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_EMBOutput]  DEFAULT ((0)) FOR [EMBOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_EMBRemark]  DEFAULT ('') FOR [EMBRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_EMBExclusion]  DEFAULT ((0)) FOR [EMBExclusion]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FIOutput]  DEFAULT ((0)) FOR [FIOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FIRemark]  DEFAULT ('') FOR [FIRemark]
GO

ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FIExclusion]  DEFAULT ((0)) FOR [FIExclusion]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingOutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CDCodeNew' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'CDCodeNew'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'買家交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單件量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計畫分配生產數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AlloQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Artwork' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'Artwork'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing InLine -14天(不含假日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'JITDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing InLine -2天(不含假日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BCSDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫進線日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨預備日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ReadyDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫下線日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingOffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準產出/日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'StardardOutputPerDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工時/日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'WorkHourPerDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁床' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'CuttingOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁床備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'CuttingRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'耗量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'Consumption'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實耗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ActConsOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loading' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'LoadingOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loading備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'LoadingRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loading不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'LoadingExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ATOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AT備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ATRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AT不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ATExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AUT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AUTOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AUT備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AUTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AUT不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AUTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'HTOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HT備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'HTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HT不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'HTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BOOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BO備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BORemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BO不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BOExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FMOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FM備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FMRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FM不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FMExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PRT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PRTOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PRT備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PRTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PRT不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PRTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PAD-PRT產出數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PADPRTOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PAD-PRT供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PADPRTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PAD-PRT完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PADPRTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段EMB產出數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'EMBOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段EMB供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'EMBRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段EMB完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'EMBExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FI產出數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FIOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FI供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FIRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FI完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FIExclusion'
GO
