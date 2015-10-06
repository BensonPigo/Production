CREATE TABLE [dbo].[FtyShipper] (
    [BrandID]   VARCHAR (8)  CONSTRAINT [DF_FtyShipper_BrandID] DEFAULT ('') NOT NULL,
    [FactoryID] VARCHAR (8)  CONSTRAINT [DF_FtyShipper_FactoryID] DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME     NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_FtyShipper_AddName] DEFAULT ('') NULL,
    [EditDate]  DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_FtyShipper_EditName] DEFAULT ('') NULL,
    CONSTRAINT [PK_FtyShipper] PRIMARY KEY CLUSTERED ([BrandID] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory Shipper 成衣出貨工廠設定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper', @level2type = N'COLUMN', @level2name = N'EditName';

