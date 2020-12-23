CREATE TABLE [dbo].[POShippingList] (
    [Ukey]            BIGINT         NOT NULL,
    [IssueDate]       DATE           NULL,
    [POID]            VARCHAR (13)   NOT NULL,
    [Seq1]            VARCHAR (3)    NOT NULL,
    [T1Name]          NVARCHAR (40)  NOT NULL,
    [T1MR]            NVARCHAR (50)  NOT NULL,
    [T1FtyName]       VARCHAR (8)    NOT NULL,
    [T1FtyBrandCode]  VARCHAR (10)   NOT NULL,
    [T2Name]          NVARCHAR (255) NOT NULL,
    [T2SuppName]      NVARCHAR (255) NOT NULL,
    [T2SuppBrandCode] VARCHAR (6)    NOT NULL,
    [T2SuppCountry]   VARCHAR (2)    NOT NULL,
    [BrandID]         VARCHAR (8)    NOT NULL,
    [CurrencyID]      VARCHAR (3)    NOT NULL,
    [PackingNo]       VARCHAR (50)   NOT NULL,
    [PackingDate]     DATE           NULL,
    [InvoiceNo]       VARCHAR (50)   NOT NULL,
    [InvoiceDate]     DATE           NULL,
    [CloseDate]       DATE           NULL,
    [Vessel]          NVARCHAR (30)  NOT NULL,
    [ETD]             DATE           NULL,
    [FinalShipmodeID] VARCHAR (10)   NOT NULL,
    [SuppID]          VARCHAR (6)    NOT NULL,
    [AddName]         VARCHAR (10)   NOT NULL,
    [AddDate]         DATETIME       NULL,
    CONSTRAINT [PK_POShippingList] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Final Ship Mode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'FinalShipmodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ETD Port Of Load Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'ETD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Vessel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'Vessel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Closing Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'CloseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'InvoiceDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'InvoiceNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing list date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'PackingDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing list No. ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'PackingNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Currency', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'T1 Customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Country of Origin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'T2SuppCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'T2 adidas Factory Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'T2SuppBrandCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Actual Manufacturer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'T2SuppName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Seller', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'T2Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'T1 adidas Factory Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'T1FtyBrandCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ship To', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'T1FtyName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contact Person (Buyer)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'T1MR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Buyer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'T1Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Issue Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'IssueDate';

