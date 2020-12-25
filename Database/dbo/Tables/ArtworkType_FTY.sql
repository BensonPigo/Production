CREATE TABLE [dbo].[ArtworkType_FTY](
	[ArtworkTypeID] [varchar](20) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[IsShowinIEP01] [bit] NULL,
	[IsShowinIEP03] [bit] NULL,
	[IsSewingline] [bit] NULL,
 CONSTRAINT [PK_ArtworkType_FTY] PRIMARY KEY CLUSTERED 
(
	[ArtworkTypeID] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ArtworkType_FTY] ADD  CONSTRAINT [DF_ArtworkType_FTY_IsShowinIEP01]  DEFAULT ((0)) FOR [IsShowinIEP01]
GO

ALTER TABLE [dbo].[ArtworkType_FTY] ADD  CONSTRAINT [DF_ArtworkType_FTY_IsShowinIEP03]  DEFAULT ((0)) FOR [IsShowinIEP03]
GO

ALTER TABLE [dbo].[ArtworkType_FTY] ADD  CONSTRAINT [DF_ArtworkType_FTY_IsSewingline]  DEFAULT ((0)) FOR [IsSewingline]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType_FTY', @level2type=N'COLUMN',@level2name=N'ArtworkTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType_FTY', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否顯示在IE P01 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType_FTY', @level2type=N'COLUMN',@level2name=N'IsShowinIEP01'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否顯示在IE P03 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType_FTY', @level2type=N'COLUMN',@level2name=N'IsShowinIEP03'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為Sewingline的Artwork' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType_FTY', @level2type=N'COLUMN',@level2name=N'IsSewingline'
GO
