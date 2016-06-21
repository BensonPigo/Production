CREATE TABLE [dbo].[FarmIn_Detail] (
    [ID]                   VARCHAR (13)  CONSTRAINT [DF_FarmIn_Detail_ID] DEFAULT ('') NOT NULL,
    [ArtworkPoID]          VARCHAR (13)  CONSTRAINT [DF_FarmIn_Detail_ArtworkPoID] DEFAULT ('') NOT NULL,
    [Orderid]              VARCHAR (13)  CONSTRAINT [DF_FarmIn_Detail_Orderid] DEFAULT ('') NOT NULL,
    [ArtworkID]            VARCHAR (20)  CONSTRAINT [DF_FarmIn_Detail_ArtworkID] DEFAULT ('') NOT NULL,
    [PatternCode]          VARCHAR (20)  CONSTRAINT [DF_FarmIn_Detail_PatternCode] DEFAULT ('') NOT NULL,
    [PatternDesc]          NVARCHAR (40) CONSTRAINT [DF_FarmIn_Detail_PatternDesc] DEFAULT ('') NULL,
    [ArtworkPoQty]         NUMERIC (6)   CONSTRAINT [DF_FarmIn_Detail_ArtworkPoQty] DEFAULT ((0)) NULL,
    [OnHand]               NUMERIC (6)   CONSTRAINT [DF_FarmIn_Detail_OnHand] DEFAULT ((0)) NULL,
    [Qty]                  NUMERIC (6)   CONSTRAINT [DF_FarmIn_Detail_Qty] DEFAULT ((0)) NULL,
    [Bundleno]             VARCHAR (10)  CONSTRAINT [DF_FarmIn_Detail_Bundleno] DEFAULT ('') NOT NULL,
    [ArtworkPo_DetailUkey] BIGINT        CONSTRAINT [DF_FarmIn_Detail_ArtworkPo_DetailUkey] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FarmIn_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [ArtworkPoID] ASC, [Orderid] ASC, [ArtworkID] ASC, [PatternCode] ASC, [Bundleno] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工返回明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發放單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkPoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'Orderid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工版型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片縮寫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkPoQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'已返回數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'OnHand';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次返回數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捆包編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'Bundleno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkPo_DetailUkey';


GO
CREATE NONCLUSTERED INDEX [ArtworkPo_DetailUkey]
    ON [dbo].[FarmIn_Detail]([ArtworkPo_DetailUkey] ASC)
    INCLUDE([ID], [Qty]);


GO
CREATE NONCLUSTERED INDEX [IX_FarmIn_Detail_ID_ArtworkID_PatternCode_Qty]
    ON [dbo].[FarmIn_Detail]([Orderid] ASC)
    INCLUDE([ID], [ArtworkID], [PatternCode], [Qty]);

