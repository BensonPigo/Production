CREATE TABLE [dbo].[Order_BOA_Expend] (
    [Id]              VARCHAR (13)    CONSTRAINT [DF_Order_BOA_Expend_Id] DEFAULT ('') NOT NULL,
    [UKEY]            BIGINT          CONSTRAINT [DF_Order_BOA_Expend_UKEY] DEFAULT ((0)) NOT NULL,
    [Order_BOAUkey]   BIGINT          CONSTRAINT [DF_Order_BOA_Expend_Order_BOAUkey] DEFAULT ((0)) NOT NULL,
    [OrderQty]        NUMERIC (6)     CONSTRAINT [DF_Order_BOA_Expend_OrderQty] DEFAULT ((0)) NULL,
    [Refno]           VARCHAR (20)    CONSTRAINT [DF_Order_BOA_Expend_Refno] DEFAULT ('') NOT NULL,
    [SCIRefno]        VARCHAR (30)    CONSTRAINT [DF_Order_BOA_Expend_SCIRefno] DEFAULT ('') NOT NULL,
    [Price]           NUMERIC (16, 4) CONSTRAINT [DF_Order_BOA_Expend_Price] DEFAULT ((0)) NULL,
    [UsageQty]        NUMERIC (11, 2) CONSTRAINT [DF_Order_BOA_Expend_UsageQty] DEFAULT ((0)) NULL,
    [UsageUnit]       VARCHAR (8)     CONSTRAINT [DF_Order_BOA_Expend_UsageUnit] DEFAULT ('') NULL,
    [Article]         VARCHAR (8)     CONSTRAINT [DF_Order_BOA_Expend_Article] DEFAULT ('') NULL,
    [ColorId]         NVARCHAR (70)   CONSTRAINT [DF_Order_BOA_Expend_ColorId] DEFAULT ('') NULL,
    [SuppColor]       NVARCHAR (MAX)  CONSTRAINT [DF_Order_BOA_Expend_SuppColor] DEFAULT ('') NULL,
    [SizeCode]        VARCHAR (8)     CONSTRAINT [DF_Order_BOA_Expend_SizeCode] DEFAULT ('') NULL,
    [Sizespec]        VARCHAR (15)    CONSTRAINT [DF_Order_BOA_Expend_Sizespec] DEFAULT ('') NULL,
    [SizeUnit]        VARCHAR (8)     CONSTRAINT [DF_Order_BOA_Expend_SizeUnit] DEFAULT ('') NULL,
    [OrderIdList]     NVARCHAR (MAX)  CONSTRAINT [DF_Order_BOA_Expend_OrderIdList] DEFAULT ('') NULL,
    [SysUsageQty]     NUMERIC (11, 2) CONSTRAINT [DF_Order_BOA_Expend_SysUsageQty] DEFAULT ((0)) NOT NULL,
    [Remark]          NVARCHAR (MAX)  CONSTRAINT [DF_Order_BOA_Expend_Remark] DEFAULT ('') NULL,
    [BomFactory]      VARCHAR (8)     CONSTRAINT [DF_Order_BOA_Expend_BomFactory] DEFAULT ('') NULL,
    [BomCountry]      VARCHAR (2)     CONSTRAINT [DF_Order_BOA_Expend_BomCountry] DEFAULT ('') NULL,
    [BomStyle]        VARCHAR (15)    CONSTRAINT [DF_Order_BOA_Expend_BomStyle] DEFAULT ('') NULL,
    [BomCustCD]       VARCHAR (20)    CONSTRAINT [DF_Order_BOA_Expend_BomCustCD] DEFAULT ('') NULL,
    [BomArticle]      VARCHAR (8)     CONSTRAINT [DF_Order_BOA_Expend_BomArticle] DEFAULT ('') NULL,
    [BomZipperInsert] VARCHAR (5)     CONSTRAINT [DF_Order_BOA_Expend_BomZipperInsert] DEFAULT ('') NULL,
    [BomBuymonth]     VARCHAR (10)    CONSTRAINT [DF_Order_BOA_Expend_BomBuymonth] DEFAULT ('') NULL,
    [BomCustPONo]     VARCHAR (30)    CONSTRAINT [DF_Order_BOA_Expend_BomCustPONo] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)    CONSTRAINT [DF_Order_BOA_Expend_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME        NULL,
    [EditName]        VARCHAR (10)    CONSTRAINT [DF_Order_BOA_Expend_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME        NULL,
    [Keyword]         VARCHAR (MAX)   NULL,
    [Special]         NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Order_BOA_Expend] PRIMARY KEY CLUSTERED ([UKEY] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill of Accessories - 副料展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'UKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BOA的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'Order_BOAUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'OrderQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'UsageQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'UsageUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'ColorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'SuppColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'Sizespec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'SizeUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'OrderIdList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始使用數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'SysUsageQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'BomFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'BomCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'BomStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'BomCustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'BomArticle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉鍊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'BomZipperInsert';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單月份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'BomBuymonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶的訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'BomCustPONo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Expend', @level2type = N'COLUMN', @level2name = N'EditDate';

