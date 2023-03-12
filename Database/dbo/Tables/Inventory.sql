CREATE TABLE [dbo].[Inventory] (
    [Ukey]             BIGINT          CONSTRAINT [DF_Inventory_Ukey] DEFAULT ((0)) NOT NULL,
    [POID]             VARCHAR (13)    CONSTRAINT [DF_Inventory_POID] DEFAULT ('') NOT NULL,
    [Seq1]             VARCHAR (3)     CONSTRAINT [DF_Inventory_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]             VARCHAR (2)     CONSTRAINT [DF_Inventory_Seq2] DEFAULT ('') NOT NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_Inventory_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]        VARCHAR (8)     CONSTRAINT [DF_Inventory_FactoryID] DEFAULT ('') NOT NULL,
    [UnitID]           VARCHAR (8)     CONSTRAINT [DF_Inventory_UnitID] DEFAULT ('') NOT NULL,
    [ProjectID]        VARCHAR (5)     CONSTRAINT [DF_Inventory_ProjectID] DEFAULT ('') NOT NULL,
    [BrandGroup]       VARCHAR (8)     CONSTRAINT [DF_Inventory_BrandGroup] DEFAULT ('') NULL,
    [InventoryRefnoID] BIGINT          CONSTRAINT [DF_Inventory_InventoryRefnoID] DEFAULT ((0)) NOT NULL,
    [LimitHandle]      VARCHAR (10)    CONSTRAINT [DF_Inventory_LimitHandle] DEFAULT ('') NULL,
    [LimitSmr]         VARCHAR (10)    CONSTRAINT [DF_Inventory_LimitSmr] DEFAULT ('') NULL,
    [AuthMr]           VARCHAR (10)    CONSTRAINT [DF_Inventory_AuthMr] DEFAULT ('') NULL,
    [Refno]            VARCHAR (36)    CONSTRAINT [DF_Inventory_Refno] DEFAULT ('') NULL,
    [BrandID]          VARCHAR (8)     CONSTRAINT [DF_Inventory_BrandID] DEFAULT ('') NULL,
    [Payable]          VARCHAR (1)     CONSTRAINT [DF_Inventory_Payable] DEFAULT ('') NULL,
    [CurrencyID]       VARCHAR (3)     CONSTRAINT [DF_Inventory_CurrencyID] DEFAULT ('') NOT NULL,
    [Qty]              NUMERIC (10, 2) CONSTRAINT [DF_Inventory_Qty] DEFAULT ((0)) NULL,
    [InputQty]         NUMERIC (10, 2) CONSTRAINT [DF_Inventory_InputQty] DEFAULT ((0)) NULL,
    [OutputQty]        NUMERIC (10, 2) CONSTRAINT [DF_Inventory_OutputQty] DEFAULT ((0)) NULL,
    [Deadline]         DATE            NULL,
    [PoFactory]        VARCHAR (8)     CONSTRAINT [DF_Inventory_PoFactory] DEFAULT ('') NULL,
    [OrderHandle]      VARCHAR (10)    CONSTRAINT [DF_Inventory_OrderHandle] DEFAULT ('') NULL,
    [OrderSmr]         VARCHAR (10)    CONSTRAINT [DF_Inventory_OrderSmr] DEFAULT ('') NULL,
    [PoHandle]         VARCHAR (10)    CONSTRAINT [DF_Inventory_PoHandle] DEFAULT ('') NULL,
    [PoSmr]            VARCHAR (10)    CONSTRAINT [DF_Inventory_PoSmr] DEFAULT ('') NULL,
    [StyleID]          VARCHAR (15)    CONSTRAINT [DF_Inventory_StyleID] DEFAULT ('') NULL,
    [SeasonID]         VARCHAR (10)    CONSTRAINT [DF_Inventory_SeasonID] DEFAULT ('') NULL,
    [FabricType]       VARCHAR (1)     CONSTRAINT [DF_Inventory_FabricType] DEFAULT ('') NOT NULL,
    [MtlTypeID]        VARCHAR (20)    CONSTRAINT [DF_Inventory_MtlTypeID] DEFAULT ('') NULL,
    [ReasonID]         VARCHAR (5)     CONSTRAINT [DF_Inventory_ReasonID] DEFAULT ('') NULL,
    [Remark]           NVARCHAR (MAX)   CONSTRAINT [DF_Inventory_Remark] DEFAULT ('') NULL,
    [IcrNo]            VARCHAR (13)    CONSTRAINT [DF_Inventory_IcrNo] DEFAULT ('') NULL,
    [DebitID]          VARCHAR (13)    CONSTRAINT [DF_Inventory_DebitID] DEFAULT ('') NULL,
    [Lock]             BIT     CONSTRAINT [DF_Inventory_Lock] DEFAULT ((0)) NULL,
    [ETA]              DATE            NULL,
    [AddName]          VARCHAR (10)    CONSTRAINT [DF_Inventory_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME        NULL,
    [EditName]         VARCHAR (10)    CONSTRAINT [DF_Inventory_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME        NULL,
    [SCIRefno]         VARCHAR (30)    NULL DEFAULT (''),
    [SuppID]           VARCHAR (6)     NULL DEFAULT (''),
    CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED ([Ukey])
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SP#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'專案代號(ex.PR/QR/EG/EA)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'ProjectID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BRAND', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'BrandGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存規格編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'InventoryRefnoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鎖定人員可以使用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'LimitHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鎖定同主管的人員可以使用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'LimitSmr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'允許使用的人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'AuthMr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單的Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶付費', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Payable';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總入庫數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'InputQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總領料數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'OutputQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後可使用日　', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Deadline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bulk factory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'PoFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ORDER HANDLE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'OrderHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ORDER SMR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'OrderSmr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PO HANDLE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'PoHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PO SMR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'PoSmr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'style', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Season', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric,Accessior', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'MtlTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'入庫原因代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'ReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'異常成本單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'IcrNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Debit note No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'DebitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lock', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'Lock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'ETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Inventory]([POID] ASC, [Seq1] ASC, [Seq2] ASC, [FactoryID] ASC);

