CREATE TABLE [dbo].[PackingList_Detail] (
    [ID]                                  VARCHAR (13)    CONSTRAINT [DF_PackingList_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderID]                             VARCHAR (13)    CONSTRAINT [DF_PackingList_Detail_OrderID] DEFAULT ('') NOT NULL,
    [OrderShipmodeSeq]                    VARCHAR (2)     CONSTRAINT [DF_PackingList_Detail_OrderShipmodeSeq] DEFAULT ('') NOT NULL,
    [RefNo]                               VARCHAR (21)    CONSTRAINT [DF_PackingList_Detail_RefNo] DEFAULT ('') NULL,
    [CTNStartNo]                          VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_CTNStartNo] DEFAULT ('') NOT NULL,
    [CTNEndNo]                            VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_CTNEndNo] DEFAULT ('') NULL,
    [CTNQty]                              INT             CONSTRAINT [DF_PackingList_Detail_CTNQty] DEFAULT ((0)) NULL,
    [Seq]                                 VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_Seq] DEFAULT ('') NULL,
    [Article]                             VARCHAR (8)     CONSTRAINT [DF_PackingList_Detail_Article] DEFAULT ('') NOT NULL,
    [Color]                               VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_Color] DEFAULT ('') NULL,
    [SizeCode]                            VARCHAR (8)     CONSTRAINT [DF_PackingList_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [QtyPerCTN]                           INT             CONSTRAINT [DF_PackingList_Detail_QtyPerCTN] DEFAULT ((0)) NULL,
    [ShipQty]                             INT             CONSTRAINT [DF_PackingList_Detail_ShipQty] DEFAULT ((0)) NULL,
    [NW]                                  NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_NW] DEFAULT ((0)) NULL,
    [GW]                                  NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_GW] DEFAULT ((0)) NULL,
    [NNW]                                 NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_NNW] DEFAULT ((0)) NULL,
    [NWPerPcs]                            NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_NWPerPcs] DEFAULT ((0)) NULL,
    [TransferDate]                        DATE            NULL,
    [ReceiveDate]                         DATE            NULL,
    [ClogLocationId]                      NVARCHAR (50)   CONSTRAINT [DF_PackingList_Detail_ClogLocationId] DEFAULT ('') NULL,
    [ReturnDate]                          DATE            NULL,
    [Barcode]                             VARCHAR (30)    CONSTRAINT [DF_PackingList_Detail_Barcode] DEFAULT ('') NULL,
    [ScanQty]                             SMALLINT        CONSTRAINT [DF_PackingList_Detail_ScanQty] DEFAULT ((0)) NULL,
    [ScanEditDate]                        DATETIME        NULL,
    [Remark]                              NVARCHAR (40)   CONSTRAINT [DF_PackingList_Detail_Remark] DEFAULT ('') NULL,
    [Ukey]                                BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TransferCFADate]                     DATE            NULL,
    [CFAReceiveDate]                      DATE            NULL,
    [CFAReturnFtyDate]                    DATE            NULL,
    [CFAReturnClogDate]                   DATE            NULL,
    [ClogReceiveCFADate]                  DATE            NULL,
    [CFANeedInsp]                         BIT             CONSTRAINT [DF_PackingList_Detail_CFANeedInsp] DEFAULT ((0)) NOT NULL,
    [CFAInspDate]                         DATE            NULL,
    [ScanName]                            VARCHAR (10)    DEFAULT ('') NULL,
    [CustCTN]                             VARCHAR (30)    DEFAULT ('') NOT NULL,
    [DRYReceiveDate]                      DATE            NULL,
    [EditLocationDate]                    DATETIME        NULL,
    [ActCTNWeight]                        NUMERIC (7, 3)  NULL,
    [EditLocationName]                    VARCHAR (10)    DEFAULT ('') NULL,
    [PackErrTransferDate]                 DATE            NULL,
    [Lacking]                             BIT             DEFAULT ((0)) NULL,
    [FtyReqReturnDate]                    DATE            NULL,
    [FtyReqReturnReason]                  VARCHAR (5)     DEFAULT ('') NOT NULL,
    [DisposeFromClog]                     BIT             CONSTRAINT [DF_PackingList_Detail_DisposeFromClog] DEFAULT ((0)) NULL,
    [SCICtnNo]                            VARCHAR (16)    CONSTRAINT [DF_PackingList_Detail_SCICtnNo] DEFAULT ('') NULL,
    [Pallet]                              NVARCHAR (50)   NULL,
    [NewGW]                               NUMERIC (7, 3)  NULL,
    [OrigID]                              VARCHAR (13)    CONSTRAINT [DF_PackingList_Detail_OrigID] DEFAULT ('') NOT NULL,
    [OrigOrderID]                         VARCHAR (13)    CONSTRAINT [DF_PackingList_Detail_OrigOrderID] DEFAULT ('') NOT NULL,
    [OrigCTNStartNo]                      VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_OrigCTNStartNo] DEFAULT ('') NOT NULL,
    [DisposeDate]                         DATE            NULL,
    [APPBookingVW]                        NUMERIC (20, 2) DEFAULT ((0)) NULL,
    [APPEstAmtVW]                         NUMERIC (20, 2) DEFAULT ((0)) NULL,
    [ClogPulloutName]                     VARCHAR (10)    DEFAULT ('') NOT NULL,
    [ClogPulloutDate]                     DATE            NULL,
    [PulloutTransport]                    VARCHAR (1)     DEFAULT ('') NOT NULL,
    [PulloutTransportNo]                  VARCHAR (10)    DEFAULT ('') NOT NULL,
    [CFALocationID]                       VARCHAR (10)    DEFAULT ('') NULL,
    [EditCFALocationDate]                 DATETIME        NULL,
    [EditCFALocationName]                 VARCHAR (10)    DEFAULT ('') NULL,
    [CFASelectInspDate]                   DATE            NULL,
    [StaggeredCFAInspectionRecordID]      VARCHAR (15)    CONSTRAINT [DF_StaggeredCFAInspectionRecordID] DEFAULT ('') NOT NULL,
    [FirstStaggeredCFAInspectionRecordID] VARCHAR (500)   CONSTRAINT [DF_PackingList_Detail_FirstStaggeredCFAInspectionRecordID] DEFAULT ('') NOT NULL,
    [PrePackQty]                          INT             CONSTRAINT [DF_PackingList_Detail_PrePackQty] DEFAULT ((0)) NOT NULL,
    [DRYTransferDate]                     DATE            NULL,
    [PackingReasonERID]                   VARCHAR (5)     CONSTRAINT [DF_PackingList_Detail_PackingReasonERID] DEFAULT ('') NOT NULL,
    [ErrQty]                              SMALLINT        CONSTRAINT [DF_PackingList_Detail_ErrQty] DEFAULT ((0)) NOT NULL,
    [AuditQCName]                         VARCHAR (30)    CONSTRAINT [DF_PackingList_Detail_AuditQCName] DEFAULT ('') NOT NULL,
    [PackingErrQty]                       SMALLINT        CONSTRAINT [DF_PackingList_Detail_PackingErrQty] DEFAULT ((0)) NOT NULL,
    [PackingErrorID]                      VARCHAR (8)     CONSTRAINT [DF_PackingList_Detail_PackingErrorID] DEFAULT ('') NOT NULL,
    [HaulingDate]                         DATE            NULL,
    [OriClogLocationID]                   NVARCHAR (50)   DEFAULT ('') NOT NULL,
    [PackingAuditDate]                    DATE            NULL,
    [PackingAuditStatus]                  VARCHAR (6)     DEFAULT ('') NOT NULL,
    [ClogPackingErrorDate]                DATE            NULL,
    [ClogPackingErrorID]                  VARCHAR (8)     CONSTRAINT [DF_PackingList_Detail_ClogPackingErrorID] DEFAULT ('') NOT NULL,
    [ClogPackingErrorQty]                 SMALLINT        CONSTRAINT [DF_PackingList_Detail_ClogPackingErrorQty] DEFAULT ((0)) NOT NULL,
    [ScanPackMDDate]                      DATETIME        NULL,
    [ClogScanPackMDDate]                  DATETIME        NULL,
    [DryRoomMDScanDate]                   DATE            NULL,
    [DryRoomMDFailQty]                    INT             CONSTRAINT [DF_PackingList_Detail_DryRoomMDFailQty] DEFAULT ((0)) NOT NULL,
    [DryRoomMDScanName]                   VARCHAR (10)    CONSTRAINT [DF_PackingList_Detail_DryRoomMDScanName] DEFAULT ('') NOT NULL,
    [DryRoomMDStatus]                     VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_DryRoomMDStatus] DEFAULT ('') NOT NULL,
    [HaulingStatus]                       VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_HaulingStatus] DEFAULT ('') NOT NULL,
    [M360MDStatus]                        VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_M360MDStatus] DEFAULT ('') NOT NULL,
    [M360MDScanDate]                      DATE            NULL,
    [M360MDFailQty]                       INT             CONSTRAINT [DF_PackingList_Detail_[M360MDFailQty] DEFAULT ((0)) NOT NULL,
    [M360MDScanName]                      VARCHAR (10)    CONSTRAINT [DF_PackingList_Detail_M360MDScanName] DEFAULT ('') NOT NULL,
    [MDMachineNo]                         VARCHAR (30)    DEFAULT ('') NOT NULL,
    [CustCTN2]                            VARCHAR (30)    CONSTRAINT [DF_PackingList_Detail_CustCTN2] DEFAULT ('') NOT NULL,
    [ClogScanQty]                         SMALLINT        CONSTRAINT [DF_PackingList_Detail_ClogScanQty] DEFAULT ((0)) NOT NULL,
    [ClogLackingQty]                      SMALLINT        CONSTRAINT [DF_PackingList_Detail_ClogLackingQty] DEFAULT ((0)) NOT NULL,
    [ClogScanDate]                        DATETIME        CONSTRAINT [DF_PackingList_Detail_ClogScanDate] DEFAULT ((0)) NULL,
    [ClogScanName]                        VARCHAR (10)    CONSTRAINT [DF_PackingList_Detail_ClogScanName] DEFAULT ((0)) NOT NULL,
    [ClogPulloutIsFrom]                   INT             CONSTRAINT [DF_PackingList_Detail_ClogPulloutIsFrom] DEFAULT ((0)) NOT NULL,
    [ClogActCTNWeight]                    NUMERIC (7, 3)  CONSTRAINT [DF_PackingList_Detail_ClogActCTNWeight] DEFAULT ((0)) NOT NULL,
    [HangerPackScanTime]                  DATE            NULL,
    [HangerPackStatus]                    VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_HangerPackStatus] DEFAULT ('') NOT NULL,
    [JokerTagScanTime]                    DATE            NULL,
    [JokerTagStatus]                      VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_JokerTagStatus] DEFAULT ('') NOT NULL,
    [HeatSealScanTime]                    DATE            NULL,
    [HeatSealStatus]                      VARCHAR (6)     CONSTRAINT [DF_PackingList_Detail_HeatSealStatus] DEFAULT ('') NOT NULL,
    [HaulingFailQty]                      INT             CONSTRAINT [DF_PackingList_Detail_HaulingFailQty] DEFAULT ((0)) NOT NULL,
    [PackingAuditFailQty]                 INT             CONSTRAINT [DF_PackingList_Detail_PackingAuditFailQty] DEFAULT ((0)) NOT NULL,
    [HangerPackFailQty]                   INT             CONSTRAINT [DF_PackingList_Detail_HangerPackFailQty] DEFAULT ((0)) NOT NULL,
    [JokerTagFailQty]                     INT             CONSTRAINT [DF_PackingList_Detail_JokerTagFailQty] DEFAULT ((0)) NOT NULL,
    [HeatSealFailQty]                     INT             CONSTRAINT [DF_PackingList_Detail_HeatSealFailQty] DEFAULT ((0)) NOT NULL,
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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'任一箱曾經出分撿倉 或是 曾經入過成品倉', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'TransferCFADate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA接收日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFAReceiveDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA退回Fty日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFAReturnFtyDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA退回Clog日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFAReturnClogDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clog檢驗接收日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogReceiveCFADate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA需要檢驗的箱子', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFANeedInsp';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'除溼室收箱日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'DRYReceiveDate';
GO
CREATE NONCLUSTERED INDEX [IDX_PackingList_Detail_ID]
    ON [dbo].[PackingList_Detail]([ID] ASC);
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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際箱子總重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ActCTNWeight';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後更新Location人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'EditLocationName';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後更新Location時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'EditLocationDate';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠要求退回日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'FtyReqReturnDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否缺件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'Lacking';


GO

GO

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'要求退箱原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'FtyReqReturnReason';


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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA 紙箱從倉庫移出準備出貨(裝上卡車/貨櫃)的日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogPulloutDate';





GO




GO




GO


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'混尺碼裝箱各色組尺寸 1 個塑膠袋裝入的件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'PrePackQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'移轉日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'DRYTransferDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'此次檢驗的紙箱箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'FirstStaggeredCFAInspectionRecordID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA 挑箱的日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'CFASelectInspDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'包裝異常的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'PackingErrQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'包裝異常的原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'PackingErrorID';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA 發現包裝錯誤日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogPackingErrorDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA 包裝異常的原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogPackingErrorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA 包裝異常的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogPackingErrorQty';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄第一次過MD的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ScanPackMDDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'用來記錄第二次過MD的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ClogScanPackMDDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Hauling狀態
通過 : status = Haul
退回 : status = Return',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'HaulingStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'(M360)MD狀態
通過 : Pass
不通過 : Hold
退回 : Return
',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'M360MDStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'(M360)MD日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'M360MDScanDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'(M360)MD Fail數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'M360MDFailQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'(M360)MD登入ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'M360MDScanName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'成品倉掃描數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ClogScanQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'成品倉缺件數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ClogLackingQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'成品倉最後掃描日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ClogScanDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'成品倉最後掃描人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ClogScanName'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'	
是否來自M360 Clog Pullout
0 = 否，來自PMS Clog P12
1 = 是，來自M360 Clog P12', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'ClogPulloutIsFrom';


go
CREATE NONCLUSTERED INDEX [IDX_Packinglist_Detail_CustCTN]
    ON [dbo].[PackingList_Detail]([CustCTN] ASC)
GO
CREATE NONCLUSTERED INDEX [Index_ScanEditDate]
    ON [dbo].[PackingList_Detail]([OrderID] ASC)
    INCLUDE([ScanEditDate]);


GO
CREATE NONCLUSTERED INDEX [Index_QAR31]
    ON [dbo].[PackingList_Detail]([OrderID] ASC, [OrderShipmodeSeq] ASC, [CTNQty] ASC)
    INCLUDE([CTNStartNo], [ReceiveDate], [CFAReceiveDate]);


GO
CREATE NONCLUSTERED INDEX [Index_ID_CTNStartNo_to_SizeCode_QtyPerCTN]
    ON [dbo].[PackingList_Detail]([ID] ASC, [CTNStartNo] ASC)
    INCLUDE([SizeCode], [QtyPerCTN]);


GO
CREATE NONCLUSTERED INDEX [IDX_PackingList_Detail_SCICtnNo]
    ON [dbo].[PackingList_Detail]([SCICtnNo] DESC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'MDMachineNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Joker Tag 掃描時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'JokerTagScanTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Heat Seal 是否被 Return', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'HeatSealStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Heat Seal 掃描時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'HeatSealScanTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Hanger Pack 是否被 Return', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'HangerPackStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Hanger Pack 掃描時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'HangerPackScanTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing Audit Fail數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'PackingAuditFailQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Joker Tag 是否被 Return', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'JokerTagStatus';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Joker Tag Fail數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'JokerTagFailQty';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Heat Seal Fail數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'HeatSealFailQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hauling Fail數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'HaulingFailQty';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hanger Pack Fail數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList_Detail', @level2type = N'COLUMN', @level2name = N'HangerPackFailQty';

