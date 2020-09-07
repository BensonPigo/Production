CREATE TABLE [dbo].[Order_QtyShip] (
    [Id]                  VARCHAR (13)   CONSTRAINT [DF_Order_QtyShip_Id] DEFAULT ('') NOT NULL,
    [Seq]                 VARCHAR (2)    CONSTRAINT [DF_Order_QtyShip_Seq] DEFAULT ('') NOT NULL,
    [ShipmodeID]          VARCHAR (10)   CONSTRAINT [DF_Order_QtyShip_ShipmodeID] DEFAULT ('') NULL,
    [BuyerDelivery]       DATE           NULL,
    [FtyKPI]              DATE           NULL,
    [ReasonID]            VARCHAR (5)    CONSTRAINT [DF_Order_QtyShip_ReasonID] DEFAULT ('') NULL,
    [Qty]                 INT            CONSTRAINT [DF_Order_QtyShip_Qty] DEFAULT ((0)) NULL,
	[OriQty]			  INT			 CONSTRAINT [DF_Order_QtyShip_OriQty] DEFAULT ((0)) NULL,
    [EstPulloutDate]      DATE           NULL,
    [SDPDate]             DATE           NULL,
    [ProdRemark]          NVARCHAR (100) CONSTRAINT [DF_Order_QtyShip_ProdRemark] DEFAULT ('') NULL,
    [ShipRemark]          NVARCHAR (30)  CONSTRAINT [DF_Order_QtyShip_ShipRemark] DEFAULT ('') NULL,
    [OutstandingInCharge] VARCHAR (10)   CONSTRAINT [DF_Order_QtyShip_OutstandingInCharge] DEFAULT ('') NULL,
    [OutstandingDate]     DATETIME       NULL,
    [OutstandingReason]   VARCHAR (5)    CONSTRAINT [DF_Order_QtyShip_OutstandingReason] DEFAULT ('') NULL,
    [OutstandingRemark]   NVARCHAR (60)  CONSTRAINT [DF_Order_QtyShip_OutstandingRemark] DEFAULT ('') NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Order_QtyShip_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Order_QtyShip_EditName] DEFAULT ('') NULL,
    [EditDate]            DATETIME       NULL,
    [ReadyDate]           DATE           NULL,
	[CFAIs3rdInspect]	  bit NOT NULL DEFAULT ((0)),
	[CFA3rdInspectResult] varchar(15) CONSTRAINT [DF_CFA3rdInspectResult]  DEFAULT ('')  NULL,
	[CFA3rdInspectDate]   date NULL,
	[CFAFinalInspectResult] varchar(15) CONSTRAINT [DF_CFAFinalInspectResult]  DEFAULT ('') NULL ,
	[CFAFinalInspectDate]   date NULL,
	[CFAUpdateDate]			date  NULL,
	[CFARemark]				nvarchar(500)  CONSTRAINT [DF_CFARemark]  DEFAULT ('') NULL,
	[IDD]				Date   NULL,
	[IDDEditName]             VARCHAR (10)   CONSTRAINT [DF_Order_QtyShip_IDDEditName] DEFAULT ('') NULL,
    [IDDEditDate]             DATETIME       NULL,
    [CFAIs3rdInspectHandle] VARCHAR(10) NOT NULL CONSTRAINT [DF_Order_QtyShip_CFAIs3rdInspectHandle] DEFAULT(''),
    [CFAFinalInspectHandle] Varchar(10) NOT NULL CONSTRAINT [DF_Order_QtyShip_CFAFinalInspectHandle] DEFAULT(''), 
    CONSTRAINT [PK_Order_QtyShip] PRIMARY KEY CLUSTERED ([Id] ASC, [Seq] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order Qty Breakdown By Shipmode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'運輸方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'ShipmodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'BuyerDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠KPI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'FtyKPI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改客戶交期原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'ReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單數量Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計Pullout日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'EstPulloutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SDP Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'SDPDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Production schedule remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'ProdRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipment schedule remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'ShipRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'延出原因修改人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'OutstandingInCharge';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'延出原因修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'OutstandingDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'延出原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'OutstandingReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'延出備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'OutstandingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_QtyShip', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'原始數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_QtyShip',
    @level2type = N'COLUMN',
    @level2name = N'OriQty'
go
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Intended Delivery Date – 工廠預計出貨日，後續基本上會依照此日期安排出貨',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_QtyShip',
    @level2type = N'COLUMN',
    @level2name = N'IDD'