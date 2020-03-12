CREATE TABLE [dbo].[ShippingAP] (
    [ID]               VARCHAR (13)    CONSTRAINT [DF_ShippingAP_ID] DEFAULT ('') NOT NULL,
    [CDate]            DATE            NOT NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_ShippingAP_FactoryID] DEFAULT ('') NOT NULL,
    [Type]             VARCHAR (6)     CONSTRAINT [DF_ShippingAP_Type] DEFAULT ('') NULL,
    [SubType]          VARCHAR (25)    CONSTRAINT [DF_ShippingAP_SubType] DEFAULT ('') NULL,
    [LocalSuppID]      VARCHAR (8)     CONSTRAINT [DF_ShippingAP_LocalSuppID] DEFAULT ('') NOT NULL,
    [PayTermID]        VARCHAR (6)     CONSTRAINT [DF_ShippingAP_PayTermID] DEFAULT ('') NULL,
    [Remark]           NVARCHAR (60)   CONSTRAINT [DF_ShippingAP_Remark] DEFAULT ('') NULL,
    [InvNo]            VARCHAR (20)    CONSTRAINT [DF_ShippingAP_InvNo] DEFAULT ('') NULL,
    [CurrencyID]       VARCHAR (3)     CONSTRAINT [DF_ShippingAP_CurrencyID] DEFAULT ('') NULL,
    [Amount]           NUMERIC (12, 2) CONSTRAINT [DF_ShippingAP_Amount] DEFAULT ((0)) NULL,
    [VATRate]          NUMERIC (3, 1)  CONSTRAINT [DF_ShippingAP_VATRate] DEFAULT ((0)) NULL,
    [VAT]              NUMERIC (11, 2) CONSTRAINT [DF_ShippingAP_VAT] DEFAULT ((0)) NULL,
    [BLNo]             VARCHAR (20)    CONSTRAINT [DF_ShippingAP_BLNo] DEFAULT ('') NULL,
    [Handle]           VARCHAR (10)    CONSTRAINT [DF_ShippingAP_Handle] DEFAULT ('') NULL,
    [Accountant]       VARCHAR (10)    CONSTRAINT [DF_ShippingAP_Accountant] DEFAULT ('') NULL,
    [Status]           VARCHAR (15)    CONSTRAINT [DF_ShippingAP_Status] DEFAULT ('') NULL,
    [ApvDate]          DATETIME        NULL,
    [VoucherID]        VARCHAR (16)    CONSTRAINT [DF_ShippingAP_VoucherNo] DEFAULT ('') NULL,
    [AddName]          VARCHAR (10)    CONSTRAINT [DF_ShippingAP_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME        NULL,
    [EditName]         VARCHAR (10)    CONSTRAINT [DF_ShippingAP_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME        NULL,
    [FactoryID]        VARCHAR (8)     DEFAULT ('') NOT NULL,
    [ExVoucherID]      VARCHAR (16)    DEFAULT ('') NULL,
    [Reason]           VARCHAR (5)     DEFAULT ('') NULL,
    [VoucherDate]      DATE            NULL,
    [SharedAmtFactory] NUMERIC (12, 2) CONSTRAINT [DF_ShippingAP_SharedAmtFactory] DEFAULT ((0)) NOT NULL,
    [SharedAmtOther]   NUMERIC (12, 2) CONSTRAINT [DF_ShippingAP_SharedAmtOther] DEFAULT ((0)) NOT NULL,
    [APPExchageRate]   NUMERIC (11, 6) DEFAULT ((0)) NOT NULL,
    [VoucherEditDate] DATE NULL, 
    [SisFtyAPID] VARCHAR(13) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_ShippingAP] PRIMARY KEY CLUSTERED ([ID] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Accounts payable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A/P 單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'CDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'費用產生原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'SubType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'LocalSuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'PayTermID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'VATRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'VAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'BLNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'審核人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'Accountant';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'審核日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'VoucherID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��]', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'Reason';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�~�b�ǲ�ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'ExVoucherID';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票資訊最後編輯日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingAP', @level2type = N'COLUMN', @level2name = N'VoucherEditDate';
GO
