CREATE TABLE [dbo].[VNFixedDeclareItem] (
    [NLCode]       VARCHAR (9)     CONSTRAINT [DF_FixedDeclareItem_NLCode] DEFAULT ('') NOT NULL,
    [HSCode]       VARCHAR (11)    CONSTRAINT [DF_FixedDeclareItem_HSCode] DEFAULT ('') NOT NULL,
    [UnitID]       VARCHAR (8)     CONSTRAINT [DF_FixedDeclareItem_UnitID] DEFAULT ('') NULL,
    [Qty]          NUMERIC (14, 3) CONSTRAINT [DF_FixedDeclareItem_Qty] DEFAULT ((0)) NULL,
    [Type]         TINYINT         CONSTRAINT [DF_FixedDeclareItem_Type] DEFAULT ((0)) NOT NULL,
    [TissuePaper]  BIT             CONSTRAINT [DF_FixedDeclareItem_TissuePaper] DEFAULT ((0)) NULL,
    [AddName]      VARCHAR (10)    CONSTRAINT [DF_FixedDeclareItem_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME        NULL,
    [EditName]     VARCHAR (10)    CONSTRAINT [DF_FixedDeclareItem_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME        NULL,
    [Refno]        VARCHAR (36)    CONSTRAINT [DF_FixedDeclareItem_Refno] DEFAULT ('') NOT NULL,
    [StockUnit]    VARCHAR (8)     CONSTRAINT [DF_FixedDeclareItem_StockUnit] DEFAULT ('') NULL,
    [FabricType]   VARCHAR (1)     CONSTRAINT [DF_FixedDeclareItem_FabricType] DEFAULT ('') NOT NULL,
    [VNContractID] VARCHAR (15)    DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_FixedDeclareItem] PRIMARY KEY CLUSTERED ([VNContractID] ASC, [NLCode] ASC, [Refno] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tissue paper項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'TissuePaper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報關單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際商品統一分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'HSCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NL Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem', @level2type = N'COLUMN', @level2name = N'NLCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口報關固定項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNFixedDeclareItem';

