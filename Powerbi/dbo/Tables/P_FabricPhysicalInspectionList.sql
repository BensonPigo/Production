CREATE TABLE [dbo].[P_FabricPhysicalInspectionList] (
    [FactoryID]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_FactoryID_New] DEFAULT ('') NOT NULL,
    [Category]               NVARCHAR (1000) CONSTRAINT [DF_P_FabricPhysicalInspectionList_Category_New] DEFAULT ('') NOT NULL,
    [Season]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Season_New] DEFAULT ('') NOT NULL,
    [SP]                     VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_SP_New] DEFAULT ('') NOT NULL,
    [SEQ]                    VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_SEQ_New] DEFAULT ('') NOT NULL,
    [WKNo]                   VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_WKNo_New] DEFAULT ('') NOT NULL,
    [Invoice]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Invoice_New] DEFAULT ('') NOT NULL,
    [ReceivingID]            VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_ReceivingID_New] DEFAULT ('') NOT NULL,
    [Style]                  VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Style_New] DEFAULT ('') NOT NULL,
    [Brand]                  VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Brand_New] DEFAULT ('') NOT NULL,
    [SupplierName]           VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_SupplierName_New] DEFAULT ('') NOT NULL,
    [Refno]                  VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Refno_New] DEFAULT ('') NOT NULL,
    [Color]                  VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Color_New] DEFAULT ('') NOT NULL,
    [CuttingDate]            DATE            NULL,
    [ArriveWHDate]           DATE            NULL,
    [ArriveQty]              NUMERIC (38, 2) CONSTRAINT [DF_P_FabricPhysicalInspectionList_ArriveQty_New] DEFAULT ((0)) NOT NULL,
    [WeaveType]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_WeaveType_New] DEFAULT ('') NOT NULL,
    [TotalRoll]              INT             CONSTRAINT [DF_P_FabricPhysicalInspectionList_TotalRoll_New] DEFAULT ((0)) NOT NULL,
    [TotalDyeLot]            INT             CONSTRAINT [DF_P_FabricPhysicalInspectionList_TotalDyeLot_New] DEFAULT ((0)) NOT NULL,
    [AlreadyInspectedDyelot] INT             CONSTRAINT [DF_P_FabricPhysicalInspectionList_AlreadyInspectedDyelot_New] DEFAULT ((0)) NOT NULL,
    [NotInspectedDyelot]     INT             CONSTRAINT [DF_P_FabricPhysicalInspectionList_NotInspectedDyelot_New] DEFAULT ((0)) NOT NULL,
    [NonInspection]          VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_NonInspection_New] DEFAULT ('') NOT NULL,
    [PhysicalInspection]     VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_PhysicalInspection_New] DEFAULT ('') NOT NULL,
    [PhysicalInspector]      VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_PhysicalInspector_New] DEFAULT ('') NOT NULL,
    [Approver]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Approver_New] DEFAULT ('') NOT NULL,
    [ApproveDate]            DATETIME        NULL,
    [Roll]                   VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Roll_New] DEFAULT ('') NOT NULL,
    [Dyelot]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Dyelot_New] DEFAULT ('') NOT NULL,
    [TicketYds]              NUMERIC (38, 2) CONSTRAINT [DF_P_FabricPhysicalInspectionList_TicketYds_New] DEFAULT ((0)) NOT NULL,
    [ActYdsInsdpected]       NUMERIC (38, 2) CONSTRAINT [DF_P_FabricPhysicalInspectionList_ActYdsInsdpected_New] DEFAULT ((0)) NOT NULL,
    [LthOfDiff]              NUMERIC (38, 2) CONSTRAINT [DF_P_FabricPhysicalInspectionList_LthOfDiff_New] DEFAULT ((0)) NOT NULL,
    [TransactionID]          VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_TransactionID_New] DEFAULT ('') NOT NULL,
    [CutWidth]               NUMERIC (38, 2) CONSTRAINT [DF_P_FabricPhysicalInspectionList_CutWidth_New] DEFAULT ((0)) NOT NULL,
    [FullWidth]              NUMERIC (38, 2) CONSTRAINT [DF_P_FabricPhysicalInspectionList_FullWidth_New] DEFAULT ((0)) NOT NULL,
    [ActualWidth]            NUMERIC (38, 2) CONSTRAINT [DF_P_FabricPhysicalInspectionList_ActualWidth_New] DEFAULT ((0)) NOT NULL,
    [TotalPoints]            NUMERIC (38)    CONSTRAINT [DF_P_FabricPhysicalInspectionList_TotalPoints_New] DEFAULT ((0)) NOT NULL,
    [PointRate]              NUMERIC (38, 2) CONSTRAINT [DF_P_FabricPhysicalInspectionList_PointRate_New] DEFAULT ((0)) NOT NULL,
    [Result]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Result_New] DEFAULT ('') NOT NULL,
    [Grade]                  VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Grade_New] DEFAULT ('') NOT NULL,
    [Moisture]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Moisture_New] DEFAULT ('') NOT NULL,
    [Remark]                 NVARCHAR (1000) CONSTRAINT [DF_P_FabricPhysicalInspectionList_Remark_New] DEFAULT ('') NOT NULL,
    [InspDate]               DATE            NULL,
    [Inspector]              NVARCHAR (1000) CONSTRAINT [DF_P_FabricPhysicalInspectionList_Inspector_New] DEFAULT ('') NOT NULL,
    [OrderType]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_OrderType_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]           DATETIME        NULL,
    [BIStatus]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricPhysicalInspectionList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricPhysicalInspectionList] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [SP] ASC, [SEQ] ASC, [ReceivingID] ASC, [Roll] ASC, [Dyelot] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'WKNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發票號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Invoice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收貨單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'SupplierName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁切日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'CuttingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達倉庫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'WeaveType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料捲數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TotalRoll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料缸數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TotalDyeLot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已驗缸數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'AlreadyInspectedDyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未驗缸數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'NotInspectedDyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需實體驗布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'NonInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Over All 實體驗布結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'PhysicalInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Over All 實體驗布人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'PhysicalInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Over All 實體驗布日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ApproveDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫收料長度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TicketYds'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際驗布長度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ActYdsInsdpected'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'長度差異' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'LthOfDiff'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易系統ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TransactionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'撐布寬度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'CutWidth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實寬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'FullWidth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際撐布寬度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ActualWidth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'疵點總分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TotalPoints'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'疵點分率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'PointRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布種等級' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Grade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布種含水度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Moisture'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'InspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布員工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Inspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricPhysicalInspectionList', @level2type = N'COLUMN', @level2name = N'BIStatus';

