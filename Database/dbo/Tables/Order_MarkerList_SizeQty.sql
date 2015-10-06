CREATE TABLE [dbo].[Order_MarkerList_SizeQty] (
    [Order_MarkerListUkey] BIGINT       CONSTRAINT [DF_Order_MarkerList_SizeQty_Order_MarkerListUkey] DEFAULT ((0)) NOT NULL,
    [Id]                   VARCHAR (13) CONSTRAINT [DF_Order_MarkerList_SizeQty_Id] DEFAULT ('') NOT NULL,
    [SizeCode]             VARCHAR (8)  CONSTRAINT [DF_Order_MarkerList_SizeQty_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                  NUMERIC (4)  CONSTRAINT [DF_Order_MarkerList_SizeQty_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_MarkerList_SizeQty] PRIMARY KEY CLUSTERED ([Order_MarkerListUkey] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單-馬克檔-Size & Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_SizeQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Marker List的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_SizeQty', @level2type = N'COLUMN', @level2name = N'Order_MarkerListUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_SizeQty', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_SizeQty', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_SizeQty', @level2type = N'COLUMN', @level2name = N'Qty';

