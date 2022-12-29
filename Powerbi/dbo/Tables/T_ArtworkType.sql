CREATE TABLE [dbo].[T_ArtworkType](
	[Ukey] [bigint] NOT NULL,
	[Artwork Type No] [varchar](4) NOT NULL,
	[Artwork Type] [varchar](20) NOT NULL,
	[Classify] [varchar](1) NOT NULL,
	[Artwork Type Unit] [varchar](10) NOT NULL,
	[ArtworkTypeKey] [varchar](35) NOT NULL,
 CONSTRAINT [PK_T_ArtworkType] PRIMARY KEY CLUSTERED 
(
	[ArtworkTypeKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]