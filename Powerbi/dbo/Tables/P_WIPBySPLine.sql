CREATE TABLE [dbo].[P_WIPBySPLine] (
    [MDivisionID]                VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]                  VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_FactoryID_New] DEFAULT ('') NOT NULL,
    [SewingLineID]               VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_SewingLineID_New] DEFAULT ('') NOT NULL,
    [BuyerDelivery]              DATE            NULL,
    [SCIDelivery]                DATE            NULL,
    [SewInLine]                  DATE            NULL,
    [SewOffLine]                 DATE            NULL,
    [IDD]                        NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_IDD_New] DEFAULT ('') NOT NULL,
    [BrandID]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_BrandID_New] DEFAULT ('') NOT NULL,
    [SPNO]                       VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_SPNO_New] DEFAULT ('') NOT NULL,
    [MasterSP]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_MasterSP_New] DEFAULT ('') NOT NULL,
    [IsBuyBack]                  VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_IsBuyBack_New] DEFAULT ('') NOT NULL,
    [Cancelled]                  VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_Cancelled_New] DEFAULT ('') NOT NULL,
    [CancelledStillNeedProd]     VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_CancelledStillNeedProd_New] DEFAULT ('') NOT NULL,
    [Dest]                       VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_Dest_New] DEFAULT ('') NOT NULL,
    [StyleID]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_StyleID_New] DEFAULT ('') NOT NULL,
    [OrderTypeID]                VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_OrderTypeID_New] DEFAULT ('') NOT NULL,
    [ShipMode]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_ShipMode_New] DEFAULT ('') NOT NULL,
    [PartialShipping]            VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_PartialShipping_New] DEFAULT ('') NOT NULL,
    [OrderNo]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_OrderNo_New] DEFAULT ('') NOT NULL,
    [PONO]                       VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_PONO_New] DEFAULT ('') NOT NULL,
    [ProgramID]                  NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_ProgramID_New] DEFAULT ('') NOT NULL,
    [CDCodeID]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_CDCodeID_New] DEFAULT ('') NOT NULL,
    [CDCodeNew]                  VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_CDCodeNew_New] DEFAULT ('') NOT NULL,
    [ProductType]                NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_ProductType_New] DEFAULT ('') NOT NULL,
    [FabricType]                 NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_FabricType_New] DEFAULT ('') NOT NULL,
    [Lining]                     VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_Lining_New] DEFAULT ('') NOT NULL,
    [Gender]                     VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_Gender_New] DEFAULT ('') NOT NULL,
    [Construction]               NVARCHAR (1000) NOT NULL,
    [KPILETA]                    DATE            NULL,
    [SCHDLETA]                   DATE            NULL,
    [ActMTLETA_MasterSP]         DATE            NULL,
    [SewMTLETA_SP]               DATE            NULL,
    [PkgMTLETA_SP]               DATE            NULL,
    [Cpu]                        NUMERIC (38, 3) CONSTRAINT [DF_P_WIPBySPLine_Cpu_New] DEFAULT ((0)) NOT NULL,
    [TTLCPU]                     NUMERIC (38, 3) CONSTRAINT [DF_P_WIPBySPLine_TTLCPU_New] DEFAULT ((0)) NOT NULL,
    [CPUClosed]                  NUMERIC (38, 3) CONSTRAINT [DF_P_WIPBySPLine_CPUClosed_New] DEFAULT ((0)) NOT NULL,
    [CPUBal]                     NUMERIC (38, 3) CONSTRAINT [DF_P_WIPBySPLine_CPUBal_New] DEFAULT ((0)) NOT NULL,
    [Article]                    NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_Article_New] DEFAULT ('') NOT NULL,
    [Qty]                        INT             CONSTRAINT [DF_P_WIPBySPLine_Qty_New] DEFAULT ((0)) NOT NULL,
    [StandardOutput]             NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_StandardOutput_New] DEFAULT ('') NOT NULL,
    [OrigArtwork]                NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_OrigArtwork_New] DEFAULT ('') NOT NULL,
    [AddedArtwork]               NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_AddedArtwork_New] DEFAULT ('') NOT NULL,
    [BundleArtwork]              NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_BundleArtwork_New] DEFAULT ('') NOT NULL,
    [SubProcessDest]             NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_SubProcessDest_New] DEFAULT ('') NOT NULL,
    [EstCutDate]                 DATE            NULL,
    [1stCutDate]                 DATE            NULL,
    [CutQty]                     NUMERIC (38, 2) CONSTRAINT [DF_P_WIPBySPLine_CutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDCutQty]                 NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDCutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDSewingLineInQty]        NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDSewingLineInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDLoadingQty]             NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDLoadingQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDEmbFarmInQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDEmbFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDEmbFarmOutQty]          NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDEmbFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDBondFarmInQty]          NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDBondFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDBondFarmOutQty]         NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDBondFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDPrintFarmInQty]         NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDPrintFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDPrintFarmOutQty]        NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDPrintFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDATFarmInQty]            NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDATFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDATFarmOutQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDATFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDPadPrintFarmInQty]      NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDPadPrintFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDPadPrintFarmOutQty]     NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDPadPrintFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDEmbossDebossFarmInQty]  NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDEmbossDebossFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDEmbossDebossFarmOutQty] NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDEmbossDebossFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDHTFarmInQty]            NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDHTFarmInQty_New] DEFAULT ((0)) NOT NULL,
    [RFIDHTFarmOutQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDHTFarmOutQty_New] DEFAULT ((0)) NOT NULL,
    [SubProcessStatus]           VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_SubProcessStatus_New] DEFAULT ('') NOT NULL,
    [EmbQty]                     NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_EmbQty_New] DEFAULT ((0)) NOT NULL,
    [BondQty]                    NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_BondQty_New] DEFAULT ((0)) NOT NULL,
    [PrintQty]                   NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_PrintQty_New] DEFAULT ((0)) NOT NULL,
    [SewQty]                     NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_SewQty_New] DEFAULT ((0)) NOT NULL,
    [SewBal]                     NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_SewBal_New] DEFAULT ((0)) NOT NULL,
    [1stSewDate]                 DATE            NULL,
    [LastSewDate]                DATE            NULL,
    [AverageDailyOutput]         INT             CONSTRAINT [DF_P_WIPBySPLine_AverageDailyOutput_New] DEFAULT ((0)) NOT NULL,
    [EstOfflinedate]             DATE            NULL,
    [ScannedQty]                 INT             CONSTRAINT [DF_P_WIPBySPLine_ScannedQty_New] DEFAULT ((0)) NOT NULL,
    [PackedRate]                 NUMERIC (38, 4) CONSTRAINT [DF_P_WIPBySPLine_PackedRate_New] DEFAULT ((0)) NOT NULL,
    [TTLCTN]                     INT             CONSTRAINT [DF_P_WIPBySPLine_TTLCTN_New] DEFAULT ((0)) NOT NULL,
    [FtyCTN]                     INT             CONSTRAINT [DF_P_WIPBySPLine_FtyCTN_New] DEFAULT ((0)) NOT NULL,
    [cLogCTN]                    INT             CONSTRAINT [DF_P_WIPBySPLine_cLogCTN_New] DEFAULT ((0)) NOT NULL,
    [CFACTN]                     INT             CONSTRAINT [DF_P_WIPBySPLine_CFACTN_New] DEFAULT ((0)) NOT NULL,
    [InspDate]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_InspDate_New] DEFAULT ('') NOT NULL,
    [InspResult]                 VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_InspResult_New] DEFAULT ('') NOT NULL,
    [CFAName]                    NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_CFAName_New] DEFAULT ('') NOT NULL,
    [ActPulloutDate]             DATE            NULL,
    [KPIDeliveryDate]            DATETIME        NULL,
    [UpdateDeliveryReason]       NVARCHAR (1000) CONSTRAINT [DF_P_WIPBySPLine_UpdateDeliveryReason_New] DEFAULT ('') NOT NULL,
    [PlanDate]                   DATE            NULL,
    [SMR]                        VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_SMR_New] DEFAULT ('') NOT NULL,
    [Handle]                     VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_Handle_New] DEFAULT ('') NOT NULL,
    [Posmr]                      VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_Posmr_New] DEFAULT ('') NOT NULL,
    [PoHandle]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_PoHandle_New] DEFAULT ('') NOT NULL,
    [MCHandle]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_MCHandle_New] DEFAULT ('') NOT NULL,
    [doxtype]                    VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_doxtype_New] DEFAULT ('') NOT NULL,
    [SpecialMark]                VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_SpecialMark_New] DEFAULT ('') NOT NULL,
    [GlobalFoundationRange]      BIT             CONSTRAINT [DF_P_WIPBySPLine_GlobalFoundationRange_New] DEFAULT ((0)) NOT NULL,
    [SampleReason]               VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_SampleReason_New] DEFAULT ('') NOT NULL,
    [TMS]                        NUMERIC (38, 3) CONSTRAINT [DF_P_WIPBySPLine_TMS_New] DEFAULT ((0)) NOT NULL,
    [UKey]                       BIGINT          IDENTITY (1, 1) NOT NULL,
    [RFIDAUTFarmInQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDAUTFarmInQty_New] DEFAULT ((0)) NULL,
    [RFIDAUTFarmOutQty]          NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDAUTFarmOutQty_New] DEFAULT ((0)) NULL,
    [RFIDFMFarmInQty]            NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDFMFarmInQty_New] DEFAULT ((0)) NULL,
    [RFIDFMFarmOutQty]           NUMERIC (38)    CONSTRAINT [DF_P_WIPBySPLine_RFIDFMFarmOutQty_New] DEFAULT ((0)) NULL,
    [BIFactoryID]                VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]               DATETIME        NULL,
    [BIStatus]                   VARCHAR (8000)  CONSTRAINT [DF_P_WIPBySPLine_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_WIPBySPLine] PRIMARY KEY CLUSTERED ([UKey] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_WIPBySPLine', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_WIPBySPLine', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_WIPBySPLine', @level2type = N'COLUMN', @level2name = N'BIStatus';

