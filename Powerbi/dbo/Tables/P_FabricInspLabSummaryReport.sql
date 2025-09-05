CREATE TABLE [dbo].[P_FabricInspLabSummaryReport] (
    [Category]                          NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_Category_New] DEFAULT ('') NOT NULL,
    [POID]                              VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_POID_New] DEFAULT ('') NOT NULL,
    [SEQ]                               VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_SEQ_New] DEFAULT ('') NOT NULL,
    [FactoryID]                         VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_FactoryID_New] DEFAULT ('') NOT NULL,
    [BrandID]                           VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]                           VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_StyleID_New] DEFAULT ('') NOT NULL,
    [SeasonID]                          VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_SeasonID_New] DEFAULT ('') NOT NULL,
    [Wkno]                              VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Wkno_New] DEFAULT ('') NOT NULL,
    [InvNo]                             VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_InvNo_New] DEFAULT ('') NOT NULL,
    [CuttingDate]                       DATE            NULL,
    [ArriveWHDate]                      DATE            NULL,
    [ArriveQty]                         INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_ArriveQty_New] DEFAULT ((0)) NOT NULL,
    [Inventory]                         INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_Inventory_New] DEFAULT ((0)) NOT NULL,
    [Bulk]                              INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_Bulk_New] DEFAULT ((0)) NOT NULL,
    [BalanceQty]                        INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_BalanceQty_New] DEFAULT ((0)) NOT NULL,
    [TtlRollsCalculated]                INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlRollsCalculated_New] DEFAULT ((0)) NOT NULL,
    [BulkLocation]                      VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_BulkLocation_New] DEFAULT ('') NOT NULL,
    [FirstUpdateBulkLocationDate]       DATETIME        NULL,
    [InventoryLocation]                 VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_InventoryLocation_New] DEFAULT ('') NOT NULL,
    [FirstUpdateStocksLocationDate]     DATETIME        NULL,
    [EarliestSCIDelivery]               DATE            NULL,
    [BuyerDelivery]                     DATE            NULL,
    [Refno]                             VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Refno_New] DEFAULT ('') NOT NULL,
    [Description]                       NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_Description_New] DEFAULT ('') NOT NULL,
    [Color]                             VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Color_New] DEFAULT ('') NOT NULL,
    [ColorName]                         NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorName_New] DEFAULT ('') NOT NULL,
    [SupplierCode]                      VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_SupplierCode_New] DEFAULT ('') NOT NULL,
    [SupplierName]                      VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_SupplierName_New] DEFAULT ('') NOT NULL,
    [WeaveType]                         VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeaveType_New] DEFAULT ('') NOT NULL,
    [NAPhysical]                        VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAPhysical_New] DEFAULT ('') NOT NULL,
    [InspectionOverallResult]           VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_InspectionOverallResult_New] DEFAULT ('') NOT NULL,
    [PhysicalInspResult]                VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhysicalInspResult_New] DEFAULT ('') NOT NULL,
    [TtlYrdsUnderBCGrade]               NUMERIC (38, 2) CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlYrdsUnderBCGrade_New] DEFAULT ((0)) NOT NULL,
    [TtlPointsUnderBCGrade]             NUMERIC (38)    CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlPointsUnderBCGrade_New] DEFAULT ((0)) NOT NULL,
    [TtlPointsUnderAGrade]              NUMERIC (38)    CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlPointsUnderAGrade_New] DEFAULT ((0)) NOT NULL,
    [PhysicalInspector]                 NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhysicalInspector_New] DEFAULT ('') NOT NULL,
    [PhysicalInspDate]                  DATETIME        NULL,
    [ActTtlYdsInspection]               NUMERIC (38, 2) CONSTRAINT [PK_P_FabricInspLabSummaryReport_ActTtlYdsInspection_New] DEFAULT ((0)) NOT NULL,
    [InspectionPCT]                     NUMERIC (38, 1) CONSTRAINT [PK_P_FabricInspLabSummaryReport_InspectionPCT_New] DEFAULT ((0)) NOT NULL,
    [PhysicalInspDefectPoint]           INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhysicalInspDefectPoint_New] DEFAULT ((0)) NOT NULL,
    [CustInspNumber]                    VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CustInspNumber_New] DEFAULT ('') NOT NULL,
    [WeightTestResult]                  VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeightTestResult_New] DEFAULT ('') NOT NULL,
    [WeightTestInspector]               NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeightTestInspector_New] DEFAULT ('') NOT NULL,
    [WeightTestDate]                    DATETIME        NULL,
    [CutShadebandQtyByRoll]             INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_CutShadebandQtyByRoll_New] DEFAULT ((0)) NOT NULL,
    [CutShadebandPCT]                   NUMERIC (38, 2) CONSTRAINT [PK_P_FabricInspLabSummaryReport_CutShadebandPCT_New] DEFAULT ((0)) NOT NULL,
    [ShadeBondResult]                   VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ShadeBondResult_New] DEFAULT ('') NOT NULL,
    [ShadeBondInspector]                NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_ShadeBondInspector_New] DEFAULT ('') NOT NULL,
    [ShadeBondDate]                     DATETIME        NULL,
    [NoOfRollShadebandPass]             INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_NoOfRollShadebandPass_New] DEFAULT ((0)) NOT NULL,
    [NoOfRollShadebandFail]             INT             CONSTRAINT [PK_P_FabricInspLabSummaryReport_NoOfRollShadebandFail_New] DEFAULT ((0)) NOT NULL,
    [ContinuityResult]                  VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_[ContinuityResult_New] DEFAULT ('') NOT NULL,
    [ContinuityInspector]               NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_ContinuityInspector_New] DEFAULT ('') NOT NULL,
    [ContinuityDate]                    DATETIME        NULL,
    [OdorResult]                        VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_OdorResult_New] DEFAULT ('') NOT NULL,
    [OdorInspector]                     NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_[OdorInspector_New] DEFAULT ('') NOT NULL,
    [OdorDate]                          DATETIME        NULL,
    [MoistureResult]                    VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_MoistureResult_New] DEFAULT ('') NOT NULL,
    [MoistureDate]                      DATE            NULL,
    [CrockingShrinkageOverAllResult]    VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingShrinkageOverAllResult_New] DEFAULT ('') NOT NULL,
    [NACrocking]                        VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NACrocking_New] DEFAULT ('') NOT NULL,
    [CrockingResult]                    VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingResult_New] DEFAULT ('') NOT NULL,
    [CrockingInspector]                 NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingInspector_New] DEFAULT ('') NOT NULL,
    [CrockingTestDate]                  DATE            NULL,
    [NAHeatShrinkage]                   VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAHeatShrinkage_New] DEFAULT ('') NOT NULL,
    [HeatShrinkageTestResult]           VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_HeatShrinkageTestResult_New] DEFAULT ('') NOT NULL,
    [HeatShrinkageInspector]            NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_HeatShrinkageInspector_New] DEFAULT ('') NOT NULL,
    [HeatShrinkageTestDate]             DATE            NULL,
    [NAWashShrinkage]                   VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAWashShrinkage_New] DEFAULT ('') NOT NULL,
    [WashShrinkageTestResult]           VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_WashShrinkageTestResult_New] DEFAULT ('') NOT NULL,
    [WashShrinkageInspector]            NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_WashShrinkageInspector_New] DEFAULT ('') NOT NULL,
    [WashShrinkageTestDate]             DATE            NULL,
    [OvenTestResult]                    VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_OvenTestResult_New] DEFAULT ('') NOT NULL,
    [OvenTestInspector]                 NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_OvenTestInspector_New] DEFAULT ('') NOT NULL,
    [ColorFastnessResult]               VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorFastnessResult_New] DEFAULT ('') NOT NULL,
    [ColorFastnessInspector]            NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorFastnessInspector_New] DEFAULT ('') NOT NULL,
    [LocalMR]                           NVARCHAR (1000) CONSTRAINT [PK_P_FabricInspLabSummaryReport_LocalMR_New] DEFAULT ('') NOT NULL,
    [OrderType]                         VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_OrderType_New] DEFAULT ('') NOT NULL,
    [ReceivingID]                       VARCHAR (8000)  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ReceivingID_New] DEFAULT ('') NOT NULL,
    [AddDate]                           DATETIME        NULL,
    [EditDate]                          DATETIME        NULL,
    [StockType]                         VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_StockType_New] DEFAULT ('') NOT NULL,
    [TotalYardageForInspection]         NUMERIC (38, 2) CONSTRAINT [DF_P_FabricInspLabSummaryReport_TotalYardageForInspection_New] DEFAULT ((0)) NOT NULL,
    [ActualRemainingYardsForInspection] NUMERIC (38, 2) CONSTRAINT [DF_P_FabricInspLabSummaryReport_ActualRemainingYardsForInspection_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]                       VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                      DATETIME        NULL,
    [KPILETA]                           DATE            NULL,
    [ACTETA]                            DATE            NULL,
    [Packages]                          INT             CONSTRAINT [DF_P_FabricInspLabSummaryReport_Packages_New] DEFAULT ((0)) NOT NULL,
    [SampleRcvDate]                     DATE            NULL,
    [InspectionGroup]                   VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_InspectionGroup_New] DEFAULT ('') NOT NULL,
    [CGradeTOP3Defects]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_CGradeTOP3Defects_New] DEFAULT ('') NOT NULL,
    [AGradeTOP3Defects]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_AGradeTOP3Defects_New] DEFAULT ('') NOT NULL,
    [ActTotalRollInspection]            INT             CONSTRAINT [DF_P_FabricInspLabSummaryReport_ActTotalRollInspection_New] DEFAULT ((0)) NOT NULL,
    [TotalLotNumber]                    INT             CONSTRAINT [DF_P_FabricInspLabSummaryReport_TotalLotNumber_New] DEFAULT ((0)) NOT NULL,
    [InspectedLotNumber]                INT             CONSTRAINT [DF_P_FabricInspLabSummaryReport_InspectedLotNumber_New] DEFAULT ((0)) NOT NULL,
    [CutShadebandTime]                  DATETIME        NULL,
    [OvenTestDate]                      VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_OvenTestDate_New] DEFAULT ('') NOT NULL,
    [ColorFastnessTestDate]             VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_ColorFastnessTestDate_New] DEFAULT ('') NOT NULL,
    [MCHandle]                          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_MCHandle_New] DEFAULT ('') NOT NULL,
    [OrderQty]                          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [Complete]                          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_Complete_New] DEFAULT ('') NOT NULL,
    [BIStatus]                          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspLabSummaryReport_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricInspLabSummaryReport] PRIMARY KEY CLUSTERED ([POID] ASC, [SEQ] ASC, [FactoryID] ASC, [StockType] ASC, [ReceivingID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CategoryName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項-小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Wkno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Wkno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Invoice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InvNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CuttingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Arrive W/H Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Arrive Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inventory Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Inventory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bulk Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Bulk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Balance Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BalanceQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Rolls Calculated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TtlRollsCalculated'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A倉儲位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BulkLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'FirstUpdateBulkLocationDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'B倉儲位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InventoryLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1st Update Stocks Location Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'FirstUpdateStocksLocationDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Earliest SCI Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'EarliestSCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Buyer Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商Refno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fabric描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ColorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SupplierCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'英文簡稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SupplierName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WeaveType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否檢驗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NAPhysical'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InspectionOverallResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布瑕疵點檢驗Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'PhysicalInspResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeB&C帳面登記總碼長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TtlYrdsUnderBCGrade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeB&C總瑕疵點數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TtlPointsUnderBCGrade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeA總瑕疵點數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TtlPointsUnderAGrade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Physical Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'PhysicalInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布瑕疵點檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'PhysicalInspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際檢驗碼長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ActTtlYdsInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inspection%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InspectionPCT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總瑕疵點數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'PhysicalInspDefectPoint'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人檢驗系統中的檢驗報告單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CustInspNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重量檢驗Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WeightTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Weight Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WeightTestInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重量檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WeightTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cut Shadeband Qty(Roll)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CutShadebandQtyByRoll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'% Cut Shadeband' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CutShadebandPCT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ShadeBond  Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ShadeBondResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shadebone Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ShadeBondInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ShadeBond Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ShadeBondDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'No. of roll Shade band Pass' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NoOfRollShadebandPass'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'No. of roll Shade band Fail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NoOfRollShadebandFail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'漸進色Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ContinuityResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Continuity Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ContinuityInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'漸進色日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ContinuityDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'氣味Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OdorResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Odor Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OdorInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'氣味檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OdorDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'濕度檢測結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'MoistureResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'濕度檢測的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'MoistureDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CrockingShrinkageOverAllResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免測試色脫落' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NACrocking'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色脫落結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CrockingResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Crocking Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CrockingInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色脫落測試 日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CrockingTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'N/A Heat Shrinkage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NAHeatShrinkage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'熱壓縮律結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'HeatShrinkageTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Heat Shrinkage Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'HeatShrinkageInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'熱縮律測試日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'HeatShrinkageTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'N/A Wash Shrinkage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NAWashShrinkage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Wash Shrinkage Test Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WashShrinkageTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Wash Shrinkage Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WashShrinkageInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗縮律測試日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WashShrinkageTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Oven Test Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OvenTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Oven Test Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OvenTestInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Color Fastness Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ColorFastnessResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Color Fastness Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ColorFastnessInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Local MR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'LocalMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Stock Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI LETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'KPILETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ACT ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ACTETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裝箱數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Packages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'試驗室收驗Sample日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SampleRcvDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InspectionGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeC前三瑕疵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CGradeTOP3Defects'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeA前三瑕疵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'AGradeTOP3Defects'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際總檢驗卷數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ActTotalRollInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總染缸數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TotalLotNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗的染缸數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InspectedLotNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁切色差布的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CutShadebandTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實驗室烘箱測試時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OvenTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實驗室烘箱測試時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ColorFastnessTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MCHandle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Complete' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Complete'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricInspLabSummaryReport', @level2type = N'COLUMN', @level2name = N'BIStatus';

