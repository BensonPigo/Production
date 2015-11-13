CREATE TABLE [dbo].[PO_Supp_Detail_OrderList] (
    [ID]       VARCHAR (13) CONSTRAINT [DF_PO_Supp_Detail_OrderList_ID] DEFAULT ('') NOT NULL,
    [SEQ1]     VARCHAR (3)  CONSTRAINT [DF_PO_Supp_Detail_OrderList_SEQ1] DEFAULT ('') NOT NULL,
    [SEQ2]     VARCHAR (2)  CONSTRAINT [DF_PO_Supp_Detail_OrderList_SEQ2] DEFAULT ('') NOT NULL,
    [OrderID]  VARCHAR (13) CONSTRAINT [DF_PO_Supp_Detail_OrderList_OrderID] DEFAULT ('') NOT NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_PO_Supp_Detail_OrderList_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_PO_Supp_Detail_OrderList_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_PO_Supp_Detail_OrderList] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ1] ASC, [SEQ2] ASC, [OrderID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList', @level2type = N'COLUMN', @level2name = N'SEQ2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PO_Supp_Detail    - Orderlist', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_OrderList';

