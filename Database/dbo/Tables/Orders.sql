CREATE TABLE [dbo].[Orders] (
    [ID]                     VARCHAR (13)    CONSTRAINT [DF_Orders_ID] DEFAULT ('') NOT NULL,
    [BrandID]                VARCHAR (8)     CONSTRAINT [DF_Orders_BrandID] DEFAULT ('') NULL,
    [ProgramID]              VARCHAR (12)    CONSTRAINT [DF_Orders_ProgramID] DEFAULT ('') NULL,
    [StyleID]                VARCHAR (15)    CONSTRAINT [DF_Orders_StyleID] DEFAULT ('') NULL,
    [SeasonID]               VARCHAR (10)    CONSTRAINT [DF_Orders_SeasonID] DEFAULT ('') NULL,
    [ProjectID]              VARCHAR (5)     CONSTRAINT [DF_Orders_ProjectID] DEFAULT ('') NULL,
    [Category]               VARCHAR (1)     CONSTRAINT [DF_Orders_Category] DEFAULT ('') NULL,
    [OrderTypeID]            VARCHAR (20)    CONSTRAINT [DF_Orders_OrderTypeID] DEFAULT ('') NULL,
    [BuyMonth]               VARCHAR (16)    CONSTRAINT [DF_Orders_BuyMonth] DEFAULT ('') NULL,
    [Dest]                   VARCHAR (2)     CONSTRAINT [DF_Orders_Dest] DEFAULT ('') NULL,
    [Model]                  VARCHAR (25)    CONSTRAINT [DF_Orders_Model] DEFAULT ('') NULL,
    [HsCode1]                VARCHAR (14)    CONSTRAINT [DF_Orders_HsCode1] DEFAULT ('') NULL,
    [HsCode2]                VARCHAR (14)    CONSTRAINT [DF_Orders_HsCode2] DEFAULT ('') NULL,
    [PayTermARID]            VARCHAR (10)    CONSTRAINT [DF_Orders_PayTermARID] DEFAULT ('') NULL,
    [ShipTermID]             VARCHAR (5)     CONSTRAINT [DF_Orders_ShipTermID] DEFAULT ('') NULL,
    [ShipModeList]           VARCHAR (30)    CONSTRAINT [DF_Orders_ShipModeList] DEFAULT ('') NULL,
    [CdCodeID]               VARCHAR (6)     CONSTRAINT [DF_Orders_CdCodeID] DEFAULT ('') NULL,
    [CPU]                    NUMERIC (5, 3)  CONSTRAINT [DF_Orders_CPU] DEFAULT ((0)) NULL,
    [Qty]                    INT             CONSTRAINT [DF_Orders_Qty] DEFAULT ((0)) NULL,
    [StyleUnit]              VARCHAR (8)     CONSTRAINT [DF_Orders_StyleUnit] DEFAULT ('') NULL,
    [PoPrice]                NUMERIC (16, 4) CONSTRAINT [DF_Orders_PoPrice] DEFAULT ((0)) NULL,
    [CFMPrice]               NUMERIC (16, 4) CONSTRAINT [DF_Orders_CFMPrice] DEFAULT ((0)) NULL,
    [CurrencyID]             VARCHAR (3)     CONSTRAINT [DF_Orders_CurrecnyID] DEFAULT ('') NULL,
    [Commission]             NUMERIC (3, 2)  CONSTRAINT [DF_Orders_Commission] DEFAULT ((0)) NULL,
    [FactoryID]              VARCHAR (8)     CONSTRAINT [DF_Orders_FactoryID] DEFAULT ('') NULL,
    [BrandAreaCode]          VARCHAR (10)    CONSTRAINT [DF_Orders_BrandAreaCode] DEFAULT ('') NULL,
    [BrandFTYCode]           VARCHAR (10)    CONSTRAINT [DF_Orders_BrandFTYCode] DEFAULT ('') NULL,
    [CTNQty]                 SMALLINT        CONSTRAINT [DF_Orders_CTNQty] DEFAULT ((0)) NULL,
    [CustCDID]               VARCHAR (16)    CONSTRAINT [DF_Orders_CustCDID] DEFAULT ('') NULL,
    [CustPONo]               VARCHAR (30)    CONSTRAINT [DF_Orders_CustPONo] DEFAULT ('') NULL,
    [Customize1]             VARCHAR (30)    CONSTRAINT [DF_Orders_Customize1] DEFAULT ('') NULL,
    [Customize2]             VARCHAR (30)    CONSTRAINT [DF_Orders_Customize2] DEFAULT ('') NULL,
    [Customize3]             VARCHAR (30)    CONSTRAINT [DF_Orders_Customize3] DEFAULT ('') NULL,
    [CFMDate]                DATE            NULL,
    [BuyerDelivery]          DATE            NULL,
    [SciDelivery]            DATE            NULL,
    [SewInLine]              DATE            NULL,
    [SewOffLine]             DATE            NULL,
    [CutInLine]              DATE            NULL,
    [CutOffLine]             DATE            NULL,
    [PulloutDate]            DATE            NULL,
    [CMPUnit]                VARCHAR (8)     CONSTRAINT [DF_Orders_CMPUnit] DEFAULT ('') NULL,
    [CMPPrice]               NUMERIC (6, 2)  CONSTRAINT [DF_Orders_CMPPrice] DEFAULT ((0)) NULL,
    [CMPQDate]               DATE            NULL,
    [CMPQRemark]             NVARCHAR (MAX)  CONSTRAINT [DF_Orders_CMPQRemark] DEFAULT ('') NULL,
    [EachConsApv]            DATETIME        NULL,
    [MnorderApv]             DATETIME        NULL,
    [CRDDate]                DATE            NULL,
    [InitialPlanDate]        DATE            NULL,
    [PlanDate]               DATE            NULL,
    [FirstProduction]        DATE            NULL,
    [FirstProductionLock]    DATE            NULL,
    [OrigBuyerDelivery]      DATE            NULL,
    [ExCountry]              DATE            NULL,
    [InDCDate]               DATE            NULL,
    [CFMShipment]            DATE            NULL,
    [PFETA]                  DATE            NULL,
    [PackLETA]               DATE            NULL,
    [LETA]                   DATE            NULL,
    [MRHandle]               VARCHAR (10)    CONSTRAINT [DF_Orders_MRHandle] DEFAULT ('') NULL,
    [SMR]                    VARCHAR (10)    CONSTRAINT [DF_Orders_SMR] DEFAULT ('') NULL,
    [ScanAndPack]            BIT             CONSTRAINT [DF_Orders_ScanAndPack] DEFAULT ((0)) NULL,
    [VasShas]                BIT             CONSTRAINT [DF_Orders_VasShas] DEFAULT ((0)) NULL,
    [SpecialCust]            BIT             CONSTRAINT [DF_Orders_SpecialCust] DEFAULT ((0)) NULL,
    [TissuePaper]            BIT             CONSTRAINT [DF_Orders_TissuePaper] DEFAULT ((0)) NULL,
    [Junk]                   BIT             CONSTRAINT [DF_Orders_Junk] DEFAULT ((0)) NULL,
    [Packing]                NVARCHAR (MAX)  CONSTRAINT [DF_Orders_Packing] DEFAULT ('') NULL,
    [MarkFront]              NVARCHAR (MAX)  CONSTRAINT [DF_Orders_MarkFront] DEFAULT ('') NULL,
    [MarkBack]               NVARCHAR (MAX)  CONSTRAINT [DF_Orders_MarkBack] DEFAULT ('') NULL,
    [MarkLeft]               NVARCHAR (MAX)  CONSTRAINT [DF_Orders_MarkLeft] DEFAULT ('') NULL,
    [MarkRight]              NVARCHAR (MAX)  CONSTRAINT [DF_Orders_MarkRight] DEFAULT ('') NULL,
    [Label]                  NVARCHAR (MAX)  CONSTRAINT [DF_Orders_Label] DEFAULT ('') NULL,
    [OrderRemark]            NVARCHAR (MAX)  CONSTRAINT [DF_Orders_OrderRemark] DEFAULT ('') NULL,
    [ArtWorkCost]            VARCHAR (1)     CONSTRAINT [DF_Orders_ArtWorkCost] DEFAULT ('') NULL,
    [StdCost]                NUMERIC (7, 2)  CONSTRAINT [DF_Orders_StdCost] DEFAULT ((0)) NULL,
    [CtnType]                VARCHAR (1)     CONSTRAINT [DF_Orders_CtnType] DEFAULT ('') NULL,
    [FOCQty]                 INT             CONSTRAINT [DF_Orders_FOCQty] DEFAULT ((0)) NULL,
    [SMnorderApv]            DATE            NULL,
    [FOC]                    BIT             CONSTRAINT [DF_Orders_FOC] DEFAULT ((0)) NULL,
    [MnorderApv2]            DATETIME        NULL,
    [Packing2]               NVARCHAR (MAX)  CONSTRAINT [DF_Orders_Packing2] DEFAULT ('') NULL,
    [SampleReason]           VARCHAR (5)     CONSTRAINT [DF_Orders_SampleReason] DEFAULT ('') NULL,
    [RainwearTestPassed]     BIT             CONSTRAINT [DF_Orders_RainwearTestPassed] DEFAULT ((0)) NULL,
    [SizeRange]              NVARCHAR (MAX)  CONSTRAINT [DF_Orders_SizeRange] DEFAULT ('') NULL,
    [MTLComplete]            BIT             CONSTRAINT [DF_Orders_MTLComplete] DEFAULT ((0)) NULL,
    [SpecialMark]            VARCHAR (5)     CONSTRAINT [DF_Orders_SpecialMark] DEFAULT ('') NULL,
    [OutstandingRemark]      NVARCHAR (MAX)  CONSTRAINT [DF_Orders_OutstandingRemark] DEFAULT ('') NULL,
    [OutstandingInCharge]    VARCHAR (10)    CONSTRAINT [DF_Orders_OutstandingInCharge] DEFAULT ('') NULL,
    [OutstandingDate]        DATETIME        NULL,
    [OutstandingReason]      VARCHAR (5)     CONSTRAINT [DF_Orders_OutstandingReason] DEFAULT ('') NULL,
    [StyleUkey]              BIGINT          CONSTRAINT [DF_Orders_StyleUkey] DEFAULT ((0)) NULL,
    [POID]                   VARCHAR (13)    CONSTRAINT [DF_Orders_POID] DEFAULT ('') NULL,
    [OrderComboID]           VARCHAR (13)    NULL,
    [IsNotRepeatOrMapping]   BIT             CONSTRAINT [DF_Orders_IsProPhet] DEFAULT ((0)) NULL,
    [SplitOrderId]           VARCHAR (13)    CONSTRAINT [DF_Orders_SplitOrderId] DEFAULT ('') NULL,
    [FtyKPI]                 DATETIME        NULL,
    [AddName]                VARCHAR (10)    CONSTRAINT [DF_Orders_AddName] DEFAULT ('') NULL,
    [AddDate]                DATETIME        NULL,
    [EditName]               VARCHAR (10)    CONSTRAINT [DF_Orders_EditName] DEFAULT ('') NULL,
    [EditDate]               DATETIME        NULL,
    [SewLine]                VARCHAR (60)    CONSTRAINT [DF_Orders_SewLine] DEFAULT ('') NULL,
    [ActPulloutDate]         DATE            NULL,
    [ProdSchdRemark]         NVARCHAR (100)  CONSTRAINT [DF_Orders_ProdSchdRemark] DEFAULT ('') NULL,
    [IsForecast]             BIT             CONSTRAINT [DF_Orders_IsForecast] DEFAULT ((0)) NULL,
    [LocalOrder]             BIT             CONSTRAINT [DF_Orders_LocalOrder] DEFAULT ((0)) NULL,
    [GMTClose]               DATE            NULL,
    [TotalCTN]               INT             CONSTRAINT [DF_Orders_TotalCTN] DEFAULT ((0)) NULL,
    [ClogCTN]                INT             CONSTRAINT [DF_Orders_ClogCTN] DEFAULT ((0)) NULL,
    [FtyCTN]                 INT             CONSTRAINT [DF_Orders_FtyCTN] DEFAULT ((0)) NULL,
    [PulloutComplete]        BIT             CONSTRAINT [DF_Orders_PulloutComplete] DEFAULT ((0)) NULL,
    [ReadyDate]              DATE            NULL,
    [PulloutCTNQty]          INT             CONSTRAINT [DF_Orders_PulloutCTNQty] DEFAULT ((0)) NULL,
    [Finished]               BIT             CONSTRAINT [DF_Orders_Finished] DEFAULT ((0)) NULL,
    [PFOrder]                BIT             CONSTRAINT [DF_Orders_PFOrder] DEFAULT ((0)) NULL,
    [SDPDate]                DATE            NULL,
    [InspDate]               DATE            NULL,
    [InspResult]             VARCHAR (1)     CONSTRAINT [DF_Orders_InspResult] DEFAULT ('') NULL,
    [InspHandle]             VARCHAR (10)    CONSTRAINT [DF_Orders_InspHandle] DEFAULT ('') NULL,
    [KPILETA]                DATE            NULL,
    [MTLETA]                 DATE            NULL,
    [SewETA]                 DATE            NULL,
    [PackETA]                DATE            NULL,
    [MTLExport]              VARCHAR (2)     CONSTRAINT [DF_Orders_MTLExport] DEFAULT ('') NULL,
    [DoxType]                VARCHAR (8)     CONSTRAINT [DF_Orders_DoxType] DEFAULT ('') NULL,
    [FtyGroup]               VARCHAR (8)     CONSTRAINT [DF_Orders_FtyGroup] DEFAULT ('') NULL,
    [MDivisionID]            VARCHAR (8)     CONSTRAINT [DF_Orders_MDivisionID] DEFAULT ('') NULL,
    [CutReadyDate]           DATE            NULL,
    [SewRemark]              NVARCHAR (60)   CONSTRAINT [DF_Orders_SewRemark] DEFAULT ('') NULL,
    [WhseClose]              DATE            NULL,
    [SubconInSisterFty]      BIT             CONSTRAINT [DF_Orders_SubconInSisterFty] DEFAULT ((0)) NULL,
    [MCHandle]               VARCHAR (10)    CONSTRAINT [DF_Orders_MCHandle] DEFAULT ('') NULL,
    [LocalMR]                VARCHAR (10)    CONSTRAINT [DF_Orders_LocalMR] DEFAULT ('') NULL,
    [KPIChangeReason]        VARCHAR (5)     CONSTRAINT [DF_Orders_KPIChangeReason] DEFAULT ('') NULL,
    [MDClose]                DATE            NULL,
    [MDEditName]             VARCHAR (10)    CONSTRAINT [DF_Orders_MDEditName] DEFAULT ('') NULL,
    [MDEditDate]             DATETIME        NULL,
    [ClogLastReceiveDate]    DATE            NULL,
    [CPUFactor]              NUMERIC (3, 1)  NULL,
    [SizeUnit]               VARCHAR (8)     CONSTRAINT [DF_Orders_SizeUnit] DEFAULT ('') NULL,
    [CuttingSP]              VARCHAR (13)    CONSTRAINT [DF_Orders_CuttingSP] DEFAULT ('') NULL,
    [IsMixMarker]            INT             CONSTRAINT [DF_Orders_IsMixMarker] DEFAULT ((0)) NULL,
    [EachConsSource]         VARCHAR (1)     NULL,
    [KPIEachConsApprove]     DATE            NULL,
    [KPICmpq]                DATE            NULL,
    [KPIMNotice]             DATE            NULL,
    [GMTComplete]            VARCHAR (1)     CONSTRAINT [DF__Orders__GMTCompl__6C39D5A3] DEFAULT ('') NULL,
    [GFR]                    BIT             CONSTRAINT [DF__Orders__GFR__6D2DF9DC] DEFAULT ((0)) NULL,
    [CfaCTN]                 INT             CONSTRAINT [DF_Orders_CfaCTN] DEFAULT ((0)) NULL,
    [DRYCTN]                 INT             CONSTRAINT [DF_Orders_DRYCTN] DEFAULT ((0)) NOT NULL,
    [PackErrCTN]             INT             CONSTRAINT [DF_Orders_PackErrCTN] DEFAULT ((0)) NULL,
    [ForecastSampleGroup]    VARCHAR (1)     CONSTRAINT [DF_Orders_ForecastSampleGroup] DEFAULT ('') NULL,
    [DyeingLoss]             NUMERIC (3)     DEFAULT ((0)) NULL,
    [SubconInType]           VARCHAR (1)     NULL,
    [LastProductionDate]     DATE            NULL,
    [EstPODD]                DATE            NULL,
    [AirFreightByBrand]      BIT             DEFAULT ((0)) NULL,
    [AllowanceComboID]       VARCHAR (13)    NULL,
    [ChangeMemoDate]         DATE            NULL,
    [BuyBack]                VARCHAR (20)    NULL,
    [BuyBackOrderID]         VARCHAR (13)    NULL,
    [ForecastCategory]       VARCHAR (1)     DEFAULT ('') NULL,
    [OnSiteSample]           BIT             DEFAULT ((0)) NULL,
    [PulloutCmplDate]        DATE            NULL,
    [NeedProduction]         BIT             DEFAULT ((0)) NULL,
    [IsBuyBack]              BIT             DEFAULT ((0)) NOT NULL,
    [KeepPanels]             BIT             DEFAULT ((0)) NULL,
    [BuyBackReason]          VARCHAR (20)    CONSTRAINT [DF_Orders_BuyBackReason] DEFAULT ('') NOT NULL,
    [IsBuyBackCrossArticle]  BIT             CONSTRAINT [DF_Orders_IsBuyBackCrossArticle] DEFAULT ((0)) NOT NULL,
    [IsBuyBackCrossSizeCode] BIT             CONSTRAINT [DF_Orders_IsBuyBackCrossSizeCode] DEFAULT ((0)) NOT NULL,
    [KpiEachConsCheck]       DATE            NULL,
    [NonRevenue]             BIT             CONSTRAINT [DF_Orders_NonRevenue] DEFAULT ((0)) NOT NULL,
    [CAB]                    VARCHAR (10)    CONSTRAINT [DF_Orders_CAB] DEFAULT ('') NOT NULL,
    [FinalDest]              VARCHAR (50)    CONSTRAINT [DF_Orders_FinalDest] DEFAULT ('') NOT NULL,
    [Customer_PO]            VARCHAR (50)    CONSTRAINT [DF_Orders_Customer_PO] DEFAULT ('') NOT NULL,
    [AFS_STOCK_CATEGORY]     VARCHAR (50)    CONSTRAINT [DF_Orders_AFS_STOCK_CATEGORY] DEFAULT ('') NOT NULL,
    [CMPLTDATE]              DATE            NULL,
    [DelayCode]              VARCHAR (4)     NULL,
    [DelayDesc]              VARCHAR (100)   NULL,
    [HangerPack]             BIT             DEFAULT ((0)) NULL,
    [CDCodeNew]              VARCHAR (5)     NULL,
    [SizeUnitWeight]         VARCHAR (8)     NULL,
    [BrokenNeedles]          BIT             CONSTRAINT [DF_Orders_BrokenNeedles] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([ID] ASC)
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


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ProgramID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'專案代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OrderTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'BuyMonth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'進口國別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Dest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'場域模式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Model'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中國海關HS編碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'HsCode1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中國海關HS編碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'HsCode2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'付款方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PayTermARID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交貨條件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ShipTermID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交貨方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ShipModeList'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CdCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單件耗用產能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'StyleUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PoPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'確認單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CFMPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CurrencyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'應付佣金%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Commission'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的區域代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'BrandAreaCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'BrandFTYCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每箱的包裝數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CTNQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CustCDID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CustPONo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Customize1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Customize2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Customize3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CFMDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飛雁交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SciDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SewInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠下線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SewOffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪上線日　' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CutInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪下線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CutOffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠出口日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PulloutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'cmp單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CMPUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'cmp單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CMPPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CMPQ的確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CMPQDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CMPQ上的備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CMPQRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each-con 確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'EachConsApv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'製造單確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MnorderApv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CRD date.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CRDDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Initial Plan Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'InitialPlanDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plan Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PlanDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First production date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FirstProduction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1st Production Lock' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FirstProductionLock'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orig. buyer delivery date:' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OrigBuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ex-Country Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ExCountry'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'In DC Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'InDCDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirm Shipment Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CFMShipment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P/F ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PFETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包材的預計到貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PackLETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'14天LOCK的採購單交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'LETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MRHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Scan and Pack' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ScanAndPack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VAS/SHAS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'VasShas'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Special customer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SpecialCust'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'棉紙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'TissuePaper'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Packing'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨嘜頭(正面)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MarkFront'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨嘜頭(背面)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MarkBack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨嘜頭(左面)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MarkLeft'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨嘜頭(右面)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MarkRight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'圖片與商標位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Label'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OrderRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工的展開方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ArtWorkCost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'StdCost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝配比方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CtnType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FOCQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMNotice Approved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SMnorderApv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FOC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'mnoti_apv第二階段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MnorderApv2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing第二階段資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Packing2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SAMPLE REASON' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SampleReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗測式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'RainwearTestPassed'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸範圍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SizeRange'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料出清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MTLComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Special Mark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SpecialMark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延出備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OutstandingRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延出原因修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OutstandingInCharge'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延出原因修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OutstandingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延出備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OutstandingReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式的唯一值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'StyleUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為非格子布或非Repeat或非Body Mapping' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'IsNotRepeatOrMapping'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆單的原訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SplitOrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FtyKPI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SewLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際出貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ActPulloutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Production Schedules Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ProdSchdRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預估單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'IsForecast'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠自行接單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'LocalOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment 結單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'GMTClose'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單總箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'TotalCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'cLog 已收到箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ClogCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠已完成箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FtyCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨出貨結清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PulloutComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨Ready 的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ReadyDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pullout箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PulloutCTNQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipment(Pull out)完成' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Finished'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pull forward 訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PFOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SDPDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣首次通過檢驗日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'InspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'InspResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA Finial檢驗人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'InspHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI L/ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'KPILETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MTLETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Mtl ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SewETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing Mtl ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PackETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料出貨結清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MTLExport'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FormA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'DoxType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Factory Group' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FtyGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manufacturing Division ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cutting Ready Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CutReadyDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SewRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫關單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'WhseClose'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subcon In From Sister Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SubconInSisterFty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MC Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Local MR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'LocalMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI Date變更理由' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'KPIChangeReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD Finished' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MDClose'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD 最後編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MDEditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD 最後編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MDEditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後收箱日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ClogLastReceiveDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為MixMarker ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'IsMixMarker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each Cons. KPI Date (PMS only)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'KPIEachConsApprove'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cmpq KPI Date (PMS only)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'KPICmpq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M Notice KPI Date (PMS only)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'KPIMNotice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment Complete ( From Trade)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'GMTComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Global Foundation Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'GFR'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA�c��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'CfaCTN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���ëǽc��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'DRYCTN';


GO


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PulloutComplete �̫᪺��s�ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'PulloutCmplDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��ܥi�H��Article���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'IsBuyBackCrossArticle';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��ܥi�H��Size���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'IsBuyBackCrossSizeCode';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Each Cons KPI�ˬd���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'KpiEachConsCheck';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ư����q��Ͳ������A1:�ư��A0���ư�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'NonRevenue';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - CAB' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CAB'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - FinalDest' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FinalDest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - Customer_PO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Customer_PO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - AFS_STOCK_CATEGORY' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'AFS_STOCK_CATEGORY'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders'
GO
CREATE NONCLUSTERED INDEX [IX_SciDelivery]
    ON [dbo].[Orders]([SciDelivery] ASC, [MDivisionID] ASC, [ID] ASC);


GO
CREATE NONCLUSTERED INDEX [Index_POID]
    ON [dbo].[Orders]([POID] ASC)
    INCLUDE([ID]);


GO
CREATE NONCLUSTERED INDEX [Index_ForShipmentSchedule]
    ON [dbo].[Orders]([Category] ASC, [PulloutComplete] ASC, [Finished] ASC, [MDivisionID] ASC, [Qty] ASC)
    INCLUDE([ID], [ScanAndPack], [RainwearTestPassed], [SewLine], [InspDate], [DoxType], [CustPONo], [Customize1], [Customize2], [SciDelivery], [SewOffLine], [CRDDate], [BrandID], [StyleID], [BuyMonth], [Dest], [FactoryID], [CustCDID]);


GO
CREATE NONCLUSTERED INDEX [Index_CuttingSP]
    ON [dbo].[Orders]([CuttingSP] ASC)
    INCLUDE([IsForecast], [LocalOrder]);


GO
CREATE NONCLUSTERED INDEX [IDX_Orders_MES_EndlineR01]
    ON [dbo].[Orders]([CustPONo] ASC, [StyleUkey] ASC)
    INCLUDE([StyleUnit]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單在生產過程中是否有出現斷針', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'BrokenNeedles';

