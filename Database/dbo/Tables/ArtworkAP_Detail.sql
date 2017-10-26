CREATE TABLE [dbo].[ArtworkAP_Detail] (
    [ID]                   VARCHAR (13)    CONSTRAINT [DF_ArtworkAP_Detail_ID] DEFAULT ('') NOT NULL,
    [ArtworkPoID]          VARCHAR (13)    CONSTRAINT [DF_ArtworkAP_Detail_ArtworkPoID] DEFAULT ('') NOT NULL,
    [OrderID]              VARCHAR (13)    CONSTRAINT [DF_ArtworkAP_Detail_OrderID] DEFAULT ('') NOT NULL,
    [ArtworkID]            VARCHAR (20)    CONSTRAINT [DF_ArtworkAP_Detail_ArtworkID] DEFAULT ('') NOT NULL,
    [PatternCode]          VARCHAR (20)    CONSTRAINT [DF_ArtworkAP_Detail_PatternCode] DEFAULT ('') NULL,
    [PatternDesc]          NVARCHAR (40)   CONSTRAINT [DF_ArtworkAP_Detail_PatternDesc] DEFAULT ('') NULL,
    [Stitch]               NUMERIC (6)     CONSTRAINT [DF_ArtworkAP_Detail_Stitch] DEFAULT ((0)) NULL,
    [Price]                NUMERIC (12, 4) CONSTRAINT [DF_ArtworkAP_Detail_Price] DEFAULT ((0)) NULL,
    [Amount]               NUMERIC (14, 4) CONSTRAINT [DF_ArtworkAP_Detail_Amount] DEFAULT ((0)) NULL,
    [Farmin]               NUMERIC (6)     CONSTRAINT [DF_ArtworkAP_Detail_Farmin] DEFAULT ((0)) NULL,
    [ApQty]                NUMERIC (6)     CONSTRAINT [DF_ArtworkAP_Detail_ApQty] DEFAULT ((0)) NULL,
    [AccumulatedQty]       NUMERIC (6)     CONSTRAINT [DF_ArtworkAP_Detail_AccumulatedQty] DEFAULT ((0)) NULL,
    [ArtworkPo_DetailUkey] BIGINT          CONSTRAINT [DF_ArtworkAP_Detail_ArtworkPo_DetailUkey] DEFAULT ((0)) NULL,
    [Ukey]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ArtworkAP_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工應付明細檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkPoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工版型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'個數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'Stitch';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格/件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'累計返回數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'Farmin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結帳數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'ApQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'累計結帳數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'AccumulatedQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkPo_DetailUkey';

GO
CREATE NONCLUSTERED INDEX index_OrderID ON dbo.ArtworkAP_Detail(OrderID ASC) include([ID],[Price],[ApQty])

