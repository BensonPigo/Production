CREATE TABLE [dbo].[P_CuttingOutputStatistic] (
    [TransferDate]     DATE            CONSTRAINT [DF_P_CuttingOutputStatistic_TransferDate_New] DEFAULT ('') NOT NULL,
    [FactoryID]        VARCHAR (8000)  CONSTRAINT [DF_P_CuttingOutputStatistic_FactoryID_New] DEFAULT ('') NOT NULL,
    [CutRateByDate]    NUMERIC (38, 2) CONSTRAINT [DF_P_CuttingOutputStatistic_CutRateByDate_New] DEFAULT ((0)) NOT NULL,
    [CutRateByMonth]   NUMERIC (38, 2) CONSTRAINT [DF_P_CuttingOutputStatistic_CutRateByMonth_New] DEFAULT ((0)) NOT NULL,
    [CutOutputByDate]  NUMERIC (38, 4) CONSTRAINT [DF_P_CuttingOutputStatistic_CutOutputByDate_New] DEFAULT ((0)) NOT NULL,
    [CutOutputIn7Days] NUMERIC (38, 4) CONSTRAINT [DF_P_CuttingOutputStatistic_CutOutputIn7Days_New] DEFAULT ((0)) NOT NULL,
    [CutDelayIn7Days]  NUMERIC (38, 4) CONSTRAINT [DF_P_CuttingOutputStatistic_CutDelayIn7Days_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]      VARCHAR (8000)  CONSTRAINT [DF_P_CuttingOutputStatistic_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]     DATETIME        NULL,
    [BIStatus]         VARCHAR (8000)  CONSTRAINT [DF_P_CuttingOutputStatistic_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_CuttingOutputStatistic] PRIMARY KEY CLUSTERED ([TransferDate] ASC, [FactoryID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉換日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rate計算BY天' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'CutRateByDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rate計算BY月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'CutRateByMonth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Output計算BY天' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'CutOutputByDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Output計算BY周' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'CutOutputIn7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Delay計算BY周' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'CutDelayIn7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatistic', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingOutputStatistic', @level2type = N'COLUMN', @level2name = N'BIStatus';

