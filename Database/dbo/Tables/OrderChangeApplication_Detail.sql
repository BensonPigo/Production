CREATE TABLE [dbo].[OrderChangeApplication_Detail](
	[Ukey] [bigint] NOT NULL,
	[ID] [varchar](13) NULL,
	[Seq] [varchar](2) NULL,
	[Article] [varchar](8) NULL,
	[SizeCode] [varchar](8) NULL,
	[Qty] [numeric](6, 0) NULL,
	[OriQty] [numeric](6, 0) NULL,
	[NowQty] [numeric](6, 0) NULL,
 CONSTRAINT [PK_OrderChangeApplication_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderChangeApplication_Detail] ADD  CONSTRAINT [DF_OrderChangeApplication_Detail_NowQty]  DEFAULT ((0)) FOR [NowQty]
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整後的數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderChangeApplication_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Qty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整前的數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderChangeApplication_Detail',
    @level2type = N'COLUMN',
    @level2name = N'NowQty'