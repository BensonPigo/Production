CREATE TABLE [dbo].[Pattern_GL_Article] (
    [ID]           VARCHAR (10)   CONSTRAINT [DF_Pattern_GL_Article_ID] DEFAULT ('') NOT NULL,
    [Seq]          VARCHAR (2)    CONSTRAINT [DF__Pattern_GL___Seq__7A1DE0A6] DEFAULT ('') NOT NULL,
    [Version]      VARCHAR (3)    CONSTRAINT [DF_Pattern_GL_Article_Version] DEFAULT ('') NOT NULL,
    [PatternUKEY]  BIGINT         CONSTRAINT [DF_Pattern_GL_Article_PatternUKEY] DEFAULT ((0)) NOT NULL,
    [ArticleGroup] VARCHAR (6)    CONSTRAINT [DF_Pattern_GL_Article_ArticleGroup] DEFAULT ('') NOT NULL,
    [Article]      NVARCHAR (100) CONSTRAINT [DF_Pattern_GL_Article_Article] DEFAULT ('') NOT NULL,
    [SizeRange]    NVARCHAR (MAX) CONSTRAINT [DF_Pattern_GL_Article_SizeRange] DEFAULT ('') NULL,
    [Remark]       VARCHAR (20)   CONSTRAINT [DF_Pattern_GL_Article_Remark] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_Pattern_GL_Article_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_Pattern_GL_Article_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL,
    [App]          VARCHAR (2)    CONSTRAINT [DF_Pattern_GL_Article_App] DEFAULT ('') NULL,
    CONSTRAINT [PK_Pattern_GL_Article] PRIMARY KEY CLUSTERED ([ID] ASC, [Seq] ASC, [Version] ASC, [Article] ASC, [ArticleGroup] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版作業 - 版片配色檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PATTERN ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Version', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連結 Pattern', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'PatternUKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色群組代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'ArticleGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'指定用SIze', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'SizeRange';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Seq',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Pattern_GL_Article',
    @level2type = N'COLUMN',
    @level2name = N'Seq'
GO
CREATE NONCLUSTERED INDEX [article]
    ON [dbo].[Pattern_GL_Article]([Article] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紀錄該成衣檔特殊用途的id(DropDownList.Type= ''GLApplication'')', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern_GL_Article', @level2type = N'COLUMN', @level2name = N'App';

