CREATE TABLE [dbo].[TransferExport_Detail]
(
	[ID] VARCHAR(13) NOT NULL , 
    [Ukey] BIGINT NOT NULL, 
    [PoID] VARCHAR(13) NOT NULL CONSTRAINT [DF_TransferExport_Detail_PoID] DEFAULT (''), 
    [Seq1] VARCHAR(3) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Seq1] DEFAULT (''),  
    [Seq2] VARCHAR(2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Seq2] DEFAULT (''), 
    [PoType] VARCHAR NOT NULL CONSTRAINT [DF_TransferExport_Detail_PoType] DEFAULT (''), 
    [FabricType] VARCHAR NOT NULL CONSTRAINT [DF_TransferExport_Detail_FabricType] DEFAULT (''), 
    [Qty] NUMERIC(12, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Qty] DEFAULT (0), 
    [Foc] NUMERIC(12, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Foc] DEFAULT (0), 
    [Carton] NVARCHAR(500) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton] DEFAULT (''), 
    [Confirm] BIT NOT NULL CONSTRAINT [DF_TransferExport_Detail_Confirm] DEFAULT (0), 
    [UnitID] VARCHAR(8) NOT NULL CONSTRAINT [DF_TransferExport_Detail_UnitID] DEFAULT (''), 
    [Price] NUMERIC(16, 4) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Price] DEFAULT (0), 
    [NetKg] NUMERIC(10, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_NetKg] DEFAULT (0), 
    [WeightKg] NUMERIC(10, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_WeightKg] DEFAULT (0), 
    [Remark] NVARCHAR(300) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Remark] DEFAULT (''), 
    [PayDesc] NVARCHAR(300) NOT NULL CONSTRAINT [DF_TransferExport_Detail_PayDesc] DEFAULT (''),
    [LastEta] DATE NULL, 
    [Refno] VARCHAR(36) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Refno] DEFAULT (''), 
    [SCIRefno] VARCHAR(30) NOT NULL CONSTRAINT [DF_TransferExport_Detail_SCIRefno] DEFAULT (''), 
    [SuppID] VARCHAR(6) NOT NULL CONSTRAINT [DF_TransferExport_Detail_SuppID] DEFAULT (''), 
    [Pino] VARCHAR(25) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Pino] DEFAULT (''), 
    [Description] NVARCHAR(MAX) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Description] DEFAULT (''), 
    [BalanceQty] NUMERIC(12, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_BalanceQty] DEFAULT (0), 
    [BalanceFoc] NUMERIC(12, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_BalanceFoc] DEFAULT (0), 
    [PoHandle] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_Detail_PoHandle] DEFAULT (''), 
    [PcHandle] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_Detail_PcHandle] DEFAULT (''), 
    [AddName] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_Detail_AddName] DEFAULT (''), 
    [AddDate] DATETIME NULL,
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_Detail_EditName] DEFAULT (''),  
    [PoQty] NUMERIC(12, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_PoQty] DEFAULT (0), 
    [PoFoc] NUMERIC(12, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_PoFoc] DEFAULT (0), 
    [Duty] VARCHAR NOT NULL CONSTRAINT [DF_TransferExport_Detail_Duty] DEFAULT (''), 
    [DutyID] VARCHAR(13) NOT NULL CONSTRAINT [DF_TransferExport_Detail_DutyID] DEFAULT (''), 
    [Export_ShareAmount_Ukey] BIGINT NULL,
    [CBM] NUMERIC(10, 5) NOT NULL CONSTRAINT [DF_TransferExport_Detail_CBM] DEFAULT (0), 
    [ShipmentTerm] VARCHAR(5) NOT NULL CONSTRAINT [DF_TransferExport_Detail_ShipmentTerm] DEFAULT (''), 
    [InventoryPOID] VARCHAR(13) NOT NULL CONSTRAINT [DF_TransferExport_Detail_InventoryPOID] DEFAULT (''), 
    [InventorySeq1] VARCHAR(3) NOT NULL CONSTRAINT [DF_TransferExport_Detail_InventorySeq1] DEFAULT (''), 
    [InventorySeq2] VARCHAR(2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_InventorySeq2] DEFAULT (''), 
    [InvTrans_Ukey] BIGINT NULL, 
    [TransferExportReason] VARCHAR(5) NOT NULL CONSTRAINT [DF_TransferExport_Detail_TransferExportReason] DEFAULT (''),  
    [Ori_DetailUkey] BIGINT NULL, 
    CONSTRAINT [PK_TransferExport_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'轉廠 WK#',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'採購單種類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'PoType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物料種類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'FabricType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出貨數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Qty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出貨數量(免費)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Foc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'箱號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Carton'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'UnitID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'價格',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Price'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'淨重',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'NetKg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總重',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'WeightKg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SCI料號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SCIRefno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'料號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Refno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠商',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SuppID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發票號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Pino'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物品描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'剩餘數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'BalanceQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'剩餘數量(免費)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'BalanceFoc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單負責人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'PoHandle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'採購負責人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'PcHandle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'PoQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單數量(免費)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'PoFoc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'責任歸屬種類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Duty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'責任歸屬單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'DutyID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'運保費分攤Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Export_ShareAmount_Ukey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裁積',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CBM'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'庫存 POID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InventoryPOID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'庫存 Seq1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InventorySeq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'庫存 Seq2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InventorySeq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'領庫存料 InvTrans',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InvTrans_Ukey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'轉出物料不足的原因',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'TransferExportReason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'TK 拆單前的 TransferExport_Detail.Ukey，如果此欄位值與目前的 TransferExport_Detail.Ukey 一致代表該箱沒有拆到其他 TK，主要用在 Transfer Out 更新與拆分',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Ori_DetailUkey'