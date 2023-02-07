CREATE TABLE [dbo].[LocalAP] (
    [Id]          VARCHAR (13)    CONSTRAINT [DF_LocalAP_Id] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)     CONSTRAINT [DF_LocalAP_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryId]   VARCHAR (8)     CONSTRAINT [DF_LocalAP_FactoryId] DEFAULT ('') NOT NULL,
    [IssueDate]   DATE            NOT NULL,
    [LocalSuppID] VARCHAR (8)     CONSTRAINT [DF_LocalAP_LocalSuppID] DEFAULT ('') NOT NULL,
    [CurrencyID]  VARCHAR (3)     CONSTRAINT [DF_LocalAP_CurrcneyID] DEFAULT ('') NOT NULL,
    [Amount]      NUMERIC (12, 2) CONSTRAINT [DF_LocalAP_Amount] DEFAULT ((0)) NULL,
    [VatRate]     NUMERIC (3, 1)  CONSTRAINT [DF_LocalAP_VatRate] DEFAULT ((0)) NULL,
    [Vat]         NUMERIC (11, 2) CONSTRAINT [DF_LocalAP_Vat] DEFAULT ((0)) NULL,
    [PaytermID]   VARCHAR (6)     CONSTRAINT [DF_LocalAP_PaytermID] DEFAULT ('') NULL,
    [Category]    VARCHAR (20)    CONSTRAINT [DF_LocalAP_Category] DEFAULT ('') NOT NULL,
    [InvNo]       VARCHAR (100)   CONSTRAINT [DF_LocalAP_InvNo] DEFAULT ('') NULL,
    [Remark]      NVARCHAR (120)  CONSTRAINT [DF_LocalAP_Remark] DEFAULT ('') NULL,
    [Handle]      VARCHAR (10)    CONSTRAINT [DF_LocalAP_Handle] DEFAULT ('') NULL,
    [ApvName]     VARCHAR (10)    CONSTRAINT [DF_LocalAP_ApvName] DEFAULT ('') NULL,
    [ApvDate]     DATE            NULL,
    [AddName]     VARCHAR (10)    CONSTRAINT [DF_LocalAP_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME        NULL,
    [EditName]    VARCHAR (10)    CONSTRAINT [DF_LocalAP_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME        NULL,
    [VoucherID]   VARCHAR (16)    CONSTRAINT [DF_LocalAP_VarcherNo] DEFAULT ('') NULL,
    [Status]      VARCHAR (15)    CONSTRAINT [DF_LocalAP_Status] DEFAULT ('') NULL,
    [ExVoucherID] VARCHAR(16)     CONSTRAINT [DF_LocalAP_ExVoucherID] DEFAULT (''), 
    CONSTRAINT [PK_LocalAP] PRIMARY KEY CLUSTERED ([Id] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local AP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'LocalSuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'VatRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'Vat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款條件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'PaytermID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Category', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核可人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核可日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'EditDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP', @level2type = N'COLUMN', @level2name = N'VoucherID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'外帳傳票ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalAP',
    @level2type = N'COLUMN',
    @level2name = N'ExVoucherID'