CREATE TABLE [dbo].[P_ReplacementReport] (
    [ID]                    VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_ID_New] DEFAULT ('') NOT NULL,
    [Type]                  VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_Type_New] DEFAULT ('') NOT NULL,
    [MDivisionID]           VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]             VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_FactoryID_New] DEFAULT ('') NOT NULL,
    [SPNo]                  VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_SPNo_New] DEFAULT ('') NOT NULL,
    [Style]                 VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_Style_New] DEFAULT ('') NOT NULL,
    [Season]                VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_Season_New] DEFAULT ('') NOT NULL,
    [Brand]                 VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_Brand_New] DEFAULT ('') NOT NULL,
    [Status]                VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_Status_New] DEFAULT ('') NOT NULL,
    [Cdate]                 DATE            NULL,
    [FtyApvDate]            DATE            NULL,
    [CompleteDate]          DATE            NULL,
    [LockDate]              DATE            NULL,
    [Responsibility]        NVARCHAR (1000) CONSTRAINT [DF_P_ReplacementReport_Responsibility_New] DEFAULT ('') NOT NULL,
    [TtlEstReplacementAMT]  NUMERIC (38, 2) CONSTRAINT [DF_P_ReplacementReport_TtlEstReplacementAMT_New] DEFAULT ((0)) NOT NULL,
    [RMtlUS]                NUMERIC (38, 2) CONSTRAINT [DF_P_ReplacementReport_RMtlUS_New] DEFAULT ((0)) NOT NULL,
    [ActFreightUS]          NUMERIC (38, 2) CONSTRAINT [DF_P_ReplacementReport_ActFreightUS_New] DEFAULT ((0)) NOT NULL,
    [EstFreightUS]          NUMERIC (38, 2) CONSTRAINT [DF_P_ReplacementReport_EstFreightUS_New] DEFAULT ((0)) NOT NULL,
    [SurchargeUS]           NUMERIC (38, 2) CONSTRAINT [DF_P_ReplacementReport_SurchargeUS_New] DEFAULT ((0)) NOT NULL,
    [TotalUS]               NUMERIC (38, 2) CONSTRAINT [DF_P_ReplacementReport_TotalUS_New] DEFAULT ((0)) NOT NULL,
    [ResponsibilityFty]     VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_ResponsibilityFty_New] DEFAULT ('') NOT NULL,
    [ResponsibilityDept]    VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_ResponsibilityDept_New] DEFAULT ('') NOT NULL,
    [ResponsibilityPercent] NUMERIC (38, 2) CONSTRAINT [DF_P_ReplacementReport_ResponsibilityPercent_New] DEFAULT ((0)) NOT NULL,
    [ShareAmount]           NUMERIC (38, 2) CONSTRAINT [DF_P_ReplacementReport_ShareAmount_New] DEFAULT ((0)) NOT NULL,
    [VoucherNo]             VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_VoucherNo_New] DEFAULT ('') NOT NULL,
    [VoucherDate]           DATE            NULL,
    [POSMR]                 VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_POSMR_New] DEFAULT ('') NOT NULL,
    [POHandle]              VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_POHandle_New] DEFAULT ('') NOT NULL,
    [PCSMR]                 VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_PCSMR_New] DEFAULT ('') NOT NULL,
    [PCHandle]              VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_PCHandle_New] DEFAULT ('') NOT NULL,
    [Prepared]              VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_Prepared_New] DEFAULT ('') NOT NULL,
    [PPIC/Factory mgr]      VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_PPIC/Factory mgr_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]           VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]          DATETIME        NULL,
    [BIStatus]              VARCHAR (8000)  CONSTRAINT [DF_P_ReplacementReport_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ReplacementReport] PRIMARY KEY CLUSTERED ([ID] ASC, [FactoryID] ASC, [ResponsibilityFty] ASC, [ResponsibilityDept] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ReplacementReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ReplacementReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ReplacementReport', @level2type = N'COLUMN', @level2name = N'BIStatus';

