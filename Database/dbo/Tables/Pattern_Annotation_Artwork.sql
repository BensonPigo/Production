CREATE TABLE [dbo].[Pattern_Annotation_Artwork](
		[ID] [varchar](20) NOT NULL,
		[ArtworkTypeID] [varchar](20) NULL,
		[NameCH] [varchar](50) NULL,
		[NameEN] [varchar](50) NULL,
		[IEPatternCode] [bit] NULL,
		[Combine] [bit] NULL,
		CONSTRAINT [PK_Pattern_Annotation_Artwork] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
ALTER TABLE [dbo].[Pattern_Annotation_Artwork] ADD  CONSTRAINT [DF_Pattern_Annotation_Artwork_ID]  DEFAULT ('') FOR [ID]


ALTER TABLE [dbo].[Pattern_Annotation_Artwork] ADD  CONSTRAINT [DF_Pattern_Annotation_Artwork_ArtworkTypeID]  DEFAULT ('') FOR [ArtworkTypeID]


ALTER TABLE [dbo].[Pattern_Annotation_Artwork] ADD  CONSTRAINT [DF_Pattern_Annotation_Artwork_NameCH]  DEFAULT ('') FOR [NameCH]


ALTER TABLE [dbo].[Pattern_Annotation_Artwork] ADD  CONSTRAINT [DF_Pattern_Annotation_Artwork_NameEN]  DEFAULT ('') FOR [NameEN]


ALTER TABLE [dbo].[Pattern_Annotation_Artwork] ADD  DEFAULT ((0)) FOR [IEPatternCode]


ALTER TABLE [dbo].[Pattern_Annotation_Artwork] ADD  DEFAULT ((0)) FOR [Combine]


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pattern_Annotation_Artwork', @level2type=N'COLUMN',@level2name=N'ID'


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ArtworkTypeID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pattern_Annotation_Artwork', @level2type=N'COLUMN',@level2name=N'ArtworkTypeID'


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'NameCH' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pattern_Annotation_Artwork', @level2type=N'COLUMN',@level2name=N'NameCH'


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'NameEN' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pattern_Annotation_Artwork', @level2type=N'COLUMN',@level2name=N'NameEN'


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pattern內Annotation代碼檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pattern_Annotation_Artwork'

