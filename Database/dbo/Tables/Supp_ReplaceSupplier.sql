CREATE TABLE [dbo].[Supp_ReplaceSupplier] (
    [ID]        VARCHAR (6)  NOT NULL,
    [BrandID]   VARCHAR (8)  NOT NULL,
    [CountryID] VARCHAR (2)  NOT NULL,
    [SuppID]    VARCHAR (6)  NULL,
    [AddName]   VARCHAR (10) NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) NULL,
    [EditDate]  DATETIME     NULL,
    [IsECFA]    BIT          DEFAULT ((0)) NOT NULL,
    [FactoryID] VARCHAR (8)  CONSTRAINT [DF_Supp_ReplaceSupplier_FactoryID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Supp_ReplaceSupplier] PRIMARY KEY CLUSTERED ([ID] ASC, [BrandID] ASC, [CountryID] ASC, [IsECFA] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ECFA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_ReplaceSupplier', @level2type = N'COLUMN', @level2name = N'IsECFA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Supplier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_ReplaceSupplier', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Country', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_ReplaceSupplier', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_ReplaceSupplier', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Supplier Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_ReplaceSupplier', @level2type = N'COLUMN', @level2name = N'ID';

