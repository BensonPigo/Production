CREATE TABLE [dbo].[ShareRule]
(
	[AccountID] VARCHAR(8) NOT NULL, 
    [ExpenseReason] VARCHAR(25) NOT NULL, 
    [ShareBase] VARCHAR(1) NOT NULL, 
    [ShipModeID] VARCHAR(90) NOT NULL CONSTRAINT [DF_ShareRule_ShipModeID] DEFAULT (''),
    [AddName] VARCHAR(10) NOT NULL CONSTRAINT [DF_ShareRule_AddName] DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL CONSTRAINT [DF_ShareRule_EditName] DEFAULT (''),
    [EditDate] DATETIME NULL,
    [Junk] BIT NOT NULL CONSTRAINT [DF_ShareRule_Junk] DEFAULT (0),
    CONSTRAINT [PK_ShareRule] PRIMARY KEY CLUSTERED ([AccountID] ASC, [ExpenseReason] ASC, [ShareBase] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'會計科目',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareRule',
    @level2type = N'COLUMN',
    @level2name = N'AccountID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'費用產生原因',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareRule',
    @level2type = N'COLUMN',
    @level2name = N'ExpenseReason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'分攤基準',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareRule',
    @level2type = N'COLUMN',
    @level2name = N'ShareBase'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Shipping Mode',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareRule',
    @level2type = N'COLUMN',
    @level2name = N'ShipModeID'