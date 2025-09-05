CREATE TABLE [dbo].[P_LoadingProductionOutput] (
    [MDivisionID]            VARCHAR (8000)  NULL,
    [FtyZone]                VARCHAR (8000)  NULL,
    [FactoryID]              VARCHAR (8000)  NOT NULL,
    [BuyerDelivery]          DATE            NULL,
    [SciDelivery]            DATE            NULL,
    [SCIKey]                 NVARCHAR (4000) NULL,
    [SCIKeyHalf]             NVARCHAR (4000) NULL,
    [BuyerKey]               NVARCHAR (4000) NULL,
    [BuyerKeyHalf]           NVARCHAR (4000) NULL,
    [SPNO]                   VARCHAR (8000)  NULL,
    [Category]               VARCHAR (8000)  NULL,
    [Cancelled]              VARCHAR (8000)  NOT NULL,
    [IsCancelNeedProduction] VARCHAR (8000)  NOT NULL,
    [PartialShipment]        VARCHAR (8000)  NULL,
    [LastBuyerDelivery]      DATE            NULL,
    [StyleID]                VARCHAR (8000)  NULL,
    [SeasonID]               VARCHAR (8000)  NULL,
    [CustPONO]               VARCHAR (8000)  NULL,
    [BrandID]                VARCHAR (8000)  NULL,
    [CPU]                    NUMERIC (38, 3) NULL,
    [Qty]                    INT             NULL,
    [FOCQty]                 INT             NULL,
    [PulloutQty]             INT             NULL,
    [OrderShortageCPU]       NUMERIC (38, 4) NULL,
    [TotalCPU]               NUMERIC (38, 4) NULL,
    [SewingOutput]           NUMERIC (38, 6) NULL,
    [SewingOutputCPU]        NUMERIC (38, 6) NULL,
    [BalanceQty]             NUMERIC (38, 6) NULL,
    [BalanceCPU]             NUMERIC (38, 6) NULL,
    [BalanceCPUIrregular]    NUMERIC (38, 6) NULL,
    [SewLine]                VARCHAR (8000)  NULL,
    [Dest]                   VARCHAR (8000)  NULL,
    [OrderTypeID]            VARCHAR (8000)  NULL,
    [ProgramID]              VARCHAR (8000)  NULL,
    [CdCodeID]               VARCHAR (8000)  NULL,
    [ProductionFamilyID]     VARCHAR (8000)  NULL,
    [FtyGroup]               VARCHAR (8000)  NULL,
    [PulloutComplete]        VARCHAR (8000)  NOT NULL,
    [SewInLine]              DATE            NULL,
    [SewOffLine]             DATE            NULL,
    [TransFtyZone]           VARCHAR (8000)  NULL,
    [CDCodeNew]              VARCHAR (8000)  NULL,
    [ProductType]            NVARCHAR (1000) NULL,
    [FabricType]             NVARCHAR (1000) NULL,
    [Lining]                 VARCHAR (8000)  NULL,
    [Gender]                 VARCHAR (8000)  NULL,
    [Construction]           NVARCHAR (1000) NULL,
    [FM Sister]              VARCHAR (8000)  NULL,
    [Sample Group]           NVARCHAR (1000) NULL,
    [Order Reason]           NVARCHAR (1000) NULL,
    [Ukey]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [BuyBackReason]          VARCHAR (8000)  CONSTRAINT [DF_P_LoadingProductionOutput_BuyBackReason_New] DEFAULT ('') NOT NULL,
    [LastProductionDate]     DATE            NULL,
    [CRDDate]                DATE            NULL,
    [BuyerMonthHalf]         NVARCHAR (4000) NULL,
    [BIFactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_LoadingProductionOutput_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]           DATETIME        NULL,
    [BIStatus]               VARCHAR (8000)  CONSTRAINT [DF_P_LoadingProductionOutput_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_LoadingProductionOutput] PRIMARY KEY CLUSTERED ([Ukey] ASC, [FactoryID] ASC)
);



GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyBack Reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'BuyBackReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LastProductionDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'LastProductionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CRD date.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'CRDDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingProductionOutput', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_LoadingProductionOutput', @level2type = N'COLUMN', @level2name = N'BIStatus';

