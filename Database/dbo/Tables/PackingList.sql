CREATE TABLE [dbo].[PackingList] (
    [ID]                VARCHAR (13)    CONSTRAINT [DF_PackingList_ID] DEFAULT ('') NOT NULL,
    [Type]              VARCHAR (1)     CONSTRAINT [DF_PackingList_Type] DEFAULT ('') NOT NULL,
    [MDivisionID]       VARCHAR (8)     CONSTRAINT [DF_PackingList_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]         VARCHAR (8)     CONSTRAINT [DF_PackingList_FactoryID] DEFAULT ('') NOT NULL,
    [OrderID]           VARCHAR (13)    CONSTRAINT [DF_PackingList_OrderID] DEFAULT ('') NULL,
    [OrderShipmodeSeq]  VARCHAR (2)     CONSTRAINT [DF_PackingList_OrderShipmodeSeq] DEFAULT ('') NULL,
    [ShipModeID]        VARCHAR (10)    CONSTRAINT [DF_PackingList_ShipModeID] DEFAULT ('') NULL,
    [BrandID]           VARCHAR (8)     CONSTRAINT [DF_PackingList_BrandID] DEFAULT ('') NULL,
    [Dest]              VARCHAR (2)     CONSTRAINT [DF_PackingList_Dest] DEFAULT ('') NULL,
    [CustCDID]          VARCHAR (16)    CONSTRAINT [DF_PackingList_CustCDID] DEFAULT ('') NULL,
    [CTNQty]            INT             CONSTRAINT [DF_PackingList_CTNQty] DEFAULT ((0)) NULL,
    [ShipQty]           INT             CONSTRAINT [DF_PackingList_ShipQty] DEFAULT ((0)) NULL,
    [NW]                NUMERIC (9, 3)  CONSTRAINT [DF_PackingList_NW] DEFAULT ((0)) NULL,
    [GW]                NUMERIC (9, 3)  CONSTRAINT [DF_PackingList_GW] DEFAULT ((0)) NULL,
    [NNW]               NUMERIC (9, 3)  CONSTRAINT [DF_PackingList_NNW] DEFAULT ((0)) NULL,
    [CBM]               NUMERIC (11, 4) CONSTRAINT [DF_PackingList_CBM] DEFAULT ((0)) NULL,
    [CargoReadyDate]    DATE            NULL,
    [Remark]            NVARCHAR (150)  CONSTRAINT [DF_PackingList_Remark] DEFAULT ('') NULL,
    [EstCTNBooking]     DATE            NULL,
    [EstCTNArrive]      DATE            NULL,
    [ApvToPurchase]     BIT             CONSTRAINT [DF_PackingList_ApvToPurchase] DEFAULT ((0)) NULL,
    [ApvToPurchaseDate] DATE            NULL,
    [LocalPOID]         VARCHAR (16)    CONSTRAINT [DF_PackingList_LocalPOID] DEFAULT ('') NULL,
    [Status]            VARCHAR (15)    CONSTRAINT [DF_PackingList_Status] DEFAULT ('') NULL,
    [INVNo]             VARCHAR (25)    CONSTRAINT [DF_PackingList_INVNo] DEFAULT ('') NULL,
    [GMTBookingLock]    VARCHAR (1)     CONSTRAINT [DF_PackingList_GMTBookingLock] DEFAULT ('') NULL,
    [ShipPlanID]        VARCHAR (13)    CONSTRAINT [DF_PackingList_ShipPlanID] DEFAULT ('') NULL,
    [PulloutDate]       DATE            NULL,
    [PulloutID]         VARCHAR (13)    CONSTRAINT [DF_PackingList_PulloutID] DEFAULT ('') NULL,
    [ExpressID]         VARCHAR (13)    CONSTRAINT [DF_PackingList_ExpressID] DEFAULT ('') NULL,
    [InspDate]          DATE            NULL,
    [InspStatus]        VARCHAR (10)    CONSTRAINT [DF_PackingList_InspStatus] DEFAULT ('') NULL,
    [AddName]           VARCHAR (10)    CONSTRAINT [DF_PackingList_AddName] DEFAULT ('') NULL,
    [AddDate]           DATETIME        NULL,
    [EditName]          VARCHAR (10)    CONSTRAINT [DF_PackingList_EditName] DEFAULT ('') NULL,
    [EditDate]          DATETIME        NULL,
    [TransFerToClogID]  VARCHAR (13)    NULL,
    [ClogReceiveID]     VARCHAR (13)    NULL,
    [QueryDate]         DATE            NULL,
    [RepackFrom]        VARCHAR (13)    NULL,
	[CannotModify]      BIT             CONSTRAINT [DF_PackingList_CannotModify] DEFAULT ((0)) not NULL,
    CONSTRAINT [PK_PackingList] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List Weight & Summary', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'任一箱曾經出分撿倉 或是 曾經入過成品倉', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'CannotModify';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty Breakdown Shipmode的Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'OrderShipmodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Destination', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'Dest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'CustCDID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'CTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總出貨數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'NW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'GW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨淨重(不含包裝的重量)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'NNW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'CBM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cargo Ready Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'CargoReadyDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱子預計下單日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'EstCTNBooking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱子預計到達日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'EstCTNArrive';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認採購箱子', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'ApvToPurchase';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認採購箱子日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'ApvToPurchaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'LocalPOID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'INVNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment Booking Lock', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'GMTBookingLock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ship Plan ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'ShipPlanID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'PulloutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'PulloutID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際快遞單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'ExpressID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'InspDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'InspStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingList', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
CREATE NONCLUSTERED INDEX [Index_OrderIDOrderShipmodeSeq]
    ON [dbo].[PackingList]([OrderID] ASC, [OrderShipmodeSeq] ASC);


GO
CREATE NONCLUSTERED INDEX [INVNo]
    ON [dbo].[PackingList]([INVNo] ASC);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'查詢日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingList',
    @level2type = N'COLUMN',
    @level2name = N'QueryDate'