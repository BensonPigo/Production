CREATE TABLE [dbo].[Order_QtyShip_Detail] (
    [Id]       VARCHAR (13) CONSTRAINT [DF_Order_QtyShip_Detail_Id] DEFAULT ('') NULL,
    [Seq]      VARCHAR (2)  CONSTRAINT [DF_Order_QtyShip_Detail_Seq] DEFAULT ('') NULL,
    [Article]  VARCHAR (8)  CONSTRAINT [DF_Order_QtyShip_Detail_Article] DEFAULT ('') NULL,
    [SizeCode] VARCHAR (8)  CONSTRAINT [DF_Order_QtyShip_Detail_SizeCode] DEFAULT ('') NULL,
    [Qty]      INT          CONSTRAINT [DF_Order_QtyShip_Detail_Qty] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_Order_QtyShip_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_Order_QtyShip_Detail_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME     NULL,
    [Ukey]     BIGINT       CONSTRAINT [DF_Order_QtyShip_Detail_Ukey] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_QtyShip_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order Qty Breakdown By Shipmode Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
CREATE NONCLUSTERED INDEX [Index_Id]
    ON [dbo].[Order_QtyShip_Detail]([Id] ASC);

