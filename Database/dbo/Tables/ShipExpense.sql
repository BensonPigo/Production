CREATE TABLE [dbo].[ShipExpense] (
    [ID]          VARCHAR (21)    CONSTRAINT [DF_ShipExpense_ID] DEFAULT ('') NOT NULL,
    [BrandID]     VARCHAR (8)     CONSTRAINT [DF_ShipExpense_BrandID] DEFAULT ('') NULL,
    [Junk]        BIT             CONSTRAINT [DF_ShipExpense_Junk] DEFAULT ((0)) NULL,
    [Description] NVARCHAR (MAX)  CONSTRAINT [DF_ShipExpense_Description] DEFAULT ('') NOT NULL,
    [LocalSuppID] VARCHAR (8)     CONSTRAINT [DF_ShipExpense_LocalSuppID] DEFAULT ('') NULL,
    [Price]       NUMERIC (13, 5) CONSTRAINT [DF_ShipExpense_Price] DEFAULT ((0)) NULL,
	[UnitID] VARCHAR(8) NULL DEFAULT (''),
    [CanvassDate] DATE            NULL,
    [AccountID]   VARCHAR (8)     CONSTRAINT [DF_ShipExpense_AccountNo] DEFAULT ('') NULL,
    [CurrencyID]  VARCHAR (3)     CONSTRAINT [DF_ShipExpense_CurrencyID] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)    CONSTRAINT [DF_ShipExpense_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME        NULL,
    [EditName]    VARCHAR (10)    CONSTRAINT [DF_ShipExpense_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME        NULL, 
    CONSTRAINT [PK_ShipExpense] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Expense', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'LocalSuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'招商日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'CanvassDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipExpense', @level2type = N'COLUMN', @level2name = N'AccountID';

