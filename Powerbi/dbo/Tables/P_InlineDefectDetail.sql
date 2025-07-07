CREATE TABLE [dbo].[P_InlineDefectDetail](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Zone] [varchar](6) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[Team] [varchar](10) NOT NULL,
	[Shift] [varchar](5) NOT NULL,
	[CustPoNo] [varchar](30) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[OrderId] [varchar](13) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[FirstInspectionDate] [date] NULL,
	[FirstInspectedTime] [datetime] NULL,
	[InspectedQC] [nvarchar](30) NOT NULL,
	[ProductType] [varchar](10) NOT NULL,
	[Operation] [nvarchar](50) NOT NULL,
	[SewerName] [nvarchar](80) NOT NULL,
	[GarmentDefectTypeID] [varchar](3) NOT NULL,
	[GarmentDefectTypeDesc] [nvarchar](60) NOT NULL,
	[GarmentDefectCodeID] [varchar](5) NOT NULL,
	[GarmentDefectCodeDesc] [nvarchar](100) NOT NULL,
	[IsCriticalDefect] [varchar](1) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_InlineDefectDetail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_Line]  DEFAULT ('') FOR [Line]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_Team]  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_Shift]  DEFAULT ('') FOR [Shift]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_CustPoNo]  DEFAULT ('') FOR [CustPoNo]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_OrderId]  DEFAULT ('') FOR [OrderId]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_InspectedQC]  DEFAULT ('') FOR [InspectedQC]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_ProductType]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_Operation]  DEFAULT ('') FOR [Operation]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_SewerName]  DEFAULT ('') FOR [SewerName]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectTypeID]  DEFAULT ('') FOR [GarmentDefectTypeID]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectTypeDesc]  DEFAULT ('') FOR [GarmentDefectTypeDesc]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectCodeID]  DEFAULT ('') FOR [GarmentDefectCodeID]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectCodeDesc]  DEFAULT ('') FOR [GarmentDefectCodeDesc]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_IsCriticalDefect]  DEFAULT ('') FOR [IsCriticalDefect]
GO

ALTER TABLE [dbo].[P_InlineDefectDetail] ADD  CONSTRAINT [DF_P_InlineDefectDetail_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠地區別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Zone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線線別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'CustPoNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'OrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次檢驗日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'FirstInspectionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次檢驗時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'FirstInspectedTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inspected QC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'InspectedQC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Operation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewer NM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'SewerName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵種類代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'GarmentDefectTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵種類描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'GarmentDefectTypeDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'GarmentDefectCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'GarmentDefectCodeDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為嚴重defect code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'IsCriticalDefect'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
