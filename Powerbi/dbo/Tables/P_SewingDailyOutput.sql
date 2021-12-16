﻿CREATE TABLE [dbo].[P_SewingDailyOutput](
	[Ukey] [bigint] NOT NULL,
	[MDivisionID] [varchar](20) NOT NULL,
	[FactoryID] [varchar](20) NULL,
	[ComboType] [varchar](1) NOT NULL,
	[Category] [varchar](20) NULL,
	[CountryID] [varchar](20) NULL,
	[OutputDate] [date] NULL,
	[SewingLineID] [varchar](10) NULL,
	[Shift] [varchar](30) NULL,
	[SubconOutFty] [varchar](15) NULL,
	[SubConOutContractNumber] [varchar](50) NULL,
	[Team] [varchar](10) NULL,
	[OrderID] [varchar](13) NULL,
	[Article] [varchar](8) NULL,
	[SizeCode] [varchar](8) NULL,
	[CustPONo] [varchar](30) NULL,
	[BuyerDelivery] [date] NULL,
	[OrderQty] [int] NULL,
	[BrandID] [varchar](20) NULL,
	[OrderCategory] [varchar](20) NULL,
	[ProgramID] [varchar](20) NULL,
	[OrderTypeID] [varchar](20) NULL,
	[DevSample] [varchar](5) NULL,
	[CPURate] [numeric](15, 1) NULL,
	[StyleID] [varchar](20) NULL,
	[Season] [varchar](10) NULL,
	[CdCodeID] [varchar](15) NULL,
	[ActualManpower] [numeric](12, 1) NULL,
	[NoOfHours] [numeric](12, 3) NULL,
	[TotalManhours] [numeric](12, 3) NULL,
	[TargetCPU] [numeric](10, 3) NULL,
	[TMS] [int] NULL,
	[CPUPrice] [numeric](10, 3) NULL,
	[TargetQty] [int] NULL,
	[TotalOutputQty] [int] NULL,
	[TotalCPU] [numeric](10, 3) NULL,
	[CPUSewerHR] [numeric](10, 3) NULL,
	[EFF] [numeric](10, 2) NULL,
	[RFT] [numeric](10, 2) NULL,
	[CumulateOfDays] [int] NULL,
	[DateRange] [varchar](15) NULL,
	[ProdOutput] [int] NULL,
	[Diff] [int] NULL,
	[Rate] [numeric](10, 2) NULL,
	[SewingReasonDesc] [nvarchar](1000) NULL,
	[SciDelivery] [date] NULL,
	[CDCodeNew] [varchar](5) NULL,
	[ProductType] [nvarchar](500) NULL,
	[FabricType] [nvarchar](500) NULL,
	[Lining] [varchar](20) NULL,
	[Gender] [varchar](10) NULL,
	[Construction] [nvarchar](50) NULL,
 CONSTRAINT [PK_P_SewingDailyOutput] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[MDivisionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CountryID]  DEFAULT ('') FOR [CountryID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Shift]  DEFAULT ('') FOR [Shift]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SubconOutFty]  DEFAULT ('') FOR [SubconOutFty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SubConOutContractNumber]  DEFAULT ('') FOR [SubConOutContractNumber]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Team]  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SizeCode]  DEFAULT ('') FOR [SizeCode]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CustPONo]  DEFAULT ('') FOR [CustPONo]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_OrderCategory]  DEFAULT ('') FOR [OrderCategory]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_ProgramID]  DEFAULT ('') FOR [ProgramID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_OrderTypeID]  DEFAULT ('') FOR [OrderTypeID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_DevSample]  DEFAULT ('') FOR [DevSample]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CPURate]  DEFAULT ((0)) FOR [CPURate]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CdCodeID]  DEFAULT ('') FOR [CdCodeID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_ActualManpower]  DEFAULT ((0)) FOR [ActualManpower]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_NoOfHours]  DEFAULT ((0)) FOR [NoOfHours]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TotalManhours]  DEFAULT ((0)) FOR [TotalManhours]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TargetCPU]  DEFAULT ((0)) FOR [TargetCPU]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TMS]  DEFAULT ((0)) FOR [TMS]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CPUPrice]  DEFAULT ((0)) FOR [CPUPrice]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TargetQty]  DEFAULT ((0)) FOR [TargetQty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TotalOutputQty]  DEFAULT ((0)) FOR [TotalOutputQty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TotalCPU]  DEFAULT ((0)) FOR [TotalCPU]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CPUSewerHR]  DEFAULT ((0)) FOR [CPUSewerHR]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_EFF]  DEFAULT ((0)) FOR [EFF]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_RFT]  DEFAULT ((0)) FOR [RFT]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CumulateOfDays]  DEFAULT ((0)) FOR [CumulateOfDays]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_DateRange]  DEFAULT ('') FOR [DateRange]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_ProdOutput]  DEFAULT ((0)) FOR [ProdOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Diff]  DEFAULT ((0)) FOR [Diff]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Rate]  DEFAULT ((0)) FOR [Rate]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SewingReasonDesc]  DEFAULT ('') FOR [SewingReasonDesc]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingOutput_Detail_Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manufacturing Division ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組合型態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order or Mockup order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國家別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CountryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發外工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SubconOutFty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發外條款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SubConOutContractNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SizeCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CustPONo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OrderCategory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ProgramID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OrderTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'開發樣品 OrderType.IsDevSample轉入寫入 Y/N' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'DevSample'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單CPU Rate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CPURate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CdCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ActualManpower'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均一人工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'NoOfHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總人力工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TotalManhours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目標CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TargetCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costing TMS (sec)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每件需多少CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CPUPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目標數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TargetQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際產出數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TotalOutputQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TotalCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直接人員每人每小時產出(PPH)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CPUSewerHR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'效率值EFF(%)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'EFF'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Right First Time(%)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'RFT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該Style在這條線上累積做多久' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CumulateOfDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等同於CumulateOfDays，當大於10則顯示>=10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'DateRange'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InlineQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ProdOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'差異(QAQty-InlineQty)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Diff'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Rate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原因描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SewingReasonDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飛雁交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SciDelivery'
GO
