CREATE TABLE [dbo].[Order_BOF_Expend_OrderList] (
    [ID]                   VARCHAR (13) CONSTRAINT [DF_Order_BOF_Expend_OrderList_ID] DEFAULT ('') NOT NULL,
    [Order_BOF_ExpendUkey] BIGINT       CONSTRAINT [DF_Order_BOF_Expend_OrderList_Order_BOF_ExpendUkey] DEFAULT ((0)) NOT NULL,
    [OrderID]              VARCHAR (13) CONSTRAINT [DF_Order_BOF_Expend_OrderList_OrderID] DEFAULT ('') NOT NULL,
    [AddName]              VARCHAR (10) CONSTRAINT [DF_Order_BOF_Expend_OrderList_AddName] DEFAULT ('') NULL,
    [AddDate]              DATETIME     NULL,
    [EditName]             VARCHAR (10) CONSTRAINT [DF_Order_BOF_Expend_OrderList_EditName] DEFAULT ('') NULL,
    [EditDate]             DATETIME     NULL,
    CONSTRAINT [PK_Order_BOF_Expend_OrderList] PRIMARY KEY CLUSTERED ([Order_BOF_ExpendUkey] ASC, [OrderID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend_OrderList', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend_OrderList', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend_OrderList', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend_OrderList', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend_OrderList', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend_OrderList', @level2type = N'COLUMN', @level2name = N'Order_BOF_ExpendUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend_OrderList', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order_BOF_Expend   - Orderlist', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend_OrderList';

