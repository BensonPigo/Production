CREATE TABLE [dbo].[Pullout_Detail] (
    [ID]               VARCHAR (13)  CONSTRAINT [DF_Pullout_Detail_ID] DEFAULT ('') NOT NULL,
    [PulloutDate]      DATE          NOT NULL,
    [OrderID]          VARCHAR (13)  CONSTRAINT [DF_Pullout_Detail_OrderID] DEFAULT ('') NOT NULL,
    [OrderShipmodeSeq] VARCHAR (2)   CONSTRAINT [DF_Pullout_Detail_OrderShipmodeSeq] DEFAULT ('') NOT NULL,
    [ShipQty]          INT           CONSTRAINT [DF_Pullout_Detail_ShipQty] DEFAULT ((0)) NULL,
    [OrderQty]         INT           CONSTRAINT [DF_Pullout_Detail_OrderQty] DEFAULT ((0)) NULL,
    [ShipModeSeqQty]   INT           CONSTRAINT [DF_Pullout_Detail_ShipModeSeqQty] DEFAULT ((0)) NULL,
    [Status]           VARCHAR (1)   CONSTRAINT [DF_Pullout_Detail_Status] DEFAULT ('') NULL,
    [PackingListID]    VARCHAR (13)  CONSTRAINT [DF_Pullout_Detail_PackingListID] DEFAULT ('') NULL,
    [PackingListType]  VARCHAR (1)   CONSTRAINT [DF_Pullout_Detail_PackingListType] DEFAULT ('') NULL,
    [INVNo]            VARCHAR (25)  CONSTRAINT [DF_Pullout_Detail_INVNo] DEFAULT ('') NULL,
    [ShipmodeID]       VARCHAR (10)  CONSTRAINT [DF_Pullout_Detail_ShipmodeID] DEFAULT ('') NULL,
    [Remark]           NVARCHAR (30) CONSTRAINT [DF_Pullout_Detail_Remark] DEFAULT ('') NULL,
    [ReviseDate]       DATETIME      NULL,
    [UKey]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [OldUkey]          VARCHAR (13)  CONSTRAINT [DF_Pullout_Detail_OldUkey] DEFAULT ('') NULL,
	AddName			VARCHAR(10) CONSTRAINT DF_Pullout_Detail_AddName DEFAULT ('') NOT NULL,
	AddDate			DATETIME NULL,
	EditName		VARCHAR(10) CONSTRAINT DF_Pullout_Detail_EditName DEFAULT ('') NOT NULL,
	EditDate		DATETIME NULL,
    CONSTRAINT [PK_Pullout_Detail] PRIMARY KEY CLUSTERED ([UKey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout Report  Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'PulloutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty Breakdown Shipmode的Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'OrderShipmodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'OrderQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty Breakdown Shipmode的Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'ShipModeSeqQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'PackingListID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'PackingListType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment Booking ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'INVNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipmode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'ShipmodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠最後Revise日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'ReviseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'UKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail', @level2type = N'COLUMN', @level2name = N'OldUkey';


GO
CREATE NONCLUSTERED INDEX [Index_OrderID]
    ON [dbo].[Pullout_Detail]([OrderID] ASC);

