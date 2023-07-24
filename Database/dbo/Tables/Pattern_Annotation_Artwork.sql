CREATE TABLE [dbo].[Pattern_Annotation_Artwork] (
    [ID]            VARCHAR (20) CONSTRAINT [DF_Pattern_Annotation_Artwork_ID] DEFAULT ('') NOT NULL,
    [ArtworkTypeID] VARCHAR (20) CONSTRAINT [DF_Pattern_Annotation_Artwork_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [NameCH]        VARCHAR (50) CONSTRAINT [DF_Pattern_Annotation_Artwork_NameCH] DEFAULT ('') NOT NULL,
    [NameEN]        VARCHAR (50) CONSTRAINT [DF_Pattern_Annotation_Artwork_NameEN] DEFAULT ('') NOT NULL,
    [IEPatternCode] BIT          CONSTRAINT [DF_Pattern_Annotation_Artwork_IEPatternCode] DEFAULT ((0)) NOT NULL,
    [Combine]       BIT          CONSTRAINT [DF_Pattern_Annotation_Artwork_Combine] DEFAULT ((0)) NOT NULL,
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




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NameEN', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_Annotation_Artwork', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NameCH', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_Annotation_Artwork', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_Annotation_Artwork', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ArtworkTypeID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_Annotation_Artwork', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';

