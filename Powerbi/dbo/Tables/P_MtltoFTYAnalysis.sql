CREATE TABLE [dbo].[P_MtltoFTYAnalysis] (
    [Factory]          VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Factory_New] DEFAULT ('') NOT NULL,
    [Country]          VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Country_New] DEFAULT ('') NOT NULL,
    [Brand]            VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Brand_New] DEFAULT ('') NOT NULL,
    [WeaveType]        VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_WeaveType_New] DEFAULT ('') NOT NULL,
    [ETD]              DATE            NULL,
    [ETA]              DATE            NULL,
    [CloseDate]        DATE            NULL,
    [ActDate]          DATE            NULL,
    [Category]         NVARCHAR (1000) CONSTRAINT [DF_P_MtltoFTYAnalysis_Category_New] DEFAULT ('') NOT NULL,
    [OrderID]          VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_OrderID_New] DEFAULT ('') NOT NULL,
    [Seq1]             VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Seq1_New] DEFAULT ('') NOT NULL,
    [Seq2]             VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Seq2_New] DEFAULT ('') NOT NULL,
    [OrderCfmDate]     DATE            NULL,
    [SciDelivery]      DATE            NULL,
    [Refno]            VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Refno_New] DEFAULT ('') NOT NULL,
    [SCIRefno]         VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_SCIRefno_New] DEFAULT ('') NOT NULL,
    [SuppID]           VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_SuppID_New] DEFAULT ('') NOT NULL,
    [SuppName]         NVARCHAR (1000) CONSTRAINT [DF_P_MtltoFTYAnalysis_SuppName_New] DEFAULT ('') NOT NULL,
    [CurrencyID]       VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_CurrencyID_New] DEFAULT ('') NOT NULL,
    [CurrencyRate]     NUMERIC (38)    CONSTRAINT [DF_P_MtltoFTYAnalysis_CurrencyRate_New] DEFAULT ((0)) NOT NULL,
    [Price]            NUMERIC (38, 4) CONSTRAINT [DF_P_MtltoFTYAnalysis_Price_New] DEFAULT ((0)) NOT NULL,
    [Price(TWD)]       NUMERIC (38, 4) CONSTRAINT [DF_P_MtltoFTYAnalysis_Price(TWD)_New] DEFAULT ((0)) NOT NULL,
    [Unit]             VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Unit_New] DEFAULT ('') NOT NULL,
    [PoQty]            NUMERIC (38, 2) CONSTRAINT [DF_P_MtltoFTYAnalysis_PoQty_New] DEFAULT ((0)) NOT NULL,
    [PoFoc]            NUMERIC (38, 2) CONSTRAINT [DF_P_MtltoFTYAnalysis_PoFoc_New] DEFAULT ((0)) NOT NULL,
    [ShipQty]          NUMERIC (38, 2) CONSTRAINT [DF_P_MtltoFTYAnalysis_ShipQty_New] DEFAULT ((0)) NOT NULL,
    [ShipFoc]          NUMERIC (38, 2) CONSTRAINT [DF_P_MtltoFTYAnalysis_ShipFoc_New] DEFAULT ((0)) NOT NULL,
    [TTShipQty]        NUMERIC (38, 2) CONSTRAINT [DF_P_MtltoFTYAnalysis_TTShipQty_New] DEFAULT ((0)) NOT NULL,
    [ShipAmt(TWD)]     NUMERIC (38, 4) CONSTRAINT [DF_P_MtltoFTYAnalysis_ShipAmt(TWD)_New] DEFAULT ((0)) NOT NULL,
    [FabricJunk]       VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_FabricJunk_New] DEFAULT ('') NOT NULL,
    [WKID]             VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_WKID_New] DEFAULT ('') NOT NULL,
    [ShipmentTerm]     VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_ShipmentTerm_New] DEFAULT ('') NOT NULL,
    [FabricType]       VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_FabricType_New] DEFAULT ('') NOT NULL,
    [PINO]             VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_PINO_New] DEFAULT ('') NOT NULL,
    [PIDATE]           DATE            NULL,
    [Color]            VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Color_New] DEFAULT ('') NOT NULL,
    [ColorName]        NVARCHAR (1000) CONSTRAINT [DF_P_MtltoFTYAnalysis_ColorName_New] DEFAULT ('') NOT NULL,
    [Season]           VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Season_New] DEFAULT ('') NOT NULL,
    [PCHandle]         NVARCHAR (1000) CONSTRAINT [DF_P_MtltoFTYAnalysis_PCHandle_New] DEFAULT ('') NOT NULL,
    [POHandle]         NVARCHAR (1000) CONSTRAINT [DF_P_MtltoFTYAnalysis_POHandle_New] DEFAULT ('') NOT NULL,
    [POSMR]            NVARCHAR (1000) CONSTRAINT [DF_P_MtltoFTYAnalysis_POSMR_New] DEFAULT ('') NOT NULL,
    [Style]            VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_Style_New] DEFAULT ('') NOT NULL,
    [OrderType]        VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_OrderType_New] DEFAULT ('') NOT NULL,
    [ShipModeID]       VARCHAR (8000)  NOT NULL,
    [Supp1stCfmDate]   DATE            NULL,
    [BrandSuppCode]    VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_BrandSuppCode_New] DEFAULT ('') NOT NULL,
    [BrandSuppName]    NVARCHAR (1000) CONSTRAINT [DF_P_MtltoFTYAnalysis_BrandSuppName_New] DEFAULT ('') NOT NULL,
    [CountryofLoading] VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_CountryofLoading_New] DEFAULT ('') NOT NULL,
    [SupdelRvsd]       DATE            NULL,
    [ProdItem]         VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_ProdItem_New] DEFAULT ('') NOT NULL,
    [KPILETA]          DATE            NULL,
    [MaterialConfirm]  VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_MaterialConfirm_New] DEFAULT ('') NOT NULL,
    [SupplierGroup]    VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_SupplierGroup_New] DEFAULT ('') NOT NULL,
    [TransferBIDate]   DATE            NULL,
    [Ukey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [BIFactoryID]      VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]     DATETIME        NULL,
    [BIStatus]         VARCHAR (8000)  CONSTRAINT [DF_P_MtltoFTYAnalysis_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_MtltoFTYAnalysis] PRIMARY KEY CLUSTERED ([WKID] ASC, [OrderID] ASC, [Seq1] ASC, [Seq2] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裝船日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ETD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'結關日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'CloseDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達W/H日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ActDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Seq1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Seq2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'OrderCfmDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飛雁交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'SciDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'SuppName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'CurrencyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'與台幣匯率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'CurrencyRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Price'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單價(TWD)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Price(TWD)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Unit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'PoQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'PoFoc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口FOC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ShipFoc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ShipQty+ShipFoc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'TTShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運費(TWD)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ShipAmt(TWD)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料Junk與否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'FabricJunk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商的三聯式發票號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'PINO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收到廠商三聯式發票號碼的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'PIDATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ColorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌供應商代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'BrandSuppCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌供應商名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'BrandSuppName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RevisedETD' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'SupdelRvsd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產分類(用於Pull forward)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ProdItem'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_MtltoFTYAnalysis', @level2type = N'COLUMN', @level2name = N'BIStatus';

