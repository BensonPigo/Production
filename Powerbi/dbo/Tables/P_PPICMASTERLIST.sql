CREATE TABLE [dbo].[P_PPICMASTERLIST] (
    [M]                            VARCHAR (8)     CONSTRAINT [DF_P_PPICMASTERLIST_M] DEFAULT ('') NOT NULL,
    [FactoryID]                    VARCHAR (8)     CONSTRAINT [DF_P_PPICMASTERLIST_FactoryID] DEFAULT ('') NOT NULL,
    [Delivery]                     DATE            NULL,
    [Delivery(YYYYMM)]             VARCHAR (6)     CONSTRAINT [DF_P_PPICMASTERLIST_DELIVERY(YYYYMM)] DEFAULT ('') NOT NULL,
    [Earliest SCIDlv]              DATE            NULL,
    [SCIDlv]                       DATE            NULL,
    [KEY]                          VARCHAR (6)     CONSTRAINT [DF_P_PPICMASTERLIST_KEY] DEFAULT ('') NOT NULL,
    [IDD]                          VARCHAR (500)   NOT NULL,
    [CRD]                          DATE            NULL,
    [CRD(YYYYMM)]                  VARCHAR (6)     CONSTRAINT [DF_P_PPICMASTERLIST_CRD(YYYYMM)] DEFAULT ('') NOT NULL,
    [Check CRD]                    VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_CHECK CRD] DEFAULT ('') NOT NULL,
    [OrdCFM]                       DATE            NULL,
    [CRD-OrdCFM]                   INT             CONSTRAINT [DF_P_PPICMASTERLIST_CRD-ORDCFM] DEFAULT ((0)) NOT NULL,
    [SPNO]                         VARCHAR (13)    CONSTRAINT [DF_P_PPICMASTERLIST_SPNO] DEFAULT ('') NOT NULL,
    [Category]                     VARCHAR (20)    CONSTRAINT [DF_P_PPICMASTERLIST_CATEGORY] DEFAULT ('') NOT NULL,
    [Est. download date]           VARCHAR (16)    CONSTRAINT [DF_P_PPICMASTERLIST_EST. DOWNLOAD DATE] DEFAULT ('') NOT NULL,
    [Buy Back]                     VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_BUY BACK] DEFAULT ('') NOT NULL,
    [Cancelled]                    VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_CANCELLED] DEFAULT ('') NOT NULL,
    [NeedProduction]               VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_NEEDPRODUCTION] DEFAULT ('') NOT NULL,
    [Dest]                         VARCHAR (2)     CONSTRAINT [DF_P_PPICMASTERLIST_DEST] DEFAULT ('') NULL,
    [Style]                        VARCHAR (15)    CONSTRAINT [DF_P_PPICMASTERLIST_STYLE] DEFAULT ('') NOT NULL,
    [Style Name]                   NVARCHAR (50)   CONSTRAINT [DF_P_PPICMASTERLIST_STYLE NAME] DEFAULT ('') NOT NULL,
    [Modular Parent]               VARCHAR (20)    CONSTRAINT [DF_P_PPICMASTERLIST_MODULAR PARENT] DEFAULT ('') NOT NULL,
    [CPUAdjusted]                  NUMERIC (38, 6) CONSTRAINT [DF_P_PPICMASTERLIST_CPUADJUSTED] DEFAULT ((0)) NOT NULL,
    [Similar Style]                VARCHAR (MAX)   CONSTRAINT [DF_P_PPICMASTERLIST_SIMILAR STYLE] DEFAULT ('') NOT NULL,
    [Season]                       VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_SEASON] DEFAULT ('') NOT NULL,
    [Garment L/T]                  NUMERIC (3)     CONSTRAINT [DF_P_PPICMASTERLIST_GARMENT L/T] DEFAULT ((0)) NOT NULL,
    [Order Type]                   VARCHAR (20)    CONSTRAINT [DF_P_PPICMASTERLIST_ORDER TYPE] DEFAULT ('') NOT NULL,
    [Project]                      VARCHAR (5)     CONSTRAINT [DF_P_PPICMASTERLIST_PROJECT] DEFAULT ('') NOT NULL,
    [PackingMethod]                NVARCHAR (50)   CONSTRAINT [DF_P_PPICMASTERLIST_PACKINGMETHOD] DEFAULT ('') NOT NULL,
    [Hanger pack]                  BIT             CONSTRAINT [DF_P_PPICMASTERLIST_HANGER PACK] DEFAULT ((0)) NOT NULL,
    [Order#]                       VARCHAR (30)    CONSTRAINT [DF_P_PPICMASTERLIST_ORDER#] DEFAULT ('') NOT NULL,
    [Buy Month]                    VARCHAR (16)    CONSTRAINT [DF_P_PPICMASTERLIST_BUY MONTH] DEFAULT ('') NOT NULL,
    [PONO]                         VARCHAR (30)    CONSTRAINT [DF_P_PPICMASTERLIST_PONO] DEFAULT ('') NOT NULL,
    [VAS/SHAS]                     VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_VAS/SHAS] DEFAULT ('') NOT NULL,
    [VAS/SHAS Apv.]                DATETIME        NULL,
    [VAS/SHAS Cut Off Date]        DATETIME        NULL,
    [M/Notice Date]                DATETIME        NULL,
    [Est M/Notice Apv.]            DATE            NULL,
    [Tissue]                       VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_TISSUE] DEFAULT ('') NOT NULL,
    [AF by adidas]                 VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_AF BY ADIDAS] DEFAULT ('') NOT NULL,
    [Factory Disclaimer]           NVARCHAR (50)   CONSTRAINT [DF_P_PPICMASTERLIST_FACTORY DISCLAIMER] DEFAULT ('') NOT NULL,
    [Factory Disclaimer Remark]    NVARCHAR (MAX)  CONSTRAINT [DF_P_PPICMASTERLIST_FACTORY DISCLAIMER REMARK] DEFAULT ('') NOT NULL,
    [Approved/Rejected Date]       DATE            NULL,
    [Global Foundation Range]      VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_GLOBAL FOUNDATION RANGE] DEFAULT ('') NOT NULL,
    [Brand]                        VARCHAR (8)     CONSTRAINT [DF_P_PPICMASTERLIST_BRAND] DEFAULT ('') NOT NULL,
    [Cust CD]                      VARCHAR (16)    CONSTRAINT [DF_P_PPICMASTERLIST_CUST CD] DEFAULT ('') NOT NULL,
    [KIT]                          VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_KIT] DEFAULT ('') NOT NULL,
    [Fty Code]                     VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_FTY CODE] DEFAULT ('') NOT NULL,
    [Program]                      NVARCHAR (12)   CONSTRAINT [DF_P_PPICMASTERLIST_PROGRAM] DEFAULT ('') NOT NULL,
    [Non Revenue]                  VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_NON REVENUE] DEFAULT ('') NOT NULL,
    [New CD Code]                  VARCHAR (5)     CONSTRAINT [DF_P_PPICMASTERLIST_NEW CD CODE] DEFAULT ('') NOT NULL,
    [ProductType]                  NVARCHAR (500)  CONSTRAINT [DF_P_PPICMASTERLIST_PRODUCTTYPE] DEFAULT ('') NOT NULL,
    [FabricType]                   NVARCHAR (500)  CONSTRAINT [DF_P_PPICMASTERLIST_FABRICTYPE] DEFAULT ('') NOT NULL,
    [Lining]                       VARCHAR (20)    CONSTRAINT [DF_P_PPICMASTERLIST_LINING] DEFAULT ('') NOT NULL,
    [Gender]                       VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_GENDER] DEFAULT ('') NOT NULL,
    [Construction]                 NVARCHAR (50)   CONSTRAINT [DF_P_PPICMASTERLIST_CONSTRUCTION] DEFAULT ('') NOT NULL,
    [Cpu]                          NUMERIC (38, 6) CONSTRAINT [DF_P_PPICMASTERLIST_CPU] DEFAULT ((0)) NOT NULL,
    [Qty]                          INT             CONSTRAINT [DF_P_PPICMASTERLIST_QTY] DEFAULT ((0)) NOT NULL,
    [FOC Qty]                      INT             CONSTRAINT [DF_P_PPICMASTERLIST_FOC QTY] DEFAULT ((0)) NOT NULL,
    [Total CPU]                    NUMERIC (38, 6) CONSTRAINT [DF_P_PPICMASTERLIST_TOTAL CPU] DEFAULT ((0)) NOT NULL,
    [SewQtyTop]                    INT             CONSTRAINT [DF_P_PPICMASTERLIST_SEWQTYTOP] DEFAULT ((0)) NOT NULL,
    [SewQtyBottom]                 INT             CONSTRAINT [DF_P_PPICMASTERLIST_SEWQTYBOTTOM] DEFAULT ((0)) NOT NULL,
    [SewQtyInner]                  INT             CONSTRAINT [DF_P_PPICMASTERLIST_SEWQTYINNER] DEFAULT ((0)) NOT NULL,
    [SewQtyOuter]                  INT             CONSTRAINT [DF_P_PPICMASTERLIST_SEWQTYOUTER] DEFAULT ((0)) NOT NULL,
    [Total Sewing Output]          INT             CONSTRAINT [DF_P_PPICMASTERLIST_TOTAL SEWING OUTPUT] DEFAULT ((0)) NOT NULL,
    [Cut Qty]                      NUMERIC (38, 6) CONSTRAINT [DF_P_PPICMASTERLIST_CUT QTY] DEFAULT ((0)) NOT NULL,
    [By Comb]                      VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_BY COMB] DEFAULT ('') NOT NULL,
    [Cutting Status]               VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_CUTTING STATUS] DEFAULT ('') NOT NULL,
    [Packing Qty]                  INT             CONSTRAINT [DF_P_PPICMASTERLIST_PACKING QTY] DEFAULT ((0)) NOT NULL,
    [Packing FOC Qty]              INT             CONSTRAINT [DF_P_PPICMASTERLIST_PACKING FOC QTY] DEFAULT ((0)) NOT NULL,
    [Booking Qty]                  INT             CONSTRAINT [DF_P_PPICMASTERLIST_BOOKING QTY] DEFAULT ((0)) NOT NULL,
    [FOC Adj Qty]                  INT             CONSTRAINT [DF_P_PPICMASTERLIST_FOC ADJ QTY] DEFAULT ((0)) NOT NULL,
    [Not FOC Adj Qty]              INT             CONSTRAINT [DF_P_PPICMASTERLIST_NOT FOC ADJ QTY] DEFAULT ((0)) NOT NULL,
    [Total]                        NUMERIC (38, 6) NOT NULL,
    [KPI L/ETA]                    DATE            NULL,
    [PF ETA (SP)]                  DATE            NULL,
    [Pull Forward Remark]          VARCHAR (MAX)   CONSTRAINT [DF_P_PPICMASTERLIST_PULL FORWARD REMARK] DEFAULT ('') NOT NULL,
    [Pack L/ETA]                   DATE            NULL,
    [SCHD L/ETA]                   DATE            NULL,
    [Actual Mtl. ETA]              DATE            NULL,
    [Fab ETA]                      DATE            NULL,
    [Acc ETA]                      DATE            NULL,
    [Sewing Mtl Complt(SP)]        VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_SEWING MTL COMPLT(SP)] DEFAULT ('') NOT NULL,
    [Packing Mtl Complt(SP)]       VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_PACKING MTL COMPLT(SP)] DEFAULT ('') NOT NULL,
    [Sew. MTL ETA (SP)]            DATE            NULL,
    [Pkg. MTL ETA (SP)]            DATE            NULL,
    [MTL Delay]                    VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_MTL DELAY] DEFAULT ('') NOT NULL,
    [MTL Cmplt]                    VARCHAR (3)     CONSTRAINT [DF_P_PPICMASTERLIST_MTL CMPLT] DEFAULT ('') NULL,
    [MTL Cmplt (SP)]               VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_MTL CMPLT (SP)] DEFAULT ('') NOT NULL,
    [Arrive W/H Date]              DATE            NULL,
    [Sewing InLine]                DATE            NULL,
    [Sewing OffLine]               DATE            NULL,
    [1st Sewn Date]                DATE            NULL,
    [Last Sewn Date]               DATE            NULL,
    [First Production Date]        DATE            NULL,
    [Last Production Date]         DATE            NULL,
    [Each Cons Apv Date]           DATETIME        NULL,
    [Est Each Con Apv.]            DATE            NULL,
    [Cutting InLine]               DATE            NULL,
    [Cutting OffLine]              DATE            NULL,
    [Cutting InLine(SP)]           DATE            NULL,
    [Cutting OffLine(SP)]          DATE            NULL,
    [1st Cut Date]                 DATE            NULL,
    [Last Cut Date]                DATE            NULL,
    [Est. Pullout]                 DATE            NULL,
    [Act. Pullout Date]            DATE            NULL,
    [Pullout Qty]                  INT             CONSTRAINT [DF_P_PPICMASTERLIST_PULLOUT QTY] DEFAULT ((0)) NOT NULL,
    [Act. Pullout Times]           INT             CONSTRAINT [DF_P_PPICMASTERLIST_ACT. PULLOUT TIMES] DEFAULT ((0)) NOT NULL,
    [Act. Pullout Cmplt]           VARCHAR (2)     CONSTRAINT [DF_P_PPICMASTERLIST_ACT. PULLOUT CMPLT] DEFAULT ('') NOT NULL,
    [KPI Delivery Date]            DATE            NULL,
    [Update Delivery Reason]       NVARCHAR (500)  CONSTRAINT [DF_P_PPICMASTERLIST_UPDATE DELIVERY REASON] DEFAULT ('') NOT NULL,
    [Plan Date]                    DATE            NULL,
    [Original Buyer Delivery Date] DATE            NULL,
    [SMR]                          VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_SMR] DEFAULT ('') NOT NULL,
    [SMR Name]                     VARCHAR (50)    CONSTRAINT [DF_P_PPICMASTERLIST_SMR NAME] DEFAULT ('') NOT NULL,
    [Handle]                       VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_HANDLE] DEFAULT ('') NOT NULL,
    [Handle Name]                  VARCHAR (50)    CONSTRAINT [DF_P_PPICMASTERLIST_HANDLE NAME] DEFAULT ('') NOT NULL,
    [Posmr]                        VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_POSMR] DEFAULT ('') NOT NULL,
    [Posmr Name]                   VARCHAR (50)    CONSTRAINT [DF_P_PPICMASTERLIST_POSMR NAME] DEFAULT ('') NOT NULL,
    [PoHandle]                     VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_POHANDLE] DEFAULT ('') NOT NULL,
    [PoHandle Name]                VARCHAR (50)    CONSTRAINT [DF_P_PPICMASTERLIST_POHANDLE NAME] DEFAULT ('') NOT NULL,
    [PCHandle]                     VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_PCHANDLE] DEFAULT ('') NOT NULL,
    [PCHandle Name]                VARCHAR (50)    CONSTRAINT [DF_P_PPICMASTERLIST_PCHANDLE NAME] DEFAULT ('') NOT NULL,
    [MCHandle]                     VARCHAR (10)    CONSTRAINT [DF_P_PPICMASTERLIST_MCHANDLE] DEFAULT ('') NOT NULL,
    [MCHandle Name]                VARCHAR (50)    CONSTRAINT [DF_P_PPICMASTERLIST_MCHANDLE NAME] DEFAULT ('') NOT NULL,
    [DoxType]                      VARCHAR (8)     CONSTRAINT [DF_P_PPICMASTERLIST_DOXTYPE] DEFAULT ('') NOT NULL,
    [Packing CTN]                  INT             CONSTRAINT [DF_P_PPICMASTERLIST_PACKING CTN] DEFAULT ((0)) NOT NULL,
    [TTLCTN]                       INT             CONSTRAINT [DF_P_PPICMASTERLIST_TTLCTN] DEFAULT ((0)) NOT NULL,
    [Pack Error CTN]               INT             CONSTRAINT [DF_P_PPICMASTERLIST_PACK ERROR CTN] DEFAULT ((0)) NOT NULL,
    [FtyCTN]                       INT             CONSTRAINT [DF_P_PPICMASTERLIST_FTYCTN] DEFAULT ((0)) NOT NULL,
    [cLog CTN]                     INT             CONSTRAINT [DF_P_PPICMASTERLIST_CLOG CTN] DEFAULT ((0)) NOT NULL,
    [CFA CTN]                      INT             CONSTRAINT [DF_P_PPICMASTERLIST_CFA CTN] DEFAULT ((0)) NOT NULL,
    [cLog Rec. Date]               DATE            NULL,
    [Final Insp. Date]             VARCHAR (500)   NULL,
    [Insp. Result]                 VARCHAR (500)   CONSTRAINT [DF_P_PPICMASTERLIST_INSP. RESULT] DEFAULT ('') NOT NULL,
    [CFA Name]                     VARCHAR (500)   CONSTRAINT [DF_P_PPICMASTERLIST_CFA NAME] DEFAULT ('') NOT NULL,
    [Sewing Line#]                 VARCHAR (60)    CONSTRAINT [DF_P_PPICMASTERLIST_SEWING LINE#] DEFAULT ('') NOT NULL,
    [ShipMode]                     VARCHAR (30)    CONSTRAINT [DF_P_PPICMASTERLIST_SHIPMODE] DEFAULT ('') NOT NULL,
    [SI#]                          VARCHAR (30)    CONSTRAINT [DF_P_PPICMASTERLIST_SI#] DEFAULT ('') NOT NULL,
    [ColorWay]                     NVARCHAR (MAX)  CONSTRAINT [DF_P_PPICMASTERLIST_COLORWAY] DEFAULT ('') NOT NULL,
    [Special Mark]                 VARCHAR (50)    CONSTRAINT [DF_P_PPICMASTERLIST_SPECIAL MARK] DEFAULT ('') NOT NULL,
    [Fty Remark]                   NVARCHAR (MAX)  CONSTRAINT [DF_P_PPICMASTERLIST_FTY REMARK] DEFAULT ('') NOT NULL,
    [Sample Reason]                NVARCHAR (500)  CONSTRAINT [DF_P_PPICMASTERLIST_SAMPLE REASON] DEFAULT ('') NOT NULL,
    [IS MixMarker]                 VARCHAR (25)    CONSTRAINT [DF_P_PPICMASTERLIST_IS MIXMARKER] DEFAULT ('') NOT NULL,
    [Cutting SP]                   VARCHAR (13)    CONSTRAINT [DF_P_PPICMASTERLIST_CUTTING SP] DEFAULT ('') NOT NULL,
    [Rainwear test]                VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_RAINWEAR TEST] DEFAULT ('') NOT NULL,
    [TMS]                          NUMERIC (38, 6) CONSTRAINT [DF_P_PPICMASTERLIST_TMS] DEFAULT ((0)) NOT NULL,
    [MD room scan date]            DATETIME        NULL,
    [Dry Room received date]       DATETIME        NULL,
    [Dry room trans date]          DATETIME        NULL,
    [Last ctn trans date]          DATETIME        NULL,
    [Last ctn recvd date]          DATETIME        NULL,
    [OrganicCotton]                VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_ORGANICCOTTON] DEFAULT ('') NOT NULL,
    [Direct Ship]                  VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_DIRECT SHIP] DEFAULT ('') NOT NULL,
    [StyleCarryover]               VARCHAR (1)     NOT NULL,
    [Ukey]                         BIGINT          IDENTITY (1, 1) NOT NULL,
    [SCHDL/ETA(SP)]                DATE            NULL,
    [SewingMtlETA(SPexclRepl)]     DATE            NULL,
    [ActualMtlETA(exclRepl)]       DATE            NULL,
    [HalfKey]                      VARCHAR (8)     CONSTRAINT [DF_P_PPICMASTERLIST_HalfKey] DEFAULT ('') NOT NULL,
    [DevSample]                    VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_DevSample] DEFAULT ('') NOT NULL,
    [POID]                         VARCHAR (13)    CONSTRAINT [DF_P_PPICMASTERLIST_POID] DEFAULT ('') NOT NULL,
    [KeepPanels]                   VARCHAR (1)     CONSTRAINT [DF_P_PPICMASTERLIST_KeepPanels] DEFAULT ('') NOT NULL,
    [BuyBackReason]                VARCHAR (20)    CONSTRAINT [DF_P_PPICMASTERLIST_BuyBackReason] DEFAULT ('') NOT NULL,
    [SewQtybyRate]                 NUMERIC (38, 6) CONSTRAINT [DF_P_PPICMASTERLIST_SewQtybyRate] DEFAULT ((0)) NOT NULL,
    [Unit]                         VARCHAR (8)     CONSTRAINT [DF_P_PPICMASTERLIST_Unit] DEFAULT ('') NOT NULL,
    [SubconInType]                 VARCHAR (100)   CONSTRAINT [DF_P_PPICMASTERLIST_SubconInType] DEFAULT ('') NOT NULL,
    [Article]                      VARCHAR (500)   CONSTRAINT [DF_P_PPICMASTERLIST_Article] DEFAULT ('') NOT NULL,
    [ProduceRgPMS]                 VARCHAR (3)     CONSTRAINT [DF_P_PPICMASTERLIST_ProduceRgPMS] DEFAULT ('') NOT NULL,
    [Buyerhalfkey]                 VARCHAR (8)     CONSTRAINT [DF_P_PPICMASTERLIST_Buyerhalfkey] DEFAULT ('') NOT NULL,
    [Country]                      VARCHAR (30)    CONSTRAINT [DF_P_PPICMASTERLIST_Country] DEFAULT ('') NOT NULL,
    [Last scan and pack date]      DATETIME        NULL,
    [Third_Party_Insepction]       BIT             CONSTRAINT [DF__P_PPICMAS__Third__5C5C6C48] DEFAULT ((0)) NOT NULL,
    [ColorID]                      NVARCHAR (MAX)  DEFAULT ('') NOT NULL,
    [FtyToClogTransit]             INT             CONSTRAINT [DF_P_PPICMASTERLIST_FtyToClogTransit] DEFAULT ((0)) NOT NULL,
    [ClogToCFATansit]              INT             CONSTRAINT [DF_P_PPICMASTERLIST_ClogToCFATansit] DEFAULT ((0)) NOT NULL,
    [CFAToClogTransit]             INT             CONSTRAINT [DF_P_PPICMASTERLIST_CFAToClogTransit] DEFAULT ((0)) NOT NULL,
    [Shortage]                     NUMERIC(6,0)    CONSTRAINT [DF_P_PPICMasterList_Shortage] DEFAULT 0 NOT NULL
    CONSTRAINT [PK_P_PPICMASTERLIST] PRIMARY KEY CLUSTERED ([Ukey] DESC), 
    [Original CustPO] VARCHAR(30) NOT NULL DEFAULT (''), 
    [Line Aggregator] VARCHAR(30) NOT NULL DEFAULT (''), 
    [JokerTag] VARCHAR NOT NULL DEFAULT (''), 
    [HeatSeal] VARCHAR NOT NULL DEFAULT ('')
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


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mdivision' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'M'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyerDelivery日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Delivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyerDelivery日期(yyyy/MM格式)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Delivery(YYYYMM)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單組合中最早SCI Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Earliest SCIDlv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SCIDlv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KEY'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Intended Delivery Date – 工廠預計出貨日，後續基本上會依照此日期安排出貨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'IDD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CRD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收貨日(yyyy/mm)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CRD(YYYYMM)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢查船務是否到到齊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Check CRD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'業務確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'OrdCFM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'船務到齊日-業務確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CRD-OrdCFM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SPNO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Est. download date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Est. download date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為Buy Back' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Buy Back'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為取消訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cancelled'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單是否取消了仍需生產' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'NeedProduction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.Dest' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Dest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Style Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Modular Parent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Modular Parent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPUAdjusted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CPUAdjusted'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相似款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Similar Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新的Garment list' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Garment L/T'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Order Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ProjectID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Project'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PackingMethod'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單包裝是否懸掛' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Hanger pack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Order#'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Buy Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PONO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直客訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'VAS/SHAS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'mnoti_apv第二階段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'VAS/SHAS Apv.'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直客訂單裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'VAS/SHAS Cut Off Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'製造單確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'M/Notice Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M Notice KPI Date (PMS only)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Est M/Notice Apv.'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'棉紙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Tissue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否空運' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'AF by adidas'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠免責聲明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Factory Disclaimer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠免責聲明註解' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Factory Disclaimer Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'紀錄日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Approved/Rejected Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Global Foundation Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Global Foundation Range'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cust CD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UAWIP中會用到的CustCD對應Kit#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KIT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Fty Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Program'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除此訂單生產成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Non Revenue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'New CD Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'New CD Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產品類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Lining'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Construction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單件耗用產能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cpu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FOC Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單CPU總計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Total CPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量-上衣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtyTop'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量-褲子' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtyBottom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量-內襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtyInner'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量-外衣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtyOuter'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總車縫數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Total Sewing Output'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cut Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1= By Combination 2= by SP#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'By Comb'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cutting Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'非FOC的包裝數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Packing Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FOC的包裝數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Packing FOC Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'保留數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Booking Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FOC訂單調整數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FOC Adj Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'非FOC訂單調整數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Not FOC Adj Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI L/ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KPI L/ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P/F ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PF ETA (SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PF備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Pull Forward Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'14天LOCK的採購單交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SCHD L/ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Actual Mtl. ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Fab ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'輔料預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Acc ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單車縫原料是否到齊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sewing Mtl Complt(SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單包裝原料是否到齊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Packing Mtl Complt(SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫原料預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sew. MTL ETA (SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝原料預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Pkg. MTL ETA (SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料是否延遲到貨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MTL Delay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料到貨狀態By母單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MTL Cmplt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料到貨狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MTL Cmplt (SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達倉庫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Arrive W/H Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sewing InLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠下線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sewing OffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最早車縫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'1st Sewn Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後車縫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last Sewn Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First production date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'First Production Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last production date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last Production Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each-con 確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Each Cons Apv Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each Cons. KPI Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Est Each Con Apv.'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cutting InLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪下線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cutting OffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'初次裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'1st Cut Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last Cut Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠出口日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Est. Pullout'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際出貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Act. Pullout Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總出貨數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Pullout Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Act. Pullout Times'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨完成狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Act. Pullout Cmplt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠KPI日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KPI Delivery Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單日期更新原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Update Delivery Reason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plan Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Plan Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orig. buyer delivery date:' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Original Buyer Delivery Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組長名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SMR Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Handle Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購主管' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Posmr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購主管名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Posmr Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PoHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PoHandle Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排船表的PC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排船表的PC人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PCHandle Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MC Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MC Handle人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MCHandle Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Form A' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'DoxType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Packing CTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bulk/Local訂單總箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'TTLCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝錯誤箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Pack Error CTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠端箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FtyCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clog端箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'cLog CTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CFA CTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clog最後接收日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'cLog Rec. Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Final Insp. Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Insp. Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後檢驗人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CFA Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sewing Line#'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運送方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ShipMode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SI#'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ColorWay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Special Mark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Special Mark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Planning 放置工廠備註用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Fty Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'樣品重製原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sample Reason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單是否混馬克' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'IS MixMarker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cutting SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗測試結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Rainwear test'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time & Motion Study' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'TMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD room scan date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MD room scan date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dry Room received date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Dry Room received date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dry room trans date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Dry room trans date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last ctn trans date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last ctn trans date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last ctn recvd date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last ctn recvd date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否使用有機棉' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'OrganicCotton'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否運送直達' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Direct Ship'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.Max_ScheETAbySP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SCHDL/ETA(SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.Sew_ScheETAnoReplace' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewingMtlETA(SPexclRepl)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.MaxShipETA_Exclude5x' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ActualMtlETA(exclRepl)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI delivery-7 後分為上下半月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'HalfKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderType.IsDevSample' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'DevSample'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.POID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.KeepPanels' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KeepPanels'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.BuyBackReason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'BuyBackReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingOutput_Detail.QAQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtybyRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.StyleUnit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Unit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.SubconInType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SubconInType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order_Article.Article ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCIFty.ProduceRgCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ProduceRgPMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Buyer Delivery-7 後分為上下半月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Buyerhalfkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運送目的地' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Country'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單短交數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList', @level2type=N'COLUMN',@level2name=N'Shortage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'自訂欄位5',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_PPICMASTERLIST',
    @level2type = N'COLUMN',
    @level2name = N'Line Aggregator'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'自訂欄位5',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_PPICMASTERLIST',
    @level2type = N'COLUMN',
    @level2name = N'Original CustPO'