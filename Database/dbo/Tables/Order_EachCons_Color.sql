CREATE TABLE [dbo].[Order_EachCons_Color] (
    [Id]                 VARCHAR (13)   CONSTRAINT [DF_Order_EachCons_Color_Id] DEFAULT ('') NOT NULL,
    [Order_EachConsUkey] BIGINT         CONSTRAINT [DF_Order_EachCons_Color_Order_EachConsUkey] DEFAULT ((0)) NOT NULL,
    [Ukey]               BIGINT         CONSTRAINT [DF_Order_EachCons_Color_Ukey] DEFAULT ((0)) NOT NULL,
    [ColorID]            VARCHAR (6)    CONSTRAINT [DF_Order_EachCons_Color_ColorID] DEFAULT ('') NOT NULL,
    [CutQty]             NUMERIC (6)    CONSTRAINT [DF_Order_EachCons_Color_CutQty] DEFAULT ((0)) NULL,
    [Layer]              NUMERIC (5)    CONSTRAINT [DF_Order_EachCons_Color_Layer] DEFAULT ((0)) NULL,
    [Orderqty]           NUMERIC (6)    CONSTRAINT [DF_Order_EachCons_Color_Orderqty] DEFAULT ((0)) NULL,
    [SizeList]           NVARCHAR (100) CONSTRAINT [DF_Order_EachCons_Color_SizeList] DEFAULT ('') NULL,
    [Variance]           NUMERIC (6)    CONSTRAINT [DF_Order_EachCons_Color_Variance] DEFAULT ((0)) NULL,
    [YDS]                NUMERIC (6, 2) CONSTRAINT [DF_Order_EachCons_Color_YDS] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_EachCons_Color] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : Each Cons by Color 展開.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EachCons的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'Order_EachConsUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'CutQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'Layer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'Orderqty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸及ratio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'SizeList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'差異', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'Variance';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color', @level2type = N'COLUMN', @level2name = N'YDS';

