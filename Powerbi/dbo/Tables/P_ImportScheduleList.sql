CREATE TABLE [dbo].[P_ImportScheduleList] (
    [WK]                   VARCHAR (13)    CONSTRAINT [DF_P_ImportScheduleList_WK] DEFAULT ('') NOT NULL,
    [ExportDetailUkey]     BIGINT          CONSTRAINT [DF_P_ImportScheduleList_ExportDetailUkey] DEFAULT ((0)) NOT NULL,
    [ETA]                  DATE            CONSTRAINT [DF_P_ImportScheduleList_ETA] DEFAULT (NULL) NULL,
    [MDivisionID]          VARCHAR (8)     CONSTRAINT [DF_P_ImportScheduleList_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]            VARCHAR (8)     CONSTRAINT [DF_P_ImportScheduleList_FactoryID] DEFAULT ('') NOT NULL,
    [Consignee]            VARCHAR (8)     CONSTRAINT [DF_P_ImportScheduleList_Consignee] DEFAULT ('') NOT NULL,
    [ShipModeID]           VARCHAR (10)    CONSTRAINT [DF_P_ImportScheduleList_ShipModeID] DEFAULT ('') NOT NULL,
    [CYCFS]                VARCHAR (6)     CONSTRAINT [DF_P_ImportScheduleList_CYCFS] DEFAULT ('') NOT NULL,
    [Blno]                 VARCHAR (20)    CONSTRAINT [DF_P_ImportScheduleList_Blno] DEFAULT ('') NOT NULL,
    [Packages]             NUMERIC (5)     CONSTRAINT [DF_P_ImportScheduleList_Packages] DEFAULT ((0)) NOT NULL,
    [Vessel]               NVARCHAR (60)   CONSTRAINT [DF_P_ImportScheduleList_Vessel] DEFAULT ('') NOT NULL,
    [ProdFactory]          VARCHAR (8)     CONSTRAINT [DF_P_ImportScheduleList_ProdFactory] DEFAULT ('') NOT NULL,
    [OrderTypeID]          VARCHAR (20)    CONSTRAINT [DF_P_ImportScheduleList_OrderTypeID] DEFAULT ('') NOT NULL,
    [ProjectID]            VARCHAR (5)     CONSTRAINT [DF_P_ImportScheduleList_ProjectID] DEFAULT ('') NOT NULL,
    [Category]             VARCHAR (10)    CONSTRAINT [DF_P_ImportScheduleList_Category] DEFAULT ('') NOT NULL,
    [BrandID]              VARCHAR (8)     CONSTRAINT [DF_P_ImportScheduleList_BrandID] DEFAULT ('') NOT NULL,
    [SeasonID]             VARCHAR (10)    CONSTRAINT [DF_P_ImportScheduleList_SeasonID] DEFAULT ('') NOT NULL,
    [StyleID]              VARCHAR (15)    CONSTRAINT [DF_P_ImportScheduleList_StyleID] DEFAULT ('') NOT NULL,
    [StyleName]            NVARCHAR (50)   CONSTRAINT [DF_P_ImportScheduleList_StyleName] DEFAULT ('') NOT NULL,
    [PoID]                 VARCHAR (13)    CONSTRAINT [DF_P_ImportScheduleList_PoID] DEFAULT ('') NOT NULL,
    [Seq]                  VARCHAR (6)     CONSTRAINT [DF_P_ImportScheduleList_Seq] DEFAULT ('') NOT NULL,
    [Refno]                VARCHAR (36)    CONSTRAINT [DF_P_ImportScheduleList_Refno] DEFAULT ('') NOT NULL,
    [Color]                VARCHAR (50)    CONSTRAINT [DF_P_ImportScheduleList_Color] DEFAULT ('') NOT NULL,
    [ColorName]            VARCHAR (300)   CONSTRAINT [DF_P_ImportScheduleList_ColorName] DEFAULT ('') NOT NULL,
    [Description]          VARCHAR (3000)  CONSTRAINT [DF_P_ImportScheduleList_Description] DEFAULT ('') NOT NULL,
    [MtlType]              VARCHAR (10)    CONSTRAINT [DF_P_ImportScheduleList_MtlType] DEFAULT ('') NOT NULL,
    [SubMtlType]           VARCHAR (20)    CONSTRAINT [DF_P_ImportScheduleList_SubMtlType] DEFAULT ('') NOT NULL,
    [WeaveType]            VARCHAR (20)    CONSTRAINT [DF_P_ImportScheduleList_WeaveType] DEFAULT ('') NOT NULL,
    [SuppID]               VARCHAR (6)     CONSTRAINT [DF_P_ImportScheduleList_SuppID] DEFAULT ('') NOT NULL,
    [SuppName]             VARCHAR (12)    CONSTRAINT [DF_P_ImportScheduleList_SuppName] DEFAULT ('') NOT NULL,
    [UnitID]               VARCHAR (8)     CONSTRAINT [DF_P_ImportScheduleList_UnitID] DEFAULT ('') NOT NULL,
    [SizeSpec]             VARCHAR (50)    CONSTRAINT [DF_P_ImportScheduleList_SizeSpec] DEFAULT ('') NOT NULL,
    [ShipQty]              NUMERIC (12, 2) CONSTRAINT [DF_P_ImportScheduleList_ShipQty] DEFAULT ((0)) NOT NULL,
    [FOC]                  NUMERIC (12, 2) CONSTRAINT [DF_P_ImportScheduleList_FOC] DEFAULT ((0)) NOT NULL,
    [NetKg]                NUMERIC (10, 2) CONSTRAINT [DF_P_ImportScheduleList_NetKg] DEFAULT ((0)) NOT NULL,
    [WeightKg]             NUMERIC (10, 2) CONSTRAINT [DF_P_ImportScheduleList_WeightKg] DEFAULT ((0)) NOT NULL,
    [ArriveQty]            NUMERIC (12, 2) CONSTRAINT [DF_P_ImportScheduleList_ArriveQty] DEFAULT ((0)) NOT NULL,
    [ArriveQtyStockUnit]   NUMERIC (12, 2) CONSTRAINT [DF_P_ImportScheduleList_ArriveQtyStockUnit] DEFAULT ((0)) NOT NULL,
    [FirstBulkSewInLine]   DATE            CONSTRAINT [DF_P_ImportScheduleList_FirstBulkSewInLine] DEFAULT (NULL) NULL,
    [FirstCutDate]         DATE            CONSTRAINT [DF_P_ImportScheduleList_FirstCutDate] DEFAULT (NULL) NULL,
    [ReceiveQty]           NUMERIC (11, 2) CONSTRAINT [DF_P_ImportScheduleList_ReceiveQty] DEFAULT ((0)) NOT NULL,
    [TotalRollsCalculated] INT             CONSTRAINT [DF_P_ImportScheduleList_TotalRollsCalculated] DEFAULT ((0)) NOT NULL,
    [StockUnit]            VARCHAR (8)     CONSTRAINT [DF_P_ImportScheduleList_StockUnit] DEFAULT ('') NOT NULL,
    [MCHandle]             VARCHAR (100)   CONSTRAINT [DF_P_ImportScheduleList_MCHandle] DEFAULT ('') NOT NULL,
    [ContainerType]        VARCHAR (255)   CONSTRAINT [DF_P_ImportScheduleList_ContainerType] DEFAULT ('') NOT NULL,
    [ContainerNo]          VARCHAR (255)   CONSTRAINT [DF_P_ImportScheduleList_ContainerNo] DEFAULT ('') NOT NULL,
    [PortArrival]          DATE            CONSTRAINT [DF_P_ImportScheduleList_PortArrival] DEFAULT (NULL) NULL,
    [WhseArrival]          DATE            CONSTRAINT [DF_P_ImportScheduleList_WhseArrival] DEFAULT (NULL) NULL,
    [KPILETA]              DATE            CONSTRAINT [DF_P_ImportScheduleList_KPILETA] DEFAULT (NULL) NULL,
    [PFETA]                DATE            CONSTRAINT [DF_P_ImportScheduleList_PFETA] DEFAULT (NULL) NULL,
    [EarliestSCIDelivery]  DATE            CONSTRAINT [DF_P_ImportScheduleList_EarliestSCIDelivery] DEFAULT (NULL) NULL,
    [EarlyDays]            INT             CONSTRAINT [DF_P_ImportScheduleList_EarlyDays] DEFAULT ((0)) NOT NULL,
    [EarliestPFETA]        INT             CONSTRAINT [DF_P_ImportScheduleList_EarliestPFETA] DEFAULT ((0)) NOT NULL,
    [MRMail]               VARCHAR (100)   CONSTRAINT [DF_P_ImportScheduleList_MRMail] DEFAULT ('') NOT NULL,
    [SMRMail]              VARCHAR (100)   CONSTRAINT [DF_P_ImportScheduleList_SMRMail] DEFAULT ('') NOT NULL,
    [EditName]             VARCHAR (10)    CONSTRAINT [DF_P_ImportScheduleList_EditName] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME        CONSTRAINT [DF_P_ImportScheduleList_AddDate] DEFAULT (NULL) NULL,
    [EditDate]             DATETIME        CONSTRAINT [DF_P_ImportScheduleList_EditDate] DEFAULT (NULL) NULL,
    CONSTRAINT [PK_P_ImportScheduleList] PRIMARY KEY CLUSTERED ([ExportDetailUkey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Export的編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Export的新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Export的編輯人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMR的Mail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'SMRMail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MR的Mail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'MRMail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達W/H日期-P/F ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'EarliestPFETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達W/H日期-KPI L/ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'EarlyDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最早SCI交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'EarliestSCIDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'P/F ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'PFETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KPI L/ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'KPILETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達W/H日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'WhseArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到港日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'PortArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨櫃編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ContainerNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨櫃類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ContainerType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MC Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'MCHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'StockUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總卷數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'TotalRollsCalculated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存實收數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ReceiveQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最初裁剪日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'FirstCutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最初上產線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'FirstBulkSewInLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ArriveQtyStockUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次出口數量+本次出口FOC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ArriveQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'WeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'NetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次出口FOC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'FOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次出口數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'SizeSpec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商英文簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'SuppName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'織法類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'WeaveType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料細項類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'SubMtlType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'MtlType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'詳細介紹', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ColorName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Color';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'PoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'StyleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'專案代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ProjectID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單分類細項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'OrderTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ProdFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Vessel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總件/箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Packages';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Blno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'整櫃或散櫃', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'CYCFS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'運送類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'Consignee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠代', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計到達日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨明細Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'ExportDetailUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WK No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ImportScheduleList', @level2type = N'COLUMN', @level2name = N'WK';

