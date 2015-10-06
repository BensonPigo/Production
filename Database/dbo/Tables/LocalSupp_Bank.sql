CREATE TABLE [dbo].[LocalSupp_Bank] (
    [ID]           VARCHAR (8)    CONSTRAINT [DF_LocalSupp_Bank_ID] DEFAULT ('') NOT NULL,
    [AccountNo]    VARCHAR (30)   CONSTRAINT [DF_LocalSupp_Bank_AccountNo] DEFAULT ('') NOT NULL,
    [AccountName]  NVARCHAR (60)  CONSTRAINT [DF_LocalSupp_Bank_AccountName] DEFAULT ('') NOT NULL,
    [BankName]     NVARCHAR (70)  CONSTRAINT [DF_LocalSupp_Bank_BankName] DEFAULT ('') NOT NULL,
    [CountryID]    VARCHAR (2)    CONSTRAINT [DF_LocalSupp_Bank_CountryID] DEFAULT ('') NOT NULL,
    [City]         NVARCHAR (20)  CONSTRAINT [DF_LocalSupp_Bank_City] DEFAULT ('') NULL,
    [SWIFTCode]    VARCHAR (11)   CONSTRAINT [DF_LocalSupp_Bank_SWIFTCode] DEFAULT ('') NULL,
    [MidSWIFTCode] VARCHAR (11)   CONSTRAINT [DF_LocalSupp_Bank_MidSWIFTCode] DEFAULT ('') NULL,
    [MidBankName]  NVARCHAR (70)  CONSTRAINT [DF_LocalSupp_Bank_MidBankName] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (MAX) CONSTRAINT [DF_LocalSupp_Bank_Remark] DEFAULT ('') NULL,
    [IsDefault]    BIT            CONSTRAINT [DF_LocalSupp_Bank_IsDefault] DEFAULT ((0)) NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_LocalSupp_Bank_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_LocalSupp_Bank_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL,
    CONSTRAINT [PK_LocalSupp_Bank] PRIMARY KEY CLUSTERED ([ID] ASC, [AccountNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Supplier_bank', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'匯款帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'AccountNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳戶名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'AccountName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銀行名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'BankName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'城市', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'City';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銀行SWIFT Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'SWIFTCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中間銀行SWCODE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'MidSWIFTCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中間銀行名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'MidBankName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Default', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'IsDefault';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank', @level2type = N'COLUMN', @level2name = N'EditDate';

