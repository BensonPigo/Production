CREATE TABLE [dbo].[AirPP] (
    [ID]                VARCHAR (13)    CONSTRAINT [DF_AirPP_ID] DEFAULT ('') NOT NULL,
    [CDate]             DATE            NOT NULL,
    [OrderID]           VARCHAR (13)    CONSTRAINT [DF_AirPP_OrderID] DEFAULT ('') NOT NULL,
    [OrderShipmodeSeq]  VARCHAR (2)     CONSTRAINT [DF_AirPP_OrderShipmodeSeq] DEFAULT ('') NULL,
    [MDivisionID]       VARCHAR (8)     CONSTRAINT [DF_AirPP_MDivisionID] DEFAULT ('') NULL,
    [ShipQty]           INT             CONSTRAINT [DF_AirPP_ShipQty] DEFAULT ((0)) NULL,
    [ETA]               DATE            NULL,
    [ReceiveDoxDate]    DATE            NULL,
    [GW]                NUMERIC (7, 2)  CONSTRAINT [DF_AirPP_GW] DEFAULT ((0)) NULL,
    [VW]                NUMERIC (8, 2)  CONSTRAINT [DF_AirPP_VW] DEFAULT ((0)) NULL,
    [CW]                NUMERIC (8, 2)  NULL,
    [Forwarder]         VARCHAR (6)     CONSTRAINT [DF_AirPP_Forwarder] DEFAULT ('') NULL,
    [Quotation]         NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_Quotation] DEFAULT ((0)) NULL,
    [Forwarder1]        VARCHAR (6)     CONSTRAINT [DF_AirPP_Forwarder1] DEFAULT ('') NULL,
    [Quotation1]        NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_Quotation1] DEFAULT ((0)) NULL,
    [Forwarder2]        VARCHAR (6)     CONSTRAINT [DF_AirPP_Forwarder2] DEFAULT ('') NULL,
    [Quotation2]        NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_Quotation2] DEFAULT ((0)) NULL,
    [EstAmount]         NUMERIC (13, 4) CONSTRAINT [DF_AirPP_EstAmount] DEFAULT ((0)) NULL,
    [ActualAmount]      NUMERIC (13, 2) CONSTRAINT [DF_AirPP_ActualAmount] DEFAULT ((0)) NULL,
    [Rate]              NUMERIC (8, 3)  CONSTRAINT [DF_AirPP_Rate] DEFAULT ((0)) NULL,
    [SRNo]              VARCHAR (30)    CONSTRAINT [DF_AirPP_SRNo] DEFAULT ('') NULL,
    [Voucher]           VARCHAR (45)    CONSTRAINT [DF_AirPP_Voucher] DEFAULT ('') NULL,
    [PayDate]           DATE            NULL,
    [ReasonID]          VARCHAR (5)     CONSTRAINT [DF_AirPP_ReasonID] DEFAULT ('') NULL,
    [FtyDesc]           NVARCHAR (50)   CONSTRAINT [DF_AirPP_FtyDesc] DEFAULT ('') NULL,
    [Remark]            NVARCHAR (MAX)  CONSTRAINT [DF_AirPP_Remark] DEFAULT ('') NULL,
    [MRComment]         NVARCHAR (MAX)  CONSTRAINT [DF_AirPP_MRComment] DEFAULT ('') NULL,
    [ResponsibleFty]    BIT             CONSTRAINT [DF_AirPP_ResponsibleFty] DEFAULT ((0)) NULL,
    [RatioFty]          NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_RatioFty] DEFAULT ((0)) NULL,
    [ResponsibleFtyNo]  VARCHAR (8)     CONSTRAINT [DF_AirPP_ResponsibleFtyNo] DEFAULT ('') NULL,
    [ResponsibleSubcon] BIT             CONSTRAINT [DF_AirPP_ResponsibleSubcon] DEFAULT ((0)) NULL,
    [RatioSubcon]       NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_RatioSubcon] DEFAULT ((0)) NULL,
    [SubconDBCNo]       VARCHAR (13)    CONSTRAINT [DF_AirPP_SubconDBCNo] DEFAULT ('') NULL,
    [SubconDBCRemark]   NVARCHAR (MAX)  CONSTRAINT [DF_AirPP_SubconDBCRemark] DEFAULT ('') NULL,
    [SubConName]        NVARCHAR (20)   CONSTRAINT [DF_AirPP_SubConName] DEFAULT ('') NULL,
    [ResponsibleSCI]    BIT             CONSTRAINT [DF_AirPP_ResponsibleSCI] DEFAULT ((0)) NULL,
    [RatioSCI]          NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_RatioSCI] DEFAULT ((0)) NULL,
    [SCIICRNo]          VARCHAR (13)    CONSTRAINT [DF_AirPP_SCIICRNo] DEFAULT ('') NULL,
    [SCIICRRemark]      NVARCHAR (MAX)  CONSTRAINT [DF_AirPP_SCIICRRemark] DEFAULT ('') NULL,
    [ResponsibleSupp]   BIT             CONSTRAINT [DF_AirPP_ResponsibleSupp] DEFAULT ((0)) NULL,
    [RatioSupp]         NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_RatioSupp] DEFAULT ((0)) NULL,
    [SuppDBCNo]         VARCHAR (13)    CONSTRAINT [DF_AirPP_SuppDBCNo] DEFAULT ('') NULL,
    [SuppDBCRemark]     NVARCHAR (MAX)  CONSTRAINT [DF_AirPP_SuppDBCRemark] DEFAULT ('') NULL,
    [ResponsibleBuyer]  BIT             CONSTRAINT [DF_AirPP_ResponsibleBuyer] DEFAULT ((0)) NULL,
    [RatioBuyer]        NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_RatioBuyer] DEFAULT ((0)) NULL,
    [BuyerDBCNo]        VARCHAR (13)    CONSTRAINT [DF_AirPP_BuyerDBCNo] DEFAULT ('') NULL,
    [BuyerDBCRemark]    NVARCHAR (MAX)  CONSTRAINT [DF_AirPP_BuyerDBCRemark] DEFAULT ('') NULL,
    [BuyerICRNo]        VARCHAR (13)    CONSTRAINT [DF_AirPP_BuyerICRNo] DEFAULT ('') NULL,
    [BuyerICRRemark]    NVARCHAR (MAX)  CONSTRAINT [DF_AirPP_BuyerICRRemark] DEFAULT ('') NULL,
    [BuyerRemark]       NVARCHAR (MAX)  CONSTRAINT [DF_AirPP_BuyerRemark] DEFAULT ('') NULL,
    [PPICMgr]           VARCHAR (10)    CONSTRAINT [DF_AirPP_PPICMgr] DEFAULT ('') NULL,
    [PPICMgrApvDate]    DATETIME        NULL,
    [FtyMgr]            VARCHAR (10)    CONSTRAINT [DF_AirPP_FtyMgr] DEFAULT ('') NULL,
    [FtyMgrApvDate]     DATETIME        NULL,
    [POHandle]          VARCHAR (10)    CONSTRAINT [DF_AirPP_POHandle] DEFAULT ('') NULL,
    [POSMR]             VARCHAR (10)    CONSTRAINT [DF_AirPP_POSMR] DEFAULT ('') NULL,
    [MRHandle]          VARCHAR (10)    CONSTRAINT [DF_AirPP_MRHandle] DEFAULT ('') NULL,
    [SMR]               VARCHAR (10)    CONSTRAINT [DF_AirPP_SMR] DEFAULT ('') NULL,
    [SMRApvDate]        DATETIME        NULL,
    [Task]              VARCHAR (10)    CONSTRAINT [DF_AirPP_Task] DEFAULT ('') NULL,
    [TaskApvDate]       DATETIME        NULL,
    [Status]            VARCHAR (15)    CONSTRAINT [DF_AirPP_Status] DEFAULT ('') NULL,
    [FtySendDate]       DATETIME        NULL,
    [AddName]           VARCHAR (10)    CONSTRAINT [DF_AirPP_AddName] DEFAULT ('') NULL,
    [AddDate]           DATETIME        NULL,
    [EditName]          VARCHAR (10)    CONSTRAINT [DF_AirPP_EditName] DEFAULT ('') NULL,
    [EditDate]          DATETIME        NULL,
    [TPEEditName]       VARCHAR (10)    CONSTRAINT [DF_AirPP_TPEEditName] DEFAULT ('') NULL,
    [TPEEditDate]       DATETIME        NULL,
    [ActETD]            DATE            NULL,
    [ShipLeader]        VARCHAR (10)    NULL,
    [QuotationAVG]      NUMERIC (5, 2)  CONSTRAINT [DF_AirPP_QuotationAVG] DEFAULT ((0)) NOT NULL,
    [APReceiveDoxDate] DATE NULL, 
    [APAmountEditDate] DATE NULL, 
	[ActualAmountWVAT] numeric(13, 2) NOT NULL DEFAULT 0,
    CONSTRAINT [PK_AirPP] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Air Pre-Paid', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'CDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Seq of Qty Breakdown Shipmode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'OrderShipmodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收到文件日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ReceiveDoxDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'GW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'VW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'空運公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Forwarder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價 (USD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Quotation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'空運公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Forwarder1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價 (USD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Quotation1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'空運公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Forwarder2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價 (USD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Quotation2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預估金額 (USD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'EstAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際金額  (USD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ActualAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'匯率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代工廠名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SRNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Voucher';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pay Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'PayDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歸屬原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'FtyDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MRs Comments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'MRComment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬-工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ResponsibleFty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ratio Factory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'RatioFty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ResponsibleFtyNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬-Subcon', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ResponsibleSubcon';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ratio Subcon', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'RatioSubcon';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DBC No by Subcon', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SubconDBCNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DBC by Subcon Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SubconDBCRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SubCon Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SubConName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬-SCI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ResponsibleSCI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ratio SCI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'RatioSCI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ICR No by SCI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SCIICRNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ICR by SCI Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SCIICRRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬-Supplier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ResponsibleSupp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ratio Supplier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'RatioSupp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DBC No by Supplier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SuppDBCNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DBC by Supplier Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SuppDBCRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬-Buyer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'ResponsibleBuyer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ratio Buyer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'RatioBuyer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DBC No by Buyer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'BuyerDBCNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DBC by Buyer Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'BuyerDBCRemark';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款金額確認日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'APAmountEditDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ICR No by Buyer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'BuyerICRNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ICR by Buyer Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'BuyerICRRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Buyer的說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'BuyerRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PPIC Manager', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'PPICMgr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PPIC Manager Approve Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'PPICMgrApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory Manager', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'FtyMgr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory Manager Approve Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'FtyMgrApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PO Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'POHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬為SCI時的SMR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'POSMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'MRHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMR Approved Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'SMRApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Task Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Task';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Task Handle Approve Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'TaskApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠最後傳送日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'FtySendDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'TPEEditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'TPEEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價平均價格(USD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'QuotationAVG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP', @level2type = N'COLUMN', @level2name = N'CW';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'財務收件日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AirPP',
    @level2type = N'COLUMN',
    @level2name = N'APReceiveDoxDate'