CREATE TABLE [dbo].[Order_BOF_Expend] (
    [Id]                VARCHAR (13)    CONSTRAINT [DF_Order_BOF_Expend_Id] DEFAULT ('') NOT NULL,
    [Order_BOFUkey]     BIGINT          CONSTRAINT [DF_Order_BOF_Expend_Order_BOFUkey] DEFAULT ((0)) NOT NULL,
    [ColorId]           VARCHAR (6)     CONSTRAINT [DF_Order_BOF_Expend_ColorId] DEFAULT ('') NOT NULL,
    [SuppColor]         NVARCHAR (MAX)  CONSTRAINT [DF_Order_BOF_Expend_SuppColor] DEFAULT ('') NULL,
    [OrderQty]          NUMERIC (10, 4) CONSTRAINT [DF_Order_BOF_Expend_OrderQty] DEFAULT ((0)) NULL,
    [Price]             NUMERIC (16, 4) CONSTRAINT [DF_Order_BOF_Expend_Price] DEFAULT ((0)) NULL,
    [UsageQty]          NUMERIC (9, 2)  CONSTRAINT [DF_Order_BOF_Expend_UsageQty] DEFAULT ((0)) NULL,
    [UsageUnit]         VARCHAR (8)     CONSTRAINT [DF_Order_BOF_Expend_UsageUnit] DEFAULT ('') NULL,
    [Width]             NUMERIC (5, 2)  CONSTRAINT [DF_Order_BOF_Expend_Width] DEFAULT ((0)) NULL,
    [SysUsageQty]       NUMERIC (9, 2)  CONSTRAINT [DF_Order_BOF_Expend_SysUsageQty] DEFAULT ((0)) NULL,
    [QTFabricPanelCode] NVARCHAR (100)  CONSTRAINT [DF_Order_BOF_Expend_QTFabricPanelCode] DEFAULT ('') NULL,
    [Remark]            NVARCHAR (60)   CONSTRAINT [DF_Order_BOF_Expend_Remark] DEFAULT ('') NULL,
    [OrderIdList]       NVARCHAR (MAX)  CONSTRAINT [DF_Order_BOF_Expend_OrderIdList] DEFAULT ('') NULL,
    [AddName]           VARCHAR (10)    CONSTRAINT [DF_Order_BOF_Expend_AddName] DEFAULT ('') NULL,
    [AddDate]           DATETIME        NULL,
    [EditName]          VARCHAR (10)    CONSTRAINT [DF_Order_BOF_Expend_EditName] DEFAULT ('') NULL,
    [EditDate]          DATETIME        NULL,
    [UKEY]              BIGINT          CONSTRAINT [DF_Order_BOF_Expend_UKEY] DEFAULT ((0)) NOT NULL,
    [Special]           NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Order_BOF_Expend] PRIMARY KEY CLUSTERED ([UKEY] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill of Fabric - 布別展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BOF的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'Order_BOFUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'ColorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'SuppColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'OrderQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'UsageQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'UsageUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'SysUsageQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QT組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'QTFabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'OrderIdList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Expend', @level2type = N'COLUMN', @level2name = N'UKEY';


GO
CREATE NONCLUSTERED INDEX [Order_BOF_Expend_ID_Order_BOFUkey]
    ON [dbo].[Order_BOF_Expend]([Id] ASC, [Order_BOFUkey] ASC);

