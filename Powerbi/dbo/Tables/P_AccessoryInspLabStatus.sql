CREATE TABLE [dbo].[P_AccessoryInspLabStatus] (
    [POID]                    VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_POID_New] DEFAULT ('') NOT NULL,
    [SEQ]                     VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_SEQ_New] DEFAULT ('') NOT NULL,
    [Factory]                 VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_Factory_New] DEFAULT ('') NOT NULL,
    [BrandID]                 VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]                 VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_StyleID_New] DEFAULT ('') NOT NULL,
    [SeasonID]                VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_SeasonID_New] DEFAULT ('') NOT NULL,
    [ShipModeID]              VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_ShipModeID_New] DEFAULT ('') NOT NULL,
    [Wkno]                    VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_Wkno_New] DEFAULT ('') NOT NULL,
    [Invo]                    VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_Invo_New] DEFAULT ('') NOT NULL,
    [ArriveWHDate]            DATE            NULL,
    [ArriveQty]               NUMERIC (38, 2) CONSTRAINT [DF_P_AccessoryInspLabStatus_ArriveQty_New] DEFAULT ((0)) NOT NULL,
    [Inventory]               NUMERIC (38, 2) CONSTRAINT [DF_P_AccessoryInspLabStatus_Inventory_New] DEFAULT ((0)) NOT NULL,
    [Bulk]                    NUMERIC (38, 2) CONSTRAINT [DF_P_AccessoryInspLabStatus_Bulk_New] DEFAULT ((0)) NOT NULL,
    [BalanceQty]              NUMERIC (38, 2) CONSTRAINT [DF_P_AccessoryInspLabStatus_BalanceQty_New] DEFAULT ((0)) NOT NULL,
    [EarliestSCIDelivery]     DATE            NULL,
    [BuyerDelivery]           DATE            NULL,
    [RefNo]                   VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_RefNo_New] DEFAULT ('') NOT NULL,
    [Article]                 VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_Article_New] DEFAULT ('') NOT NULL,
    [MaterialType]            VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_MaterialType_New] DEFAULT ('') NOT NULL,
    [Color]                   VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_Color_New] DEFAULT ('') NOT NULL,
    [ColorName]               NVARCHAR (1000) CONSTRAINT [DF_P_AccessoryInspLabStatus_ColorName_New] DEFAULT ('') NOT NULL,
    [Size]                    VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_Size_New] DEFAULT ('') NOT NULL,
    [Unit]                    VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_Unit_New] DEFAULT ('') NOT NULL,
    [Supplier]                VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_Supplier_New] DEFAULT ('') NOT NULL,
    [OrderQty]                NUMERIC (38, 2) CONSTRAINT [DF_P_AccessoryInspLabStatus_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [InspectionResult]        VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_InspectionResult_New] DEFAULT ('') NOT NULL,
    [InspectedQty]            NUMERIC (38, 2) CONSTRAINT [DF_P_AccessoryInspLabStatus_InspectedQty_New] DEFAULT ((0)) NOT NULL,
    [RejectedQty]             NUMERIC (38, 2) CONSTRAINT [DF_P_AccessoryInspLabStatus_RejectedQty_New] DEFAULT ((0)) NOT NULL,
    [DefectType]              VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_DefectType_New] DEFAULT ('') NOT NULL,
    [InspectionDate]          DATE            NULL,
    [Inspector]               NVARCHAR (1000) CONSTRAINT [DF_P_AccessoryInspLabStatus_Inspector_New] DEFAULT ('') NOT NULL,
    [Remark]                  NVARCHAR (1000) CONSTRAINT [DF_P_AccessoryInspLabStatus_Remark_New] DEFAULT ('') NOT NULL,
    [NALaboratory]            VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_NALaboratory_New] DEFAULT ('') NOT NULL,
    [LaboratoryOverallResult] VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_LaboratoryOverallResult_New] DEFAULT ('') NOT NULL,
    [NAOvenTest]              VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_NAOvenTest_New] DEFAULT ('') NOT NULL,
    [OvenTestResult]          VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_OvenTestResult_New] DEFAULT ('') NOT NULL,
    [OvenScale]               VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_OvenScale_New] DEFAULT ('') NOT NULL,
    [OvenTestDate]            DATE            NULL,
    [NAWashTest]              VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_NAWashTest_New] DEFAULT ('') NOT NULL,
    [WashTestResult]          VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_WashTestResult_New] DEFAULT ('') NOT NULL,
    [WashScale]               VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_WashScale_New] DEFAULT ('') NOT NULL,
    [WashTestDate]            DATE            NULL,
    [ReceivingID]             VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_ReceivingID_New] DEFAULT ('') NOT NULL,
    [AddDate]                 DATETIME        NULL,
    [EditDate]                DATETIME        NULL,
    [CategoryType]            VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_CategoryType_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]             VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]            DATETIME        NULL,
    [BIStatus]                VARCHAR (8000)  CONSTRAINT [DF_P_AccessoryInspLabStatus_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_AccessoryInspLabStatus] PRIMARY KEY CLUSTERED ([POID] ASC, [SEQ] ASC, [ReceivingID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Factory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipping mode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'ShipModeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作底稿編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Wkno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發票號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Invo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料日或單據日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存實收數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InvStock' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Inventory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BulkStock' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Bulk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BalanceQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BalanceQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EarliestSCIDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'EarliestSCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyerDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Brand Refno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'RefNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Article' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'MaterialType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SpecValue' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SpecValue' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Size'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Unit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商代碼-英文簡稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Supplier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'InspectionResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'InspectedQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返修數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'RejectedQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有問題數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'DefectType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'烘箱結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'OvenTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'烘箱灰階' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'OvenScale'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'烘箱日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'OvenTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'不檢驗水洗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'NAWashTest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'WashTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗灰階' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'WashScale'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'WashTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Material Type的大類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'CategoryType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_AccessoryInspLabStatus', @level2type = N'COLUMN', @level2name = N'BIStatus';

