CREATE TABLE [dbo].[TransferExport_Detail_Carton]
(
	[TransferExport_DetailUkey] BIGINT NOT NULL , 
    [ID] VARCHAR(13) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_ID] DEFAULT (''),  
    [PoID] VARCHAR(13) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_PoID] DEFAULT (''),  
    [Seq1] VARCHAR(3) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_Seq1] DEFAULT (''),  
    [Seq2] VARCHAR(2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_Seq2] DEFAULT (''),  
    [Carton] NVARCHAR(100) NOT NULL DEFAULT (''), 
    [LotNo] VARCHAR(30) NOT NULL, 
    [Qty] NUMERIC(8, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_Qty] DEFAULT (0),  
    [Foc] NUMERIC(8, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_Foc] DEFAULT (0),  
    [EditName] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_EditName] DEFAULT (''),  
    [EditDate] DATETIME NULL, 
    [StockUnitID] VARCHAR(8) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_StockUnitID] DEFAULT (''),  
    [StockQty] NUMERIC(11, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_StockQty] DEFAULT (0),  
    [Tone] VARCHAR(8) NOT NULL DEFAULT (''), 
    [Roll] VARCHAR(30) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_Roll] DEFAULT (''), 
    [NetKg] NUMERIC(10, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_NetKg] DEFAULT ((0)), 
    [WeightKg] NUMERIC(10, 2) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_WeightKg] DEFAULT ((0)), 
    [CBM] NUMERIC(10, 5) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_CBM] DEFAULT ((0)), 
    [GroupID] NVARCHAR(50) NOT NULL CONSTRAINT [DF_TransferExport_Detail_Carton_GroupID] DEFAULT (''), 
    CONSTRAINT [PK_TransferExport_Detail_Carton] PRIMARY KEY CLUSTERED ([TransferExport_DetailUkey],[Carton],[LotNo],[Roll] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'轉廠WK表身Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'TransferExport_DetailUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'轉廠WKID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'箱號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'Carton'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布缸號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'LotNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'數量 ( 採購單位 )',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'Qty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'數量 ( 免費, 採購單位 )',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'Foc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'庫存單位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'StockUnitID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'數量 ( 庫存單位 )',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'StockQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布捲號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'Roll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'淨重',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'NetKg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'毛重',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'WeightKg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'材積',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'CBM'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠端拆單群組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport_Detail_Carton',
    @level2type = N'COLUMN',
    @level2name = N'GroupID'