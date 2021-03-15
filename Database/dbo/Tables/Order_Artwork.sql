CREATE TABLE [dbo].[Order_Artwork] (
    [ID]            VARCHAR (13)    CONSTRAINT [DF_Order_Artwork_ID] DEFAULT ('') NOT NULL,
    [ArtworkTypeID] VARCHAR (20)    CONSTRAINT [DF_Order_Artwork_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [Article]       VARCHAR (8)     CONSTRAINT [DF_Order_Artwork_Article] DEFAULT ('') NOT NULL,
    [PatternCode]   VARCHAR (20)    CONSTRAINT [DF_Order_Artwork_PatternCode] DEFAULT ('') NULL,
    [PatternDesc]   NVARCHAR (100)  CONSTRAINT [DF_Order_Artwork_PatternDesc] DEFAULT ('') NULL,
    [ArtworkID]     VARCHAR (20)    CONSTRAINT [DF_Order_Artwork_ArtworkID] DEFAULT ('') NOT NULL,
    [ArtworkName]   NVARCHAR (40)   CONSTRAINT [DF_Order_Artwork_ArtworkName] DEFAULT ('') NULL,
    [Qty]           INT             CONSTRAINT [DF_Order_Artwork_Qty] DEFAULT ((0)) NULL,
    [TMS]           INT             CONSTRAINT [DF_Order_Artwork_TMS] DEFAULT ((0)) NULL,
    [Price]         NUMERIC (16, 4) CONSTRAINT [DF_Order_Artwork_Price] DEFAULT ((0)) NULL,
    [Cost]          NUMERIC (8, 4)  CONSTRAINT [DF_Order_Artwork_Cost] DEFAULT ((0)) NULL,
    [Remark]        NVARCHAR (1000) CONSTRAINT [DF_Order_Artwork_Remark] DEFAULT ('') NULL,
    [Ukey]          BIGINT          CONSTRAINT [DF_Order_Artwork_Ukey] DEFAULT ((0)) NOT NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_Order_Artwork_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_Order_Artwork_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME        NULL,
    [PPU]           NUMERIC (8, 3)  DEFAULT ((0)) NULL,
    [InkType]       VARCHAR (50)    NULL,
    [Length]        NUMERIC (5, 1)  NULL,
    [Width]         NUMERIC (5, 1)  NULL,
    [Colors] VARCHAR(100) NULL, 
    CONSTRAINT [PK_Order_Artwork] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order Artwork', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'印繡花模號或板號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'ArtworkID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'印繡花名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'ArtworkName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'針數/件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'Cost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Artwork', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
CREATE NONCLUSTERED INDEX [<Name2 of Missing Index, sysname,>]
    ON [dbo].[Order_Artwork]([ID] ASC, [Article] ASC)
    INCLUDE([ArtworkTypeID], [PatternCode], [PatternDesc], [ArtworkID], [Qty], [Cost]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Order_Artwork]([ArtworkTypeID] ASC)
    INCLUDE([ID], [PatternCode], [PatternDesc], [ArtworkID], [Qty], [Cost]);

