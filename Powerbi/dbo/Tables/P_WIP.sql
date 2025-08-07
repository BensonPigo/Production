CREATE TABLE [dbo].[P_WIP] (
    [MDivisionID]                VARCHAR (8000)  CONSTRAINT [DF_P_WIP_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]                  VARCHAR (8000)  CONSTRAINT [DF_P_WIP_FactoryID_New] DEFAULT ('') NOT NULL,
    [SewLine]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIP_SewLine_New] DEFAULT ('') NOT NULL,
    [BuyerDelivery]              DATE            NULL,
    [SciDelivery]                DATE            NULL,
    [SewInLine]                  DATE            NULL,
    [SewOffLine]                 DATE            NULL,
    [IDD]                        NVARCHAR (1000) CONSTRAINT [DF_P_WIP_IDD_New] DEFAULT ('') NOT NULL,
    [BrandID]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIP_BrandID_New] DEFAULT ('') NOT NULL,
    [SPNO]                       VARCHAR (8000)  CONSTRAINT [DF_P_WIP_SPNO_New] DEFAULT ('') NOT NULL,
    [MasterSP]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIP_MasterSP_New] DEFAULT ('') NOT NULL,
    [IsBuyBack]                  VARCHAR (8000)  CONSTRAINT [DF_P_WIP_IsBuyBack_New] DEFAULT ('') NOT NULL,
    [Cancelled]                  VARCHAR (8000)  CONSTRAINT [DF_P_WIP_Cancelled_New] DEFAULT ('') NOT NULL,
    [CancelledStillNeedProd]     VARCHAR (8000)  CONSTRAINT [DF_P_WIP_CancelledStillNeedProd_New] DEFAULT ('') NOT NULL,
    [Dest]                       VARCHAR (8000)  CONSTRAINT [DF_P_WIP_Dest_New] DEFAULT ('') NOT NULL,
    [StyleID]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIP_StyleID_New] DEFAULT ('') NOT NULL,
    [OrderTypeID]                VARCHAR (8000)  CONSTRAINT [DF_P_WIP_OrderTypeID_New] DEFAULT ('') NOT NULL,
    [ShipMode]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIP_ShipMode_New] DEFAULT ('') NOT NULL,
    [PartialShipping]            VARCHAR (8000)  CONSTRAINT [DF_P_WIP_PartialShipping_New] DEFAULT ('') NOT NULL,
    [OrderNo]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIP_OrderNo_New] DEFAULT ('') NOT NULL,
    [PONO]                       VARCHAR (8000)  CONSTRAINT [DF_P_WIP_PONO_New] DEFAULT ('') NOT NULL,
    [ProgramID]                  NVARCHAR (1000) CONSTRAINT [DF_P_WIP_ProgramID_New] DEFAULT ('') NOT NULL,
    [CdCodeID]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIP_CdCodeID_New] DEFAULT ('') NOT NULL,
    [CDCodeNew]                  VARCHAR (8000)  CONSTRAINT [DF_P_WIP_CDCodeNew_New] DEFAULT ('') NOT NULL,
    [ProductType]                NVARCHAR (1000) CONSTRAINT [DF_P_WIP_ProductType_New] DEFAULT ('') NOT NULL,
    [FabricType]                 NVARCHAR (1000) CONSTRAINT [DF_P_WIP_FabricType_New] DEFAULT ('') NOT NULL,
    [Lining]                     VARCHAR (8000)  CONSTRAINT [DF_P_WIP_Lining_New] DEFAULT ('') NOT NULL,
    [Gender]                     VARCHAR (8000)  CONSTRAINT [DF_P_WIP_Gender_New] DEFAULT ('') NOT NULL,
    [Construction]               NVARCHAR (1000) CONSTRAINT [DF_P_WIP_Construction_New] DEFAULT ('') NOT NULL,
    [KPILETA]                    DATE            NULL,
    [SCHDLETA]                   DATE            NULL,
    [ActMTLETA_Master SP]        DATE            NULL,
    [SewMTLETA_SP]               DATE            NULL,
    [PkgMTLETA_SP]               DATE            NULL,
    [Cpu]                        NUMERIC (38, 3) CONSTRAINT [DF_P_WIP_Cpu_New] DEFAULT ((0)) NOT NULL,
    [TTLCPU]                     NUMERIC (38, 3) CONSTRAINT [DF_P_WIP_TTLCPU_New] DEFAULT ((0)) NOT NULL,
    [CPUClosed]                  NUMERIC (38, 3) CONSTRAINT [DF_P_WIP_CPUClosed_New] DEFAULT ((0)) NOT NULL,
    [CPUBal]                     NUMERIC (38, 3) CONSTRAINT [DF_P_WIP_CPUBal_New] DEFAULT ((0)) NOT NULL,
    [Article]                    NVARCHAR (1000) CONSTRAINT [DF_P_WIP_Article_New] DEFAULT ('') NOT NULL,
    [Qty]                        INT             CONSTRAINT [DF_P_WIP_Qty_New] DEFAULT ((0)) NOT NULL,
    [StandardOutput]             NVARCHAR (1000) CONSTRAINT [DF_P_WIP_StandardOutput_New] DEFAULT ('') NOT NULL,
    [OrigArtwork]                NVARCHAR (1000) CONSTRAINT [DF_P_WIP_OrigArtwork_New] DEFAULT ('') NOT NULL,
    [AddedArtwork]               NVARCHAR (1000) CONSTRAINT [DF_P_WIP_AddedArtwork_New] DEFAULT ('') NOT NULL,
    [BundleArtwork]              NVARCHAR (1000) CONSTRAINT [DF_P_WIP_BundleArtwork_New] DEFAULT ('') NOT NULL,
    [SubProcessDest]             NVARCHAR (1000) CONSTRAINT [DF_P_WIP_SubProcessDest_New] DEFAULT ('') NOT NULL,
    [EstCutDate]                 DATE            NULL,
    [1stCutDate]                 DATE            NULL,
    [CutQty]                     NUMERIC (38, 2) CONSTRAINT [DF_P_WIP_CutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDCutQty]                 NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDCut Qty_New] DEFAULT ((0)) NOT NULL,
    [RFIDSewingLineInQty]        NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDSewingLineInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDLoadingQty]             NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDLoadingQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDEmbFarmInQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDEmbFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDEmbFarmOutQty]          NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDEmbFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDBondFarmInQty]          NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDBondFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDBondFarmOutQty]         NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDBondFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDPrintFarmInQty]         NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDPrintFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDPrintFarmOutQty]        NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDPrintFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDATFarmInQty]            NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDATFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDATFarmOutQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDATFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDPadPrintFarmInQty]      NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDPadPrintFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDPadPrintFarmOutQty]     NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDPadPrintFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDEmbossDebossFarmInQty]  NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDEmbossDebossFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDEmbossDebossFarmOutQty] NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDEmbossDebossFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDHTFarmInQty]            NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDHTFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDHTFarmOutQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDHTFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [SubProcessStatus]           VARCHAR (8000)  CONSTRAINT [DF_P_WIP_SubProcessStatus_New] DEFAULT ('') NOT NULL,
    [EmbQty]                     NUMERIC (38)    CONSTRAINT [DF_P_WIP_EmbQty_New] DEFAULT ((0)) NOT NULL,
    [BondQty]                    NUMERIC (38)    CONSTRAINT [DF_P_WIP_BondQty_New] DEFAULT ((0)) NOT NULL,
    [PrintQty]                   NUMERIC (38)    CONSTRAINT [DF_P_WIP_PrintQty_New] DEFAULT ((0)) NOT NULL,
    [SewQty]                     NUMERIC (38)    CONSTRAINT [DF_P_WIP_SewQty_New] DEFAULT ((0)) NOT NULL,
    [SewBal]                     NUMERIC (38)    CONSTRAINT [DF_P_WIP_SewBal_New] DEFAULT ((0)) NOT NULL,
    [1stSewDate]                 DATE            NULL,
    [LastSewDate]                DATE            NULL,
    [AverageDailyOutput]         INT             CONSTRAINT [DF_P_WIP_AverageDailyOutput_New] DEFAULT ((0)) NOT NULL,
    [EstOfflinedate]             DATE            NULL,
    [ScannedQty]                 INT             CONSTRAINT [DF_P_WIP_ScannedQty_New] DEFAULT ((0)) NOT NULL,
    [PackedRate]                 NUMERIC (38, 2) CONSTRAINT [DF_P_WIP_PackedRate_New] DEFAULT ((0)) NOT NULL,
    [TTLCTN]                     INT             CONSTRAINT [DF_P_WIP_TTLCTN_New] DEFAULT ((0)) NOT NULL,
    [FtyCTN]                     INT             CONSTRAINT [DF_P_WIP_FtyCTN_New] DEFAULT ((0)) NOT NULL,
    [cLogCTN]                    INT             CONSTRAINT [DF_P_WIP_cLogCTN_New] DEFAULT ((0)) NOT NULL,
    [CFACTN]                     INT             CONSTRAINT [DF_P_WIP_CFACTN_New] DEFAULT ((0)) NOT NULL,
    [InspDate]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIP_InspDate_New] DEFAULT ('') NOT NULL,
    [InspResult]                 VARCHAR (8000)  CONSTRAINT [DF_P_WIP_InspResult_New] DEFAULT ('') NOT NULL,
    [CFAName]                    NVARCHAR (1000) CONSTRAINT [DF_P_WIP_CFAName_New] DEFAULT ('') NOT NULL,
    [ActPulloutDate]             DATE            NULL,
    [KPIDeliveryDate]            DATETIME        NULL,
    [UpdateDeliveryReason]       NVARCHAR (1000) CONSTRAINT [DF_P_WIP_UpdateDeliveryReason_New] DEFAULT ('') NOT NULL,
    [PlanDate]                   DATE            NULL,
    [SMR]                        VARCHAR (8000)  CONSTRAINT [DF_P_WIP_SMR_New] DEFAULT ('') NOT NULL,
    [Handle]                     VARCHAR (8000)  CONSTRAINT [DF_P_WIP_Handle_New] DEFAULT ('') NOT NULL,
    [Posmr]                      VARCHAR (8000)  CONSTRAINT [DF_P_WIP_Posmr_New] DEFAULT ('') NOT NULL,
    [PoHandle]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIP_PoHandle_New] DEFAULT ('') NOT NULL,
    [MCHandle]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIP_MCHandle_New] DEFAULT ('') NOT NULL,
    [doxtype]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIP_doxtype_New] DEFAULT ('') NOT NULL,
    [SpecialMark]                VARCHAR (8000)  CONSTRAINT [DF_P_WIP_SpecialMark_New] DEFAULT ('') NOT NULL,
    [GlobalFoundationRange]      BIT             CONSTRAINT [DF_P_WIP_GlobalFoundationRange_New] DEFAULT ((0)) NOT NULL,
    [SampleReason]               VARCHAR (8000)  CONSTRAINT [DF_P_WIP_SampleReason_New] DEFAULT ('') NOT NULL,
    [TMS]                        NUMERIC (38, 3) CONSTRAINT [DF_P_WIP_TMS_New] DEFAULT ((0)) NOT NULL,
    [RFIDAUTFarmInQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDAUTFarmInQty_New] DEFAULT ((0)) NULL,
    [RFIDAUTFarmOutQty]          NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDAUTFarmOutQty_New] DEFAULT ((0)) NULL,
    [RFIDFMFarmInQty]            NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDFMFarmInQty_New] DEFAULT ((0)) NULL,
    [RFIDFMFarmOutQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIP_RFIDFMFarmOutQty_New] DEFAULT ((0)) NULL,
    [BIFactoryID]                VARCHAR (8000)  CONSTRAINT [DF_P_WIP_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]               DATETIME        NULL,
    [BIStatus]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIP_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_WIP] PRIMARY KEY CLUSTERED ([SPNO] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_WIP', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_WIP', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_WIP', @level2type = N'COLUMN', @level2name = N'BIStatus';

