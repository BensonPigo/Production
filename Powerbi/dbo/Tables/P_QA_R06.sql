CREATE TABLE [dbo].[P_QA_R06](
	[SuppID] [varchar](15) NOT NULL,
	[Refno] [varchar](50) NOT NULL,
	[SupplierName] [varchar](50) NULL,
	[BrandID] [varchar](500) NULL,
	[StockQty] [numeric](9, 2) NULL,
	[TotalInspYds] [numeric](10, 2) NULL,
	[TotalPoCnt] [int] NULL,
	[TotalDyelot] [int] NULL,
	[TotalDyelotAccepted] [int] NULL,
	[InspReport] [numeric](9, 2) NULL,
	[TestReport] [numeric](9, 2) NULL,
	[ContinuityCard] [numeric](9, 2) NULL,
	[BulkDyelot] [numeric](9, 2) NULL,
	[TotalPoint] [int] NULL,
	[TotalRoll] [int] NULL,
	[GradeARoll] [int] NULL,
	[GradeBRoll] [int] NULL,
	[GradeCRoll] [int] NULL,
	[Inspected] [numeric](9, 2) NULL,
	[Yds] [numeric](9, 2) NULL,
	[FabricPercent] [numeric](9, 2) NULL,
	[FabricLevel] [varchar](1) NULL,
	[Point] [varchar](150) NULL,
	[SHRINKAGEyards] [numeric](9, 2) NULL,
	[SHRINKAGEPercent] [numeric](9, 2) NULL,
	[SHINGKAGELevel] [varchar](1) NULL,
	[MIGRATIONyards] [numeric](9, 2) NULL,
	[MIGRATIONPercent] [numeric](9, 2) NULL,
	[MIGRATIONLevel] [varchar](1) NULL,
	[SHADINGyards] [numeric](9, 2) NULL,
	[SHADINGPercent] [numeric](9, 2) NULL,
	[SHADINGLevel] [varchar](1) NULL,
	[ActualYds] [numeric](9, 2) NULL,
	[LACKINGYARDAGEPercent] [numeric](9, 2) NULL,
	[LACKINGYARDAGELevel] [varchar](1) NULL,
	[SHORTWIDTH] [numeric](9, 2) NULL,
	[SHORTWidthPercent] [numeric](9, 2) NULL,
	[SHORTWIDTHLevel] [varchar](1) NULL,
	[TotalDefectRate] [numeric](9, 2) NULL,
	[TotalLevel] [varchar](1) NULL,
	[WhseArrival] [varchar](15) NOT NULL,
	[FactoryID] [varchar](30) NOT NULL,
	[Clima] [bit] NULL,
	[POID] [varchar](13) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_QA_R06] PRIMARY KEY CLUSTERED 
(
	[SuppID] ASC,
	[Refno] ASC,
	[FactoryID] ASC,
	[WhseArrival] ASC,
	[POID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SupplierName]  DEFAULT ('') FOR [SupplierName]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_StockQty]  DEFAULT ('') FOR [StockQty]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_TotalInspYds]  DEFAULT ((0)) FOR [TotalInspYds]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_TotalPoCnt]  DEFAULT ((0)) FOR [TotalPoCnt]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_TotalDyelot]  DEFAULT ((0)) FOR [TotalDyelot]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_]  DEFAULT ((0)) FOR [TotalDyelotAccepted]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_InspReport]  DEFAULT ((0)) FOR [InspReport]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_TestReport]  DEFAULT ((0)) FOR [TestReport]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_ContinuityCard]  DEFAULT ((0)) FOR [ContinuityCard]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_BulkDyelot]  DEFAULT ((0)) FOR [BulkDyelot]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_TotalPoint]  DEFAULT ((0)) FOR [TotalPoint]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_TotalRoll]  DEFAULT ((0)) FOR [TotalRoll]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_GradeARoll]  DEFAULT ((0)) FOR [GradeARoll]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_GradeBRoll]  DEFAULT ((0)) FOR [GradeBRoll]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_GradeCRoll]  DEFAULT ((0)) FOR [GradeCRoll]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_Inspected]  DEFAULT ((0)) FOR [Inspected]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_Yds]  DEFAULT ((0)) FOR [Yds]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_FabricPercent]  DEFAULT ((0)) FOR [FabricPercent]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_FabricLevel]  DEFAULT ('') FOR [FabricLevel]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_Point]  DEFAULT ('') FOR [Point]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHRINKAGEyards]  DEFAULT ((0)) FOR [SHRINKAGEyards]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHRINKAGEPercent]  DEFAULT ((0)) FOR [SHRINKAGEPercent]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHINGKAGELevel]  DEFAULT ('') FOR [SHINGKAGELevel]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_MIGRATIONyards]  DEFAULT ((0)) FOR [MIGRATIONyards]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_MIGRATIONPercent]  DEFAULT ((0)) FOR [MIGRATIONPercent]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_MIGRATIONLevel]  DEFAULT ('') FOR [MIGRATIONLevel]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHADINGyards]  DEFAULT ((0)) FOR [SHADINGyards]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHADINGPercent]  DEFAULT ((0)) FOR [SHADINGPercent]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHADINGLevel]  DEFAULT ('') FOR [SHADINGLevel]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_ActualYds]  DEFAULT ((0)) FOR [ActualYds]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_LACKINGYARDAGEPercent]  DEFAULT ((0)) FOR [LACKINGYARDAGEPercent]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_LACKINGYARDAGELevel]  DEFAULT ('') FOR [LACKINGYARDAGELevel]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHORTWIDTH]  DEFAULT ((0)) FOR [SHORTWIDTH]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHORTWidthPercent]  DEFAULT ((0)) FOR [SHORTWidthPercent]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_SHORTWIDTHLevel]  DEFAULT ('') FOR [SHORTWIDTHLevel]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_TotalDefectRate]  DEFAULT ((0)) FOR [TotalDefectRate]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_TotalLevel]  DEFAULT ('') FOR [TotalLevel]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_Clima]  DEFAULT ((0)) FOR [Clima]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_QA_R06] ADD  CONSTRAINT [DF_P_QA_R06_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_R06', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_R06', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
