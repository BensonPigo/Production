CREATE TABLE [dbo].[P_InventoryStockListReport] (
    [MDivisionID]       VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]         VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_FactoryID_New] DEFAULT ('') NOT NULL,
    [SewLine]           VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_SewLine_New] DEFAULT ('') NOT NULL,
    [POID]              VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_POID_New] DEFAULT ('') NOT NULL,
    [Category]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_Category_New] DEFAULT ('') NOT NULL,
    [OrderTypeID]       VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_OrderTypeID_New] DEFAULT ('') NOT NULL,
    [WeaveTypeID]       VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_WeaveTypeID_New] DEFAULT ('') NOT NULL,
    [BuyerDelivery]     DATE            NULL,
    [OrigBuyerDelivery] DATE            NULL,
    [MaterialComplete]  VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_MaterialComplete_New] DEFAULT ('') NOT NULL,
    [ETA]               DATE            NULL,
    [ArriveWHDate]      VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_ArriveWHDate_New] DEFAULT ('') NOT NULL,
    [ExportID]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_ExportID_New] DEFAULT ('') NOT NULL,
    [Packages]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_Packages_New] DEFAULT ('') NOT NULL,
    [ContainerNo]       VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_ContainerNo_New] DEFAULT ('') NOT NULL,
    [BrandID]           VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]           VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_StyleID_New] DEFAULT ('') NOT NULL,
    [SeasonID]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_SeasonID_New] DEFAULT ('') NOT NULL,
    [ProjectID]         VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_ProjectID_New] DEFAULT ('') NOT NULL,
    [ProgramID]         NVARCHAR (1000) CONSTRAINT [DF_P_InventoryStockListReport_ProgramID_New] DEFAULT ('') NOT NULL,
    [SEQ1]              VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_SEQ1_New] DEFAULT ('') NOT NULL,
    [SEQ2]              VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_SEQ2_New] DEFAULT ('') NOT NULL,
    [MaterialType]      VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_MaterialType_New] DEFAULT ('') NOT NULL,
    [StockPOID]         VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_StockPOID_New] DEFAULT ('') NOT NULL,
    [StockSeq1]         VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_StockSeq1_New] DEFAULT ('') NOT NULL,
    [StockSeq2]         VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_StockSeq2_New] DEFAULT ('') NOT NULL,
    [Refno]             VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_Refno_New] DEFAULT ('') NOT NULL,
    [SCIRefno]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_SCIRefno_New] DEFAULT ('') NOT NULL,
    [Description]       NVARCHAR (1000) CONSTRAINT [DF_P_InventoryStockListReport_Description_New] DEFAULT ('') NOT NULL,
    [ColorID]           VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_ColorID_New] DEFAULT ('') NOT NULL,
    [ColorName]         NVARCHAR (1000) CONSTRAINT [DF_P_InventoryStockListReport_ColorName_New] DEFAULT ('') NOT NULL,
    [Size]              VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_Size_New] DEFAULT ('') NOT NULL,
    [StockUnit]         VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_StockUnit_New] DEFAULT ('') NOT NULL,
    [PurchaseQty]       NUMERIC (38, 2) CONSTRAINT [DF_P_InventoryStockListReport_PurchaseQty_New] DEFAULT ((0)) NULL,
    [OrderQty]          INT             CONSTRAINT [DF_P_InventoryStockListReport_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [ShipQty]           NUMERIC (38, 2) CONSTRAINT [DF_P_InventoryStockListReport_ShipQty_New] DEFAULT ((0)) NULL,
    [Roll]              VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_Roll_New] DEFAULT ('') NOT NULL,
    [Dyelot]            VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_Dyelot_New] DEFAULT ('') NOT NULL,
    [StockType]         VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_StockType_New] DEFAULT ('') NOT NULL,
    [InQty]             NUMERIC (38, 2) CONSTRAINT [DF_P_InventoryStockListReport_InQty_New] DEFAULT ((0)) NOT NULL,
    [OutQty]            NUMERIC (38, 2) CONSTRAINT [DF_P_InventoryStockListReport_OutQty_New] DEFAULT ((0)) NOT NULL,
    [AdjustQty]         NUMERIC (38, 2) CONSTRAINT [DF_P_InventoryStockListReport_AdjustQty_New] DEFAULT ((0)) NOT NULL,
    [ReturnQty]         NUMERIC (38, 2) CONSTRAINT [DF_P_InventoryStockListReport_ReturnQty_New] DEFAULT ((0)) NOT NULL,
    [BalanceQty]        NUMERIC (38, 2) CONSTRAINT [DF_P_InventoryStockListReport_BalanceQty_New] DEFAULT ((0)) NOT NULL,
    [MtlLocationID]     VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_MtlLocationID_New] DEFAULT ('') NOT NULL,
    [MCHandle]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_MCHandle_New] DEFAULT ('') NOT NULL,
    [POHandle]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_POHandle_New] DEFAULT ('') NOT NULL,
    [POSMR]             VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_POSMR_New] DEFAULT ('') NOT NULL,
    [Supplier]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_Supplier_New] DEFAULT ('') NOT NULL,
    [VID]               VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_VID_New] DEFAULT ('') NOT NULL,
    [AddDate]           DATETIME        NULL,
    [EditDate]          DATETIME        NULL,
    [Grade]             VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_Grade_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]      DATETIME        NULL,
    [BIStatus]          VARCHAR (8000)  CONSTRAINT [DF_P_InventoryStockListReport_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_InventoryStockListReport] PRIMARY KEY CLUSTERED ([POID] ASC, [SEQ1] ASC, [SEQ2] ASC, [Roll] ASC, [Dyelot] ASC, [StockType] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manufacturing Division ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SewLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'OrderTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'WeaveTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orig. buyer delivery date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'OrigBuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'結清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MaterialComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫收料日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ExportID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總件/箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Packages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貨櫃種類+貨櫃編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ContainerNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'專案代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ProgramID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SEQ1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SEQ2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料型態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MaterialType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用庫存SP#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockPOID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用庫存SEQ1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockSeq1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用庫存SEQ2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockSeq2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飛雁料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SCIRefno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ColorID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ColorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Size'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'PurchaseQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'InQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發出量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'OutQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'調整量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'AdjustQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退回數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ReturnQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料量+發出量-調整量+退回數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BalanceQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MtlLocationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'POHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購主管' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'POSMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商ID+Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Supplier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'含VID標的PO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'VID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_InventoryStockListReport', @level2type = N'COLUMN', @level2name = N'BIStatus';

