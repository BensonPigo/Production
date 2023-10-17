CREATE TABLE [dbo].[P_FabricInspLabSummaryReport]
(
	 [Category]                         nvarchar(100)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_Category]                        DEFAULT ((''))	NOT NULL
	,[POID]							    varchar(13)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_POID]                            DEFAULT ((''))	NOT NULL
	,[SEQ]							    varchar(6)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_SEQ]                             DEFAULT ((''))	NOT NULL
	,[FactoryID]					    varchar(8)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_FactoryID]                       DEFAULT ((''))	NOT NULL
	,[BrandID]						    varchar(8)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_BrandID]                         DEFAULT ((''))	NOT NULL
	,[StyleID]						    varchar(15)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_StyleID]                         DEFAULT ((''))	NOT NULL
	,[SeasonID]						    varchar(10)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_SeasonID]                        DEFAULT ((''))	NOT NULL
	,[Wkno]							    varchar(13)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_Wkno]                            DEFAULT ((''))	NOT NULL
	,[InvNo]						    varchar(25)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_InvNo]                           DEFAULT ((''))	NOT NULL
	,[CuttingDate]					    date			                    
	,[ArriveWHDate]					    date			                    
	,[ArriveQty]					    INT	CONSTRAINT [PK_P_FabricInspLabSummaryReport_ArriveQty]                       DEFAULT ((0))	NOT NULL
	,[Inventory]					    INT	CONSTRAINT [PK_P_FabricInspLabSummaryReport_Inventory]                       DEFAULT ((0))	NOT NULL
	,[Bulk]							    INT	CONSTRAINT [PK_P_FabricInspLabSummaryReport_Bulk]                            DEFAULT ((0))	NOT NULL
	,[BalanceQty]					    INT	CONSTRAINT [PK_P_FabricInspLabSummaryReport_BalanceQty]                      DEFAULT ((0))	NOT NULL
	,[TtlRollsCalculated]			    int				CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlRollsCalculated]              DEFAULT ((0))	NOT NULL
	,[BulkLocation]					    varchar(5000)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_BulkLocation]                    DEFAULT ((''))	NOT NULL
	,[FirstUpdateBulkLocationDate]	    datetime      
	,[InventoryLocation]			    varchar(5000)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_InventoryLocation]               DEFAULT ((''))	NOT NULL
	,[FirstUpdateStocksLocationDate]    DATETIME          
	,[EarliestSCIDelivery]			    date          
	,[BuyerDelivery]				    date          
	,[Refno]						    varchar(36)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_Refno]                           DEFAULT ((''))	NOT NULL
	,[Description]					    nvarchar(150)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_Description]                     DEFAULT ((''))	NOT NULL
	,[Color]						    varchar(50)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_Color]                           DEFAULT ((''))	NOT NULL
	,[ColorName]					    nvarchar(150)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorName]                       DEFAULT ((''))	NOT NULL
	,[SupplierCode]					    varchar(6)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_SupplierCode]                    DEFAULT ((''))	NOT NULL
	,[SupplierName]					    varchar(12)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_SupplierName]                    DEFAULT ((''))	NOT NULL
	,[WeaveType]					    varchar(20)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeaveType]                       DEFAULT ((''))	NOT NULL
	,[NAPhyscial]					    VARCHAR				CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAPhyscial]                      DEFAULT ((0))	NOT NULL				
	,[InspectionOverallResult]		    varchar(16)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_InspectionOverallResult]         DEFAULT ((''))	NOT NULL
	,[PhyscialInspResult]			    varchar(10)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhyscialInspResult]              DEFAULT ((''))	NOT NULL
	,[TtlYrdsUnderBCGrade]			    numeric(12, 2)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlYrdsUnderBCGrade]             DEFAULT ((0))	NOT NULL
	,[TtlPointsUnderBCGrade]		    numeric(9, 0)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlPointsUnderBCGrade]           DEFAULT ((0))	NOT NULL
	,[TtlPointsUnderAGrade]			    numeric(9, 0)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlPointsUnderAGrade]            DEFAULT ((0))	NOT NULL
	,[PhyscialInspector]			    nvarchar(30)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhyscialInspector]               DEFAULT ((''))	NOT NULL
	,[PhysicalInspDate]				    datetime      
	,[ActTtlYdsInspection]			    numeric(10, 2)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_ActTtlYdsInspection]             DEFAULT ((0))	NOT NULL
	,[InspectionPCT]				    numeric(6, 1)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_InspectionPCT]                   DEFAULT ((0))	NOT NULL
	,[PhysicalInspDefectPoint]		    int				CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhysicalInspDefectPoint]         DEFAULT ((0))	NOT NULL
	,[CustInspNumber]				    varchar(20)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_CustInspNumber]                  DEFAULT ((''))	NOT NULL
	,[WeightTestResult]				    varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeightTestResult]                DEFAULT ((''))	NOT NULL
	,[WeightTestInspector]			    nvarchar(30)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeightTestInspector]             DEFAULT ((''))	NOT NULL
	,[WeightTestDate]				    datetime      
	,[CutShadebandQtyByRoll]		    int				CONSTRAINT [PK_P_FabricInspLabSummaryReport_CutShadebandQtyByRoll]           DEFAULT ((0))	NOT NULL
	,[CutShadebandPCT]				    numeric(5, 2)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_CutShadebandPCT]                 DEFAULT ((0))	NOT NULL
	,[ShadeBondResult]				    varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_ShadeBondResult]                 DEFAULT ((''))	NOT NULL
	,[ShadeBondInspector]			    nvarchar(30)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_ShadeBondInspector]              DEFAULT ((''))	NOT NULL
	,[ShadeBondDate]				    datetime      
	,[NoOfRollShadebandPass]		    int				CONSTRAINT [PK_P_FabricInspLabSummaryReport_NoOfRollShadebandPass]           DEFAULT ((0))	NOT NULL
	,[NoOfRollShadebandFail]		    int				CONSTRAINT [PK_P_FabricInspLabSummaryReport_NoOfRollShadebandFail]           DEFAULT ((0))	NOT NULL
	,[ContinuityResult]				    varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_[ContinuityResult]               DEFAULT ((''))	NOT NULL
	,[ContinuityInspector]			    nvarchar(30)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_ContinuityInspector]             DEFAULT ((''))	NOT NULL
	,[ContinuityDate]				    datetime      
	,[OdorResult]					    varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_OdorResult]                      DEFAULT ((''))	NOT NULL
	,[OdorInspector]				    nvarchar(30)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_[OdorInspector]                  DEFAULT ((''))	NOT NULL
	,[OdorDate]						    datetime     
	,[MoistureResult]				    varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_MoistureResult]                  DEFAULT ((''))	NOT NULL
	,[MoistureDate]					    date         
	,[CrockingShrinkageOverAllResult]   varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingShrinkageOverAllResult]  DEFAULT ((''))	NOT NULL
	,[NACrocking]					    varchar(1)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_NACrocking]                      DEFAULT ((''))	NOT NULL
	,[CrockingResult]				    varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingResult]                  DEFAULT ((''))	NOT NULL
	,[CrockingInspector]			    nvarchar(30)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingInspector]               DEFAULT ((''))	NOT NULL
	,[CrockingTestDate]				    date         
	,[NAHeatShrinkage]				    varchar(1)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAHeatShrinkage]                 DEFAULT ((''))	NOT NULL
	,[HeatShrinkageTestResult]		    varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_HeatShrinkageTestResult]         DEFAULT ((''))	NOT NULL
	,[HeatShrinkageInspector]		    nvarchar(30)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_HeatShrinkageInspector]          DEFAULT ((''))	NOT NULL
	,[HeatShrinkageTestDate]		    date          
	,[NAWashShrinkage]				    varchar(1)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAWashShrinkage]                 DEFAULT ((''))	NOT NULL
	,[WashShrinkageTestResult]		    varchar(5)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_WashShrinkageTestResult]         DEFAULT ((''))	NOT NULL
	,[WashShrinkageInspector]		    nvarchar(30)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_WashShrinkageInspector]          DEFAULT ((''))	NOT NULL
	,[WashShrinkageTestDate]		    date         
	,[OvenTestResult]				    varchar(10)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_OvenTestResult]                  DEFAULT ((''))	NOT NULL
	,[OvenTestInspector]			    nvarchar(100)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_OvenTestInspector]               DEFAULT ((''))	NOT NULL
	,[ColorFastnessResult]			    varchar(10)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorFastnessResult]             DEFAULT ((''))	NOT NULL
	,[ColorFastnessInspector]		    nvarchar(100)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorFastnessInspector]          DEFAULT ((''))	NOT NULL
	,[LocalMR]						    nvarchar(100)	CONSTRAINT [PK_P_FabricInspLabSummaryReport_LocalMR]                         DEFAULT ((''))	NOT NULL
	,[OrderType]					    varchar(20)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_OrderType]                       DEFAULT ((''))	NOT NULL
	,[ReceivingID]					    varchar(13)		CONSTRAINT [PK_P_FabricInspLabSummaryReport_ReceivingID]                     DEFAULT ((''))	NOT NULL
	,[AddDate]						    datetime 
	,[EditDate]						    datetime, 
    CONSTRAINT [PK_P_FabricInspLabSummaryReport] PRIMARY KEY ([POID], [SEQ], [FactoryID],[ReceivingID])
)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CategoryName',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'採購單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項-小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'SEQ'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'款式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'StyleID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'季節',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'SeasonID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Wkno',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'Wkno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Invoice',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'InvNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裁剪上線日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'CuttingDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Arrive W/H Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ArriveWHDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Arrive Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ArriveQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Inventory Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'Inventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Bulk Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'Bulk'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Balance Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'BalanceQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Total Rolls Calculated',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'TtlRollsCalculated'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'A倉儲位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'BulkLocation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'FirstUpdateBulkLocationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'B倉儲位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'InventoryLocation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1st Update Stocks Location Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'FirstUpdateStocksLocationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Earliest SCI Delivery',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'EarliestSCIDelivery'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Buyer Delivery',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'BuyerDelivery'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠商Refno',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'Refno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fabric描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'Color'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ColorName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠商代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'SupplierCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'英文簡稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'SupplierName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'織法',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'WeaveType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否檢驗',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'NAPhyscial'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'InspectionOverallResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布瑕疵點檢驗Result',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'PhyscialInspResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'GradeB&C帳面登記總碼長',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'TtlYrdsUnderBCGrade'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'GradeB&C總瑕疵點數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'TtlPointsUnderBCGrade'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'GradeA總瑕疵點數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'TtlPointsUnderAGrade'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Physical Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'PhyscialInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布瑕疵點檢驗日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'PhysicalInspDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'實際檢驗碼長',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ActTtlYdsInspection'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Inspection%',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'InspectionPCT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總瑕疵點數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'PhysicalInspDefectPoint'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'客人檢驗系統中的檢驗報告單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'CustInspNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'重量檢驗Result',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'WeightTestResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Weight Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'WeightTestInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'重量檢驗日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'WeightTestDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Cut Shadeband Qty(Roll)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'CutShadebandQtyByRoll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'% Cut Shadeband',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'CutShadebandPCT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ShadeBond  Result',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ShadeBondResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Shadebone Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ShadeBondInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ShadeBond Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ShadeBondDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'No. of roll Shade band Pass',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'NoOfRollShadebandPass'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'No. of roll Shade band Fail',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'NoOfRollShadebandFail'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'漸進色Result',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ContinuityResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Continuity Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ContinuityInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'漸進色日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ContinuityDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'氣味Result',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'OdorResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Odor Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'OdorInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'氣味檢驗日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'OdorDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'濕度檢測結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'MoistureResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'濕度檢測的日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'MoistureDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'CrockingShrinkageOverAllResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'免測試色脫落',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'NACrocking'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色脫落結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'CrockingResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Crocking Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'CrockingInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色脫落測試 日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'CrockingTestDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'N/A Heat Shrinkage',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'NAHeatShrinkage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'熱壓縮律結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'HeatShrinkageTestResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Heat Shrinkage Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'HeatShrinkageInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'熱縮律測試日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'HeatShrinkageTestDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'N/A Wash Shrinkage',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'NAWashShrinkage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Wash Shrinkage Test Result',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'WashShrinkageTestResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Wash Shrinkage Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'WashShrinkageInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'水洗縮律測試日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'WashShrinkageTestDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Oven Test Result',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'OvenTestResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Oven Test Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'OvenTestInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Color Fastness Result',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ColorFastnessResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Color Fastness Inspector',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ColorFastnessInspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Local MR',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'LocalMR'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'分類細項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'OrderType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'收料單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'ReceivingID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'AddDate',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'EditDate',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspLabSummaryReport',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'