CREATE TABLE [dbo].[ArtworkReq_Detail](
	[uKey] [bigint] IDENTITY(1,1) NOT NULL,
	[ID] [varchar](13) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[ArtworkID] [varchar](20) NOT NULL,
	[PatternCode] [varchar](20) NOT NULL,
	[PatternDesc] [varchar](40) NOT NULL,
	[ReqQty] [numeric](6, 0) NOT NULL,
	[Stitch] [numeric](6, 0) NOT NULL,
	[QtyGarment] [numeric](2, 0) NOT NULL,
	[ExceedQty] [numeric](6, 0) NOT NULL,
	[ArtworkPOID] [varchar](13) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[SizeCode] [varchar](8) NOT NULL,
	[OrderArtworkUkey] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[uKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ('') FOR [ArtworkID]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ('') FOR [PatternCode]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ('') FOR [PatternDesc]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ((0)) FOR [ReqQty]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ((0)) FOR [Stitch]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ((0)) FOR [QtyGarment]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ((0)) FOR [ExceedQty]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  DEFAULT ('') FOR [ArtworkPOID]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  CONSTRAINT [DF_ArtworkReq_Detail_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  CONSTRAINT [DF_ArtworkReq_Detail_SizeCode]  DEFAULT ('') FOR [SizeCode]
GO

ALTER TABLE [dbo].[ArtworkReq_Detail] ADD  CONSTRAINT [DF_ArtworkReq_Detail_OrderArtworkUkey]  DEFAULT ((0)) FOR [OrderArtworkUkey]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order_Artwork.Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkReq_Detail', @level2type=N'COLUMN',@level2name=N'OrderArtworkUkey'
GO

