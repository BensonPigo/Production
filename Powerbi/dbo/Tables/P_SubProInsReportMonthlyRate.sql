CREATE TABLE [dbo].[P_SubProInsReportMonthlyRate] (
    [Month]          INT             CONSTRAINT [DF_P_SubProInsReportMonthlyRate_Month_New] DEFAULT ((0)) NOT NULL,
    [FactoryID]      VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_FactoryID_New] DEFAULT ('') NOT NULL,
    [SubprocessRate] NUMERIC (38, 2) CONSTRAINT [DF_P_SubProInsReportMonthlyRate_SubprocessRate_New] DEFAULT ((0)) NOT NULL,
    [TotalPassQty]   INT             CONSTRAINT [DF_P_SubProInsReportMonthlyRate_TotalPassQty_New] DEFAULT ((0)) NOT NULL,
    [TotalQty]       INT             CONSTRAINT [DF_P_SubProInsReportMonthlyRate_TotalQty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]    VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]   DATETIME        NULL,
    [BIStatus]       VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SubProInsReportMonthlyRate] PRIMARY KEY CLUSTERED ([Month] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SubprocessRate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'SubprocessRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上個月Pass的綁包總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'TotalPassQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上個月的綁包總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'TotalQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReportMonthlyRate', @level2type = N'COLUMN', @level2name = N'BIStatus';

