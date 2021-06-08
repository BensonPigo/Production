CREATE TABLE [dbo].[InvAdjust_Qty] (
    [ID]           VARCHAR (13)   CONSTRAINT [DF_InvAdjust_Qty_ID] DEFAULT ('') NOT NULL,
    [Article]      VARCHAR (8)    CONSTRAINT [DF_InvAdjust_Qty_Article] DEFAULT ('') NOT NULL,
    [SizeCode]     VARCHAR (8)    CONSTRAINT [DF_InvAdjust_Qty_SizeCode] DEFAULT ('') NOT NULL,
    [OrderQty]     INT            CONSTRAINT [DF_InvAdjust_Qty_OrderQty] DEFAULT ((0)) NULL,
    [OrigQty]      INT            CONSTRAINT [DF_InvAdjust_Qty_OrigQty] DEFAULT ((0)) NULL,
    [AdjustQty]    INT            CONSTRAINT [DF_InvAdjust_Qty_AdjustQty] DEFAULT ((0)) NULL,
    [Price]        NUMERIC (16, 4) CONSTRAINT [DF_InvAdjust_Qty_Price] DEFAULT ((0)) NULL,
    [NewItem]      BIT            CONSTRAINT [DF_InvAdjust_Qty_NewItem] DEFAULT ((0)) NULL,
    [DiffQty]      INT            CONSTRAINT [DF_InvAdjust_Qty_DiffQty] DEFAULT ((0)) NULL,
    [Pullout3Qty]  INT            CONSTRAINT [DF_InvAdjust_Qty_Pullout3Qty] DEFAULT ((0)) NULL,
    [Ukey_Pullout] BIGINT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_InvAdjust_Qty] PRIMARY KEY CLUSTERED ([ID], [Article], [SizeCode], [Ukey_Pullout])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票調整單-數量調整', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'OrderQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'OrigQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修正後數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'AdjustQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新記錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'NewItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'差異數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'DiffQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout3 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InvAdjust_Qty', @level2type = N'COLUMN', @level2name = N'Pullout3Qty';

