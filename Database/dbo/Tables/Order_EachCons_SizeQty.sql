CREATE TABLE [dbo].[Order_EachCons_SizeQty] (
    [Order_EachConsUkey] BIGINT       CONSTRAINT [DF_Order_EachCons_SizeQty_Order_EachConsUkey] DEFAULT ((0)) NOT NULL,
    [Id]                 VARCHAR (13) CONSTRAINT [DF_Order_EachCons_SizeQty_Id] DEFAULT ('') NOT NULL,
    [SizeCode]           VARCHAR (8)  CONSTRAINT [DF_Order_EachCons_SizeQty_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                NUMERIC (4)  CONSTRAINT [DF_Order_EachCons_SizeQty_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_EachCons_SizeQty] PRIMARY KEY CLUSTERED ([Order_EachConsUkey] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order EachCons SizeQty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_SizeQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Each Cons的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_SizeQty', @level2type = N'COLUMN', @level2name = N'Order_EachConsUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_SizeQty', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_SizeQty', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_SizeQty', @level2type = N'COLUMN', @level2name = N'Qty';

