CREATE TABLE [dbo].[ArtworkAP] (
    [Id]            VARCHAR (13)    CONSTRAINT [DF_ArtworkAP_Id] DEFAULT ('') NOT NULL,
    [MDivisionID]   VARCHAR (8)     NULL,
    [FactoryID]     VARCHAR (8)     CONSTRAINT [DF_ArtworkAP_FactoryID] DEFAULT ('') NOT NULL,
    [LocalSuppID]   VARCHAR (8)     CONSTRAINT [DF_ArtworkAP_LocalSuppID] DEFAULT ('') NOT NULL,
    [IssueDate]     DATE            NOT NULL,
    [CurrencyID]    VARCHAR (3)     CONSTRAINT [DF_ArtworkAP_CurrencyID] DEFAULT ('') NOT NULL,
    [Amount]        NUMERIC (12, 2) CONSTRAINT [DF_ArtworkAP_Amount] DEFAULT ((0)) NULL,
    [VatRate]       NUMERIC (3, 1)  CONSTRAINT [DF_ArtworkAP_VatRate] DEFAULT ((0)) NOT NULL,
    [Vat]           NUMERIC (11, 2) CONSTRAINT [DF_ArtworkAP_Vat] DEFAULT ((0)) NULL,
    [PayTermID]     VARCHAR (6)     CONSTRAINT [DF_ArtworkAP_PayTermID] DEFAULT ('') NOT NULL,
    [InvNo]         VARCHAR (100)   CONSTRAINT [DF_ArtworkAP_InvNo] DEFAULT ('') NULL,
    [Handle]        VARCHAR (10)    CONSTRAINT [DF_ArtworkAP_Handle] DEFAULT ('') NOT NULL,
    [ArtworkTypeID] VARCHAR (20)    CONSTRAINT [DF_ArtworkAP_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [ApvDate]       DATE            NULL,
    [ApvName]       VARCHAR (10)    CONSTRAINT [DF_ArtworkAP_ApvName] DEFAULT ('') NULL,
    [VoucherID]     VARCHAR (16)    CONSTRAINT [DF_ArtworkAP_TransID] DEFAULT ('') NULL,
    [VoucherDate]   DATE            NULL,
    [Remark]        NVARCHAR (60)   CONSTRAINT [DF_ArtworkAP_Remark] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_ArtworkAP_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_ArtworkAP_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME        NULL,
    [Status]        VARCHAR (15)    CONSTRAINT [DF_ArtworkAP_Status] DEFAULT ('') NULL,
    [ExVoucherID]   VARCHAR(16)     CONSTRAINT [DF_ArtworkAP_ExVoucherID] DEFAULT ('') NULL, 
    [ExVoucherDate] DATE            NULL, 
    CONSTRAINT [PK_ArtworkAP] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工應付主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工應付單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'LocalSuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建單日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'VatRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'Vat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款條件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'PayTermID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核單日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核單人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'VoucherID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉傳票日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkAP', @level2type = N'COLUMN', @level2name = N'VoucherDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'外帳傳票ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ArtworkAP',
    @level2type = N'COLUMN',
    @level2name = N'ExVoucherID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'外帳轉傳票日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ArtworkAP',
    @level2type = N'COLUMN',
    @level2name = N'ExVoucherDate'