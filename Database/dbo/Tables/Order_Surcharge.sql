CREATE TABLE [dbo].[Order_Surcharge] (
    [Id]        VARCHAR (13)   CONSTRAINT [DF_Order_Surcharge_Id] DEFAULT ('') NOT NULL,
    [Caption]   VARCHAR (30)   CONSTRAINT [DF_Order_Surcharge_Caption] DEFAULT ('') NOT NULL,
    [PriceType] VARCHAR (1)    CONSTRAINT [DF_Order_Surcharge_PriceType] DEFAULT ('') NULL,
    [Price]     NUMERIC (16, 4) CONSTRAINT [DF_Order_Surcharge_Price] DEFAULT ((0)) NULL,
    [Remark]    NVARCHAR (100) CONSTRAINT [DF_Order_Surcharge_Remark] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Order_Surcharge_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Order_Surcharge_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_Order_Surcharge] PRIMARY KEY CLUSTERED ([Id] ASC, [Caption] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order Surcharge', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'欄位說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'Caption';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'計算方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'PriceType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Surcharge', @level2type = N'COLUMN', @level2name = N'EditDate';

