CREATE TABLE [dbo].[P_IssueFabricByCuttingTransactionList]
(
	[IssueID] VARCHAR(13) NOT NULL, 
    [MDivisionID] VARCHAR(8) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_MDivisionID]     NOT NULL DEFAULT (('')), 
    [FactoryID] VARCHAR(8) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FactoryID]     NOT NULL DEFAULT (('')), 
    [CutplanID] VARCHAR(13) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_CutplanID]     NOT NULL DEFAULT (('')), 
    [EstCutDate] DATE NULL, 
    [IssueDate] DATE NULL, 
    [Line] VARCHAR(MAX) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Line]     NOT NULL DEFAULT (('')), 
    [CutCellID] VARCHAR(2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_CutCellID]     NOT NULL DEFAULT (('')), 
    [FabricComboAndCutNo] VARCHAR(MAX) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FabricComboAndCutNo]     NOT NULL DEFAULT (('')), 
    [IssueRemark] VARCHAR(300) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueRemark]     NOT NULL DEFAULT (('')), 
    [OrderID] VARCHAR(13) NOT NULL, 
    [Seq] VARCHAR(6) NOT NULL, 
    [Refno] VARCHAR(36) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Refno]     NOT NULL DEFAULT (('')), 
    [ColorID] VARCHAR(50) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_ColorID]     NOT NULL DEFAULT (('')), 
    [ColorName] VARCHAR(150) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_ColorName]     NOT NULL DEFAULT (('')), 
    [Description] NVARCHAR(MAX) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Description]     NOT NULL DEFAULT (('')), 
    [WeaveTypeID] VARCHAR(20) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_WeaveTypeID]     NOT NULL DEFAULT (('')), 
    [RelaxTime] NUMERIC(5, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_RelaxTime]     NOT NULL DEFAULT ((0)), 
    [Roll] VARCHAR(8) NOT NULL, 
    [Dyelot] VARCHAR(8) NOT NULL, 
    [StockType] CHAR NOT NULL, 
    [StockUnit] VARCHAR(8) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_StockUnit]     NOT NULL DEFAULT (('')), 
    [IssueQty] NUMERIC(11, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueQty]     NOT NULL DEFAULT ((0)), 
    [BulkLocation] VARCHAR(300) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_BulkLocation]     NOT NULL DEFAULT (('')), 
    [IssueCreateName] NVARCHAR(50) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueCreateName]     NOT NULL DEFAULT (('')), 
    [MINDReleaseName] NVARCHAR(50) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_MINDReleaseName]     NOT NULL DEFAULT (('')), 
    [IssueStartTime] DATETIME NULL, 
    [MINDReleaseDate] DATETIME NULL, 
    [PickingCompletion] NUMERIC(5, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_PickingCompletion]     NOT NULL DEFAULT ((0)), 
    [NeedUnroll] VARCHAR(2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_NeedUnroll]     NOT NULL DEFAULT (('')), 
    [UnrollScanName] NVARCHAR(50) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollScanName]     NOT NULL DEFAULT (('')), 
    [UnrollStartTime] DATETIME  NULL, 
    [UnrollEndTime] DATETIME  NULL, 
    [RelaxationStartTime] DATETIME NULL, 
    [RelaxationEndTime] DATETIME NULL, 
    [UnrollActualQty] NUMERIC(11, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollActualQty]     NOT NULL DEFAULT ((0)), 
    [UnrollRemark] VARCHAR(300) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollRemark]     NOT NULL DEFAULT (('')), 
    [UnrollingRelaxationCompletion] NUMERIC(5, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollingRelaxationCompletion]     NOT NULL DEFAULT ((0)), 
    [DispatchScanName] NVARCHAR(50) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_DispatchScanName]     NOT NULL DEFAULT (('')), 
    [DispatchScanTime] DATETIME NULL, 
    [RegisterTime] DATETIME NULL, 
    [DispatchTime] DATETIME NULL, 
    [FactoryReceivedName] NVARCHAR(50) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FactoryReceivedName]     NOT NULL DEFAULT (('')), 
    [FactoryReceivedTime] DATETIME NULL, 
    [AddDate] DATETIME NULL, 
    [EditDate] DATETIME NULL, 
    [Issue_DetailUkey] BIGINT NOT NULL, 
    [BIFactoryID] VARCHAR(8) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_BIFactoryID] NOT NULL DEFAULT (('')), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_IssueFabricByCuttingTransactionList] PRIMARY KEY ([Issue_DetailUkey])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料單ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'IssueID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裁簡計畫ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'CutplanID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'實際裁剪日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'EstCutDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'實際發料單日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'IssueDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'線別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'Line'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裁剪Cell的ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'CutCellID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'哪一身布及裁次',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'FabricComboAndCutNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料單備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'IssueRemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'OrderID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'Seq'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'料號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'Refno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'ColorID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'ColorName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'料的詳細說明',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布種類別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'WeaveTypeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'鬆布標準時長',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'RelaxTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布卷',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'Roll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缸號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'Dyelot'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉儲類別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'StockType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'庫存單位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'StockUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'IssueQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'儲位編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'BulkLocation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'建立發料單人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'IssueCreateName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MIND實際發料人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'MINDReleaseName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'開始發料的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'IssueStartTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MIND發料時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'MINDReleaseDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'揀料完成度',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'PickingCompletion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布是否需要攤開',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'NeedUnroll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M360 Unroll 掃描 Unroll Location 的使用者',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'UnrollScanName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉庫攤開布捲的開始時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'UnrollStartTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉庫攤開布捲的完成時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'UnrollEndTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉庫鬆布的開始時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'RelaxationStartTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉庫鬆布的完成時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'RelaxationEndTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M360 Unroll 階段實際收到的數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'UnrollActualQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M360 Unroll 階段備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'UnrollRemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布卷攤開及鬆布的完成度',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'UnrollingRelaxationCompletion'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M360 MIND Dispatch - 首次在 Register 清單中掃描的人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'DispatchScanName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M360 MIND Dispatch - 首次在 Register 清單中掃描的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'DispatchScanTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'登記至 Dispatch 清單的日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'RegisterTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布卷準備完成日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'DispatchTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠接收人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'FactoryReceivedName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠接收日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'FactoryReceivedTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料單產生時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料單編輯時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_IssueFabricByCuttingTransactionList',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'