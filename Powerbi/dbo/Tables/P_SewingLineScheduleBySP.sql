CREATE TABLE [dbo].[P_SewingLineScheduleBySP] (
    [ID]                     BIGINT          NOT NULL,
    [SewingLineID]           VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SewingLineID_New] DEFAULT ('') NOT NULL,
    [MDivisionID]            VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]              VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_FactoryID_New] DEFAULT ('') NOT NULL,
    [SPNo]                   VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SPNo_New] DEFAULT ('') NOT NULL,
    [CustPONo]               VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_CustPONo_New] DEFAULT ('') NOT NULL,
    [Category]               VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Category_New] DEFAULT ('') NOT NULL,
    [ComboType]              VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_ComboType_New] DEFAULT ('') NOT NULL,
    [SwitchToWorkorder]      VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SwitchToWorkorder_New] DEFAULT ('') NOT NULL,
    [Colorway]               NVARCHAR (MAX)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Colorway_New] DEFAULT ('') NOT NULL,
    [SeasonID]               VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SeasonID_New] DEFAULT ('') NOT NULL,
    [CDCodeNew]              VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_CDCodeNew_New] DEFAULT ('') NOT NULL,
    [ProductType]            NVARCHAR (1000) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_ProductType_New] DEFAULT ('') NOT NULL,
    [MatchFabric]            VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_MatchFabric_New] DEFAULT ('') NOT NULL,
    [FabricType]             NVARCHAR (1000) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_FabricType_New] DEFAULT ('') NOT NULL,
    [Lining]                 VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Lining_New] DEFAULT ('') NOT NULL,
    [Gender]                 VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Gender_New] DEFAULT ('') NOT NULL,
    [Construction]           NVARCHAR (1000) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Construction_New] DEFAULT ('') NOT NULL,
    [StyleID]                VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_StyleID_New] DEFAULT ('') NOT NULL,
    [OrderQty]               INT             CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [AlloQty]                INT             CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_AlloQty_New] DEFAULT ((0)) NOT NULL,
    [CutQty]                 NUMERIC (38, 2) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_CutQty_New] DEFAULT ((0)) NOT NULL,
    [SewingQty]              INT             CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SewingQty_New] DEFAULT ((0)) NOT NULL,
    [ClogQty]                INT             CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_ClogQty_New] DEFAULT ((0)) NOT NULL,
    [FirstCuttingOutputDate] DATE            NULL,
    [InspectionDate]         NVARCHAR (1000) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_InspectionDate_New] DEFAULT ('') NOT NULL,
    [TotalStandardOutput]    NUMERIC (38, 6) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_TotalStandardOutput_New] DEFAULT ((0)) NOT NULL,
    [WorkHour]               NUMERIC (38, 6) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_WorkHour_New] DEFAULT ((0)) NOT NULL,
    [StandardOutputPerHour]  INT             CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_StandardOutputPerHour_New] DEFAULT ((0)) NOT NULL,
    [Efficiency]             NUMERIC (38, 2) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Efficiency_New] DEFAULT ((0)) NOT NULL,
    [KPILETA]                DATE            NULL,
    [PFRemark]               NVARCHAR (MAX)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_PFRemark_New] DEFAULT ('') NOT NULL,
    [ActMTLETA]              DATE            NULL,
    [MTLExport]              VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_MTLExport_New] DEFAULT ('') NOT NULL,
    [CutInLine]              DATE            NULL,
    [Inline]                 DATETIME        NULL,
    [Offline]                DATETIME        NULL,
    [SCIDelivery]            DATE            NULL,
    [BuyerDelivery]          DATE            NULL,
    [CRDDate]                DATE            NULL,
    [CPU]                    NUMERIC (38, 3) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_CPU_New] DEFAULT ((0)) NOT NULL,
    [SewingCPU]              NUMERIC (38, 5) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SewingCPU_New] DEFAULT ((0)) NOT NULL,
    [VASSHAS]                VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_VASSHAS_New] DEFAULT ('') NOT NULL,
    [ShipModeList]           VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_ShipModeList_New] DEFAULT ('') NOT NULL,
    [Destination]            VARCHAR (8000)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Destination_New] DEFAULT ('') NOT NULL,
    [Artwork]                NVARCHAR (MAX)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Artwork_New] DEFAULT ('') NOT NULL,
    [Remarks]                NVARCHAR (MAX)  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Remarks_New] DEFAULT ('') NOT NULL,
    [TTL_PRINTING_PCS]       NUMERIC (38, 6) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_TTL_PRINTING_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_PRINTING_PPU_PPU]   NUMERIC (38, 6) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_TTL_PRINTING_PPU_PPU_New] DEFAULT ((0)) NOT NULL,
    [SubCon]                 NVARCHAR (1000) CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SubCon_New] DEFAULT ('') NOT NULL,
    [SewETA]                 DATE            NULL,
    [TransferDate]           DATETIME        NULL,
    [BIFactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_SewingLineScheduleBySP_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]           DATETIME        NULL,
    [BIStatus]               VARCHAR (8000)  CONSTRAINT [DF_P_SewingLineScheduleBySP_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SewingLineScheduleBySP] PRIMARY KEY CLUSTERED ([ID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineScheduleBySP', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineScheduleBySP', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料轉入時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineScheduleBySP', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO
>>>>>>> develop

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料轉入時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingLineScheduleBySP', @level2type = N'COLUMN', @level2name = N'TransferDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingLineScheduleBySP', @level2type = N'COLUMN', @level2name = N'BIStatus';

