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
	[ArticleFrom] VARCHAR(8) NOT NULL, 
    [SizeCodeFrom] VARCHAR(20) NOT NULL, 
    CONSTRAINT [PK_Order_BuyBack_Qty] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[OrderIDFrom] ASC,
	[Article] ASC,
	[SizeCode] ASC,
	[ArticleFrom] ASC,
	[SizeCodeFrom] ASC
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

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_ArticleFrom]  DEFAULT ('') FOR [ArticleFrom]
GO

ALTER TABLE [dbo].[Order_BuyBack_Qty] ADD  CONSTRAINT [DF_Order_BuyBack_Qty_SizeCodeFrom]  DEFAULT ('') FOR [SizeCodeFrom]
GO

EXEC sys.sp_addextendedproperty @name=N'ArticleFrom', @value=N'從哪一個 Article 轉入訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_BuyBack_Qty', @level2type=N'COLUMN',@level2name=N'ArticleFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'SizeCodeFrom', @value=N'從哪一個 Size Code 轉入訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_BuyBack_Qty', @level2type=N'COLUMN',@level2name=N'SizeCodeFrom'
GO