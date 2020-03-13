CREATE TABLE [dbo].[Order_BuyBack](
	[ID] [varchar](13) NOT NULL,
	[OrderIDFrom] [varchar](13) NOT NULL,
	[BuyBackReason] [varchar](20) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Order_BuyBack] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[OrderIDFrom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order_BuyBack] ADD  CONSTRAINT [DF_Order_BuyBack_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[Order_BuyBack] ADD  CONSTRAINT [DF_Order_BuyBack_OrderIDFrom]  DEFAULT ('') FOR [OrderIDFrom]
GO

ALTER TABLE [dbo].[Order_BuyBack] ADD  CONSTRAINT [DF_Order_BuyBack_BuyBackReason]  DEFAULT ('') FOR [BuyBackReason]
GO

ALTER TABLE [dbo].[Order_BuyBack] ADD  CONSTRAINT [DF_Order_BuyBack_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Order_BuyBack] ADD  CONSTRAINT [DF_Order_BuyBack_EditName]  DEFAULT ('') FOR [EditName]
GO