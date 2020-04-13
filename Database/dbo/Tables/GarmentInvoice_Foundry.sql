CREATE TABLE [dbo].[GarmentInvoice_Foundry] (
    [InvoiceNo]    VARCHAR (25)    NOT NULL,
    [FactoryGroup] VARCHAR (4)     NOT NULL,
    [GW]           NUMERIC (9, 3)  NULL,
    [CBM]          NUMERIC (11, 4) NULL,
    [Ratio]        INT             NULL,
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

