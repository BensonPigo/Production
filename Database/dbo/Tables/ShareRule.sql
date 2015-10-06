CREATE TABLE [dbo].[ShareRule] (
    [AccountNo]     VARCHAR (8)  CONSTRAINT [DF_ShareRule_AccountNo] DEFAULT ('') NOT NULL,
    [ExpenseReason] VARCHAR (25) CONSTRAINT [DF_ShareRule_ExpenseReason] DEFAULT ('') NOT NULL,
    [ShareBase]     VARCHAR (1)  CONSTRAINT [DF_ShareRule_ShareBase] DEFAULT ('') NOT NULL,
    [ShipModeID]    VARCHAR (90) CONSTRAINT [DF_ShareRule_ShipModeID] DEFAULT ('') NULL,
    CONSTRAINT [PK_ShareRule] PRIMARY KEY CLUSTERED ([AccountNo] ASC, [ExpenseReason] ASC, [ShareBase] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進出口費用分攤規則', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareRule';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareRule', @level2type = N'COLUMN', @level2name = N'AccountNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'費用產生原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareRule', @level2type = N'COLUMN', @level2name = N'ExpenseReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分攤基準', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareRule', @level2type = N'COLUMN', @level2name = N'ShareBase';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShareRule', @level2type = N'COLUMN', @level2name = N'ShipModeID';

