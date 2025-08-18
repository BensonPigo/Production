CREATE TABLE [dbo].[P_QA_R06] (
    [SuppID]                VARCHAR (8000)  NOT NULL,
    [Refno]                 VARCHAR (8000)  NOT NULL,
    [SupplierName]          VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_SupplierName_New] DEFAULT ('') NULL,
    [BrandID]               VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_BrandID_New] DEFAULT ('') NULL,
    [StockQty]              NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_StockQty_New] DEFAULT ('') NULL,
    [TotalInspYds]          NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_TotalInspYds_New] DEFAULT ((0)) NULL,
    [TotalPoCnt]            INT             CONSTRAINT [DF_P_QA_R06_TotalPoCnt_New] DEFAULT ((0)) NULL,
    [TotalDyelot]           INT             CONSTRAINT [DF_P_QA_R06_TotalDyelot_New] DEFAULT ((0)) NULL,
    [TotalDyelotAccepted]   INT             CONSTRAINT [DF_P_QA_R06__New] DEFAULT ((0)) NULL,
    [InspReport]            NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_InspReport_New] DEFAULT ((0)) NULL,
    [TestReport]            NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_TestReport_New] DEFAULT ((0)) NULL,
    [ContinuityCard]        NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_ContinuityCard_New] DEFAULT ((0)) NULL,
    [BulkDyelot]            NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_BulkDyelot_New] DEFAULT ((0)) NULL,
    [TotalPoint]            INT             CONSTRAINT [DF_P_QA_R06_TotalPoint_New] DEFAULT ((0)) NULL,
    [TotalRoll]             INT             CONSTRAINT [DF_P_QA_R06_TotalRoll_New] DEFAULT ((0)) NULL,
    [GradeARoll]            INT             CONSTRAINT [DF_P_QA_R06_GradeARoll_New] DEFAULT ((0)) NULL,
    [GradeBRoll]            INT             CONSTRAINT [DF_P_QA_R06_GradeBRoll_New] DEFAULT ((0)) NULL,
    [GradeCRoll]            INT             CONSTRAINT [DF_P_QA_R06_GradeCRoll_New] DEFAULT ((0)) NULL,
    [Inspected]             NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_Inspected_New] DEFAULT ((0)) NULL,
    [Yds]                   NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_Yds_New] DEFAULT ((0)) NULL,
    [FabricPercent]         NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_FabricPercent_New] DEFAULT ((0)) NULL,
    [FabricLevel]           VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_FabricLevel_New] DEFAULT ('') NULL,
    [Point]                 VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_Point_New] DEFAULT ('') NULL,
    [SHRINKAGEyards]        NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_SHRINKAGEyards_New] DEFAULT ((0)) NULL,
    [SHRINKAGEPercent]      NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_SHRINKAGEPercent_New] DEFAULT ((0)) NULL,
    [SHINGKAGELevel]        VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_SHINGKAGELevel_New] DEFAULT ('') NULL,
    [MIGRATIONyards]        NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_MIGRATIONyards_New] DEFAULT ((0)) NULL,
    [MIGRATIONPercent]      NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_MIGRATIONPercent_New] DEFAULT ((0)) NULL,
    [MIGRATIONLevel]        VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_MIGRATIONLevel_New] DEFAULT ('') NULL,
    [SHADINGyards]          NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_SHADINGyards_New] DEFAULT ((0)) NULL,
    [SHADINGPercent]        NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_SHADINGPercent_New] DEFAULT ((0)) NULL,
    [SHADINGLevel]          VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_SHADINGLevel_New] DEFAULT ('') NULL,
    [ActualYds]             NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_ActualYds_New] DEFAULT ((0)) NULL,
    [LACKINGYARDAGEPercent] NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_LACKINGYARDAGEPercent_New] DEFAULT ((0)) NULL,
    [LACKINGYARDAGELevel]   VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_LACKINGYARDAGELevel_New] DEFAULT ('') NULL,
    [SHORTWIDTH]            NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_SHORTWIDTH_New] DEFAULT ((0)) NULL,
    [SHORTWidthPercent]     NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_SHORTWidthPercent_New] DEFAULT ((0)) NULL,
    [SHORTWIDTHLevel]       VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_SHORTWIDTHLevel_New] DEFAULT ('') NULL,
    [TotalDefectRate]       NUMERIC (38, 2) CONSTRAINT [DF_P_QA_R06_TotalDefectRate_New] DEFAULT ((0)) NULL,
    [TotalLevel]            VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_TotalLevel_New] DEFAULT ('') NULL,
    [WhseArrival]           VARCHAR (8000)  NOT NULL,
    [FactoryID]             VARCHAR (8000)  NOT NULL,
    [Clima]                 BIT             CONSTRAINT [DF_P_QA_R06_Clima_New] DEFAULT ((0)) NULL,
    [POID]                  VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_POID_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]           VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]          DATETIME        NULL,
    [BIStatus]              VARCHAR (8000)  CONSTRAINT [DF_P_QA_R06_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_QA_R06] PRIMARY KEY CLUSTERED ([SuppID] ASC, [Refno] ASC, [FactoryID] ASC, [WhseArrival] ASC, [POID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_R06', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_R06', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_QA_R06', @level2type = N'COLUMN', @level2name = N'BIStatus';

