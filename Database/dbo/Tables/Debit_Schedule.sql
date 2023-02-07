CREATE TABLE [dbo].[Debit_Schedule] (
    [ID]        VARCHAR (13)    CONSTRAINT [DF_Debit_Schedule_ID] DEFAULT ('') NOT NULL,
    [IssueDate] DATE            NOT NULL,
    [Amount]    NUMERIC (12, 2) CONSTRAINT [DF_Debit_Schedule_Amount] DEFAULT ((0)) NULL,
    [VoucherID] VARCHAR (16)    CONSTRAINT [DF_Debit_Schedule_TransID] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)    CONSTRAINT [DF_Debit_Schedule_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME        NULL,
    [EditName]  VARCHAR (10)    CONSTRAINT [DF_Debit_Schedule_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME        NULL,
    [SysDate]   DATETIME        NULL, 
    [CurrencyID] VARCHAR(3) NOT NULL DEFAULT (''), 
    [ExVoucherID] VARCHAR(16)   CONSTRAINT [DF_Debit_Schedule_ExVoucherID] DEFAULT ('') NULL, 
    CONSTRAINT [PK_Debit_Schedule] PRIMARY KEY ([ID], [IssueDate])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Debit Schedule', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dbc序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'Amount';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'SysDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠沖帳傳票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Schedule', @level2type = N'COLUMN', @level2name = N'VoucherID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'外帳傳票ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Debit_Schedule',
    @level2type = N'COLUMN',
    @level2name = N'ExVoucherID'