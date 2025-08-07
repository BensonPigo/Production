CREATE TABLE [dbo].[P_DailyAccuCPULoading] (
    [Year]                      VARCHAR (8000)  CONSTRAINT [DF__P_DailyAcc__Year__544D3D79_New] DEFAULT ('') NOT NULL,
    [Month]                     VARCHAR (8000)  CONSTRAINT [DF__P_DailyAc__Month__554161B2_New] DEFAULT ('') NOT NULL,
    [FactoryID]                 VARCHAR (8000)  CONSTRAINT [DF__P_DailyAc__Facto__563585EB_New] DEFAULT ('') NOT NULL,
    [TTLCPULoaded]              INT             CONSTRAINT [DF__P_DailyAc__TTLCP__5729AA24_New] DEFAULT ((0)) NOT NULL,
    [UnfinishedLastMonth]       INT             CONSTRAINT [DF__P_DailyAc__Unfin__581DCE5D_New] DEFAULT ((0)) NOT NULL,
    [FinishedLastMonth]         INT             CONSTRAINT [DF__P_DailyAc__Finis__5911F296_New] DEFAULT ((0)) NOT NULL,
    [CanceledStillNeedProd]     INT             CONSTRAINT [DF__P_DailyAc__Cance__5A0616CF_New] DEFAULT ((0)) NOT NULL,
    [SubconToSisFactory]        INT             CONSTRAINT [DF__P_DailyAc__Subco__5AFA3B08_New] DEFAULT ((0)) NOT NULL,
    [SubconFromSisterFactory]   INT             CONSTRAINT [DF__P_DailyAc__Subco__5BEE5F41_New] DEFAULT ((0)) NOT NULL,
    [PullForwardFromNextMonths] INT             CONSTRAINT [DF__P_DailyAc__PullF__5CE2837A_New] DEFAULT ((0)) NOT NULL,
    [LoadingDelayFromThisMonth] INT             CONSTRAINT [DF__P_DailyAc__Loadi__5DD6A7B3_New] DEFAULT ((0)) NOT NULL,
    [LocalSubconInCPU]          INT             CONSTRAINT [DF__P_DailyAc__Local__5ECACBEC_New] DEFAULT ((0)) NOT NULL,
    [LocalSubconOutCPU]         INT             CONSTRAINT [DF__P_DailyAc__Local__5FBEF025_New] DEFAULT ((0)) NOT NULL,
    [RemainCPUThisMonth]        INT             CONSTRAINT [DF__P_DailyAc__Remai__60B3145E_New] DEFAULT ((0)) NOT NULL,
    [AddName]                   VARCHAR (8000)  CONSTRAINT [DF__P_DailyAc__AddNa__61A73897_New] DEFAULT ('') NOT NULL,
    [AddDate]                   DATETIME        NULL,
    [EditName]                  VARCHAR (8000)  CONSTRAINT [DF__P_DailyAc__EditN__629B5CD0_New] DEFAULT ('') NOT NULL,
    [EditDate]                  DATETIME        NULL,
    [Date]                      VARCHAR (8000)  CONSTRAINT [DF__P_DailyAcc__Date__638F8109_New] DEFAULT ('') NOT NULL,
    [WeekDay]                   VARCHAR (8000)  CONSTRAINT [DF__P_DailyAc__WeekD__6483A542_New] DEFAULT ('') NOT NULL,
    [DailyCPULoading]           INT             CONSTRAINT [DF__P_DailyAc__Daily__6577C97B_New] DEFAULT ((0)) NOT NULL,
    [NewTarget]                 INT             CONSTRAINT [DF__P_DailyAc__NewTa__666BEDB4_New] DEFAULT ((0)) NOT NULL,
    [ActCPUPerformed]           DECIMAL (18, 3) CONSTRAINT [DF__P_DailyAc__ActCP__676011ED_New] DEFAULT ((0)) NOT NULL,
    [DailyCPUVarience]          INT             CONSTRAINT [DF__P_DailyAc__Daily__68543626_New] DEFAULT ((0)) NOT NULL,
    [AccuLoading]               INT             CONSTRAINT [DF__P_DailyAc__AccuL__69485A5F_New] DEFAULT ((0)) NOT NULL,
    [AccuActCPUPerformed]       INT             CONSTRAINT [DF__P_DailyAc__AccuA__6A3C7E98_New] DEFAULT ((0)) NOT NULL,
    [AccuCPUVariance]           INT             CONSTRAINT [DF__P_DailyAc__AccuC__6B30A2D1_New] DEFAULT ((0)) NOT NULL,
    [LeftWorkDays]              INT             CONSTRAINT [DF__P_DailyAc__LeftW__6C24C70A_New] DEFAULT ((0)) NOT NULL,
    [AvgWorkhours]              DECIMAL (18, 2) CONSTRAINT [DF__P_DailyAc__AvgWo__6D18EB43_New] DEFAULT ((0)) NOT NULL,
    [PPH]                       DECIMAL (18, 2) CONSTRAINT [DF__P_DailyAccu__PPH__6E0D0F7C_New] DEFAULT ((0)) NOT NULL,
    [Direct]                    INT             CONSTRAINT [DF__P_DailyAc__Direc__6F0133B5_New] DEFAULT ((0)) NOT NULL,
    [Active]                    INT             CONSTRAINT [DF__P_DailyAc__Activ__6FF557EE_New] DEFAULT ((0)) NOT NULL,
    [VPH]                       DECIMAL (18, 2) CONSTRAINT [DF__P_DailyAccu__VPH__70E97C27_New] DEFAULT ((0)) NOT NULL,
    [ManpowerRatio]             DECIMAL (18, 2) CONSTRAINT [DF__P_DailyAc__Manpo__71DDA060_New] DEFAULT ((0)) NOT NULL,
    [LineNo]                    INT             CONSTRAINT [DF__P_DailyAc__LineN__72D1C499_New] DEFAULT ((0)) NOT NULL,
    [LineManpower]              INT             CONSTRAINT [DF__P_DailyAc__LineM__73C5E8D2_New] DEFAULT ((0)) NOT NULL,
    [GPH]                       DECIMAL (18, 2) CONSTRAINT [DF__P_DailyAccu__GPH__74BA0D0B_New] DEFAULT ((0)) NOT NULL,
    [SPH]                       DECIMAL (18, 2) CONSTRAINT [DF__P_DailyAccu__SPH__75AE3144_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]               VARCHAR (8000)  CONSTRAINT [DF_P_DailyAccuCPULoading_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]              DATETIME        NULL,
    [BIStatus]                  VARCHAR (8000)  CONSTRAINT [DF_P_DailyAccuCPULoading_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_DailyAccuCPULoading] PRIMARY KEY CLUSTERED ([Year] ASC, [Month] ASC, [FactoryID] ASC, [Date] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_DailyAccuCPULoading', @level2type = N'COLUMN', @level2name = N'BIStatus';

