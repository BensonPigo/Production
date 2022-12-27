CREATE TABLE [dbo].[Receiving_Detail](
	[Id] [varchar](13) NOT NULL,
	[MDivisionID] [varchar](8) NULL,
	[PoId] [varchar](13) NOT NULL,
	[Seq1] [varchar](3) NOT NULL,
	[Seq2] [varchar](2) NOT NULL,
	[Roll] [varchar](8) NOT NULL,
	[Dyelot] [varchar](8) NOT NULL,
	[ShipQty] [numeric](11, 2) NULL,
	[ActualQty] [numeric](11, 2) NOT NULL,
	[PoUnit] [varchar](8) NULL,
	[Weight] [numeric](7, 2) NULL,
	[ActualWeight] [numeric](7, 2) NULL,
	[StockUnit] [varchar](8) NULL,
	[Price] [numeric](11, 3) NULL,
	[Location] [varchar](500) NULL,
	[Remark] [nvarchar](100) NULL,
	[StockQty] [numeric](11, 2) NULL,
	[StockType] [varchar](1) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[OriQty] [numeric](11, 2) NULL,
	[CompleteTime] [datetime] NULL,
	[CombineBarcode] [varchar](1) NULL,
	[Unoriginal] [bit] NULL,
	[EncodeSeq] [int] NOT NULL,
	[SentToWMS] [bit] NOT NULL,
	[Fabric2LabTime] [datetime] NULL,
	[Fabric2LabBy] [varchar](10) NOT NULL,
	[Checker] [nvarchar](30) NOT NULL,
	[MINDQRCode] [varchar](80) NOT NULL,
	[MINDChecker] [varchar](10) NOT NULL,
	[MINDCheckAddDate] [datetime] NULL,
	[MINDCheckEditDate] [datetime] NULL,
	[FullRoll] [varchar](50) NOT NULL,
	[FullDyelot] [varchar](50) NOT NULL,
	[ForInspection] [bit] NOT NULL,
	[OneYardForWashing] [bit] NOT NULL,
	[ContainerCode] [nvarchar](100) NULL,
	[ExportDetailUkey] [bigint] NULL,
	[UpdateActualWeightTime] [datetime] NULL,
	[UpdateLocationTime] [datetime] NULL,
	[ForInspectionTime] [datetime] NULL,
	[QRCode_PrintDate] [datetime] NULL,
 CONSTRAINT [PK_Receiving_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Id]  DEFAULT ('') FOR [Id]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_PoId]  DEFAULT ('') FOR [PoId]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Seq1]  DEFAULT ('') FOR [Seq1]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Seq2]  DEFAULT ('') FOR [Seq2]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Roll]  DEFAULT ('') FOR [Roll]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Dyelot]  DEFAULT ('') FOR [Dyelot]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_ShipQty]  DEFAULT ((0)) FOR [ShipQty]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_ActualQty]  DEFAULT ((0)) FOR [ActualQty]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_PoUnit]  DEFAULT ('') FOR [PoUnit]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Weight]  DEFAULT ((0)) FOR [Weight]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_ActualWeight]  DEFAULT ((0)) FOR [ActualWeight]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_StockUnit]  DEFAULT ('') FOR [StockUnit]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Price]  DEFAULT ((0)) FOR [Price]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Location]  DEFAULT ('') FOR [Location]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_StockQty]  DEFAULT ((0)) FOR [StockQty]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_StockType]  DEFAULT ('') FOR [StockType]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  DEFAULT ((0)) FOR [EncodeSeq]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  DEFAULT ((0)) FOR [SentToWMS]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Fabric2LabBy]  DEFAULT ('') FOR [Fabric2LabBy]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_Checker]  DEFAULT ('') FOR [Checker]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_MINDQRCode]  DEFAULT ('') FOR [MINDQRCode]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_MINDChecker]  DEFAULT ('') FOR [MINDChecker]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_FullRoll]  DEFAULT ('') FOR [FullRoll]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_FullDyelot]  DEFAULT ('') FOR [FullDyelot]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_ForInspection]  DEFAULT ((0)) FOR [ForInspection]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  CONSTRAINT [DF_Receiving_Detail_OneYardForWashing]  DEFAULT ((0)) FOR [OneYardForWashing]
GO

ALTER TABLE [dbo].[Receiving_Detail] ADD  DEFAULT ('') FOR [ContainerCode]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組織代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'PoId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Seq1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Seq2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捲號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'ShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實收數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'ActualQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'PoUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Weight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實收重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'ActualWeight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'StockUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Price'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Location'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存實收數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'StockQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'用來當作合併布卷條碼判斷值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'CombineBarcode'
GO

EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'是否是來源值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Unoriginal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是來源值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'EncodeSeq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫剪布給實驗室的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Fabric2LabTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新倉庫剪布給實驗室時間的人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Fabric2LabBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PH 收料時負責秤重 + 剪一小塊布 (ShadeBand) + 搬該物料入庫' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'Checker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'T2 QR Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'MINDQRCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND收料人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'MINDChecker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND收料修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'MINDCheckAddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND第一次收料時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'MINDCheckEditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鐵框號 ( 主要針對主料 )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'ContainerCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新 Actual Weight 的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'UpdateActualWeightTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新 Location 的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'UpdateLocationTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送檢驗的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'ForInspectionTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QRCode 首次列印的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail', @level2type=N'COLUMN',@level2name=N'QRCode_PrintDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Receiving Detail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receiving_Detail'
GO


