CREATE TABLE [dbo].[P_InlineDefectSummary](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstInspectedDate] [date] NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[CustPoNo] [varchar](30) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[Alias] [varchar](30) NOT NULL,
	[CDCodeID] [varchar](6) NOT NULL,
	[CDCodeNew] [varchar](5) NOT NULL,
	[ProductType] [nvarchar](30) NOT NULL,
	[FabricType] [nvarchar](30) NOT NULL,
	[Lining] [varchar](20) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Construction] [nvarchar](30) NOT NULL,
	[ProductionFamilyID] [varchar](20) NOT NULL,
	[Team] [varchar](10) NOT NULL,
	[QCName] [varchar](100) NOT NULL,
	[Shift] [varchar](5) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[SewingCell] [varchar](2) NOT NULL,
	[InspectedQty] [int] NOT NULL,
	[RejectWIP] [int] NOT NULL,
	[InlineWFT] [int] NOT NULL,
	[InlineRFT] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_InlineDefectSummary] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_CustPoNo]  DEFAULT ('') FOR [CustPoNo]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_Alias]  DEFAULT ('') FOR [Alias]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_CDCodeID]  DEFAULT ('') FOR [CDCodeID]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_CDCodeNew]  DEFAULT ('') FOR [CDCodeNew]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_ProductType]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_Lining]  DEFAULT ('') FOR [Lining]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_Gender]  DEFAULT ('') FOR [Gender]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_Construction]  DEFAULT ('') FOR [Construction]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_ProductionFamilyID]  DEFAULT ('') FOR [ProductionFamilyID]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_Team]  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_QCName]  DEFAULT ('') FOR [QCName]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_Shift]  DEFAULT ('') FOR [Shift]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_Line]  DEFAULT ('') FOR [Line]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_SewingCell]  DEFAULT ('') FOR [SewingCell]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_InspectedQty]  DEFAULT ((0)) FOR [InspectedQty]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_RejectWIP]  DEFAULT ((0)) FOR [RejectWIP]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_InlineWFT]  DEFAULT ((0)) FOR [InlineWFT]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_InlineRFT]  DEFAULT ((0)) FOR [InlineRFT]
GO

ALTER TABLE [dbo].[P_InlineDefectSummary] ADD  CONSTRAINT [DF_P_InlineDefectSummary_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次檢驗日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'FirstInspectedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'CustPoNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地區' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Alias'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'CDCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'New CD Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'CDCodeNew'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產品種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平織/針織' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Lining'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Construction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Construction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產品種類群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'ProductionFamilyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QC Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'QCName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線線別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'SewingCell'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectedQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'InspectedQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RejectWIP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'RejectWIP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InlineWFT ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'InlineWFT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InlineRFT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'InlineRFT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO