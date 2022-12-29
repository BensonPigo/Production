CREATE TABLE [dbo].[T_OrdersData_ArtworkType](
	[Ukey] [bigint] NOT NULL,
	[SP#] [varchar](13) NOT NULL,
	[Artwork Type No] [varchar](4) NOT NULL,
	[Artwork Type] [varchar](20) NOT NULL,
	[Value] [numeric](38, 6) NOT NULL,
	[Total Value] [numeric](38, 6) NOT NULL,
	[Artwork Type Unit] [varchar](10) NOT NULL,
	[Subcon In TypeID] [varchar](2) NOT NULL,
	[ArtworkTypeKey] [varchar](35) NOT NULL,
	[OrderDataKey] [varchar](22) NULL,
 CONSTRAINT [PK_T_OrdersData_ArtworkType] PRIMARY KEY CLUSTERED 
(
	[SP#] ASC,
	[ArtworkTypeKey] ASC,
	[Subcon In TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]