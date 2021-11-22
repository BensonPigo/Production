CREATE TABLE [dbo].[Style_TmsCost] (
    [StyleUkey]     BIGINT         CONSTRAINT [DF_Style_TmsCost_StyleUkey] DEFAULT ((0)) NOT NULL,
    [ArtworkTypeID] VARCHAR (20)   CONSTRAINT [DF_Style_TmsCost_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [Seq]           VARCHAR (4)    CONSTRAINT [DF_Style_TmsCost_Seq] DEFAULT ('') NOT NULL,
    [Qty]           INT            CONSTRAINT [DF_Style_TmsCost_Qty] DEFAULT ((0)) NULL,
    [ArtworkUnit]   VARCHAR (10)   CONSTRAINT [DF_Style_TmsCost_ArtworkUnit] DEFAULT ('') NULL,
    [TMS]           NUMERIC(5)            CONSTRAINT [DF_Style_TmsCost_TMS] DEFAULT ((0)) NULL,
    [Price]         NUMERIC (16, 4) CONSTRAINT [DF_Style_TmsCost_Price] DEFAULT ((0)) NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_Style_TmsCost_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_Style_TmsCost_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME       NULL,
    CONSTRAINT [PK_Style_TmsCost] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [ArtworkTypeID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TMS & COST', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號(排序用)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位(ex.Stitches/Panels/pcs)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'ArtworkUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'TMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_TmsCost', @level2type = N'COLUMN', @level2name = N'EditDate';

