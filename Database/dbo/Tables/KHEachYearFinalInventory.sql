CREATE TABLE [dbo].[KHEachYearFinalInventory] (
    [SettlementDate] DATE            NOT NULL,
    [FactoryID]      VARCHAR (8)     NOT NULL,
    [ReportQty]      NUMERIC (16, 3) CONSTRAINT [DF_KHEachYearFinalInventory_ReportQty] DEFAULT ((0)) NULL,
    [FactoryQty]     NUMERIC (16, 3) CONSTRAINT [DF_KHEachYearFinalInventory_FactoryQty] DEFAULT ((0)) NULL,
    [CustomsQty]     NUMERIC (16, 3) CONSTRAINT [DF_KHEachYearFinalInventory_CustomsQty] DEFAULT ((0)) NULL,
    [AddName]        VARCHAR (10)    NULL,
    [AddDate]        DATETIME        NULL,
    [EditName]       VARCHAR (10)    NULL,
    [EditDate]       DATETIME        NULL,
    CONSTRAINT [PK_KHEachYearFinalInventory] PRIMARY KEY CLUSTERED ([SettlementDate] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inventory - Customs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHEachYearFinalInventory', @level2type = N'COLUMN', @level2name = N'CustomsQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inventory - Factory (Each Cons.)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHEachYearFinalInventory', @level2type = N'COLUMN', @level2name = N'FactoryQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inventory - Report (Packing)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHEachYearFinalInventory', @level2type = N'COLUMN', @level2name = N'ReportQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHEachYearFinalInventory', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結算日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHEachYearFinalInventory', @level2type = N'COLUMN', @level2name = N'SettlementDate';

