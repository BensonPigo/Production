CREATE TABLE [dbo].[KHCustomsDescription_Detail] (
    [CDCCode]      VARCHAR (5)    NOT NULL,
    [PurchaseUnit] VARCHAR (8)    NOT NULL,
    [Ratio]        NUMERIC (8, 4) CONSTRAINT [DF_KHCustomsDescription_Detail_Ratio] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KHCustomsDescription_Detail_1] PRIMARY KEY CLUSTERED ([CDCCode] ASC, [PurchaseUnit] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1CDC Unit可以換多少Purchase Unit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription_Detail', @level2type = N'COLUMN', @level2name = N'Ratio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI採購單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription_Detail', @level2type = N'COLUMN', @level2name = N'PurchaseUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關大類編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription_Detail', @level2type = N'COLUMN', @level2name = N'CDCCode';

