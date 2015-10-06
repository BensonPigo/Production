CREATE TABLE [dbo].[MNOrder] (
    [ID]            VARCHAR (13)   CONSTRAINT [DF_MNOrder_ID] DEFAULT ('') NOT NULL,
    [BrandID]       VARCHAR (8)    CONSTRAINT [DF_MNOrder_BrandID] DEFAULT ('') NOT NULL,
    [ProgramID]     VARCHAR (12)   CONSTRAINT [DF_MNOrder_ProgramID] DEFAULT ('') NOT NULL,
    [StyleID]       VARCHAR (15)   CONSTRAINT [DF_MNOrder_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]      VARCHAR (10)   CONSTRAINT [DF_MNOrder_SeasonID] DEFAULT ('') NOT NULL,
    [Qty]           INT            CONSTRAINT [DF_MNOrder_Qty] DEFAULT ((0)) NULL,
    [OrderUNit]     VARCHAR (8)    CONSTRAINT [DF_MNOrder_OrderUNit] DEFAULT ('') NULL,
    [FactoryID]     VARCHAR (8)    CONSTRAINT [DF_MNOrder_FactoryID] DEFAULT ('') NOT NULL,
    [CTNQty]        SMALLINT       CONSTRAINT [DF_MNOrder_CTNQty] DEFAULT ((0)) NULL,
    [CustCDID]      VARCHAR (16)   CONSTRAINT [DF_MNOrder_CustCDID] DEFAULT ('') NULL,
    [CustPONO]      VARCHAR (30)   CONSTRAINT [DF_MNOrder_CustPONO] DEFAULT ('') NULL,
    [Customize1]    VARCHAR (30)   CONSTRAINT [DF_MNOrder_Customize1] DEFAULT ('') NULL,
    [BuyerDelivery] DATE           NOT NULL,
    [MRHandle]      VARCHAR (10)   CONSTRAINT [DF_MNOrder_MRHandle] DEFAULT ('') NOT NULL,
    [SMR]           VARCHAR (10)   CONSTRAINT [DF_MNOrder_SMR] DEFAULT ('') NOT NULL,
    [PACKING]       NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_PACKING] DEFAULT ('') NULL,
    [Packing2]      NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_Packing2] DEFAULT ('') NULL,
    [MarkBack]      NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_MarkBack] DEFAULT ('') NULL,
    [MarkFront]     NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_MarkFront] DEFAULT ('') NULL,
    [MarkLeft]      NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_MarkLeft] DEFAULT ('') NULL,
    [MarkRight]     NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_MarkRight] DEFAULT ('') NULL,
    [Label]         NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_Label] DEFAULT ('') NULL,
    [SizeRange]     NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_SizeRange] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_MNOrder_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME       NULL,
    CONSTRAINT [PK_MNOrder] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MNORDER', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SP.NO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'ProgramID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'OrderUNit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每箱的包裝數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'CTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶資料代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'CustCDID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'CustPONO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'自訂欄位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'Customize1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'BuyerDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'MRHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'SMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'包裝說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'PACKING';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'包裝說明 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'Packing2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'嘜頭 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'MarkBack';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'嘜頭 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'MarkFront';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'嘜頭 3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'MarkLeft';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'嘜頭 4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'MarkRight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片與商標位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'Label';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸清單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'SizeRange';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder', @level2type = N'COLUMN', @level2name = N'AddDate';

