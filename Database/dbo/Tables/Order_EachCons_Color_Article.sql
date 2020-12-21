CREATE TABLE [dbo].[Order_EachCons_Color_Article] (
    [Id]                       VARCHAR (13) CONSTRAINT [DF_Order_EachCons_Color_Article_Id] DEFAULT ('') NOT NULL,
    [Order_EachCons_ColorUkey] BIGINT       CONSTRAINT [DF_Order_EachCons_Color_Article_Order_EachCons_ColorUkey] DEFAULT ((0)) NOT NULL,
    [Article]                  VARCHAR (8)  CONSTRAINT [DF_Order_EachCons_Color_Article_Article] DEFAULT ('') NOT NULL,
    [ColorID]                  VARCHAR (6)  CONSTRAINT [DF_Order_EachCons_Color_Article_ColorID] DEFAULT ('') NOT NULL,
    [SizeCode]                 VARCHAR (8)  CONSTRAINT [DF_Order_EachCons_Color_Article_SizeCode] DEFAULT ('') NULL,
    [Orderqty]                 NUMERIC (6)  CONSTRAINT [DF_Order_EachCons_Color_Article_Orderqty] DEFAULT ((0)) NULL,
    [Layer]                    NUMERIC (5)  CONSTRAINT [DF_Order_EachCons_Color_Article_Layer] DEFAULT ((0)) NULL,
    [CutQty]                   NUMERIC (6)  CONSTRAINT [DF_Order_EachCons_Color_Article_CutQty] DEFAULT ((0)) NULL,
    [Variance]                 NUMERIC (6)  CONSTRAINT [DF_Order_EachCons_Color_Article_Variance] DEFAULT ((0)) NULL,
    [Ukey]                     BIGINT       CONSTRAINT [DF_Order_EachCons_Color_Article_Ukey] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_EachCons_Color_Article] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : EachCons by Article 展開.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EachCons顏色展開的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'Order_EachCons_ColorUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'Orderqty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'Layer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產出數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'CutQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'差異', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'Variance';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Color_Article', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
CREATE NONCLUSTERED INDEX [ColorUkey]
    ON [dbo].[Order_EachCons_Color_Article]([Order_EachCons_ColorUkey] ASC);

