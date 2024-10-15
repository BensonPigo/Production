CREATE TABLE [dbo].[LocalDebit] (
    [ID]               VARCHAR (13)    CONSTRAINT [DF_LocalDebit_ID] DEFAULT ('') NOT NULL,
    [Handle]           VARCHAR (10)    CONSTRAINT [DF_LocalDebit_Handle] DEFAULT ('') NOT NULL,
    [SMR]              VARCHAR (10)    CONSTRAINT [DF_LocalDebit_SMR] DEFAULT ('') NOT NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_LocalDebit_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]        VARCHAR (8)     CONSTRAINT [DF_LocalDebit_FactoryID] DEFAULT ('') NOT NULL,
    [LocalSuppID]      VARCHAR (8)     CONSTRAINT [DF_LocalDebit_LocalSuppID] DEFAULT ('') NOT NULL,
    [Description]      NVARCHAR (MAX)  CONSTRAINT [DF_LocalDebit_Description] DEFAULT ('') NOT NULL,
    [Exchange]         DECIMAL (8, 3)  CONSTRAINT [DF_LocalDebit_Exchange] DEFAULT ((0)) NOT NULL,
    [Currencyid]       VARCHAR (3)     CONSTRAINT [DF_LocalDebit_Currencyid] DEFAULT ('') NOT NULL,
    [Amount]           DECIMAL (13, 2) CONSTRAINT [DF_LocalDebit_Amount] DEFAULT ((0)) NOT NULL,
    [Tax]              DECIMAL (11, 2) CONSTRAINT [DF_LocalDebit_Tax] DEFAULT ((0)) NOT NULL,
    [TaxRate]          DECIMAL (5, 2)  CONSTRAINT [DF_LocalDebit_TaxRate] DEFAULT ((0)) NOT NULL,
    [AmtReviseDate]    DATETIME        NULL,
    [AmtReviseName]    VARCHAR (10)    CONSTRAINT [DF_LocalDebit_AmtReviseName] DEFAULT ('') NOT NULL,
    [ReceiveDate]      DATE            NULL,
    [ReceiveName]      VARCHAR (10)    CONSTRAINT [DF_LocalDebit_ReceiveName] DEFAULT ('') NOT NULL,
    [CfmDate]          DATE            NULL,
    [CfmName]          VARCHAR (10)    CONSTRAINT [DF_LocalDebit_CfmName] DEFAULT ('') NOT NULL,
    [PrintDate]        DATETIME        NULL,
    [Status]           VARCHAR (15)    CONSTRAINT [DF_LocalDebit_Status] DEFAULT ('') NOT NULL,
    [StatusEditDate]   DATETIME        NULL,
    [TaipeiDBC]        BIT             CONSTRAINT [DF_LocalDebit_TaipeiDBC] DEFAULT ((0)) NOT NULL,
    [TaipeiAMT]        DECIMAL (12, 2) CONSTRAINT [DF_LocalDebit_TaipeiAMT] DEFAULT ((0)) NOT NULL,
    [TaipeiCurrencyID] VARCHAR (3)     CONSTRAINT [DF_LocalDebit_TaipeiCurrencyID] DEFAULT ('') NOT NULL,
    [AccountID]        VARCHAR (8)     CONSTRAINT [DF_LocalDebit_AccountID] DEFAULT ('') NOT NULL,
    [Issuedate]        DATE            NULL,
    [AddName]          VARCHAR (10)    CONSTRAINT [DF_LocalDebit_AddName] DEFAULT ('') NOT NULL,
    [AddDate]          DATETIME        NULL,
    [EditName]         VARCHAR (10)    CONSTRAINT [DF_LocalDebit_EditName] DEFAULT ('') NOT NULL,
    [EditDate]         DATETIME        NULL,
    [ResponFTY]        VARCHAR (8)     DEFAULT ('') NOT NULL,
    [OrderCompanyID]   NUMERIC(2, 0)      CONSTRAINT [DF_LocalDebit_OrderCompanyID] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LocalDebit] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LocalDebit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'SMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'LocalSuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'美金匯率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'Exchange';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'Currencyid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款總額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'Tax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'TaxRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額建立/修改基準日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'AmtReviseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額建立/修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'AmtReviseName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計收單日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'ReceiveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計收單人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'ReceiveName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計費用確認日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'CfmDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計確認人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'CfmName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'列印日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'PrintDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態修改日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'StatusEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為台北單據', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'TaipeiDBC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北扣款金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'TaipeiAMT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北扣款幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'TaipeiCurrencyID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'Issuedate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'AccountID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單公司別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit', @level2type = N'COLUMN', @level2name = N'OrderCompanyID';

GO