CREATE TABLE [dbo].[SubconOutContractAP]
(
	[ID]               VARCHAR (13)     CONSTRAINT [DF_SubconOutContractAP_ID]              DEFAULT (('')) NOT NULL, 
    [MDivisionID]       VARCHAR(8)      CONSTRAINT [DF_SubconOutContractAP_MDivisionID]     DEFAULT (('')) NOT NULL, 
    [FactoryID]         VARCHAR(8)      CONSTRAINT [DF_SubconOutContractAP_FactoryID]       DEFAULT ((''))  NOT NULL , 
    [IssueDate]         DATE                                                                                NULL, 
    [LocalSuppID]       VARCHAR(8)      CONSTRAINT [DF_SubconOutContractAP_LocalSuppI]      DEFAULT ((''))  NOT NULL , 
    [CurrencyID]        VARCHAR(3)      CONSTRAINT [DF_SubconOutContractAP_CurrencyID]      DEFAULT ((''))  NOT NULL , 
    [Amount]            NUMERIC(12, 2)  CONSTRAINT [DF_SubconOutContractAP_Amount]          DEFAULT ((0))   NOT NULL , 
    [VatRate]           NUMERIC(3, 1)   CONSTRAINT [DF_SubconOutContractAP_VatRate]         DEFAULT ((0))   NOT NULL , 
    [Vat]               NUMERIC(11, 2)  CONSTRAINT [DF_SubconOutContractAP_Vat]             DEFAULT ((0))   NOT NULL , 
    [PaytermID]         VARCHAR(6)      CONSTRAINT [DF_SubconOutContractAP_PaytermID]       DEFAULT ((''))  NOT NULL, 
    [InvNo]             VARCHAR(100)    CONSTRAINT [DF_SubconOutContractAP_InvNo]           DEFAULT ((''))  NOT NULL , 
    [Remark]            NVARCHAR(120)   CONSTRAINT [DF_SubconOutContractAP_Remark]          DEFAULT ((''))  NOT NULL , 
    [Handle]            VARCHAR(10)     CONSTRAINT [DF_SubconOutContractAP_Handle]          DEFAULT ((''))  NOT NULL , 
    [ApvName]           VARCHAR(10)     CONSTRAINT [DF_SubconOutContractAP_ApvName]         DEFAULT ((''))  NOT NULL , 
    [ApvDate]           DATE                                                                                NULL, 
    [VoucherID]         VARCHAR(16)     CONSTRAINT [DF_SubconOutContractAP_VoucherID]       DEFAULT ((''))  NOT NULL, 
    [VoucherDate]       DATE                                                                                NULL, 
    [ExVoucherID]       VARCHAR(16)     CONSTRAINT [DF_SubconOutContractAP_ExVoucherID]     DEFAULT ((''))  NOT NULL, 
    [ExVoucherDate]     DATE                                                                                NULL, 
    [Status]            VARCHAR(15)     CONSTRAINT [DF_SubconOutContractAP_Status]          DEFAULT ((''))  NOT NULL , 
    [AddName]           VARCHAR(10)     CONSTRAINT [DF_SubconOutContractAP_AddName]         DEFAULT ((''))  NOT NULL, 
    [AddDate]           DATETIME        NULL, 
    [EditName]          VARCHAR(10)     CONSTRAINT [DF_SubconOutContractAP_EditName]        DEFAULT ((''))  NOT NULL, 
    [EditDate]          DATETIME                                                                            NULL, 
    CONSTRAINT [PK_SubconOutContractAP] PRIMARY KEY ([ID]),
	 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'外帳轉傳票日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'ExVoucherDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'外帳傳票編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'ExVoucherID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'轉傳票日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'VoucherDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'傳票編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'VoucherID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'核可日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'ApvDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'核可人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'ApvName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'負責人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'Handle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發票號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'InvNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'付款條件',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'PaytermID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'稅額',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'Vat'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'稅率',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'VatRate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總金額',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'Amount'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'幣別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'CurrencyID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'外發廠代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'LocalSuppID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'IssueDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'組織代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'AP單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP',
    @level2type = N'COLUMN',
    @level2name = N'ID'