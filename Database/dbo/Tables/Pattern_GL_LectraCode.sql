CREATE TABLE [dbo].[Pattern_GL_LectraCode] (
    [ID]           VARCHAR (10) CONSTRAINT [DF_Pattern_GL_LectraCode_ID] DEFAULT ('') NOT NULL,
    [Version]      VARCHAR (3)  CONSTRAINT [DF_Pattern_GL_LectraCode_Version] DEFAULT ('') NOT NULL,
    [PatternUKEY]  BIGINT       CONSTRAINT [DF_Pattern_GL_LectraCode_PatternUKEY] DEFAULT ((0)) NOT NULL,
    [SEQ]          VARCHAR (4)  CONSTRAINT [DF_Pattern_GL_LectraCode_SEQ] DEFAULT ('') NOT NULL,
    [PatternCode]  VARCHAR (20) CONSTRAINT [DF_Pattern_GL_LectraCode_PatternCode] DEFAULT ('') NOT NULL,
    [ArticleGroup] VARCHAR (6)  CONSTRAINT [DF_Pattern_GL_LectraCode_ArticleGroup] DEFAULT ('') NOT NULL,
    [LectraCode]   VARCHAR (2)  CONSTRAINT [DF_Pattern_GL_LectraCode_LectraCode] DEFAULT ('') NOT NULL,
    [PatternPanel] VARCHAR (2)  CONSTRAINT [DF_Pattern_GL_LectraCode_PatternPanel] DEFAULT ('') NOT NULL,
    [FabricCode]   VARCHAR (2)  CONSTRAINT [DF_Pattern_GL_LectraCode_FabricCode] DEFAULT ('') NULL,
    CONSTRAINT [PK_Pattern_GL_LectraCode] PRIMARY KEY CLUSTERED ([ID], [ArticleGroup], [Version], [SEQ])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版作業 - 版片明細裁片配色檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Version', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連結 Pattern', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'PatternUKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片縮寫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色群組代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'ArticleGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LectraCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_LectraCode', @level2type = N'COLUMN', @level2name = N'FabricCode';

