CREATE TABLE [dbo].[Style_Article] (
    [StyleUkey]   BIGINT         CONSTRAINT [DF_Style_Article_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Seq]         SMALLINT       CONSTRAINT [DF_Style_Article_Seq] DEFAULT ((0)) NULL,
    [Article]     VARCHAR (8)    CONSTRAINT [DF_Style_Article_Article] DEFAULT ('') NOT NULL,
    [TissuePaper] BIT            CONSTRAINT [DF_Style_Article_TissuePaper] DEFAULT ((0)) NULL,
    [ArticleName] NVARCHAR (100) CONSTRAINT [DF_Style_Article_ArticleName] DEFAULT ('') NULL,
    [Contents]     NVARCHAR (MAX) CONSTRAINT [DF_Style_Article_Content] DEFAULT ('') NULL,
    [GarmentLT] NUMERIC(3) NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Style_Article] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Article] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Article', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Article', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Article', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'棉紙', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Article', @level2type = N'COLUMN', @level2name = N'TissuePaper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Article', @level2type = N'COLUMN', @level2name = N'ArticleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣成份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Article', @level2type = N'COLUMN', @level2name = 'Contents';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Garment L/T',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style_Article',
    @level2type = N'COLUMN',
    @level2name = N'GarmentLT'