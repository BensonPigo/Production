CREATE TABLE [dbo].[Pattern_GL_FabricPanelCode] (
    [ID]           VARCHAR (10) CONSTRAINT [DF_Pattern_GL_FabricPanelCode_ID] DEFAULT ('') NOT NULL,
    [Version]      VARCHAR (3)  CONSTRAINT [DF_Pattern_GL_FabricPanelCode_Version] DEFAULT ('') NOT NULL,
    [PatternUKEY]  BIGINT       CONSTRAINT [DF_Pattern_GL_FabricPanelCode_PatternUKEY] DEFAULT ((0)) NOT NULL,
    [SEQ]          VARCHAR (4)  CONSTRAINT [DF_Pattern_GL_FabricPanelCode_SEQ] DEFAULT ('') NOT NULL,
    [PatternCode]  VARCHAR (20) CONSTRAINT [DF_Pattern_GL_FabricPanelCode_PatternCode] DEFAULT ('') NOT NULL,
    [ArticleGroup] VARCHAR (6)  CONSTRAINT [DF_Pattern_GL_FabricPanelCode_ArticleGroup] DEFAULT ('') NOT NULL,
    [FabricPanelCode]   VARCHAR (2)  CONSTRAINT [DF_Pattern_GL_FabricPanelCode_FabricPanelCode] DEFAULT ('') NOT NULL,
    [PatternPanel] VARCHAR (2)  CONSTRAINT [DF_Pattern_GL_FabricPanelCode_PatternPanel] DEFAULT ('') NOT NULL,
    [FabricCode]   VARCHAR (3)  CONSTRAINT [DF_Pattern_GL_FabricPanelCode_FabricCode] DEFAULT ('') NULL,
    CONSTRAINT [PK_Pattern_GL_FabricPanelCode] PRIMARY KEY CLUSTERED ([ID], [ArticleGroup], [Version], [SEQ])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版作業 - 版片明細裁片配色檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Version', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連結 Pattern', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'PatternUKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片縮寫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色群組代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'ArticleGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FabricPanelCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_FabricPanelCode', @level2type = N'COLUMN', @level2name = N'FabricCode';

