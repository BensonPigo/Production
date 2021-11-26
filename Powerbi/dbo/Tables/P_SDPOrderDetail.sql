CREATE TABLE [dbo].[P_SDPOrderDetail](
	[Country] [varchar](2) NOT NULL,
	[KPIGroup] [varchar](8) NOT NULL,
	[Factory] [varchar](8) NOT NULL,
	[SPNO] [varchar](13) NULL,
	[StyleID] [varchar](15) NULL,
	[Seq] [varchar](2) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[FactoryKPI] [date] NULL,
	[Extension] [date] NULL,
	[DeliveryByShipMode] [varchar](10) NOT NULL,
	[OrderQty] [numeric](6, 0) NOT NULL,
	[OnTimeQty] [numeric](6, 0) NOT NULL,
	[FailQty] [numeric](6, 0) NOT NULL,
	[PullOutDate] [date] NULL,
	[ShipMode] [varchar](10) NOT NULL,
	[P] [numeric](2, 0) NOT NULL,
	[GarmentComplete] [varchar](1) NOT NULL,
	[ReasonID] [varchar](5) NOT NULL,
	[OrderReason] [nvarchar](500) NOT NULL,
	[LastSewingOutputDate] [date] NULL,
	[LastCartonReceivedDate] [date] NULL,
	[Handle] [varchar](100) NOT NULL,
	[SMR] [varchar](100) NOT NULL,
	[POHandle] [varchar](100) NULL,
	[POSMR] [varchar](100) NOT NULL,
	[OrderType] [varchar](20) NOT NULL,
	[DevSample] [varchar](1) NOT NULL,
	[SewingQty] [numeric](6, 0) NOT NULL,
	[FOCQty] [numeric](6, 0) NULL,
	[PartialShipment] [varchar](1) NOT NULL,
	[OutstandingReason] [varchar](5) NOT NULL,
	[OutstandingRemark] [nvarchar](max) NOT NULL,
	[OSTClogCarton] [int] NOT NULL,
	[Alias] [varchar](30) NOT NULL,
	[ReasonRemark] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [Country]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [KPIGroup]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [Factory]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [DeliveryByShipMode]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ((0)) FOR [OnTimeQty]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ((0)) FOR [FailQty]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [ShipMode]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ((0)) FOR [P]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [GarmentComplete]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [ReasonID]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [OrderReason]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [Handle]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [SMR]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [POHandle]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [POSMR]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [OrderType]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [DevSample]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ((0)) FOR [SewingQty]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ((0)) FOR [FOCQty]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [PartialShipment]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [OutstandingReason]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  CONSTRAINT [DF_P_SDPOrderDetail_OutstandingRemark]  DEFAULT ('') FOR [OutstandingRemark]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ((0)) FOR [OSTClogCarton]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  DEFAULT ('') FOR [Alias]
GO

ALTER TABLE [dbo].[P_SDPOrderDetail] ADD  CONSTRAINT [DF_P_SDPOrderDetail_ReasonRemark]  DEFAULT ('') FOR [ReasonRemark]
GO


