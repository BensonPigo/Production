CREATE TABLE [dbo].[P_OustandingPO](
	[FactoryID] [varchar](8) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[CustPONo] [varchar](30) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[Seq] [varchar](2) NOT NULL,
	[ShipModeID] [varchar](10) NOT NULL,
	[Category] [varchar](10) NOT NULL,
	[PartialShipment] [varchar](1) NOT NULL,
	[Junk] [varchar](1) NOT NULL,
	[OrderQty] [int] NOT NULL,
	[PackingCtn] [int] NOT NULL,
	[PackingQty] [int] NOT NULL,
	[ClogRcvCtn] [int] NOT NULL,
	[ClogRcvQty] [varchar](10) NOT NULL,
	[LastCMPOutputDate] [date] NULL,
	[CMPQty] [varchar](10) NOT NULL,
	[LastDQSOutputDate] [date] NULL,
	[DQSQty] [varchar](10) NOT NULL,
	[OSTPackingQty] [varchar](10) NOT NULL,
	[OSTCMPQty] [varchar](10) NOT NULL,
	[OSTDQSQty] [varchar](10) NOT NULL,
	[OSTClogQty] [varchar](10) NOT NULL,
	[OSTClogCtn] [int] NOT NULL,
	[PulloutComplete] [varchar](1) NOT NULL,
	[Dest] [varchar](30) NULL,
	[KPIGroup] [varchar](8) NULL,
	[CancelledButStillNeedProduction] [varchar](8) NULL,
	[CFAInspectionResult] [varchar](16) NULL,
	[3rdPartyInspection] [varchar](8) NULL,
	[3rdPartyInspectionResult] [varchar](16) NULL,
	[BookingSP] [varchar](200) NOT NULL,
	[LastCartonReceivedDate] [datetime] NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_OustandingPO] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[OrderID] ASC,
	[Seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_CustPONo]  DEFAULT ('') FOR [CustPONo]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_ShipModeID]  DEFAULT ('') FOR [ShipModeID]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_PartialShipment]  DEFAULT ('') FOR [PartialShipment]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_Junk]  DEFAULT ('') FOR [Junk]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_PackingCtn]  DEFAULT ((0)) FOR [PackingCtn]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_PackingQty]  DEFAULT ((0)) FOR [PackingQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_ClogRcvCtn]  DEFAULT ((0)) FOR [ClogRcvCtn]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_ClogRcvQty]  DEFAULT ((0)) FOR [ClogRcvQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_CMPQty]  DEFAULT ((0)) FOR [CMPQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_DQSQty]  DEFAULT ('') FOR [DQSQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_OSTPackingQty]  DEFAULT ('') FOR [OSTPackingQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_OSTCMPQty]  DEFAULT ('') FOR [OSTCMPQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_OSTDQSQty]  DEFAULT ('') FOR [OSTDQSQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_OSTClogQty]  DEFAULT ('') FOR [OSTClogQty]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_OSTClogCtn]  DEFAULT ((0)) FOR [OSTClogCtn]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_PulloutComplete]  DEFAULT ('') FOR [PulloutComplete]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_KPIGroup]  DEFAULT ('') FOR [KPIGroup]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_CancelledButStillNeedProduction]  DEFAULT ('') FOR [CancelledButStillNeedProduction]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_CFAInspectionResult]  DEFAULT ('') FOR [CFAInspectionResult]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_3rdPartyInspection]  DEFAULT ('') FOR [3rdPartyInspection]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_3rdPartyInspectionResult]  DEFAULT ('') FOR [3rdPartyInspectionResult]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_BookingSP]  DEFAULT ('') FOR [BookingSP]
GO

ALTER TABLE [dbo].[P_OustandingPO] ADD  CONSTRAINT [DF_P_OustandingPO_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Booking SP / Is GMT Master' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO', @level2type=N'COLUMN',@level2name=N'BookingSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO', @level2type=N'COLUMN',@level2name=N'LastCartonReceivedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO