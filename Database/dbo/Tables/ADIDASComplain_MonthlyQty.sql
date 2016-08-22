CREATE TABLE [dbo].[ADIDASComplain_MonthlyQty] (
    [YearMonth] VARCHAR (6)  CONSTRAINT [DF_ADIDASComplain_MonthlyQty_YearMonth] DEFAULT ('') NOT NULL,
    [FactoryID] VARCHAR (8)  CONSTRAINT [DF_ADIDASComplain_MonthlyQty_FactoryID] DEFAULT ('') NOT NULL,
    [BrandID]   VARCHAR (8)  CONSTRAINT [DF_ADIDASComplain_MonthlyQty_BrandID] DEFAULT ('') NOT NULL,
    [Qty]       NUMERIC (7)  CONSTRAINT [DF_ADIDASComplain_MonthlyQty_Qty] DEFAULT ((0)) NOT NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_ADIDASComplain_MonthlyQty_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME     NULL,
    CONSTRAINT [PK_ADIDASComplain_MonthlyQty] PRIMARY KEY CLUSTERED ([YearMonth] ASC, [FactoryID] ASC, [BrandID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_MonthlyQty', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_MonthlyQty', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_MonthlyQty', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_MonthlyQty', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_MonthlyQty', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'YearMonth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_MonthlyQty', @level2type = N'COLUMN', @level2name = N'YearMonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ADIDAS Complain Monthly Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_MonthlyQty';

