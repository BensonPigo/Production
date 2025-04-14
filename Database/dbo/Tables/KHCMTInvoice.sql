CREATE TABLE [dbo].[KHCMTInvoice]
(
	[ID] VARCHAR(11) NOT NULL , 
    [InvDate] DATE NULL, 
    [ExchangeRate] SMALLINT CONSTRAINT [DF_KHCMTInvoice_ExchangeRate] DEFAULT (0) NOT NULL, 
    [Remark] NVARCHAR(500) CONSTRAINT [DF_KHCMTInvoice_Remark] DEFAULT ('') NOT NULL, 
    [Handle] VARCHAR(10) CONSTRAINT [DF_KHCMTInvoice_Handle] DEFAULT ('') NOT NULL, 
    [FINMgrApvName] VARCHAR(10) CONSTRAINT [DF_KHCMTInvoice_FINMgrApvName] DEFAULT ('') NOT NULL, 
    [FINMgrApvDate] DATETIME NULL, 
    [Status] VARCHAR(15) CONSTRAINT [DF_KHCMTInvoice_Status] DEFAULT ('') NOT NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_KHCMTInvoice_AddName] DEFAULT ('') NOT NULL, 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_KHCMTInvoice_EditName] DEFAULT ('') NOT NULL, 
    [EditDate] DATETIME NULL, 
    [OrderCompanyID] NUMERIC(2) NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_KHCMTInvoice] primary key ([ID])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否為訂單公司別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'KHCMTInvoice',
    @level2type = N'COLUMN',
    @level2name = N'OrderCompanyID'