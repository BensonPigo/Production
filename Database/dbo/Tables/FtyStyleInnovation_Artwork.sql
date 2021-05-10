CREATE TABLE [dbo].[FtyStyleInnovation_Artwork] (
    [FtyStyleInnovationUkey]      BIGINT       CONSTRAINT [DF_FtyStyleInnovation_Artwork_FtyStyleInnovationUkey] DEFAULT ((0)) NOT NULL,
    [Ukey]                        BIGINT       IDENTITY (1, 1) NOT NULL,
    [SubprocessId]                VARCHAR (15) CONSTRAINT [DF_FtyStyleInnovation_Artwork_SubprocessId] DEFAULT ('') NULL,
    [PostSewingSubProcess]        BIT          CONSTRAINT [DF_FtyStyleInnovation_Artwork_PostSewingSubProcess] DEFAULT ((0)) NOT NULL,
    [NoBundleCardAfterSubprocess] BIT          CONSTRAINT [DF_FtyStyleInnovation_Artwork_NoBundleCardAfterSubprocess] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FtyStyleInnovation_Artwork] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'該Subprocess之後該bundlecard就合併到MainPart', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovation_Artwork', @level2type = N'COLUMN', @level2name = N'NoBundleCardAfterSubprocess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為Sewing之後的Subprocess', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovation_Artwork', @level2type = N'COLUMN', @level2name = N'PostSewingSubProcess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovation_Artwork', @level2type = N'COLUMN', @level2name = N'SubprocessId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovation_Artwork', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FtyStyleInnovationUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovation_Artwork', @level2type = N'COLUMN', @level2name = N'FtyStyleInnovationUkey';

