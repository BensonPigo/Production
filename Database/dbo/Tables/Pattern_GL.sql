CREATE TABLE [dbo].[Pattern_GL] (
    [ID]          VARCHAR (10)   CONSTRAINT [DF_Pattern_GL_ID] DEFAULT ('') NOT NULL,
    [Version]     VARCHAR (3)    CONSTRAINT [DF_Pattern_GL_Version] DEFAULT ('') NOT NULL,
    [PatternUKEY] BIGINT         CONSTRAINT [DF_Pattern_GL_PatternUKEY] DEFAULT ((0)) NOT NULL,
    [SEQ]         VARCHAR (4)    CONSTRAINT [DF_Pattern_GL_SEQ] DEFAULT ('') NOT NULL,
    [PatternCode] VARCHAR (20)   CONSTRAINT [DF_Pattern_GL_PatternCode] DEFAULT ('') NOT NULL,
    [PatternDesc] NVARCHAR (100) CONSTRAINT [DF_Pattern_GL_PatternDesc] DEFAULT ('') NULL,
    [Annotation]  NVARCHAR (50)  CONSTRAINT [DF_Pattern_GL_Annotation] DEFAULT ('') NULL,
    [Alone]       VARCHAR (2)    CONSTRAINT [DF_Pattern_GL_Alone] DEFAULT ('') NULL,
    [PAIR]        VARCHAR (2)    CONSTRAINT [DF_Pattern_GL_PAIR] DEFAULT ('') NULL,
    [DV]          VARCHAR (2)    CONSTRAINT [DF_Pattern_GL_DV] DEFAULT ('') NULL,
    [Remarks]     NVARCHAR (30)  CONSTRAINT [DF_Pattern_GL_Remarks] DEFAULT ('') NULL,
    [Location] VARCHAR NULL DEFAULT (''), 
    CONSTRAINT [PK_Pattern_GL] PRIMARY KEY CLUSTERED ([ID], [Version], [SEQ])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版作業 - 版片明細檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PATTERN ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Version', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連結 Pattern', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'PatternUKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片縮寫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'特殊做工註解', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'Annotation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單一', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'Alone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'一對(正向)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'PAIR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'一對(反向)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'DV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL', @level2type = N'COLUMN', @level2name = N'Remarks';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Pattern_GL]([PatternUKEY] ASC);

