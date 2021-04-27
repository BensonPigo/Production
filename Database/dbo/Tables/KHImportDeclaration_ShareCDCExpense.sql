CREATE TABLE [dbo].[KHImportDeclaration_ShareCDCExpense] (
    [ID]                          VARCHAR (13)   NOT NULL,
    [KHCustomsDescriptionCDCName] VARCHAR (50)   NOT NULL,
    [OriTtlNetKg]                 NUMERIC (9, 2) CONSTRAINT [DF_KHImportDeclaration_ShareCDCExpense_OriTtlNetKg] DEFAULT ((0)) NULL,
    [OriTtlWeightKg]              NUMERIC (9, 2) CONSTRAINT [DF_KHImportDeclaration_ShareCDCExpense_OriTtlWeightKg] DEFAULT ((0)) NULL,
    [OriTtlCDCAmount]             NUMERIC (9, 4) CONSTRAINT [DF_KHImportDeclaration_ShareCDCExpense_OriTtlCDCAmount] DEFAULT ((0)) NULL,
    [ActTtlNetKg]                 NUMERIC (9, 4) CONSTRAINT [DF_KHImportDeclaration_ShareCDCExpense_ActTtlNetKg] DEFAULT ((0)) NULL,
    [ActTtlWeightKg]              NUMERIC (9, 4) CONSTRAINT [DF_KHImportDeclaration_ShareCDCExpense_ActTtlWeightKg] DEFAULT ((0)) NULL,
    [ActTtlAmount]                NUMERIC (9, 4) CONSTRAINT [DF_KHImportDeclaration_ShareCDCExpense_ActTtlAmount] DEFAULT ((0)) NULL,
    [ActHSCode]                   VARCHAR (14)   CONSTRAINT [DF_KHImportDeclaration_ShareCDCExpense_ActHSCode] DEFAULT ('') NULL,
    CONSTRAINT [PK_KHImportDeclaration_ShareCDCExpense] PRIMARY KEY CLUSTERED ([ID] ASC, [KHCustomsDescriptionCDCName] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際國際商品統一分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'ActHSCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際進口報關總金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'ActTtlAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際進口報關總毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'ActTtlWeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際進口報關總淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'ActTtlNetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始系統推算之進口報關總金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'OriTtlCDCAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始系統推算之進口報關總毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'OriTtlWeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始系統推算之進口報關總淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'OriTtlNetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關物料大類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'KHCustomsDescriptionCDCName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_ShareCDCExpense', @level2type = N'COLUMN', @level2name = N'ID';

