CREATE TABLE [dbo].[Order_BuyBack_Qty](
	[ID] [varchar](13) NOT NULL,
	[OrderIDFrom] [varchar](13) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[SizeCode] [varchar](8) NOT NULL,
	[Qty] [int] NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Order_BuyBack_Qty] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[OrderIDFrom] ASC,
	[Article] ASC,
	[SizeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_OrderIDFrom]  DEFAULT ('') FOR [OrderIDFrom]
GO

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_SizeCode]  DEFAULT ('') FOR [SizeCode]
GO

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_Qty]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_EditName]  DEFAULT ('') FOR [EditName]
GO