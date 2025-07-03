CREATE TABLE [dbo].[P_LoadingProductionOutput](
	[MDivisionID] [varchar](8) NULL,
	[FtyZone] [varchar](8) NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[SciDelivery] [date] NULL,
	[SCIKey] [nvarchar](4000) NULL,
	[SCIKeyHalf] [nvarchar](4000) NULL,
	[BuyerKey] [nvarchar](4000) NULL,
	[BuyerKeyHalf] [nvarchar](4000) NULL,
	[SPNO] [varchar](24) NULL,
	[Category] [varchar](8) NULL,
	[Cancelled] [varchar](1) NOT NULL,
	[IsCancelNeedProduction] [varchar](1) NOT NULL,
	[PartialShipment] [varchar](1) NULL,
	[LastBuyerDelivery] [date] NULL,
	[StyleID] [varchar](15) NULL,
	[SeasonID] [varchar](10) NULL,
	[CustPONO] [varchar](30) NULL,
	[BrandID] [varchar](8) NULL,
	[CPU] [numeric](8, 3) NULL,
	[Qty] [int] NULL,
	[FOCQty] [int] NULL,
	[PulloutQty] [int] NULL,
	[OrderShortageCPU] [numeric](23, 4) NULL,
	[TotalCPU] [numeric](23, 4) NULL,
	[SewingOutput] [numeric](38, 6) NULL,
	[SewingOutputCPU] [numeric](38, 6) NULL,
	[BalanceQty] [numeric](38, 6) NULL,
	[BalanceCPU] [numeric](38, 6) NULL,
	[BalanceCPUIrregular] [numeric](38, 6) NULL,
	[SewLine] [varchar](60) NULL,
	[Dest] [varchar](2) NULL,
	[OrderTypeID] [varchar](20) NULL,
	[ProgramID] [varchar](12) NULL,
	[CdCodeID] [varchar](6) NULL,
	[ProductionFamilyID] [varchar](20) NULL,
	[FtyGroup] [varchar](8) NULL,
	[PulloutComplete] [varchar](2) NOT NULL,
	[SewInLine] [date] NULL,
	[SewOffLine] [date] NULL,
	[TransFtyZone] [varchar](8) NULL,
	[CDCodeNew] [varchar](5) NULL,
	[ProductType] [nvarchar](500) NULL,
	[FabricType] [nvarchar](500) NULL,
	[Lining] [varchar](20) NULL,
	[Gender] [varchar](10) NULL,
	[Construction] [nvarchar](50) NULL,
	[FM Sister] [varchar](1) NULL,
	[Sample Group] [nvarchar](50) NULL,
	[Order Reason] [nvarchar](500) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[BuyBackReason] [varchar](20) NOT NULL,
	[LastProductionDate] [date] NULL,
	[CRDDate] [date] NULL,
	[BuyerMonthHalf] [nvarchar](4000) NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_LoadingProductionOutput] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_LoadingProductionOutput] ADD  CONSTRAINT [DF_P_LoadingProductionOutput_BuyBackReason]  DEFAULT ('') FOR [BuyBackReason]
GO

ALTER TABLE [dbo].[P_LoadingProductionOutput] ADD  CONSTRAINT [DF_P_LoadingProductionOutput_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyBack Reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'BuyBackReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LastProductionDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'LastProductionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CRD date.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'CRDDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO