CREATE TABLE [dbo].[PackingList_Detail] (
    [ID]               VARCHAR (13)   CONSTRAINT [DF_PackingList_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderID]          VARCHAR (13)   CONSTRAINT [DF_PackingList_Detail_OrderID] DEFAULT ('') NOT NULL,
    [OrderShipmodeSeq] VARCHAR (2)    CONSTRAINT [DF_PackingList_Detail_OrderShipmodeSeq] DEFAULT ('') NOT NULL,
    [RefNo]            VARCHAR (21)   CONSTRAINT [DF_PackingList_Detail_RefNo] DEFAULT ('') NULL,
    [CTNStartNo]       VARCHAR (6)    CONSTRAINT [DF_PackingList_Detail_CTNStartNo] DEFAULT ('') NOT NULL,
    [CTNEndNo]         VARCHAR (6)    CONSTRAINT [DF_PackingList_Detail_CTNEndNo] DEFAULT ('') NULL,
    [CTNQty]           INT            CONSTRAINT [DF_PackingList_Detail_CTNQty] DEFAULT ((0)) NULL,
    [Seq]              VARCHAR (6)    CONSTRAINT [DF_PackingList_Detail_Seq] DEFAULT ('') NULL,
    [Article]          VARCHAR (8)    CONSTRAINT [DF_PackingList_Detail_Article] DEFAULT ('') NOT NULL,
    [Color]            VARCHAR (6)    CONSTRAINT [DF_PackingList_Detail_Color] DEFAULT ('') NULL,
    [SizeCode]         VARCHAR (8)    CONSTRAINT [DF_PackingList_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [QtyPerCTN]        SMALLINT       CONSTRAINT [DF_PackingList_Detail_QtyPerCTN] DEFAULT ((0)) NULL,
    [ShipQty]          INT            CONSTRAINT [DF_PackingList_Detail_ShipQty] DEFAULT ((0)) NULL,
    [NW]               NUMERIC (6, 3) CONSTRAINT [DF_PackingList_Detail_NW] DEFAULT ((0)) NULL,
    [GW]               NUMERIC (6, 3) CONSTRAINT [DF_PackingList_Detail_GW] DEFAULT ((0)) NULL,
    [NNW]              NUMERIC (6, 3) CONSTRAINT [DF_PackingList_Detail_NNW] DEFAULT ((0)) NULL,
    [NWPerPcs]         NUMERIC (5, 3) CONSTRAINT [DF_PackingList_Detail_NWPerPcs] DEFAULT ((0)) NULL,
    [TransferDate]     DATE           NULL,
    [ReceiveDate]      DATE           NULL,
    [ClogLocationId]   VARCHAR (10)   CONSTRAINT [DF_PackingList_Detail_ClogLocationId] DEFAULT ('') NULL,
    [ReturnDate]       DATE           NULL,
    [Barcode]          VARCHAR (30)   CONSTRAINT [DF_PackingList_Detail_Barcode] DEFAULT ('') NULL,
    [ScanQty]          SMALLINT       CONSTRAINT [DF_PackingList_Detail_ScanQty] DEFAULT ((0)) NULL,
    [ScanEditDate]     DATETIME       NULL,
    [Remark]           NVARCHAR (40)  CONSTRAINT [DF_PackingList_Detail_Remark] DEFAULT ('') NULL,
    CONSTRAINT [PK_PackingList_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [OrderID] ASC, [OrderShipmodeSeq] ASC, [CTNStartNo] ASC, [Article] ASC, [SizeCode] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List Weight & Summary Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PackingList Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty Breakdown Shipmode的Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'OrderShipmodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱子的物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起始箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結束箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CTNEndNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CTN#中的流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'Color';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每箱數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'QtyPerCTN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'NW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'GW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨淨重(不含包裝的重量)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'NNW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每件淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'NWPerPcs';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer To Clog Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'TransferDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clog Receive Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ReceiveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton in Clog Location', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogLocationId';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clog Return Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ReturnDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'條碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'Barcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'掃瞄件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ScanQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'掃描最後修改日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ScanEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'移箱檢驗備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
CREATE NONCLUSTERED INDEX [Index_OrderIDOrderShipmodeSeq]
    ON [dbo].[PackingList_Detail]([OrderID] ASC, [OrderShipmodeSeq] ASC)
    INCLUDE([RefNo]);

