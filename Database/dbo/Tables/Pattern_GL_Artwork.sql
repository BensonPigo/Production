CREATE TABLE [dbo].[Pattern_GL_Artwork] (
    [ID]            VARCHAR (10) CONSTRAINT [DF_Pattern_GL_Artwork_ID] DEFAULT ('') NULL,
    [Version]       VARCHAR (3)  CONSTRAINT [DF_Pattern_GL_Artwork_Version] DEFAULT ('') NULL,
    [UKEY]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [SEQ]           VARCHAR (4)  CONSTRAINT [DF_Pattern_GL_Artwork_SEQ] DEFAULT ('') NOT NULL,
    [ArtworkTypeID] VARCHAR (20) CONSTRAINT [DF_Pattern_GL_Artwork_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [PatternUkey]   BIGINT       CONSTRAINT [DF_Pattern_GL_Artwork_PatternUkey] DEFAULT ((0)) NULL,
    [UKEY_OLD]      VARCHAR (10) CONSTRAINT [DF_Pattern_GL_Artwork_UKEY_OLD] DEFAULT ('') NULL,
    CONSTRAINT [PK_Pattern_GL_Artwork] PRIMARY KEY CLUSTERED ([UKEY] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKEY_OLD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Artwork', @level2type = N'COLUMN', @level2name = N'UKEY_OLD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Artwork', @level2type = N'COLUMN', @level2name = N'PatternUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Artwork', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Artwork', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKEY', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Artwork', @level2type = N'COLUMN', @level2name = N'UKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Version', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Artwork', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Artwork', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版作業 - 版片明細裁片加工檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Artwork';

