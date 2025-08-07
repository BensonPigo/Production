CREATE TABLE [dbo].[P_SubprocessWIP] (
    [Bundleno]                    VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Bundleno_New] DEFAULT ('') NOT NULL,
    [RFIDProcessLocationID]       VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_RFIDProcessLocationID_New] DEFAULT ('') NOT NULL,
    [EXCESS]                      VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_EXCESS_New] DEFAULT ('') NOT NULL,
    [FabricKind]                  VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_FabricKind_New] DEFAULT ('') NOT NULL,
    [CutRef]                      VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_CutRef_New] DEFAULT ('') NOT NULL,
    [Sp]                          VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Sp_New] DEFAULT ('') NOT NULL,
    [MasterSP]                    VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_MasterSP_New] DEFAULT ('') NOT NULL,
    [M]                           VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_M_New] DEFAULT ('') NOT NULL,
    [FactoryID]                   VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Factory_New] DEFAULT ('') NOT NULL,
    [Category]                    VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Category_New] DEFAULT ('') NOT NULL,
    [Program]                     NVARCHAR (1000) CONSTRAINT [DF_P_SubprocessWIP_Program_New] DEFAULT ('') NOT NULL,
    [Style]                       VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Style_New] DEFAULT ('') NOT NULL,
    [Season]                      VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Season_New] DEFAULT ('') NOT NULL,
    [Brand]                       VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Brand_New] DEFAULT ('') NOT NULL,
    [Comb]                        VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Comb_New] DEFAULT ('') NOT NULL,
    [CutNo]                       NUMERIC (38)    CONSTRAINT [DF_P_SubprocessWIP_CutNo_New] DEFAULT ((0)) NOT NULL,
    [FabPanelCode]                VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_FabPanelCode_New] DEFAULT ('') NOT NULL,
    [Article]                     VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Article_New] DEFAULT ('') NOT NULL,
    [Color]                       VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Color_New] DEFAULT ('') NOT NULL,
    [ScheduledLineID]             VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_ScheduledLineID_New] DEFAULT ('') NOT NULL,
    [ScannedLineID]               VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_ScannedLineID_New] DEFAULT ('') NOT NULL,
    [Cell]                        VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Cell_New] DEFAULT ('') NOT NULL,
    [Pattern]                     VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Pattern_New] DEFAULT ('') NOT NULL,
    [PtnDesc]                     NVARCHAR (1000) CONSTRAINT [DF_P_SubprocessWIP_PtnDesc_New] DEFAULT ('') NOT NULL,
    [Group]                       NUMERIC (38)    CONSTRAINT [DF_P_SubprocessWIP_Group_New] DEFAULT ((0)) NOT NULL,
    [Size]                        VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Size_New] DEFAULT ('') NOT NULL,
    [Artwork]                     NVARCHAR (1000) CONSTRAINT [DF_P_SubprocessWIP_Artwork_New] DEFAULT ('') NOT NULL,
    [Qty]                         NUMERIC (38)    CONSTRAINT [DF_P_SubprocessWIP_Qty_New] DEFAULT ((0)) NOT NULL,
    [SubprocessID]                VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_SubprocessID_New] DEFAULT ('') NOT NULL,
    [PostSewingSubProcess]        VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_PostSewingSubProcess_New] DEFAULT ('') NOT NULL,
    [NoBundleCardAfterSubprocess] VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_NoBundleCardAfterSubprocess_New] DEFAULT ('') NOT NULL,
    [Location]                    VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Location_New] DEFAULT ('') NOT NULL,
    [BundleCreateDate]            DATE            NULL,
    [BuyerDeliveryDate]           DATE            NULL,
    [SewingInline]                DATE            NULL,
    [SubprocessQAInspectionDate]  DATE            NULL,
    [InTime]                      DATETIME        NULL,
    [OutTime]                     DATETIME        NULL,
    [POSupplier]                  NVARCHAR (1000) CONSTRAINT [DF_P_SubprocessWIP_POSupplier_New] DEFAULT ('') NOT NULL,
    [AllocatedSubcon]             NVARCHAR (1000) CONSTRAINT [DF_P_SubprocessWIP_AllocatedSubcon_New] DEFAULT ('') NOT NULL,
    [AvgTime]                     NUMERIC (38, 9) CONSTRAINT [DF_P_SubprocessWIP_AvgTime_New] DEFAULT ((0)) NOT NULL,
    [TimeRange]                   NVARCHAR (1000) CONSTRAINT [DF_P_SubprocessWIP_TimeRange_New] DEFAULT ('') NOT NULL,
    [EstimatedCutDate]            DATE            NULL,
    [CuttingOutputDate]           DATE            NULL,
    [Item]                        VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_Item_New] DEFAULT ('') NOT NULL,
    [PanelNo]                     VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_PanelNo_New] DEFAULT ('') NOT NULL,
    [CutCellID]                   VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_CutCellID_New] DEFAULT ('') NOT NULL,
    [SpreadingNo]                 NVARCHAR (1000) CONSTRAINT [DF_P_SubprocessWIP_SpreadingNo_New] DEFAULT ('') NOT NULL,
    [LastSewDate]                 DATE            NULL,
    [SewQty]                      INT             CONSTRAINT [DF_P_SubprocessWIP_SewQty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]                 VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                DATETIME        NULL,
    [BIStatus]                    VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessWIP_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SubprocessWIP] PRIMARY KEY CLUSTERED ([Bundleno] ASC, [RFIDProcessLocationID] ASC, [Sp] ASC, [Pattern] ASC, [SubprocessID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessWIP', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessWIP', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubprocessWIP', @level2type = N'COLUMN', @level2name = N'BIStatus';

