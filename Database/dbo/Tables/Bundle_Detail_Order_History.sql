CREATE TABLE [dbo].[Bundle_Detail_Order_History](
	[Ukey] [bigint] NOT NULL,
	[ID] [varchar](13) NOT NULL,
	[BundleNo] [varchar](10) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[Qty] [numeric](5, 0) NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bundle_Detail_Order_History] ADD  CONSTRAINT [DF_Bundle_Detail_Order_History_BundleNo]  DEFAULT ('') FOR [BundleNo]
GO

ALTER TABLE [dbo].[Bundle_Detail_Order_History] ADD  CONSTRAINT [DF_Bundle_Detail_Order_History_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[Bundle_Detail_Order_History] ADD  CONSTRAINT [DF_Bundle_Detail_Order_History_Qty]  DEFAULT ((0)) FOR [Qty]
GO
