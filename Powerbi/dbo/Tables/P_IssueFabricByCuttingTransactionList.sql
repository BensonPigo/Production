CREATE TABLE [dbo].[P_IssueFabricByCuttingTransactionList] (
    [IssueID]                       VARCHAR (8000)  NOT NULL,
    [MDivisionID]                   VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]                     VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FactoryID_New] DEFAULT ('') NOT NULL,
    [CutplanID]                     VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_CutplanID_New] DEFAULT ('') NOT NULL,
    [EstCutDate]                    DATE            NULL,
    [IssueDate]                     DATE            NULL,
    [Line]                          VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Line_New] DEFAULT ('') NOT NULL,
    [CutCellID]                     VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_CutCellID_New] DEFAULT ('') NOT NULL,
    [FabricComboAndCutNo]           VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FabricComboAndCutNo_New] DEFAULT ('') NOT NULL,
    [IssueRemark]                   VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueRemark_New] DEFAULT ('') NOT NULL,
    [OrderID]                       VARCHAR (8000)  NOT NULL,
    [Seq]                           VARCHAR (8000)  NOT NULL,
    [Refno]                         VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Refno_New] DEFAULT ('') NOT NULL,
    [ColorID]                       VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_ColorID_New] DEFAULT ('') NOT NULL,
    [Description]                   NVARCHAR (MAX)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Description_New] DEFAULT ('') NOT NULL,
    [WeaveTypeID]                   VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_WeaveTypeID_New] DEFAULT ('') NOT NULL,
    [RelaxTime]                     NUMERIC (38, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_RelaxTime_New] DEFAULT ((0)) NOT NULL,
    [Roll]                          VARCHAR (8000)  NOT NULL,
    [Dyelot]                        VARCHAR (8000)  NOT NULL,
    [StockType]                     CHAR (1)        NOT NULL,
    [StockUnit]                     VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_StockUnit_New] DEFAULT ('') NOT NULL,
    [IssueQty]                      NUMERIC (38, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueQty_New] DEFAULT ((0)) NOT NULL,
    [BulkLocation]                  VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_BulkLocation_New] DEFAULT ('') NOT NULL,
    [IssueCreateName]               NVARCHAR (1000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_IssueCreateName_New] DEFAULT ('') NOT NULL,
    [MINDReleaseName]               NVARCHAR (1000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_MINDReleaseName_New] DEFAULT ('') NOT NULL,
    [IssueStartTime]                DATETIME        NULL,
    [MINDReleaseDate]               DATETIME        NULL,
    [PickingCompletion]             NUMERIC (38, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_PickingCompletion_New] DEFAULT ((0)) NOT NULL,
    [NeedUnroll]                    VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_NeedUnroll_New] DEFAULT ('') NOT NULL,
    [UnrollScanName]                NVARCHAR (1000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollScanName_New] DEFAULT ('') NOT NULL,
    [UnrollStartTime]               DATETIME        NULL,
    [UnrollEndTime]                 DATETIME        NULL,
    [RelaxationStartTime]           DATETIME        NULL,
    [RelaxationEndTime]             DATETIME        NULL,
    [UnrollActualQty]               NUMERIC (38, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollActualQty_New] DEFAULT ((0)) NOT NULL,
    [UnrollRemark]                  VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollRemark_New] DEFAULT ('') NOT NULL,
    [UnrollingRelaxationCompletion] NUMERIC (38, 2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollingRelaxationCompletion_New] DEFAULT ((0)) NOT NULL,
    [DispatchScanName]              NVARCHAR (1000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_DispatchScanName_New] DEFAULT ('') NOT NULL,
    [DispatchScanTime]              DATETIME        NULL,
    [RegisterTime]                  DATETIME        NULL,
    [DispatchTime]                  DATETIME        NULL,
    [FactoryReceivedName]           NVARCHAR (1000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FactoryReceivedName_New] DEFAULT ('') NOT NULL,
    [FactoryReceivedTime]           DATETIME        NULL,
    [AddDate]                       DATETIME        NULL,
    [EditDate]                      DATETIME        NULL,
    [Issue_DetailUkey]              BIGINT          NOT NULL,
    [ColorName]                     VARCHAR (8000)  CONSTRAINT [DF__P_IssueFa__Color__760336FF_New] DEFAULT ('') NOT NULL,
    [Style]                         VARCHAR (8000)  CONSTRAINT [DF__P_IssueFa__Style__76F75B38_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]                   VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                  DATETIME        NULL,
    [BIStatus]                      VARCHAR (8000)  CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_BIStatus_New] DEFAULT (N'New') NULL,
	[RequestCons]					NUMERIC(38,2) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_RequestCons] DEFAULT(0) NOT NULL,
	[UnrollMachine]					VARCHAR(8000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_UnrollMachine] DEFAULT('')  NOT NULL,
	[Rack]							VARCHAR(8000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_Rack] DEFAULT('') NOT NULL,
	[FabricRegisterBy]				VARCHAR(8000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_FabricRegisterBy] DEFAULT('') NOT NULL,
	[DispatchReason]				VARCHAR(8000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_DispatchReason] DEFAULT('') NOT NULL,
	[DispatchRemark]				VARCHAR(8000) CONSTRAINT [DF_P_IssueFabricByCuttingTransactionList_DispatchRemark] DEFAULT('') NOT NULL,
    CONSTRAINT [PK_P_IssueFabricByCuttingTransactionList] PRIMARY KEY CLUSTERED ([Issue_DetailUkey] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cons需求量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'RequestCons'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unroll 機器' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'UnrollMachine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'架子' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'Rack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fabric Register人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'FabricRegisterBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dispatch 原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'DispatchReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dispatch 備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_IssueFabricByCuttingTransactionList', @level2type=N'COLUMN',@level2name=N'DispatchRemark'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_IssueFabricByCuttingTransactionList', @level2type = N'COLUMN', @level2name = N'BIStatus';

