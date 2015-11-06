﻿CREATE TABLE [dbo].[ShareExpense] (
    [ShippingAPID] VARCHAR (13)    CONSTRAINT [DF_ShareExpense_ShippingAPID] DEFAULT ('') NOT NULL,
    [BLNo]         VARCHAR (20)    CONSTRAINT [DF_ShareExpense_BLNo] DEFAULT ('') NOT NULL,
    [WKNo]         VARCHAR (13)    CONSTRAINT [DF_ShareExpense_WKNo] DEFAULT ('') NOT NULL,
    [InvNo]        VARCHAR (25)    CONSTRAINT [DF_ShareExpense_InvNo] DEFAULT ('') NOT NULL,
    [Type]         VARCHAR (25)    CONSTRAINT [DF_ShareExpense_Type] DEFAULT ('') NULL,
    [GW]           NUMERIC (9, 2)  CONSTRAINT [DF_ShareExpense_GW] DEFAULT ((0)) NULL,
    [CBM]          NUMERIC (9, 2)  CONSTRAINT [DF_ShareExpense_CBM] DEFAULT ((0)) NULL,
    [CurrencyID]   VARCHAR (3)     CONSTRAINT [DF_ShareExpense_CurrencyID] DEFAULT ('') NULL,
    [Amount]       NUMERIC (12, 2) CONSTRAINT [DF_ShareExpense_Amount] DEFAULT ((0)) NULL,
    [ShipModeID]   VARCHAR (10)    CONSTRAINT [DF_ShareExpense_ShipModeID] DEFAULT ('') NULL,
    [ShareBase]    VARCHAR (1)     CONSTRAINT [DF_ShareExpense_ShareBase] DEFAULT ('') NULL,
    [FtyWK]        BIT             CONSTRAINT [DF_ShareExpense_FtyWK] DEFAULT ((0)) NULL,
    [AccountNo]    VARCHAR (8)     CONSTRAINT [DF_ShareExpense_AccountNo] DEFAULT ('') NOT NULL,
    [AccountName]  VARCHAR (40)    CONSTRAINT [DF_ShareExpense_AccountName] DEFAULT ('') NULL,
    [EditName]     VARCHAR (10)    CONSTRAINT [DF_ShareExpense_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME        NULL,
    CONSTRAINT [PK_ShareExpense] PRIMARY KEY CLUSTERED ([ShippingAPID] ASC, [BLNo] ASC, [WKNo] ASC, [InvNo] ASC, [AccountNo] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Share Expense', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Account Payment Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'ShippingAPID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'BLNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Working No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'WKNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice No. (Garment Booking ID)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'G.W.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'GW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CBM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'CBM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分攤金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ship Mode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分攤基準', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'ShareBase';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠自行建立的WK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'FtyWK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'AccountNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計科目名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'AccountName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareExpense', @level2type = N'COLUMN', @level2name = N'EditDate';

