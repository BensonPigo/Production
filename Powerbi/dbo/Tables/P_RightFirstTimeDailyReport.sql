CREATE TABLE [dbo].[P_RightFirstTimeDailyReport](
	[FactoryID] [varchar](8) NOT NULL,
	[CDate] [date] NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[Destination] [varchar](30) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[CDCodeID] [varchar](6) NOT NULL,
	[CDCodeNew] [varchar](max) NOT NULL,
	[ProductType] [nvarchar](500) NOT NULL,
	[FabricType] [nvarchar](500) NOT NULL,
	[Lining] [varchar](20) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Construction] [nvarchar](100) NOT NULL,
	[Team] [varchar](5) NOT NULL,
	[Shift] [varchar](1) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[Cell] [varchar](2) NOT NULL,
	[InspectQty] [numeric](7, 0) NOT NULL,
	[RejectQty] [numeric](7, 0) NOT NULL,
	[RFTPercentage] [numeric](7, 2) NOT NULL,
	[Over] [varchar](15) NOT NULL,
	[QC] [decimal](15, 4) NOT NULL,
	[Remark] [nvarchar](60) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_RightFirstTimeDailyReport] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[CDate] ASC,
	[OrderID] ASC,
	[Team] ASC,
	[Shift] ASC,
	[Line] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Destination]  DEFAULT ('') FOR [Destination]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_CDCodeID]  DEFAULT ('') FOR [CDCodeID]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_CDCodeNew]  DEFAULT ('') FOR [CDCodeNew]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_ProductType]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Lining]  DEFAULT ('') FOR [Lining]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Gender]  DEFAULT ('') FOR [Gender]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Construction]  DEFAULT ('') FOR [Construction]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Team]  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Shift]  DEFAULT ('') FOR [Shift]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Line]  DEFAULT ('') FOR [Line]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Cell]  DEFAULT ('') FOR [Cell]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_InspectQty]  DEFAULT ((0)) FOR [InspectQty]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_RejectQty]  DEFAULT ((0)) FOR [RejectQty]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_RFTPercentage]  DEFAULT ((0)) FOR [RFTPercentage]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Over]  DEFAULT ('') FOR [Over]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_QC]  DEFAULT ((0)) FOR [QC]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[P_RightFirstTimeDailyReport] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'CDCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CDCodeNew' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'CDCodeNew'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ReasonName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ReasonName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Lining'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Construction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Construction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Cell'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'InspectQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退回數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'RejectQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RFT(%)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'RFTPercentage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'確認' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Over'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'QC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO