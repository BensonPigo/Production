CREATE TABLE [dbo].[Style_SimilarStyle] (
    [MasterBrandID]   VARCHAR (8)  CONSTRAINT [DF_Style_SimilarStyle_MasterBrandID] DEFAULT ('') NOT NULL,
    [MasterStyleID]   VARCHAR (15) CONSTRAINT [DF_Style_SimilarStyle_MasterStyleID] DEFAULT ('') NOT NULL,
    [ChildrenBrandID] VARCHAR (8)  CONSTRAINT [DF_Style_SimilarStyle_ChildrenBrandID] DEFAULT ('') NOT NULL,
    [ChildrenStyleID] VARCHAR (15) CONSTRAINT [DF_Style_SimilarStyle_ChildrenStyleID] DEFAULT ('') NOT NULL,
    [AddName]         VARCHAR (10) CONSTRAINT [DF_Style_SimilarStyle_AddName] DEFAULT ('') NOT NULL,
    [AddDate]         DATETIME     NULL,
    [EditName]        VARCHAR (10) CONSTRAINT [DF_Style_SimilarStyle_EditName] DEFAULT ('') NOT NULL,
    [EditDate]        DATETIME     NULL,
    CONSTRAINT [PK_Style_SimilarStyle] PRIMARY KEY CLUSTERED ([MasterBrandID] ASC, [MasterStyleID] ASC, [ChildrenBrandID] ASC, [ChildrenStyleID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Similar Style', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'MasterBrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'母單款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'MasterStyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'母單季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'MasterSeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'MasterStyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'子單Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'ChildrenBrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'子單款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'ChildrenStyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'子單季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'ChildrenSeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'ChildrenStyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SimilarStyle', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
CREATE NONCLUSTERED INDEX [IDX_Style_SimilarStyle_Master]
    ON [dbo].[Style_SimilarStyle]([MasterBrandID] ASC, [MasterStyleID] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_Style_SimilarStyle_Children]
    ON [dbo].[Style_SimilarStyle]([ChildrenBrandID] ASC, [ChildrenStyleID] ASC);

