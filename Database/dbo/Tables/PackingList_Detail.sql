CREATE TABLE [dbo].[PackingList_Detail] (
    [ID]                  VARCHAR (13)    CONSTRAINT [DF_PackingList_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderID]             VARCHAR (13)    CONSTRAINT [DF_PackingList_Detail_OrderID] DEFAULT ('') NOT NULL,
    [OrderShipmodeSeq]    VARCHAR (2)     CONSTRAINT [DF_PackingList_Detail_OrderShipmodeSeq] DEFAULT ('') NOT NULL,
    [RefNo]               VARCHAR (21)    CONSTRAINT [DF_PackingList_Detail_RefNo] DEFAULT ('') NULL,
    [CTNStartNo]          VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_CTNStartNo] DEFAULT ('') NOT NULL,
    [CTNEndNo]            VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_CTNEndNo] DEFAULT ('') NULL,
    [CTNQty]              INT             CONSTRAINT [DF_PackingList_Detail_CTNQty] DEFAULT ((0)) NULL,
    [Seq]                 VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_Seq] DEFAULT ('') NULL,
    [Article]             VARCHAR (8)     CONSTRAINT [DF_PackingList_Detail_Article] DEFAULT ('') NOT NULL,
    [Color]               VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_Color] DEFAULT ('') NULL,
    [SizeCode]            VARCHAR (8)     CONSTRAINT [DF_PackingList_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [QtyPerCTN]           INT             CONSTRAINT [DF_PackingList_Detail_QtyPerCTN] DEFAULT ((0)) NULL,
    [ShipQty]             INT             CONSTRAINT [DF_PackingList_Detail_ShipQty] DEFAULT ((0)) NULL,
    [NW]                  NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_NW] DEFAULT ((0)) NULL,
    [GW]                  NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_GW] DEFAULT ((0)) NULL,
    [NNW]                 NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_NNW] DEFAULT ((0)) NULL,
    [NWPerPcs]            NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_NWPerPcs] DEFAULT ((0)) NULL,
    [TransferDate]        DATE            NULL,
    [ReceiveDate]         DATE            NULL,
    [ClogLocationId]      VARCHAR (10)    CONSTRAINT [DF_PackingList_Detail_ClogLocationId] DEFAULT ('') NULL,
    [ReturnDate]          DATE            NULL,
    [Barcode]             VARCHAR (30)    CONSTRAINT [DF_PackingList_Detail_Barcode] DEFAULT ('') NULL,
    [ScanQty]             SMALLINT        CONSTRAINT [DF_PackingList_Detail_ScanQty] DEFAULT ((0)) NULL,
    [ScanEditDate]        DATETIME        NULL,
    [Remark]              NVARCHAR (40)   CONSTRAINT [DF_PackingList_Detail_Remark] DEFAULT ('') NULL,
    [Ukey]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [TransferCFADate]     DATE            NULL,
    [CFAReceiveDate]      DATE            NULL,
    [CFAReturnFtyDate]    DATE            NULL,
    [CFAReturnClogDate]   DATE            NULL,
    [ClogReceiveCFADate]  DATE            NULL,
    [CFANeedInsp]         BIT             CONSTRAINT [DF_PackingList_Detail_CFANeedInsp] DEFAULT ((0)) NOT NULL,
    [CFAInspDate]         DATE            NULL,
    [ScanName]            VARCHAR (10)    DEFAULT ('') NULL,
    [CustCTN]             VARCHAR (30)    DEFAULT ('') NOT NULL,
    [DRYReceiveDate]      DATE            NULL,
    [EditLocationDate]    DATETIME        NULL,
    [ActCTNWeight]        NUMERIC (7, 3)  NULL,
    [EditLocationName]    VARCHAR (10)    DEFAULT ('') NULL,
    [PackErrTransferDate] DATE            NULL,
    [Lacking]             BIT             DEFAULT ((0)) NULL,
    [FtyReqReturnDate]    DATE            NULL,
    [FtyReqReturnReason]  VARCHAR (5)     DEFAULT ('') NOT NULL,
    [DisposeFromClog]     BIT             CONSTRAINT [DF_PackingList_Detail_DisposeFromClog] DEFAULT ((0)) NULL,
    [SCICtnNo]            VARCHAR (15)    CONSTRAINT [DF_PackingList_Detail_SCICtnNo] DEFAULT ('') NULL,
    [Pallet]              VARCHAR (10)    NULL,
    [NewGW]               NUMERIC (7, 3)  NULL,
    [OrigID]              VARCHAR (13)    CONSTRAINT [DF_PackingList_Detail_OrigID] DEFAULT ('') NOT NULL,
    [OrigOrderID]         VARCHAR (13)    CONSTRAINT [DF_PackingList_Detail_OrigOrderID] DEFAULT ('') NOT NULL,
    [OrigCTNStartNo]      VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_OrigCTNStartNo] DEFAULT ('') NOT NULL,
    [DisposeDate]         DATE            NULL,
    [APPBookingVW]        NUMERIC (20, 2) DEFAULT ((0)) NULL,
    [APPEstAmtVW]         NUMERIC (20, 2) DEFAULT ((0)) NULL,
    [CFALocationID]       VARCHAR (10)    DEFAULT ('') NULL,
    [EditCFALocationDate] DATETIME        NULL,
    [EditCFALocationName] VARCHAR (10)    DEFAULT ('') NULL,
    [ClogPulloutName]     VARCHAR (10)    DEFAULT ('') NOT NULL,
    [ClogPulloutDate]     DATE            NULL,
    [PulloutTransport]    VARCHAR (1)     DEFAULT ('') NOT NULL,
    [PulloutTransportNo]  VARCHAR (10)    DEFAULT ('') NOT NULL,
    [MDScanDate] DATE NULL, 
    [MDFailQty] INT NOT NULL DEFAULT (0), 
    [CFASelectInspDate] DATE NULL, 
    [StaggeredCFAInspectionRecordID] VARCHAR(13) NOT NULL DEFAULT (''), 
	FirstStaggeredCFAInspectionRecordID Varchar(500) NOT NULL CONSTRAINT [DF_PackingList_Detail_FirstStaggeredCFAInspectionRecordID] DEFAULT '',
	PrePackQty int NOT NULL CONSTRAINT [DF_PackingList_Detail_PrePackQty]  DEFAULT(0),
    [DRYTransferDate] DATE NULL, 
    CONSTRAINT [PK_Ukey] PRIMARY KEY CLUSTERED ([Ukey] ASC)
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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer To Clog Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'TransferDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clog Receive Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ReceiveDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton in Clog Location', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogLocationId';
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
GO


Go
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'����CFA���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'TransferCFADate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFAReceiveDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA�h�^Fty��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFAReturnFtyDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA�h�^Clog��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFAReturnClogDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clog���籵����', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogReceiveCFADate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA�ݭn���窺�c�l', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFANeedInsp';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���ëǦ��c��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'DRYReceiveDate';
GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-forOrderID]
    ON [dbo].[PackingList_Detail]([OrderID] ASC);
GO
CREATE NONCLUSTERED INDEX [QA_R23_ReceiveDate]
    ON [dbo].[PackingList_Detail]([ReceiveDate] ASC)
    INCLUDE([OrderID], [OrderShipmodeSeq]);
GO
CREATE NONCLUSTERED INDEX [Index_OrderIDOrderShipmodeSeqReceiveDate]
    ON [dbo].[PackingList_Detail]([OrderID] ASC, [ID] ASC, [ReceiveDate] ASC, [CTNStartNo] ASC)
    INCLUDE([OrderShipmodeSeq]);
GO
CREATE NONCLUSTERED INDEX [IN,CTNStartNo]
    ON [dbo].[PackingList_Detail]([ID] ASC, [CTNStartNo] ASC);
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��ڽc�l�`��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ActCTNWeight';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�̫��sLocation�H��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'EditLocationName';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ץ�location�ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'EditLocationDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�u�t�n�D�h�^��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'FtyReqReturnDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�O�_�ʥ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'Lacking';


GO

GO

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�n�D�h�c��]', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'FtyReqReturnReason';


GO
CREATE NONCLUSTERED INDEX [IX_PackingList_Detail_OrgPK]
    ON [dbo].[PackingList_Detail]([ID] ASC, [OrderID] ASC, [OrderShipmodeSeq] ASC, [CTNStartNo] ASC, [Article] ASC, [SizeCode] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新CFALocation人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'EditCFALocationName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新CFALocation時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'EditCFALocationDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA儲位代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFALocationID';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'卡車/貨櫃代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'PulloutTransportNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝上卡車/貨櫃', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'PulloutTransport';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱從倉庫移出準備出貨(裝上卡車/貨櫃)的人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogPulloutName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱從倉庫移出準備出貨(裝上卡車/貨櫃)的日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogPulloutDate';

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'金屬檢測日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'MDScanDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'金屬檢測失敗數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'MDFailQty'

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'此次檢驗的紙箱箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail ', @level2type = N'COLUMN', @level2name = N'FirstStaggeredCFAInspectionRecordID';

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
	@value = N'混尺碼裝箱各色組尺寸 1 個塑膠袋裝入的件數',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'PackingList_Detail',
	@level2type = N'COLUMN',
	@level2name = N'PrePackQty'
;
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'移轉日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'DRYTransferDate'