CREATE TABLE [dbo].[Debit] (
    [ID]             VARCHAR (13)    CONSTRAINT [DF_Debit_ID] DEFAULT ('') NOT NULL,
    [Issuedate]      DATE            NULL,
    [CurrencyID]     VARCHAR (3)     CONSTRAINT [DF_Debit_CurrencyID] DEFAULT ('') NOT NULL,
    [Amount]         DECIMAL (13, 2) CONSTRAINT [DF_Debit_Amount] DEFAULT ((0)) NOT NULL,
    [Received]       DECIMAL (13, 2) CONSTRAINT [DF_Debit_Received] DEFAULT ((0)) NOT NULL,
    [BuyerID]        VARCHAR (8)     CONSTRAINT [DF_Debit_BuyerID] DEFAULT ('') NOT NULL,
    [BrandID]        VARCHAR (8)     CONSTRAINT [DF_Debit_BrandID] DEFAULT ('') NOT NULL,
    [BankID]         VARCHAR (6)     CONSTRAINT [DF_Debit_BankID] DEFAULT ('') NOT NULL,
    [MDivisionID]    VARCHAR (8)     CONSTRAINT [DF_Debit_MDivisionID] DEFAULT ('') NOT NULL,
    [LCFNO]          VARCHAR (20)    CONSTRAINT [DF_Debit_LCFNO] DEFAULT ('') NOT NULL,
    [LCFDate]        DATE            NULL,
    [EstPayDate]     DATE            NULL,
    [Title]          NVARCHAR (60)   CONSTRAINT [DF_Debit_Title] DEFAULT ('') NOT NULL,
    [SendFrom]       NVARCHAR (80)   CONSTRAINT [DF_Debit_SendFrom] DEFAULT ('') NOT NULL,
    [Attn]           NVARCHAR (80)   CONSTRAINT [DF_Debit_Attn] DEFAULT ('') NOT NULL,
    [CC]             NVARCHAR (80)   CONSTRAINT [DF_Debit_CC] DEFAULT ('') NOT NULL,
    [Subject]        NVARCHAR (100)  CONSTRAINT [DF_Debit_Subject] DEFAULT ('') NOT NULL,
    [Handle]         VARCHAR (10)    CONSTRAINT [DF_Debit_Handle] DEFAULT ('') NOT NULL,
    [SMR]            VARCHAR (10)    CONSTRAINT [DF_Debit_SMR] DEFAULT ('') NOT NULL,
    [VoucherID]      VARCHAR (16)    CONSTRAINT [DF_Debit_VoucherID] DEFAULT ('') NOT NULL,
    [BadID]          VARCHAR (11)    CONSTRAINT [DF_Debit_BadID] DEFAULT ('') NOT NULL,
    [Status]         VARCHAR (15)    CONSTRAINT [DF_Debit_Status] DEFAULT ('') NOT NULL,
    [StatusRevise]   DATE            NULL,
    [StatusReviseNm] VARCHAR (10)    CONSTRAINT [DF_Debit_StatusReviseNm] DEFAULT ('') NOT NULL,
    [CustPayId]      VARCHAR (13)    CONSTRAINT [DF_Debit_CustPayId] DEFAULT ('') NOT NULL,
    [Settled]        VARCHAR (1)     CONSTRAINT [DF_Debit_Settled] DEFAULT ('') NOT NULL,
    [SettleDate]     DATE            NULL,
    [Cfm]            VARCHAR (10)    CONSTRAINT [DF_Debit_Cfm] DEFAULT ('') NOT NULL,
    [CfmDate]        DATE            NULL,
    [Lock]           BIT             CONSTRAINT [DF_Debit_Lock] DEFAULT ((0)) NOT NULL,
    [Lockdate]       DATE            NULL,
    [OldAmount]      DECIMAL (13, 2) CONSTRAINT [DF_Debit_OldAmount] DEFAULT ((0)) NOT NULL,
    [Type]           VARCHAR (1)     CONSTRAINT [DF_Debit_Type] DEFAULT ('') NOT NULL,
    [ShareFob]       BIT             CONSTRAINT [DF_Debit_ShareFob] DEFAULT ((0)) NOT NULL,
    [VoucherFactory] VARCHAR (16)    CONSTRAINT [DF_Debit_VoucherFactory] DEFAULT ('') NOT NULL,
    [VoucherSettle]  VARCHAR (16)    CONSTRAINT [DF_Debit_VoucherSettle] DEFAULT ('') NOT NULL,
    [IsSubcon]       BIT             CONSTRAINT [DF_Debit_IsSubcon] DEFAULT ((0)) NOT NULL,
    [LCLName]        NVARCHAR (100)  CONSTRAINT [DF_Debit_LCLName] DEFAULT ('') NOT NULL,
    [LCLCurrency]    VARCHAR (3)     CONSTRAINT [DF_Debit_LCLCurrency] DEFAULT ('') NOT NULL,
    [LCLAmount]      DECIMAL (15, 4) CONSTRAINT [DF_Debit_LCLAmount] DEFAULT ((0)) NOT NULL,
    [LCLRate]        DECIMAL (9, 4)  CONSTRAINT [DF_Debit_LCLRate] DEFAULT ((0)) NOT NULL,
    [HandleName]     NVARCHAR (20)   CONSTRAINT [DF_Debit_HandleName] DEFAULT ('') NOT NULL,
    [SMRName]        NVARCHAR (20)   CONSTRAINT [DF_Debit_SMRName] DEFAULT ('') NOT NULL,
    [CFMName]        NVARCHAR (20)   CONSTRAINT [DF_Debit_CFMName] DEFAULT ('') NOT NULL,
    [AddName]        VARCHAR (10)    CONSTRAINT [DF_Debit_AddName] DEFAULT ('') NOT NULL,
    [AddDate]        DATETIME        NULL,
    [EditName]       VARCHAR (10)    CONSTRAINT [DF_Debit_EditName] DEFAULT ('') NOT NULL,
    [EditDate]       DATETIME        NULL,
    [SysDate]        DATETIME        NULL,
    [ResponFTY]      VARCHAR (8)     CONSTRAINT [DF__Debit__ResponFTY__09A67610] DEFAULT ('') NOT NULL,
    [SubName]        NVARCHAR (20)   CONSTRAINT [DF_Debit_SubName] DEFAULT ('') NOT NULL,
    [SubconID]       VARCHAR (12)    CONSTRAINT [DF_Debit_SubconID] DEFAULT ('') NOT NULL,
    [OrderCompanyID] NUMERIC(2)      CONSTRAINT [DF_Debit_OrderCompanyID] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Debit] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'處理人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'SMR';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票編號(呆帳)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'BadID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態最後改日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'StatusRevise';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'StatusReviseNm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶付款單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'CustPayId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Settled';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'SettleDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Cfm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'CfmDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'21天鎖定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Lock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'21天鎖定日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Lockdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'21天鎖定的金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'OldAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Debit Memo 類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否分攤FOB', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'ShareFob';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否扣代工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'IsSubcon';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代工廠名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'LCLName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代工廠幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'LCLCurrency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代工廠金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'LCLAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代工廠匯率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'LCLRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'承辦人員名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'HandleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Smr_name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'SMRName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cfm_name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'CFMName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'SysDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Debit Memo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Issuedate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收款金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Received';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客人別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'BuyerID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銀行代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'BankID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'回簽單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'LCFNO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'回簽日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'LCFDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計付款日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'EstPayDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'抬頭', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Title';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄件來源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'SendFrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'附件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Attn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'副本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'CC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主旨', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'Subject';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北沖帳傳票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'VoucherSettle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'VoucherID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠傳票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'VoucherFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發廠商名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'SubName';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單公司別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit', @level2type = N'COLUMN', @level2name = N'OrderCompanyID';

GO