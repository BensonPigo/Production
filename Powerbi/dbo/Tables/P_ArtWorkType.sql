CREATE TABLE [dbo].[P_ArtWorkType](
	[ArtworkTypeNo] [varchar](4) NOT NULL,
	[ArtworkType] [varchar](20) NOT NULL,
	[Classify] [varchar](1) NOT NULL,
	[ArtworkTypeUnit] [varchar](10) NOT NULL,
	[ArtworkTypeKey] [varchar](35) NOT NULL,
 CONSTRAINT [PK_P_ArtWorkType] PRIMARY KEY CLUSTERED 
(
	[ArtworkType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ArtWorkType] ADD  CONSTRAINT [DF_P_ArtWorkType_ArtworkTypeNo]  DEFAULT ('') FOR [ArtworkTypeNo]
GO

ALTER TABLE [dbo].[P_ArtWorkType] ADD  CONSTRAINT [DF_P_ArtWorkType_ArtworkType]  DEFAULT ('') FOR [ArtworkType]
GO

ALTER TABLE [dbo].[P_ArtWorkType] ADD  CONSTRAINT [DF_P_ArtWorkType_Classify]  DEFAULT ('') FOR [Classify]
GO

ALTER TABLE [dbo].[P_ArtWorkType] ADD  CONSTRAINT [DF_P_ArtWorkType_ArtworkTypeUnit]  DEFAULT ('') FOR [ArtworkTypeUnit]
GO

ALTER TABLE [dbo].[P_ArtWorkType] ADD  CONSTRAINT [DF_P_ArtWorkType_ArtworkTypeKey]  DEFAULT ('') FOR [ArtworkTypeKey]
GO
