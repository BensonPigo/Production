CREATE TABLE [dbo].[Order_SizeItem] (
    [Id]       VARCHAR (13)   CONSTRAINT [DF_Order_SizeItem_Id] DEFAULT ('') NOT NULL,
    [SizeItem] VARCHAR (3)    CONSTRAINT [DF_Order_SizeItem_SizeItem] DEFAULT ('') NOT NULL,
    [SizeUnit] VARCHAR (8)    CONSTRAINT [DF_Order_SizeItem_SizeUnit] DEFAULT ('') NULL,
    [SizeDesc] NVARCHAR (100) CONSTRAINT [DF_Order_SizeItem_SizeDesc] DEFAULT ('') NULL,
    CONSTRAINT [PK_Order_SizeItem] PRIMARY KEY CLUSTERED ([Id] ASC, [SizeItem] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的左邊標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'項目編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸表單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'項目描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeDesc';

