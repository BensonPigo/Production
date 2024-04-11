CREATE TABLE [dbo].[OrderComparisonList] (
    [TransferDate]          DATE         NULL,
    [UpdateDate]            DATE         NULL,
    [OrderId]               VARCHAR (13) CONSTRAINT [DF_OrderComparisonList_OrderId] DEFAULT ('') NOT NULL,
    [OriginalQty]           INT          CONSTRAINT [DF_OrderComparisonList_OriginalQty] DEFAULT ((0)) NOT NULL,
    [NewQty]                INT          CONSTRAINT [DF_OrderComparisonList_NewQty] DEFAULT ((0)) NOT NULL,
    [OriginalBuyerDelivery] DATE         NULL,
    [NewBuyerDelivery]      DATE         NULL,
    [OriginalSCIDelivery]   DATE         NULL,
    [NewSCIDelivery]        DATE         NULL,
    [OriginalStyleID]       VARCHAR (15) CONSTRAINT [DF_OrderComparisonList_OriginalStyleID] DEFAULT ('') NOT NULL,
    [NewStyleID]            VARCHAR (15) CONSTRAINT [DF_OrderComparisonList_NewStyleID] DEFAULT ('') NOT NULL,
    [NewOrder]              BIT          CONSTRAINT [DF_OrderComparisonList_NewOrder] DEFAULT ((0)) NOT NULL,
    [DeleteOrder]           BIT          CONSTRAINT [DF_OrderComparisonList_DeleteOrder] DEFAULT ((0)) NOT NULL,
    [OriginalCMPQDate]      DATE         NULL,
    [NewCMPQDate]           DATE         NULL,
    [OriginalEachConsApv]   DATE         NULL,
    [NewEachConsApv]        DATE         NULL,
    [OriginalMnorderApv]    DATE         NULL,
    [NewMnorderApv]         DATE         NULL,
    [OriginalSMnorderApv]   DATE         NULL,
    [NewSMnorderApv]        DATE         NULL,
    [OriginalLETA]          DATE         NULL,
    [NewLETA]               DATE         NULL,
    [MDivisionID]           VARCHAR (8)  CONSTRAINT [DF_OrderComparisonList_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]             VARCHAR (8)  CONSTRAINT [DF_OrderComparisonList_FactoryID] DEFAULT ('') NOT NULL,
    [KPILETA]               DATE         NULL,
    [TransferToFactory]     VARCHAR (8)  CONSTRAINT [DF_OrderComparisonList_TransferToFactory] DEFAULT ('') NOT NULL,
    [MnorderApv2]           DATE         NULL,
    [JunkOrder]             BIT          CONSTRAINT [DF_OrderComparisonList_JunkOrder] DEFAULT ((0)) NOT NULL,
    [Ukey]                  BIGINT       IDENTITY (1, 1) NOT NULL,
    [BrandID]               VARCHAR (8)  DEFAULT ('') NOT NULL,
    [OriginalCustPONo]      VARCHAR (30) CONSTRAINT [DF_OrderComparisonList_OriginalCustPONo] DEFAULT ('') NOT NULL,
    [NewCustPONo]           VARCHAR (30) CONSTRAINT [DF_OrderComparisonList_NewCustPONo] DEFAULT ('') NOT NULL,
    [OriginalShipModeList]  VARCHAR (30) CONSTRAINT [DF_OrderComparisonList_OriginalShipModeList] DEFAULT ('') NOT NULL,
    [NewShipModeList]       VARCHAR (30) CONSTRAINT [DF_OrderComparisonList_NewShipModeList] DEFAULT ('') NOT NULL,
    [OriginalPFETA]         DATE         NULL,
    [NewPFETA]              DATE         NULL,
    OriginalFOC varchar(1) NOT NULL CONSTRAINT [DF_OrderComparisonList_OriginalFOC] DEFAULT '',
    NewFOC varchar(1) NOT NULL CONSTRAINT [DF_OrderComparisonList_NewFOC] DEFAULT '',
    OriginalOrderTypeID varchar(20) NOT NULL CONSTRAINT [DF_OrderComparisonList_OriginalOrderTypeID] DEFAULT '',
    NewOrderTypeID varchar(20) NOT NULL CONSTRAINT [DF_OrderComparisonList_NewOrderTypeID] DEFAULT '',
    CONSTRAINT [PK_OrderComparisonList] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Comparison List for updated orders (匯入訂單檢核表)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳送日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'TransferDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'匯入日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'UpdateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原客戶交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalBuyerDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新客戶交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewBuyerDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原飛雁交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalSCIDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新飛雁交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewSCIDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalStyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewStyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'DeleteOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原CMPQ Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalCMPQDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新CMPQ Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewCMPQDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原Each-cons Approved Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalEachConsApv';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新Each-cons Approved Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewEachConsApv';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原M/Notice APV', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalMnorderApv';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新M/Notice APV', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewMnorderApv';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原SM/Notice APV', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalSMnorderApv';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新SM/Notice APV', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewSMnorderApv';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原LETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'OriginalLETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新LETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'NewLETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KPI L/ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'KPILETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉單工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'TransferToFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'VAS/SHAS APPROVE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'MnorderApv2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單是否drop掉', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'JunkOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderComparisonList', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'update前的 PFETA',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderComparisonList',
    @level2type = N'COLUMN',
    @level2name = N'OriginalPFETA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'update後的 PFETA',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderComparisonList',
    @level2type = N'COLUMN',
    @level2name = N'NewPFETA'