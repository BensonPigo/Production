CREATE TABLE [dbo].[Order_FtyMtlStdCost](
	[OrderID] [varchar](13) NOT NULL,
	[SCIRefno] [varchar](30) NOT NULL,
	[SuppID] [varchar](6) NOT NULL,
	[PurchaseCompanyID] [numeric](2, 0) NOT NULL,
	[PurchasePrice] [numeric](16, 4) NOT NULL,
	[UsagePrice] [numeric](16, 4) NOT NULL,
	[CurrencyID] [varchar](3) NOT NULL,
	[Cons] [numeric](10, 4) NOT NULL,
	[UsageUnit] [varchar](8) NOT NULL,
	[PurchaseUnit] [varchar](8) NOT NULL,
	[AddName] [varchar](50) NOT NULL,
	[AddDate] [datetime] NULL,
 CONSTRAINT [PK_Order_FtyMtlStdCost] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[SCIRefno] ASC,
	[SuppID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order_FtyMtlStdCost] ADD  CONSTRAINT [DF_Order_FtyMtlStdCost_PurchasePrice]  DEFAULT ((0)) FOR [PurchasePrice]
GO

ALTER TABLE [dbo].[Order_FtyMtlStdCost] ADD  CONSTRAINT [DF_Order_FtyMtlStdCost_UsagePrice]  DEFAULT ((0)) FOR [UsagePrice]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'SCIRefno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quotation或採購單上的Supp' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'PurchaseCompanyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'PurchasePrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'UsagePrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'CurrencyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單件用量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'Cons'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'UsageUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'PurchaseUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_FtyMtlStdCost', @level2type=N'COLUMN',@level2name=N'AddDate'
GO