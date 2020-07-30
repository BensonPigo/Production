CREATE TABLE [dbo].[ArtworkPO_Detail] (
    [ID]            VARCHAR (13)    CONSTRAINT [DF_ArtworkPO_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13)    CONSTRAINT [DF_ArtworkPO_Detail_OrderID] DEFAULT ('') NOT NULL,
    [ArtworkId]     VARCHAR (20)    CONSTRAINT [DF_ArtworkPO_Detail_ArtworkId] DEFAULT ('') NOT NULL,
    [PatternCode]   VARCHAR (20)    CONSTRAINT [DF_ArtworkPO_Detail_PatternCode] DEFAULT ('') NOT NULL,
    [PatternDesc]   NVARCHAR (40)   CONSTRAINT [DF_ArtworkPO_Detail_PatternDesc] DEFAULT ('') NULL,
    [CostStitch]    NUMERIC (6)     CONSTRAINT [DF_ArtworkPO_Detail_CostStitch] DEFAULT ((0)) NULL,
    [Stitch]        NUMERIC (6)     CONSTRAINT [DF_ArtworkPO_Detail_Stitch] DEFAULT ((0)) NULL,
    [UnitPrice]     NUMERIC (12, 4) CONSTRAINT [DF_ArtworkPO_Detail_UnitPrice] DEFAULT ((0)) NULL,
    [Cost]          NUMERIC (8, 4)  CONSTRAINT [DF_ArtworkPO_Detail_Cost] DEFAULT ((0)) NULL,
    [QtyGarment]    NUMERIC (2)     CONSTRAINT [DF_ArtworkPO_Detail_QtyGarment] DEFAULT ((0)) NULL,
    [Price]         NUMERIC (12, 4) CONSTRAINT [DF_ArtworkPO_Detail_Price] DEFAULT ((0)) NULL,
    [Amount]        NUMERIC (14, 4) CONSTRAINT [DF_ArtworkPO_Detail_Amount] DEFAULT ((0)) NULL,
    [Farmout]       NUMERIC (6)     CONSTRAINT [DF_ArtworkPO_Detail_Farmout] DEFAULT ((0)) NULL,
    [Farmin]        NUMERIC (6)     CONSTRAINT [DF_ArtworkPO_Detail_Farmin] DEFAULT ((0)) NULL,
    [ApQty]         NUMERIC (6)     CONSTRAINT [DF_ArtworkPO_Detail_ApQty] DEFAULT ((0)) NULL,
    [PoQty]         NUMERIC (6)     CONSTRAINT [DF_ArtworkPO_Detail_PoQty] DEFAULT ((0)) NOT NULL,
    [Ukey]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [ArtworkTypeID] VARCHAR (20)    CONSTRAINT [DF_ArtworkPO_Detail_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [ExceedQty]     NUMERIC (6)     CONSTRAINT [DF_ArtworkPO_Detail_ExceedQty] DEFAULT ((0)) NULL,
	[ArtworkReqID] VARCHAR (13)    CONSTRAINT [DF_ArtworkPO_Detail_ArtworkReqID] DEFAULT ('') NOT NULL,
    [Article] VARCHAR(8) NOT NULL CONSTRAINT [DF_ArtworkPO_Detail_Article] DEFAULT (''), 
    [SizeCode] VARCHAR(8) NOT NULL CONSTRAINT [DF_ArtworkPO_Detail_SizeCode] DEFAULT (''), 
    CONSTRAINT [PK_ArtworkPO_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工明細檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工版型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工預設個數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'CostStitch';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'個數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'Stitch';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'UnitPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預設價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'Cost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量/件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'QtyGarment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格/件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發出數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'Farmout';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'返回數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'Farmin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結帳數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'ApQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'PoQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'額外加工數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO_Detail', @level2type = N'COLUMN', @level2name = N'ExceedQty';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[ArtworkPO_Detail]([OrderID] ASC);

