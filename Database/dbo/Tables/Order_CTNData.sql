CREATE TABLE [dbo].[Order_CTNData] (
    [ID]        VARCHAR (13) CONSTRAINT [DF_Order_CTNData_ID] DEFAULT ('') NOT NULL,
    [RefNo]     VARCHAR (36) CONSTRAINT [DF_Order_CTNData_RefNo] DEFAULT ('') NOT NULL,
    [QtyPerCTN] SMALLINT     CONSTRAINT [DF_Order_CTNData_QtyPerCTN] DEFAULT ((0)) NULL,
    [GMTQty]    INT          CONSTRAINT [DF_Order_CTNData_GMTQty] DEFAULT ((0)) NULL,
    [CTNQty]    INT          CONSTRAINT [DF_Order_CTNData_CTNQty] DEFAULT ((0)) NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_Order_CTNData_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_Order_CTNData_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME     NULL,
    CONSTRAINT [PK_Order_CTNData] PRIMARY KEY CLUSTERED ([ID] ASC, [RefNo] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單的箱子資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每箱件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'QtyPerCTN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'GMTQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'CTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_CTNData', @level2type = N'COLUMN', @level2name = N'EditDate';

