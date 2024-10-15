CREATE TABLE [dbo].[Fabric_UnrollandRelax](
	[Barcode] [varchar](255) NOT NULL,
	[POID] [varchar](13) NULL,
	[Seq1] [varchar](3) NULL,
	[Seq2] [varchar](2) NULL,
	[Roll] [varchar](8) NULL,
	[Dyelot] [varchar](8) NULL,
	[StockType] [char](1) NULL,
	[UnrollStatus] [varchar](10) NULL,
	[UnrollStartTime] [datetime] NULL,
	[UnrollEndTime] [datetime] NULL,
	[RelaxationStartTime] [datetime] NULL,
	[RelaxationEndTime] [datetime] NULL,
	[UnrollScanner] [varchar](10) NULL,
	[UnrollActualQty] [numeric](11, 2) NULL,
	[UnrollRemark] [nvarchar](100) NULL,
	[IsAdvance] [bit] NOT NULL,
	[MachineIoTUkey] [bigint] NULL,
 CONSTRAINT [PK_Fabric_UnrollandRelax] PRIMARY KEY CLUSTERED 
(
	[Barcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [Seq1]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [Seq2]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [Roll]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [Dyelot]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [StockType]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [UnrollStatus]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [UnrollScanner]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ((0)) FOR [UnrollActualQty]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  DEFAULT ('') FOR [UnrollRemark]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  CONSTRAINT [DF_Fabric_UnrollandRelax_IsAdvance]  DEFAULT ((0)) FOR [IsAdvance]
GO

ALTER TABLE [dbo].[Fabric_UnrollandRelax] ADD  CONSTRAINT [DF_Fabric_UnrollandRelax_MachineIoTUkey]  DEFAULT ((0)) FOR [MachineIoTUkey]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料條碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'Barcode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鬆布時對應的 POID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鬆布時對應的 Seq1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'Seq1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鬆布時對應的 Seq2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'Seq2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鬆布時對應的 Roll' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鬆布時對應的 Dyelot' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鬆布時對應的 StockType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫攤開布捲狀態，可能值包括 Ongoing 或 Done' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'UnrollStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫攤開布捲 開始時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'UnrollStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫攤開布捲 完成時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'UnrollEndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫鬆布 開始時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'RelaxationStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫鬆布 完成時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'RelaxationEndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 Unroll 掃描 Unroll Location 的使用者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'UnrollScanner'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 Unroll 階段實際收到的數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'UnrollActualQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 Unroll 階段備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'UnrollRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'判斷是否為提前鬆布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'IsAdvance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布卷攤開的機台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Fabric_UnrollandRelax', @level2type=N'COLUMN',@level2name=N'MachineIoTUkey'
GO