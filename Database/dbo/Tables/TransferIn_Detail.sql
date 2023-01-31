
CREATE TABLE [dbo].[TransferIn_Detail](
	[ID] [varchar](13) NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[POID] [varchar](13) NOT NULL,
	[Seq1] [varchar](3) NOT NULL,
	[Seq2] [varchar](2) NOT NULL,
	[Roll] [varchar](8) NOT NULL,
	[Dyelot] [varchar](8) NOT NULL,
	[StockType] [varchar](1) NOT NULL,
	[Location] [varchar](500) NULL,
	[Qty] [numeric](10, 2) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Weight] [numeric](7, 2) NOT NULL,
	[Remark] [nvarchar](100) NOT NULL,
	[OriQty] [numeric](11, 2) NULL,
	[CompleteTime] [datetime] NULL,
	[CombineBarcode] [varchar](1) NULL,
	[Unoriginal] [varchar](1) NULL,
	[ActualWeight] [numeric](7, 2) NOT NULL,
	[SentToWMS] [bit] NOT NULL,
	[Fabric2LabTime] [datetime] NULL,
	[Fabric2LabBy] [varchar](10) NOT NULL,
	[Checker] [nvarchar](30) NOT NULL,
	[ContainerCode] [nvarchar](100) NULL,
	[QRCode_PrintDate] [datetime] NULL,
	[Tone] [varchar](8) NOT NULL,
	[MINDQRCode] [varchar](80) NOT NULL,
	[MINDChecker] [varchar](10) NOT NULL,
	[MINDCheckAddDate] [datetime] NULL,
	[MINDCheckEditDate] [datetime] NULL,
	[ForInspection] [bit] NOT NULL,
	[OneYardForWashing] [bit] NOT NULL,
	[UpdateActualWeightTime] [datetime] NULL,
	[UpdateLocationTime] [datetime] NULL,
	[ForInspectionTime] [datetime] NULL,
 CONSTRAINT [PK_TransferIn_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Seq1]  DEFAULT ('') FOR [Seq1]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Seq2]  DEFAULT ('') FOR [Seq2]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Roll]  DEFAULT ('') FOR [Roll]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Dyelot]  DEFAULT ('') FOR [Dyelot]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_StockType]  DEFAULT ('') FOR [StockType]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Location]  DEFAULT ('') FOR [Location]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Qty]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  DEFAULT ((0)) FOR [Weight]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  DEFAULT ((0)) FOR [ActualWeight]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  DEFAULT ((0)) FOR [SentToWMS]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Fabric2LabBy]  DEFAULT ('') FOR [Fabric2LabBy]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Checker]  DEFAULT ('') FOR [Checker]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  DEFAULT ('') FOR [ContainerCode]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_Tone]  DEFAULT ('') FOR [Tone]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_MINDQRCode]  DEFAULT ('') FOR [MINDQRCode]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_MINDChecker]  DEFAULT ('') FOR [MINDChecker]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_ForInspection]  DEFAULT ((0)) FOR [ForInspection]
GO

ALTER TABLE [dbo].[TransferIn_Detail] ADD  CONSTRAINT [DF_TransferIn_Detail_OneYardForWashing]  DEFAULT ((0)) FOR [OneYardForWashing]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉廠入單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'Location'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貨物實際重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'ActualWeight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫剪布給實驗室的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'Fabric2LabTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新倉庫剪布給實驗室時間的人員' , @level0type=N'SCHEMA' ,@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'Fabric2LabBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PH 收料時負責秤重 + 剪一小塊布 (ShadeBand) + 搬該物料入庫' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'Checker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鐵框號 ( 主要針對主料 )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'ContainerCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QRCode 首次列印的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'QRCode_PrintDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'T2 QR Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'MINDQRCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND收料人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'MINDChecker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND第一次收料時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'MINDCheckAddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND收料修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'MINDCheckEditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已送去檢驗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'ForInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已經裁剪1 碼給水洗房' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'OneYardForWashing'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新 Actual Weight 的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'UpdateActualWeightTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新 Location 的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'UpdateLocationTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送去檢驗的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail', @level2type=N'COLUMN',@level2name=N'ForInspectionTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉廠入明細' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferIn_Detail'
GO