CREATE TABLE [dbo].[GarmentInvoice_Foundry] (
    [InvoiceNo]    VARCHAR (25)    NOT NULL,
    [FactoryGroup] VARCHAR (4)     NOT NULL,
    [GW]           DECIMAL (9, 3)  CONSTRAINT [DF_GarmentInvoice_Foundry_GW] DEFAULT ((0)) NOT NULL,
    [CBM]          DECIMAL (11, 4) CONSTRAINT [DF_GarmentInvoice_Foundry_CBM] DEFAULT ((0)) NOT NULL,
    [Ratio]        INT             CONSTRAINT [DF_GarmentInvoice_Foundry_Ratio] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GarmentInvoice_Foundry] PRIMARY KEY CLUSTERED ([InvoiceNo] ASC, [FactoryGroup] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'比例', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentInvoice_Foundry', @level2type = N'COLUMN', @level2name = N'Ratio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentInvoice_Foundry', @level2type = N'COLUMN', @level2name = N'CBM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentInvoice_Foundry', @level2type = N'COLUMN', @level2name = N'GW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GB單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentInvoice_Foundry', @level2type = N'COLUMN', @level2name = N'InvoiceNo';

