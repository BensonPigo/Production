CREATE TABLE [dbo].[InvAdjust] (
    [ID]               VARCHAR (13)   CONSTRAINT [DF_InvAdjust_ID] DEFAULT ('') NOT NULL,
    [IssueDate]        DATE           NULL,
    [REASON]           TINYINT        CONSTRAINT [DF_InvAdjust_REASON] DEFAULT ((0)) NULL,
    [NegoinvID]        VARCHAR (25)   CONSTRAINT [DF_InvAdjust_NegoinvID] DEFAULT ('') NULL,
    [OrderID]          VARCHAR (13)   CONSTRAINT [DF_InvAdjust_OrderID] DEFAULT ('') NULL,
    [PullDate]         DATE           NULL,
    [Pullout3ukey]     BIGINT         CONSTRAINT [DF_InvAdjust_Pullout3ukey] DEFAULT ((0)) NULL,
    [BrandID]          VARCHAR (8)    CONSTRAINT [DF_InvAdjust_BrandID] DEFAULT ('') NULL,
    [FactoryID]        VARCHAR (8)    CONSTRAINT [DF_InvAdjust_FactoryID] DEFAULT ('') NULL,
    [ARVoucherNo]      VARCHAR (16)   CONSTRAINT [DF_InvAdjust_ARVoucherNo] DEFAULT ('') NULL,
    [VoucherNo]        VARCHAR (16)   CONSTRAINT [DF_InvAdjust_VoucherNo] DEFAULT ('') NULL,
    [Status]           VARCHAR (1)    CONSTRAINT [DF_InvAdjust_Status] DEFAULT ('') NULL,
    [OrigPulloutQty]   INT            CONSTRAINT [DF_InvAdjust_OrigPulloutQty] DEFAULT ((0)) NULL,
    [OrigPrice]        NUMERIC (7, 2) CONSTRAINT [DF_InvAdjust_OrigPrice] DEFAULT ((0)) NULL,
    [OrigPulloutAmt]   NUMERIC (9, 2) CONSTRAINT [DF_InvAdjust_OrigPulloutAmt] DEFAULT ((0)) NULL,
    [OrigSurcharge]    NUMERIC (9, 5) CONSTRAINT [DF_InvAdjust_OrigSurcharge] DEFAULT ((0)) NULL,
    [OrigAddCharge]    NUMERIC (9, 2) CONSTRAINT [DF_InvAdjust_OrigAddCharge] DEFAULT ((0)) NULL,
    [OrigCommission]   NUMERIC (9, 5) CONSTRAINT [DF_InvAdjust_OrigCommission] DEFAULT ((0)) NULL,
    [OrigDocFee]       NUMERIC (7, 2) CONSTRAINT [DF_InvAdjust_OrigDocFee] DEFAULT ((0)) NULL,
    [AdjustPulloutQty] INT            CONSTRAINT [DF_InvAdjust_AdjustPulloutQty] DEFAULT ((0)) NULL,
    [AdjustPulloutAmt] NUMERIC (9, 2) CONSTRAINT [DF_InvAdjust_AdjustPulloutAmt] DEFAULT ((0)) NULL,
    [AdjustSurcharge]  NUMERIC (9, 5) CONSTRAINT [DF_InvAdjust_AdjustSurcharge] DEFAULT ((0)) NULL,
    [AdjustAddCharge]  NUMERIC (9, 2) CONSTRAINT [DF_InvAdjust_AdjustAddCharge] DEFAULT ((0)) NULL,
    [AdjustCommission] NUMERIC (9, 5) CONSTRAINT [DF_InvAdjust_AdjustCommission] DEFAULT ((0)) NULL,
    [AdjustDocFee]     NUMERIC (7, 2) CONSTRAINT [DF_InvAdjust_AdjustDocFee] DEFAULT ((0)) NULL,
    [AddName]          VARCHAR (10)   CONSTRAINT [DF_InvAdjust_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME       NULL,
    [Eeit_Name]        VARCHAR (10)   CONSTRAINT [DF_InvAdjust_Eeit_Name] DEFAULT ('') NULL,
    [EditDate]         DATETIME       NULL,
    [PriceCheckID]     VARCHAR (13)   CONSTRAINT [DF_InvAdjust_PriceCheckID] DEFAULT ('') NULL,
    [OrderShipmodeSeq] VARCHAR (2)    CONSTRAINT [DF_InvAdjust_OrderShipmodeSeq] DEFAULT ('') NULL,
    [MDivisionID]      VARCHAR (8)    CONSTRAINT [DF_InvAdjust_MDivisionID] DEFAULT ('') NULL,
    CONSTRAINT [PK_InvAdjust] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票調整單-主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'REASON';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'NegoinvID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'PullDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'Pullout3ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'應收帳款的立帳傳票編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'ARVoucherNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'VoucherNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrigPulloutQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始出貨單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrigPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始出貨金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrigPulloutAmt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始額外費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrigSurcharge';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始附加費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrigAddCharge';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始佣金費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrigCommission';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始文件費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrigDocFee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'AdjustPulloutQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整出貨金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'AdjustPulloutAmt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後額外費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'AdjustSurcharge';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後附加費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'AdjustAddCharge';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後佣金', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'AdjustCommission';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後文件費', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'AdjustDocFee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'Eeit_Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格修改通知單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'PriceCheckID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty Breakdown Shipmode的Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'OrderShipmodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust', @level2type = N'COLUMN', @level2name = N'MDivisionID';

