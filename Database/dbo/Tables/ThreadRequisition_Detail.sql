CREATE TABLE [dbo].[ThreadRequisition_Detail] (
    [OrderID]        VARCHAR (13)   CONSTRAINT [DF_ThreadRequisition_Detail_OrderID] DEFAULT ('') NOT NULL,
    [Refno]          VARCHAR (36)   CONSTRAINT [DF_ThreadRequisition_Detail_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID]  VARCHAR (15)   CONSTRAINT [DF_ThreadRequisition_Detail_ThreadColorID] DEFAULT ('') NOT NULL,
    [ConsumptionQty] NUMERIC (8)    CONSTRAINT [DF_ThreadRequisition_Detail_ConsumptionQty] DEFAULT ((0)) NULL,
    [TotalQty]       NUMERIC (6)    CONSTRAINT [DF_ThreadRequisition_Detail_TotalQty] DEFAULT ((0)) NULL,
    [AllowanceQty]   NUMERIC (6)    CONSTRAINT [DF_ThreadRequisition_Detail_AllowanceQty] DEFAULT ((0)) NULL,
    [UseStockQty]    NUMERIC (6)    CONSTRAINT [DF_ThreadRequisition_Detail_UseStockQty] DEFAULT ((0)) NULL,
    [PurchaseQty]    NUMERIC (6)    CONSTRAINT [DF_ThreadRequisition_Detail_PurchaseQty] DEFAULT ((0)) NOT NULL,
	[UseStockNewConeQty]    NUMERIC (6)    CONSTRAINT [DF_ThreadRequisition_Detail_UseStockNewConeQty] DEFAULT ((0)) NULL,
	[UseStockUseConeQty]    NUMERIC (6)    CONSTRAINT [DF_ThreadRequisition_Detail_UseStockUseConeQty] DEFAULT ((0)) NULL,
    [PoId]           VARCHAR (13)   CONSTRAINT [DF_ThreadRequisition_Detail_PoId] DEFAULT ('') NULL,
    [Remark]         NVARCHAR (200) CONSTRAINT [DF_ThreadRequisition_Detail_Remark] DEFAULT ('') NULL,
    [Ukey]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [AutoCreate]     BIT            CONSTRAINT [DF_ThreadRequisition_Detail_AutoCreate] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ThreadRequisition_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Requisition Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SP#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'ConsumptionQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總Cone 數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'TotalQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Allowance 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'AllowanceQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用庫存數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'UseStockQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'PurchaseQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'PoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'自動產生', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail', @level2type = N'COLUMN', @level2name = N'AutoCreate';

