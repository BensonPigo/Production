CREATE TABLE [dbo].[ShippingAP_Detail] (
    [ID]            VARCHAR (13)    CONSTRAINT [DF_ShippingAP_Detail_ID] DEFAULT ('') NOT NULL,
    [ShipExpenseID] VARCHAR (20)    CONSTRAINT [DF_ShippingAP_Detail_ShipExpenseID] DEFAULT ('') NOT NULL,
    [Qty]           NUMERIC (11, 4) CONSTRAINT [DF_ShippingAP_Detail_Qty] DEFAULT ((0)) NULL,
    [CurrencyID]    VARCHAR (3)     CONSTRAINT [DF_ShippingAP_Detail_CurrencyID] DEFAULT ('') NULL,
    [Price]         NUMERIC (14, 4) CONSTRAINT [DF_ShippingAP_Detail_Price] DEFAULT ((0)) NOT NULL,
    [Rate]          NUMERIC (11, 6) CONSTRAINT [DF_ShippingAP_Detail_Rate] DEFAULT ((0)) NULL,
    [Amount]        NUMERIC (16, 4) CONSTRAINT [DF_ShippingAP_Detail_Amount] DEFAULT ((0)) NULL,
    [Remark]        NVARCHAR (30)   CONSTRAINT [DF_ShippingAP_Detail_Remark] DEFAULT ('') NULL,
    [AccountID] VARCHAR(8) NULL DEFAULT (''), 
    CONSTRAINT [PK_ShippingAP_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [ShipExpenseID] ASC, [Price] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Accounts payable Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A/P 單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'費用代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail', @level2type = N'COLUMN', @level2name = N'ShipExpenseID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'匯率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail', @level2type = N'COLUMN', @level2name = N'Rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WK#/Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP_Detail', @level2type = N'COLUMN', @level2name = N'Remark';

