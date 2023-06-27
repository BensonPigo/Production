CREATE TABLE [dbo].[Style_Artwork] (
    [StyleUkey]         BIGINT          CONSTRAINT [DF_Style_Artwork_StyleUkey] DEFAULT ((0)) NOT NULL,
    [ArtworkTypeID]     VARCHAR (20)    CONSTRAINT [DF_Style_Artwork_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [Article]           VARCHAR (8)     CONSTRAINT [DF_Style_Artwork_Article] DEFAULT ('') NOT NULL,
    [PatternCode]       VARCHAR (20)    CONSTRAINT [DF_Style_Artwork_PatternCode] DEFAULT ('') NOT NULL,
    [PatternDesc]       NVARCHAR (100)  CONSTRAINT [DF_Style_Artwork_PatternDesc] DEFAULT ('') NOT NULL,
    [ArtworkID]         VARCHAR (36)    CONSTRAINT [DF_Style_Artwork_ArtworkID] DEFAULT ('') NOT NULL,
    [ArtworkName]       NVARCHAR (80)   CONSTRAINT [DF_Style_Artwork_ArtworkName] DEFAULT ('') NOT NULL,
    [Qty]               INT             CONSTRAINT [DF_Style_Artwork_Qty] DEFAULT ((0)) NOT NULL,
    [Price]             DECIMAL (16, 4) CONSTRAINT [DF_Style_Artwork_Price] DEFAULT ((0)) NOT NULL,
    [Cost]              DECIMAL (8, 4)  CONSTRAINT [DF_Style_Artwork_Cost] DEFAULT ((0)) NOT NULL,
    [Remark]            NVARCHAR (1000) CONSTRAINT [DF_Style_Artwork_Remark] DEFAULT ('') NOT NULL,
    [Ukey]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [AddName]           VARCHAR (10)    CONSTRAINT [DF_Style_Artwork_AddName] DEFAULT ('') NOT NULL,
    [AddDate]           DATETIME        NULL,
    [EditName]          VARCHAR (10)    CONSTRAINT [DF_Style_Artwork_EditName] DEFAULT ('') NOT NULL,
    [EditDate]          DATETIME        NULL,
    [TMS]               INT             CONSTRAINT [DF_Style_Artwork_TMS] DEFAULT ((0)) NOT NULL,
    [TradeUkey]         BIGINT          CONSTRAINT [DF_Style_Artwork_TradeUkey] DEFAULT ((0)) NOT NULL,
    [SMNoticeID]        VARCHAR (10)    CONSTRAINT [DF_Style_Artwork_SMNoticeID] DEFAULT ('') NOT NULL,
    [PatternVersion]    VARCHAR (3)     CONSTRAINT [DF_Style_Artwork_PatternVersion] DEFAULT ('') NOT NULL,
    [ActStitch]         INT             CONSTRAINT [DF_Style_Artwork_ActStitch] DEFAULT ((0)) NOT NULL,
    [PPU]               DECIMAL (8, 3)  CONSTRAINT [DF_Style_Artwork_PPU] DEFAULT ((0)) NOT NULL,
    [InkType]           VARCHAR (50)    CONSTRAINT [DF_Style_Artwork_InkType] DEFAULT ('') NOT NULL,
    [Colors]            VARCHAR (100)   CONSTRAINT [DF_Style_Artwork_Colors] DEFAULT ('') NOT NULL,
    [Length]            DECIMAL (5, 1)  CONSTRAINT [DF_Style_Artwork_Length] DEFAULT ((0)) NOT NULL,
    [Width]             DECIMAL (5, 1)  CONSTRAINT [DF_Style_Artwork_Width] DEFAULT ((0)) NOT NULL,
    [AntiMigration]     BIT             CONSTRAINT [DF_Style_Artwork_AntiMigration] DEFAULT ((0)) NOT NULL,
    [PrintType]         VARCHAR (1)     CONSTRAINT [DF_Style_Artwork_PrintType] DEFAULT ('') NOT NULL,
    [PatternAnnotation] NVARCHAR (50)   CONSTRAINT [DF_Style_Artwork_PatternAnnotation] DEFAULT ('') NOT NULL,
    [InkTypePPU]        VARCHAR (50)    CONSTRAINT [DF_Style_Artwork_InkTypePPU] DEFAULT ('') NOT NULL,
    [PatternCodeSize]   VARCHAR (10)    CONSTRAINT [DF_Style_Artwork_PatternCodeSize] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Style_Artwork] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style Artwork', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'印繡花模號或板號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'ArtworkID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'印繡花名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'ArtworkName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'Cost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
