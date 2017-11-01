CREATE TABLE [dbo].[Order_UnitPrice] (
    [Id]        VARCHAR (13)   CONSTRAINT [DF_Order_UnitPrice_Id] DEFAULT ('') NOT NULL,
    [Article]   VARCHAR (8)    CONSTRAINT [DF_Order_UnitPrice_Article] DEFAULT ('') NOT NULL,
    [SizeCode]  VARCHAR (8)    CONSTRAINT [DF_Order_UnitPrice_SizeCode] DEFAULT ('') NOT NULL,
    [POPrice]   NUMERIC (16, 4) CONSTRAINT [DF_Order_UnitPrice_POPrice] DEFAULT ((0)) NOT NULL,
    [QuotCost]  NUMERIC (7, 2) CONSTRAINT [DF_Order_UnitPrice_QuotCost] DEFAULT ((0)) NULL,
    [DestPrice] NUMERIC (16, 4) CONSTRAINT [DF_Order_UnitPrice_DestPrice] DEFAULT ((0)) NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Order_UnitPrice_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Order_UnitPrice_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_Order_UnitPrice] PRIMARY KEY CLUSTERED ([Id] ASC, [Article] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FOB & Cost', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Size Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'POPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成本(USD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'QuotCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dest. Price', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'DestPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_UnitPrice', @level2type = N'COLUMN', @level2name = N'EditDate';

