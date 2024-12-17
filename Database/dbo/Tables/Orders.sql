CREATE TABLE [dbo].[Orders] (
    [ID]                     VARCHAR (13)    CONSTRAINT [DF_Orders_ID] DEFAULT ('') NOT NULL,
    [BrandID]                VARCHAR (8)     CONSTRAINT [DF_Orders_BrandID] DEFAULT ('') NOT NULL,
    [ProgramID]              NVARCHAR (12)   CONSTRAINT [DF_Orders_ProgramID] DEFAULT ('') NOT NULL,
    [StyleID]                VARCHAR (15)    CONSTRAINT [DF_Orders_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]               VARCHAR (10)    CONSTRAINT [DF_Orders_SeasonID] DEFAULT ('') NOT NULL,
    [ProjectID]              VARCHAR (5)     CONSTRAINT [DF_Orders_ProjectID] DEFAULT ('') NOT NULL,
    [Category]               VARCHAR (1)     CONSTRAINT [DF_Orders_Category] DEFAULT ('') NOT NULL,
    [OrderTypeID]            VARCHAR (20)    CONSTRAINT [DF_Orders_OrderTypeID] DEFAULT ('') NOT NULL,
    [BuyMonth]               VARCHAR (16)    CONSTRAINT [DF_Orders_BuyMonth] DEFAULT ('') NOT NULL,
    [Dest]                   VARCHAR (2)     CONSTRAINT [DF_Orders_Dest] DEFAULT ('') NOT NULL,
    [Model]                  VARCHAR (25)    CONSTRAINT [DF_Orders_Model] DEFAULT ('') NOT NULL,
    [HsCode1]                VARCHAR (14)    CONSTRAINT [DF_Orders_HsCode1] DEFAULT ('') NOT NULL,
    [HsCode2]                VARCHAR (14)    CONSTRAINT [DF_Orders_HsCode2] DEFAULT ('') NOT NULL,
    [PayTermARID]            VARCHAR (10)    CONSTRAINT [DF_Orders_PayTermARID] DEFAULT ('') NOT NULL,
    [ShipTermID]             VARCHAR (5)     CONSTRAINT [DF_Orders_ShipTermID] DEFAULT ('') NOT NULL,
    [ShipModeList]           VARCHAR (30)    CONSTRAINT [DF_Orders_ShipModeList] DEFAULT ('') NOT NULL,
    [CdCodeID]               VARCHAR (6)     CONSTRAINT [DF_Orders_CdCodeID] DEFAULT ('') NOT NULL,
    [CPU]                    DECIMAL (5, 3)  CONSTRAINT [DF_Orders_CPU] DEFAULT ((0)) NOT NULL,
    [Qty]                    INT             CONSTRAINT [DF_Orders_Qty] DEFAULT ((0)) NOT NULL,
    [StyleUnit]              VARCHAR (8)     CONSTRAINT [DF_Orders_StyleUnit] DEFAULT ('') NOT NULL,
    [PoPrice]                DECIMAL (16, 4) CONSTRAINT [DF_Orders_PoPrice] DEFAULT ((0)) NOT NULL,
    [CFMPrice]               DECIMAL (16, 4) CONSTRAINT [DF_Orders_CFMPrice] DEFAULT ((0)) NOT NULL,
    [CurrencyID]             VARCHAR (3)     CONSTRAINT [DF_Orders_CurrencyID] DEFAULT ('') NOT NULL,
    [Commission]             DECIMAL (3, 2)  CONSTRAINT [DF_Orders_Commission] DEFAULT ((0)) NOT NULL,
    [FactoryID]              VARCHAR (8)     CONSTRAINT [DF_Orders_FactoryID] DEFAULT ('') NOT NULL,
    [BrandAreaCode]          VARCHAR (10)    CONSTRAINT [DF_Orders_BrandAreaCode] DEFAULT ('') NOT NULL,
    [BrandFTYCode]           VARCHAR (10)    CONSTRAINT [DF_Orders_BrandFTYCode] DEFAULT ('') NOT NULL,
    [CTNQty]                 SMALLINT        CONSTRAINT [DF_Orders_CTNQty] DEFAULT ((0)) NOT NULL,
    [CustCDID]               VARCHAR (16)    CONSTRAINT [DF_Orders_CustCDID] DEFAULT ('') NOT NULL,
    [CustPONo]               VARCHAR (30)    CONSTRAINT [DF_Orders_CustPONo] DEFAULT ('') NOT NULL,
    [Customize1]             VARCHAR (30)    CONSTRAINT [DF_Orders_Customize1] DEFAULT ('') NOT NULL,
    [Customize2]             VARCHAR (30)    CONSTRAINT [DF_Orders_Customize2] DEFAULT ('') NOT NULL,
    [Customize3]             VARCHAR (30)    CONSTRAINT [DF_Orders_Customize3] DEFAULT ('') NOT NULL,
    [CFMDate]                DATE            NULL,
    [BuyerDelivery]          DATE            NULL,
    [SciDelivery]            DATE            NULL,
    [SewInLine]              DATE            NULL,
    [SewOffLine]             DATE            NULL,
    [CutInLine]              DATE            NULL,
    [CutOffLine]             DATE            NULL,
    [PulloutDate]            DATE            NULL,
    [CMPUnit]                VARCHAR (8)     CONSTRAINT [DF_Orders_CMPUnit] DEFAULT ('') NOT NULL,
    [CMPPrice]               DECIMAL (6, 2)  CONSTRAINT [DF_Orders_CMPPrice] DEFAULT ((0)) NOT NULL,
    [CMPQDate]               DATE            NULL,
    [CMPQRemark]             NVARCHAR (MAX)  CONSTRAINT [DF_Orders_CMPQRemark] DEFAULT ('') NOT NULL,
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
    [MRHandle]               VARCHAR (10)    CONSTRAINT [DF_Orders_MRHandle] DEFAULT ('') NOT NULL,
    [SMR]                    VARCHAR (10)    CONSTRAINT [DF_Orders_SMR] DEFAULT ('') NOT NULL,
    [ScanAndPack]            BIT             CONSTRAINT [DF_Orders_ScanAndPack] DEFAULT ((0)) NOT NULL,
    [VasShas]                BIT             CONSTRAINT [DF_Orders_VasShas] DEFAULT ((0)) NOT NULL,
    [SpecialCust]            BIT             CONSTRAINT [DF_Orders_SpecialCust] DEFAULT ((0)) NOT NULL,
    [TissuePaper]            BIT             CONSTRAINT [DF_Orders_TissuePaper] DEFAULT ((0)) NOT NULL,
    [Junk]                   BIT             CONSTRAINT [DF_Orders_Junk] DEFAULT ((0)) NOT NULL,
    [Packing]                NVARCHAR (MAX)  CONSTRAINT [DF_Orders_Packing] DEFAULT ('') NOT NULL,
    [MarkFront]              NVARCHAR (MAX)  CONSTRAINT [DF_Orders_MarkFront] DEFAULT ('') NOT NULL,
    [MarkBack]               NVARCHAR (MAX)  CONSTRAINT [DF_Orders_MarkBack] DEFAULT ('') NOT NULL,
    [MarkLeft]               NVARCHAR (MAX)  CONSTRAINT [DF_Orders_MarkLeft] DEFAULT ('') NOT NULL,
    [MarkRight]              NVARCHAR (MAX)  CONSTRAINT [DF_Orders_MarkRight] DEFAULT ('') NOT NULL,
    [Label]                  NVARCHAR (MAX)  CONSTRAINT [DF_Orders_Label] DEFAULT ('') NOT NULL,
    [OrderRemark]            NVARCHAR (MAX)  CONSTRAINT [DF_Orders_OrderRemark] DEFAULT ('') NOT NULL,
    [ArtWorkCost]            VARCHAR (1)     CONSTRAINT [DF_Orders_ArtWorkCost] DEFAULT ('') NOT NULL,
    [StdCost]                DECIMAL (7, 2)  CONSTRAINT [DF_Orders_StdCost] DEFAULT ((0)) NOT NULL,
    [CtnType]                VARCHAR (1)     CONSTRAINT [DF_Orders_CtnType] DEFAULT ('') NOT NULL,
    [FOCQty]                 INT             CONSTRAINT [DF_Orders_FOCQty] DEFAULT ((0)) NOT NULL,
    [SMnorderApv]            DATE            NULL,
    [FOC]                    BIT             CONSTRAINT [DF_Orders_FOC] DEFAULT ((0)) NOT NULL,
    [MnorderApv2]            DATETIME        NULL,
    [Packing2]               NVARCHAR (MAX)  CONSTRAINT [DF_Orders_Packing2] DEFAULT ('') NOT NULL,
    [SampleReason]           VARCHAR (5)     CONSTRAINT [DF_Orders_SampleReason] DEFAULT ('') NOT NULL,
    [RainwearTestPassed]     BIT             CONSTRAINT [DF_Orders_RainwearTestPassed] DEFAULT ((0)) NOT NULL,
    [SizeRange]              NVARCHAR (MAX)  CONSTRAINT [DF_Orders_SizeRange] DEFAULT ('') NOT NULL,
    [MTLComplete]            BIT             CONSTRAINT [DF_Orders_MTLComplete] DEFAULT ((0)) NOT NULL,
    [SpecialMark]            VARCHAR (5)     CONSTRAINT [DF_Orders_SpecialMark] DEFAULT ('') NOT NULL,
    [OutstandingRemark]      NVARCHAR (MAX)  CONSTRAINT [DF_Orders_OutstandingRemark] DEFAULT ('') NOT NULL,
    [OutstandingInCharge]    VARCHAR (10)    CONSTRAINT [DF_Orders_OutstandingInCharge] DEFAULT ('') NOT NULL,
    [OutstandingDate]        DATETIME        NULL,
    [OutstandingReason]      VARCHAR (5)     CONSTRAINT [DF_Orders_OutstandingReason] DEFAULT ('') NOT NULL,
    [StyleUkey]              BIGINT          CONSTRAINT [DF_Orders_StyleUkey] DEFAULT ((0)) NOT NULL,
    [POID]                   VARCHAR (13)    CONSTRAINT [DF_Orders_POID] DEFAULT ('') NOT NULL,
    [OrderComboID]           VARCHAR (13)    CONSTRAINT [DF_Orders_OrderComboID] DEFAULT ('') NOT NULL,
    [IsNotRepeatOrMapping]   BIT             CONSTRAINT [DF_Orders_IsNotRepeatOrMapping] DEFAULT ((0)) NOT NULL,
    [SplitOrderId]           VARCHAR (13)    CONSTRAINT [DF_Orders_SplitOrderId] DEFAULT ('') NOT NULL,
    [FtyKPI]                 DATETIME        NULL,
    [AddName]                VARCHAR (10)    CONSTRAINT [DF_Orders_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                DATETIME        NULL,
    [EditName]               VARCHAR (10)    CONSTRAINT [DF_Orders_EditName] DEFAULT ('') NOT NULL,
    [EditDate]               DATETIME        NULL,
    [SewLine]                VARCHAR (60)    CONSTRAINT [DF_Orders_SewLine] DEFAULT ('') NOT NULL,
    [ActPulloutDate]         DATE            NULL,
    [ProdSchdRemark]         NVARCHAR (100)  CONSTRAINT [DF_Orders_ProdSchdRemark] DEFAULT ('') NOT NULL,
    [IsForecast]             BIT             CONSTRAINT [DF_Orders_IsForecast] DEFAULT ((0)) NOT NULL,
    [LocalOrder]             BIT             CONSTRAINT [DF_Orders_LocalOrder] DEFAULT ((0)) NOT NULL,
    [GMTClose]               DATE            NULL,
    [TotalCTN]               INT             CONSTRAINT [DF_Orders_TotalCTN] DEFAULT ((0)) NOT NULL,
    [ClogCTN]                INT             CONSTRAINT [DF_Orders_ClogCTN] DEFAULT ((0)) NOT NULL,
    [FtyCTN]                 INT             CONSTRAINT [DF_Orders_FtyCTN] DEFAULT ((0)) NOT NULL,
    [PulloutComplete]        BIT             CONSTRAINT [DF_Orders_PulloutComplete] DEFAULT ((0)) NOT NULL,
    [ReadyDate]              DATE            NULL,
    [PulloutCTNQty]          INT             CONSTRAINT [DF_Orders_PulloutCTNQty] DEFAULT ((0)) NOT NULL,
    [Finished]               BIT             CONSTRAINT [DF_Orders_Finished] DEFAULT ((0)) NOT NULL,
    [PFOrder]                BIT             CONSTRAINT [DF_Orders_PFOrder] DEFAULT ((0)) NOT NULL,
    [SDPDate]                DATE            NULL,
    [InspDate]               DATE            NULL,
    [InspResult]             VARCHAR (1)     CONSTRAINT [DF_Orders_InspResult] DEFAULT ('') NOT NULL,
    [InspHandle]             VARCHAR (10)    CONSTRAINT [DF_Orders_InspHandle] DEFAULT ('') NOT NULL,
    [KPILETA]                DATE            NULL,
    [MTLETA]                 DATE            NULL,
    [SewETA]                 DATE            NULL,
    [PackETA]                DATE            NULL,
    [MTLExport]              VARCHAR (2)     CONSTRAINT [DF_Orders_MTLExport] DEFAULT ('') NOT NULL,
    [DoxType]                VARCHAR (8)     CONSTRAINT [DF_Orders_DoxType] DEFAULT ('') NOT NULL,
    [FtyGroup]               VARCHAR (8)     CONSTRAINT [DF_Orders_FtyGroup] DEFAULT ('') NOT NULL,
    [MDivisionID]            VARCHAR (8)     CONSTRAINT [DF_Orders_MDivisionID] DEFAULT ('') NOT NULL,
    [CutReadyDate]           DATE            NULL,
    [SewRemark]              NVARCHAR (60)   CONSTRAINT [DF_Orders_SewRemark] DEFAULT ('') NOT NULL,
    [WhseClose]              DATE            NULL,
    [SubconInSisterFty]      BIT             CONSTRAINT [DF_Orders_SubconInSisterFty] DEFAULT ((0)) NOT NULL,
    [MCHandle]               VARCHAR (10)    CONSTRAINT [DF_Orders_MCHandle] DEFAULT ('') NOT NULL,
    [LocalMR]                VARCHAR (10)    CONSTRAINT [DF_Orders_LocalMR] DEFAULT ('') NOT NULL,
    [KPIChangeReason]        VARCHAR (5)     CONSTRAINT [DF_Orders_KPIChangeReason] DEFAULT ('') NOT NULL,
    [MDClose]                DATE            NULL,
    [MDEditName]             VARCHAR (10)    CONSTRAINT [DF_Orders_MDEditName] DEFAULT ('') NOT NULL,
    [MDEditDate]             DATETIME        NULL,
    [ClogLastReceiveDate]    DATE            NULL,
    [CPUFactor]              DECIMAL (3, 1)  CONSTRAINT [DF_Orders_CPUFactor] DEFAULT ((0)) NOT NULL,
    [SizeUnit]               VARCHAR (8)     CONSTRAINT [DF_Orders_SizeUnit] DEFAULT ('') NOT NULL,
    [CuttingSP]              VARCHAR (13)    CONSTRAINT [DF_Orders_CuttingSP] DEFAULT ('') NOT NULL,
    [IsMixMarker]            INT             CONSTRAINT [DF_Orders_IsMixMarker] DEFAULT ((0)) NOT NULL,
    [EachConsSource]         VARCHAR (1)     CONSTRAINT [DF_Orders_EachConsSource] DEFAULT ('') NOT NULL,
    [KPIEachConsApprove]     DATE            NULL,
    [KPICmpq]                DATE            NULL,
    [KPIMNotice]             DATE            NULL,
    [GMTComplete]            VARCHAR (1)     CONSTRAINT [DF_Orders_GMTComplete] DEFAULT ('') NOT NULL,
    [GFR]                    BIT             CONSTRAINT [DF_Orders_GFR] DEFAULT ((0)) NOT NULL,
    [CfaCTN]                 INT             CONSTRAINT [DF_Orders_CfaCTN] DEFAULT ((0)) NOT NULL,
    [DRYCTN]                 INT             CONSTRAINT [DF_Orders_DRYCTN] DEFAULT ((0)) NOT NULL,
    [PackErrCTN]             INT             CONSTRAINT [DF_Orders_PackErrCTN] DEFAULT ((0)) NOT NULL,
    [ForecastSampleGroup]    VARCHAR (1)     CONSTRAINT [DF_Orders_ForecastSampleGroup] DEFAULT ('') NOT NULL,
    [DyeingLoss]             DECIMAL (3)     CONSTRAINT [DF_Orders_DyeingLoss] DEFAULT ((0)) NOT NULL,
    [SubconInType]           VARCHAR (1)     CONSTRAINT [DF_Orders_SubconInType] DEFAULT ('') NOT NULL,
    [LastProductionDate]     DATE            NULL,
    [EstPODD]                DATE            NULL,
    [AirFreightByBrand]      BIT             CONSTRAINT [DF_Orders_AirFreightByBrand] DEFAULT ((0)) NOT NULL,
    [AllowanceComboID]       VARCHAR (13)    CONSTRAINT [DF_Orders_AllowanceComboID] DEFAULT ('') NOT NULL,
    [ChangeMemoDate]         DATE            NULL,
    [BuyBack]                VARCHAR (20)    CONSTRAINT [DF_Orders_BuyBack] DEFAULT ('') NOT NULL,
    [BuyBackOrderID]         VARCHAR (13)    CONSTRAINT [DF_Orders_BuyBackOrderID] DEFAULT ('') NOT NULL,
    [ForecastCategory]       VARCHAR (1)     CONSTRAINT [DF_Orders_ForecastCategory] DEFAULT ('') NOT NULL,
    [OnSiteSample]           BIT             CONSTRAINT [DF_Orders_OnSiteSample] DEFAULT ((0)) NOT NULL,
    [PulloutCmplDate]        DATE            NULL,
    [NeedProduction]         BIT             CONSTRAINT [DF_Orders_NeedProduction] DEFAULT ((0)) NOT NULL,
    [IsBuyBack]              BIT             DEFAULT ((0)) NOT NULL,
    [KeepPanels]             BIT             CONSTRAINT [DF_Orders_KeepPanels] DEFAULT ((0)) NOT NULL,
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
    [DelayCode]              VARCHAR (4)     CONSTRAINT [DF_Orders_DelayCode] DEFAULT ('') NOT NULL,
    [DelayDesc]              VARCHAR (100)   CONSTRAINT [DF_Orders_DelayDesc] DEFAULT ('') NOT NULL,
    [HangerPack]             BIT             CONSTRAINT [DF_Orders_HangerPack] DEFAULT ((0)) NOT NULL,
    [CDCodeNew]              VARCHAR (5)     CONSTRAINT [DF_Orders_CDCodeNew] DEFAULT ('') NOT NULL,
    [SizeUnitWeight]         VARCHAR (8)     CONSTRAINT [DF_Orders_SizeUnitWeight] DEFAULT ('') NOT NULL,
    [BrokenNeedles]          BIT             CONSTRAINT [DF_Orders_BrokenNeedles] DEFAULT ((0)) NOT NULL,
    [InTheNameOfBrandID]     VARCHAR (8)     CONSTRAINT [DF_Orders_InTheNameOfBrandID] DEFAULT ('') NOT NULL,
    [LastCTNTransDate]       DATETIME        NULL,
    [LastCTNRecdDate]        DATETIME        NULL,
    [DryRoomRecdDate]        DATETIME        NULL,
    [DryRoomTransDate]       DATETIME        NULL,
    [MdRoomScanDate]         DATETIME        NULL,
    [OrganicCotton]          BIT             CONSTRAINT [DF_Orders_OrganicCotton] DEFAULT ((0)) NOT NULL,
    [QMSMarketFeedback]      NVARCHAR (200)  DEFAULT ('') NOT NULL,
    [DirectShip]             BIT             CONSTRAINT [DF_Orders_DirectShip] DEFAULT ((0)) NOT NULL,
    [ScheETANoReplace]       DATE            NULL,
    [SCHDLETA]               DATE            NULL,
    [Transferdate]           DATETIME        NULL,
    [Max_ScheETAbySP]        DATE            NULL,
    [Sew_ScheETAnoReplace]   DATE            NULL,
    [MaxShipETA_Exclude5x]   DATE            NULL,
    [FtyToClogTransit]       INT             CONSTRAINT [DF_Orders_FtyToClogTransit] DEFAULT ((0)) NOT NULL,
    [ClogToCFATansit]        INT             CONSTRAINT [DF_Orders_ClogToCFATansit] DEFAULT ((0)) NOT NULL,
    [CFAToClogTransit]       INT             CONSTRAINT [DF_Orders_CFAToClogTransit] DEFAULT ((0)) NOT NULL,
    [Customize4]             VARCHAR (30)    CONSTRAINT [DF_Orders_Customize4] DEFAULT ('') NOT NULL,
    [Customize5]             VARCHAR (30)    CONSTRAINT [DF_Orders_Customize5] DEFAULT ('') NOT NULL,
    [OrderCompanyID]   NUMERIC(2, 0) NOT NULL CONSTRAINT [DF_Orders_OrderCompanyID]  DEFAULT ((0)),
    JokerTag bit NOT NULL CONSTRAINT [DF_Orders_JokerTag] DEFAULT 0	,
    HeatSeal bit NOT NULL CONSTRAINT [DF_Orders_HeatSeal] DEFAULT 0	,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([ID] ASC)
);
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CfaCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'除溼室箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'DRYCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預估單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ForecastCategory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PulloutComplete 最後的更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PulloutCmplDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示可以跨Article領用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'IsBuyBackCrossArticle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示可以跨Size領用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'IsBuyBackCrossSizeCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each Cons KPI檢查日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'KpiEachConsCheck'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除此訂單生產成本，1:排除，0不排除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'NonRevenue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - CAB' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'CAB'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - FinalDest' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FinalDest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - Customer_PO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Customer_PO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - AFS_STOCK_CATEGORY' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'AFS_STOCK_CATEGORY'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單在生產過程中是否有出現斷針' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'BrokenNeedles'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Organic Cotton/Recycle Polyester' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OrganicCotton'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCHD LETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'SCHDLETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手動調整資料日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Transferdate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SP#最晚物料抵達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Max_ScheETAbySP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除補料項後, SP#最晚Sewing物料抵達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Sew_ScheETAnoReplace'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除補料項後，SP#實際最晚物料抵達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MaxShipETA_Exclude5x'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單公司別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'OrderCompanyID';

GO
CREATE NONCLUSTERED INDEX [IDX_Orders_BrandStyle] ON [dbo].[Orders]
(
	[BrandID] ASC,
	[StyleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
go
CREATE NONCLUSTERED INDEX [IX_SciDelivery]
    ON [dbo].[Orders]([SciDelivery] ASC, [MDivisionID] ASC, [ID] ASC);


GO
CREATE NONCLUSTERED INDEX [Index_POID]
    ON [dbo].[Orders]([POID] ASC)
    INCLUDE([ID]);


GO
CREATE NONCLUSTERED INDEX [Index_Orders_StyleID]
    ON [dbo].[Orders]([StyleID] ASC);


GO
CREATE NONCLUSTERED INDEX [Index_ForShipmentSchedule]
    ON [dbo].[Orders]([Category] ASC, [PulloutComplete] ASC, [Finished] ASC, [MDivisionID] ASC, [Qty] ASC)
    INCLUDE([ID], [ScanAndPack], [RainwearTestPassed], [SewLine], [InspDate], [DoxType], [CustPONo], [Customize1], [Customize2], [SciDelivery], [SewOffLine], [CRDDate], [BrandID], [StyleID], [BuyMonth], [Dest], [FactoryID], [CustCDID]);


GO
CREATE NONCLUSTERED INDEX [Index_CuttingSP]
    ON [dbo].[Orders]([CuttingSP] ASC)
    INCLUDE([IsForecast], [LocalOrder]);


GO
CREATE NONCLUSTERED INDEX [IDX_Orders_OrderComboID]
    ON [dbo].[Orders]([OrderComboID] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_Orders_MES_EndlineR01]
    ON [dbo].[Orders]([CustPONo] ASC, [StyleUkey] ASC)
    INCLUDE([StyleUnit]);


GO
CREATE NONCLUSTERED INDEX [IDX_Orders_IsRepeatStyleBySewingOutput]
    ON [dbo].[Orders]([ID] ASC, [BrandID] ASC, [StyleID] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_Orders_Category]
    ON [dbo].[Orders]([Category] ASC)
    INCLUDE([ID], [POID]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'自訂欄位5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'Customize5';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'自訂欄位4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'Customize4';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 Carton Transfer 其中一個節點，來源為TradeOTP驗證功能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'JokerTag'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 Carton Transfer 其中一個節點，來源為Trade' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'HeatSeal'
GO