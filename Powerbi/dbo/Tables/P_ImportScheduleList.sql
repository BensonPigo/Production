CREATE TABLE [dbo].[P_ImportScheduleList] (
    [WK]                   VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_WK_New] DEFAULT ('') NOT NULL,
    [ExportDetailUkey]     BIGINT          CONSTRAINT [DF_P_ImportScheduleList_ExportDetailUkey_New] DEFAULT ((0)) NOT NULL,
    [ETA]                  DATE            CONSTRAINT [DF_P_ImportScheduleList_ETA_New] DEFAULT (NULL) NULL,
    [MDivisionID]          VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_FactoryID_New] DEFAULT ('') NOT NULL,
    [Consignee]            VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_Consignee_New] DEFAULT ('') NOT NULL,
    [ShipModeID]           VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_ShipModeID_New] DEFAULT ('') NOT NULL,
    [CYCFS]                VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_CYCFS_New] DEFAULT ('') NOT NULL,
    [Blno]                 VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_Blno_New] DEFAULT ('') NOT NULL,
    [Packages]             NUMERIC (38)    CONSTRAINT [DF_P_ImportScheduleList_Packages_New] DEFAULT ((0)) NOT NULL,
    [Vessel]               NVARCHAR (1000) CONSTRAINT [DF_P_ImportScheduleList_Vessel_New] DEFAULT ('') NOT NULL,
    [ProdFactory]          VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_ProdFactory_New] DEFAULT ('') NOT NULL,
    [OrderTypeID]          VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_OrderTypeID_New] DEFAULT ('') NOT NULL,
    [ProjectID]            VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_ProjectID_New] DEFAULT ('') NOT NULL,
    [Category]             VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_Category_New] DEFAULT ('') NOT NULL,
    [BrandID]              VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_BrandID_New] DEFAULT ('') NOT NULL,
    [SeasonID]             VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_SeasonID_New] DEFAULT ('') NOT NULL,
    [StyleID]              VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_StyleID_New] DEFAULT ('') NOT NULL,
    [StyleName]            NVARCHAR (1000) CONSTRAINT [DF_P_ImportScheduleList_StyleName_New] DEFAULT ('') NOT NULL,
    [PoID]                 VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_PoID_New] DEFAULT ('') NOT NULL,
    [Seq]                  VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_Seq_New] DEFAULT ('') NOT NULL,
    [Refno]                VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_Refno_New] DEFAULT ('') NOT NULL,
    [Color]                VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_Color_New] DEFAULT ('') NOT NULL,
    [ColorName]            VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_ColorName_New] DEFAULT ('') NOT NULL,
    [Description]          VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_Description_New] DEFAULT ('') NOT NULL,
    [MtlType]              VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_MtlType_New] DEFAULT ('') NOT NULL,
    [SubMtlType]           VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_SubMtlType_New] DEFAULT ('') NOT NULL,
    [WeaveType]            VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_WeaveType_New] DEFAULT ('') NOT NULL,
    [SuppID]               VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_SuppID_New] DEFAULT ('') NOT NULL,
    [SuppName]             VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_SuppName_New] DEFAULT ('') NOT NULL,
    [UnitID]               VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_UnitID_New] DEFAULT ('') NOT NULL,
    [SizeSpec]             VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_SizeSpec_New] DEFAULT ('') NOT NULL,
    [ShipQty]              NUMERIC (38, 2) CONSTRAINT [DF_P_ImportScheduleList_ShipQty_New] DEFAULT ((0)) NOT NULL,
    [FOC]                  NUMERIC (38, 2) CONSTRAINT [DF_P_ImportScheduleList_FOC_New] DEFAULT ((0)) NOT NULL,
    [NetKg]                NUMERIC (38, 2) CONSTRAINT [DF_P_ImportScheduleList_NetKg_New] DEFAULT ((0)) NOT NULL,
    [WeightKg]             NUMERIC (38, 2) CONSTRAINT [DF_P_ImportScheduleList_WeightKg_New] DEFAULT ((0)) NOT NULL,
    [ArriveQty]            NUMERIC (38, 2) CONSTRAINT [DF_P_ImportScheduleList_ArriveQty_New] DEFAULT ((0)) NOT NULL,
    [ArriveQtyStockUnit]   NUMERIC (38, 2) CONSTRAINT [DF_P_ImportScheduleList_ArriveQtyStockUnit_New] DEFAULT ((0)) NOT NULL,
    [FirstBulkSewInLine]   DATE            CONSTRAINT [DF_P_ImportScheduleList_FirstBulkSewInLine_New] DEFAULT (NULL) NULL,
    [FirstCutDate]         DATE            CONSTRAINT [DF_P_ImportScheduleList_FirstCutDate_New] DEFAULT (NULL) NULL,
    [ReceiveQty]           NUMERIC (38, 2) CONSTRAINT [DF_P_ImportScheduleList_ReceiveQty_New] DEFAULT ((0)) NOT NULL,
    [TotalRollsCalculated] INT             CONSTRAINT [DF_P_ImportScheduleList_TotalRollsCalculated_New] DEFAULT ((0)) NOT NULL,
    [StockUnit]            VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_StockUnit_New] DEFAULT ('') NOT NULL,
    [MCHandle]             VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_MCHandle_New] DEFAULT ('') NOT NULL,
    [ContainerType]        VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_ContainerType_New] DEFAULT ('') NOT NULL,
    [ContainerNo]          VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_ContainerNo_New] DEFAULT ('') NOT NULL,
    [PortArrival]          DATE            CONSTRAINT [DF_P_ImportScheduleList_PortArrival_New] DEFAULT (NULL) NULL,
    [WhseArrival]          DATE            CONSTRAINT [DF_P_ImportScheduleList_WhseArrival_New] DEFAULT (NULL) NULL,
    [KPILETA]              DATE            CONSTRAINT [DF_P_ImportScheduleList_KPILETA_New] DEFAULT (NULL) NULL,
    [PFETA]                DATE            CONSTRAINT [DF_P_ImportScheduleList_PFETA_New] DEFAULT (NULL) NULL,
    [EarliestSCIDelivery]  DATE            CONSTRAINT [DF_P_ImportScheduleList_EarliestSCIDelivery_New] DEFAULT (NULL) NULL,
    [EarlyDays]            INT             CONSTRAINT [DF_P_ImportScheduleList_EarlyDays_New] DEFAULT ((0)) NOT NULL,
    [EarliestPFETA]        INT             CONSTRAINT [DF_P_ImportScheduleList_EarliestPFETA_New] DEFAULT ((0)) NOT NULL,
    [MRMail]               VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_MRMail_New] DEFAULT ('') NOT NULL,
    [SMRMail]              VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_SMRMail_New] DEFAULT ('') NOT NULL,
    [EditName]             VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_EditName_New] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME        CONSTRAINT [DF_P_ImportScheduleList_AddDate_New] DEFAULT (NULL) NULL,
    [EditDate]             DATETIME        CONSTRAINT [DF_P_ImportScheduleList_EditDate_New] DEFAULT (NULL) NULL,
    [FabricCombo]          VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_FabricCombo_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_ImportScheduleList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ImportScheduleList] PRIMARY KEY CLUSTERED ([ExportDetailUkey] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'WK'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨明細Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ExportDetailUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收件人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Consignee'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運送類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ShipModeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'整櫃或散櫃' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'CYCFS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Blno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總件/箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Packages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'船名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Vessel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ProdFactory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'OrderTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'專案代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'StyleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'PoID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ColorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'詳細介紹' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'MtlType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料細項類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SubMtlType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'WeaveType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商英文簡稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SuppName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'UnitID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SizeSpec'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口FOC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FOC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'淨重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'NetKg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'毛重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'WeightKg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口數量+本次出口FOC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ArriveQtyStockUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最初上產線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FirstBulkSewInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最初裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FirstCutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存實收數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ReceiveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總卷數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'TotalRollsCalculated'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'StockUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MC Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貨櫃類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ContainerType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貨櫃編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ContainerNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到港日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'PortArrival'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達W/H日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'WhseArrival'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI L/ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'KPILETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P/F ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'PFETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最早SCI交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EarliestSCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達W/H日期-KPI L/ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EarlyDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達W/H日期-P/F ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EarliestPFETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MR的Mail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'MRMail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMR的Mail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SMRMail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Export的編輯人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Export的新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Export的編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EachCons.FabricCombo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FabricCombo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'BIStatus';

