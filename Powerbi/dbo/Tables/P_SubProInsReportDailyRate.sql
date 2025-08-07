CREATE TABLE [dbo].[P_SubProInsReportDailyRate] (
    [InspectionDate] DATE            NOT NULL,
    [FactoryID]      VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReportDailyRate_FactoryID_New] DEFAULT ('') NOT NULL,
    [SubprocessRate] NUMERIC (38, 2) CONSTRAINT [DF_P_SubProInsReportDailyRate_SubprocessRate_New] DEFAULT ((0)) NOT NULL,
    [TotalPassQty]   INT             CONSTRAINT [DF_P_SubProInsReportDailyRate_TotalPassQty_New] DEFAULT ((0)) NOT NULL,
    [TotalQty]       INT             CONSTRAINT [DF_P_SubProInsReportDailyRate_TotalQty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]    VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReportDailyRate_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]   DATETIME        NULL,
    [BIStatus]       VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReportDailyRate_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SubProInsReportDailyRate] PRIMARY KEY CLUSTERED ([InspectionDate] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectionDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'InspectionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SubprocessRate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'SubprocessRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectionDate當天Pass的綁包總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'TotalPassQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectionDate當天綁包總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'TotalQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReportDailyRate', @level2type = N'COLUMN', @level2name = N'BIStatus';

