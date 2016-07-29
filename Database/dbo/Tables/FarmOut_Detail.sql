CREATE TABLE [dbo].[FarmOut_Detail] (
    [ID]                   VARCHAR (13)  CONSTRAINT [DF_FarmOut_Detail_ID] DEFAULT ('') NOT NULL,
    [ArtworkPoid]          VARCHAR (13)  CONSTRAINT [DF_FarmOut_Detail_ArtworkPoid] DEFAULT ('') NOT NULL,
    [Orderid]              VARCHAR (13)  CONSTRAINT [DF_FarmOut_Detail_Orderid] DEFAULT ('') NOT NULL,
    [ArtworkID]            VARCHAR (20)  CONSTRAINT [DF_FarmOut_Detail_ArtworkID] DEFAULT ('') NOT NULL,
    [PatternCode]          VARCHAR (20)  CONSTRAINT [DF_FarmOut_Detail_PatternCode] DEFAULT ('') NOT NULL,
    [PatternDesc]          NVARCHAR (40) CONSTRAINT [DF_FarmOut_Detail_PatternDesc] DEFAULT ('') NULL,
    [ArtworkPoQty]         NUMERIC (6)   CONSTRAINT [DF_FarmOut_Detail_ArtworkPoQty] DEFAULT ((0)) NULL,
    [OnHand]               NUMERIC (6)   CONSTRAINT [DF_FarmOut_Detail_OnHand] DEFAULT ((0)) NULL,
    [Qty]                  NUMERIC (6)   CONSTRAINT [DF_FarmOut_Detail_Qty] DEFAULT ((0)) NULL,
    [BundleNo]             VARCHAR (10)  CONSTRAINT [DF_FarmOut_Detail_BundleNo] DEFAULT ('') NOT NULL,
    [ArtworkPo_DetailUkey] BIGINT        CONSTRAINT [DF_FarmOut_Detail_ArtworkPo_DetailUkey] DEFAULT ((0)) NOT NULL,
    [Ukey]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_FarmOut_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工發放明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發放單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkPoid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'Orderid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工版型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片縮寫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkPoQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'已發放數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'OnHand';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次發放數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捆包編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut_Detail', @level2type = N'COLUMN', @level2name = N'ArtworkPo_DetailUkey';


GO
CREATE NONCLUSTERED INDEX [ArtworkPo_DetailUkey]
    ON [dbo].[FarmOut_Detail]([ArtworkPo_DetailUkey] ASC)
    INCLUDE([ID], [Qty]);

