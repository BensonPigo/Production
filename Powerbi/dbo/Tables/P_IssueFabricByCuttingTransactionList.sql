CREATE TABLE [dbo].[P_IssueFabricByCuttingTransactionList](
	[IssueID] [varchar](13) NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[CutplanID] [varchar](13) NOT NULL,
	[EstCutDate] [date] NULL,
	[IssueDate] [date] NULL,
	[Line] [varchar](max) NOT NULL,
	[CutCellID] [varchar](2) NOT NULL,
	[FabricComboAndCutNo] [varchar](max) NOT NULL,
	[IssueRemark] [varchar](300) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[Seq] [varchar](6) NOT NULL,
	[Refno] [varchar](36) NOT NULL,
	[ColorID] [varchar](50) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[WeaveTypeID] [varchar](20) NOT NULL,
	[RelaxTime] [numeric](5, 2) NOT NULL,
	[Roll] [varchar](8) NOT NULL,
	[Dyelot] [varchar](8) NOT NULL,
	[StockType] [char](1) NOT NULL,
	[StockUnit] [varchar](8) NOT NULL,
	[IssueQty] [numeric](11, 2) NOT NULL,
	[BulkLocation] [varchar](300) NOT NULL,
	[IssueCreateName] [nvarchar](50) NOT NULL,
	[MINDReleaseName] [nvarchar](50) NOT NULL,
	[IssueStartTime] [datetime] NULL,
	[MINDReleaseDate] [datetime] NULL,
	[PickingCompletion] [numeric](5, 2) NOT NULL,
	[NeedUnroll] [varchar](2) NOT NULL,
	[UnrollScanName] [nvarchar](50) NOT NULL,
	[UnrollStartTime] [datetime] NULL,
	[UnrollEndTime] [datetime] NULL,
	[RelaxationStartTime] [datetime] NULL,
	[RelaxationEndTime] [datetime] NULL,
	[UnrollActualQty] [numeric](11, 2) NOT NULL,
	[UnrollRemark] [varchar](300) NOT NULL,
	[UnrollingRelaxationCompletion] [numeric](5, 2) NOT NULL,
	[DispatchScanName] [nvarchar](50) NOT NULL,
	[DispatchScanTime] [datetime] NULL,
	[RegisterTime] [datetime] NULL,
	[DispatchTime] [datetime] NULL,
	[FactoryReceivedName] [nvarchar](50) NOT NULL,
	[FactoryReceivedTime] [datetime] NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
	[Issue_DetailUkey] [bigint] NOT NULL,
	[ColorName] [varchar](150) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_IssueFabricByCuttingTransactionList] PRIMARY KEY CLUSTERED 
(
	[Issue_DetailUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_CutplanID]  DEFAULT ('') FOR [CutplanID]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Line]  DEFAULT ('') FOR [Line]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_CutCellID]  DEFAULT ('') FOR [CutCellID]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FabricComboAndCutNo]  DEFAULT ('') FOR [FabricComboAndCutNo]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueRemark]  DEFAULT ('') FOR [IssueRemark]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Refno]  DEFAULT ('') FOR [Refno]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_ColorID]  DEFAULT ('') FOR [ColorID]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_WeaveTypeID]  DEFAULT ('') FOR [WeaveTypeID]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_RelaxTime]  DEFAULT ((0)) FOR [RelaxTime]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_StockUnit]  DEFAULT ('') FOR [StockUnit]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueQty]  DEFAULT ((0)) FOR [IssueQty]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_BulkLocation]  DEFAULT ('') FOR [BulkLocation]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueCreateName]  DEFAULT ('') FOR [IssueCreateName]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_MINDReleaseName]  DEFAULT ('') FOR [MINDReleaseName]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_PickingCompletion]  DEFAULT ((0)) FOR [PickingCompletion]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_NeedUnroll]  DEFAULT ('') FOR [NeedUnroll]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollScanName]  DEFAULT ('') FOR [UnrollScanName]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollActualQty]  DEFAULT ((0)) FOR [UnrollActualQty]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollRemark]  DEFAULT ('') FOR [UnrollRemark]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollingRelaxationCompletion]  DEFAULT ((0)) FOR [UnrollingRelaxationCompletion]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_DispatchScanName]  DEFAULT ('') FOR [DispatchScanName]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FactoryReceivedName]  DEFAULT ('') FOR [FactoryReceivedName]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  DEFAULT ('') FOR [ColorName]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_IssueFabricByCuttingTransactionList] ADD  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發料單ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'IssueID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁簡計畫ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'CutplanID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'EstCutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際發料單日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'IssueDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'線別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪Cell的ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'CutCellID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'哪一身布及裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'FabricComboAndCutNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發料單備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'IssueRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'ColorID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料的詳細說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布種類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'WeaveTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'鬆布標準時長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'RelaxTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布卷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉儲類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'StockUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發料數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'IssueQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'BulkLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立發料單人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'IssueCreateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND實際發料人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'MINDReleaseName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'開始發料的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'IssueStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND發料時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'MINDReleaseDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'揀料完成度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'PickingCompletion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布是否需要攤開' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'NeedUnroll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 Unroll 掃描 Unroll Location 的使用者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'UnrollScanName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫攤開布捲的開始時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'UnrollStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫攤開布捲的完成時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'UnrollEndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫鬆布的開始時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'RelaxationStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫鬆布的完成時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'RelaxationEndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 Unroll 階段實際收到的數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'UnrollActualQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 Unroll 階段備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'UnrollRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布卷攤開及鬆布的完成度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'UnrollingRelaxationCompletion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 MIND Dispatch - 首次在 Register 清單中掃描的人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'DispatchScanName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 MIND Dispatch - 首次在 Register 清單中掃描的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'DispatchScanTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登記至 Dispatch 清單的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'RegisterTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布卷準備完成日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'DispatchTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠接收人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'FactoryReceivedName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠接收日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'FactoryReceivedTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發料單產生時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發料單編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO