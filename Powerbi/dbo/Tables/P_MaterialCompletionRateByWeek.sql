CREATE TABLE [dbo].[P_MaterialCompletionRateByWeek] (
    [Year]                   INT             CONSTRAINT [DF_P_CFAMasterListRelatedrate_Year_New] DEFAULT ((0)) NOT NULL,
    [WeekNo]                 INT             CONSTRAINT [DF_P_MaterialCompletionRateByWeek_WeekNo_New] DEFAULT ((0)) NOT NULL,
    [FactoryID]              VARCHAR (8000)  CONSTRAINT [DF_P_MaterialCompletionRateByWeek_FactoryID_New] DEFAULT ('') NOT NULL,
    [MaterialCompletionRate] NUMERIC (38, 2) CONSTRAINT [DF_P_MaterialCompletionRateByWeek_MaterialCompletionRate_New] DEFAULT ((0)) NOT NULL,
    [MTLCMP_SPNo]            INT             CONSTRAINT [DF_P_CFAMasterListRelatedrate_MTLCMP_SPNo_New] DEFAULT ((0)) NOT NULL,
    [TTLSPNo]                INT             CONSTRAINT [DF_P_MaterialCompletionRateByWeek_TTLSPNo_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_MaterialCompletionRateByWeek_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]           DATETIME        NULL,
    [BIStatus]               VARCHAR (8000)  CONSTRAINT [DF_P_MaterialCompletionRateByWeek_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_MaterialCompletionRateByWeek] PRIMARY KEY CLUSTERED ([Year] ASC, [WeekNo] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'Year'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年度週數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'WeekNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MTLCMP_SPNo / TTLSPNo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'MaterialCompletionRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'同Year同WeekNo下P_SewingLineScheduleBySP.MTLExport = <OK> 的SPNo總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'MTLCMP_SPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'同Year同WeekNo下SPNo的總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'TTLSPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_MaterialCompletionRateByWeek', @level2type = N'COLUMN', @level2name = N'BIStatus';

