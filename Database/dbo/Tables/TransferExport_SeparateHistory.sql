CREATE TABLE [dbo].[TransferExport_SeparateHistory](
	[ID] [varchar](13) NOT NULL,
	[NewID] [varchar](13) NOT NULL,
	[NewDetailUkey] [bigint] NOT NULL,
	[PoQty] [numeric](12, 2) NOT NULL,
	[ExportQty] [numeric](12, 2) NOT NULL,
	[UnitID] [varchar](8) NOT NULL,
	[StockExportQty] [numeric](12, 2) NOT NULL,
	[StockUnitID] [varchar](8) NOT NULL,
 CONSTRAINT [PK_TransferExport_SeparateHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[NewID] ASC,
	[NewDetailUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TransferExport_SeparateHistory] ADD  CONSTRAINT [DF_TransferExport_SeparateHistory_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[TransferExport_SeparateHistory] ADD  CONSTRAINT [DF_TransferExport_SeparateHistory_NewID]  DEFAULT ('') FOR [NewID]
GO

ALTER TABLE [dbo].[TransferExport_SeparateHistory] ADD  CONSTRAINT [DF_TransferExport_SeparateHistory_NewDetailUkey]  DEFAULT ((0)) FOR [NewDetailUkey]
GO

ALTER TABLE [dbo].[TransferExport_SeparateHistory] ADD  CONSTRAINT [DF_TransferExport_SeparateHistory_PoQty]  DEFAULT ((0)) FOR [PoQty]
GO

ALTER TABLE [dbo].[TransferExport_SeparateHistory] ADD  CONSTRAINT [DF_TransferExport_SeparateHistory_ExportQty]  DEFAULT ((0)) FOR [ExportQty]
GO

ALTER TABLE [dbo].[TransferExport_SeparateHistory] ADD  CONSTRAINT [DF_TransferExport_SeparateHistory_UnitID]  DEFAULT ('') FOR [UnitID]
GO

ALTER TABLE [dbo].[TransferExport_SeparateHistory] ADD  CONSTRAINT [DF_TransferExport_SeparateHistory_StockExportQty]  DEFAULT ((0)) FOR [StockExportQty]
GO

ALTER TABLE [dbo].[TransferExport_SeparateHistory] ADD  CONSTRAINT [DF_TransferExport_SeparateHistory_StockUnitID]  DEFAULT ('') FOR [StockUnitID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆單 [前] 的 TK ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_SeparateHistory', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆單 [後] 的 TK ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_SeparateHistory', @level2type=N'COLUMN',@level2name=N'NewID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆單 [後] 的 TK 明細' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_SeparateHistory', @level2type=N'COLUMN',@level2name=N'NewDetailUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆單 [前] 台北要求出貨的總數量(採購單位)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_SeparateHistory', @level2type=N'COLUMN',@level2name=N'PoQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆單 [前] 工廠實際出貨的總數量(採購單位)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_SeparateHistory', @level2type=N'COLUMN',@level2name=N'ExportQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_SeparateHistory', @level2type=N'COLUMN',@level2name=N'UnitID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆單 [前] 工廠實際出貨的總數量(庫存單位)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_SeparateHistory', @level2type=N'COLUMN',@level2name=N'StockExportQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_SeparateHistory', @level2type=N'COLUMN',@level2name=N'StockUnitID'
GO
