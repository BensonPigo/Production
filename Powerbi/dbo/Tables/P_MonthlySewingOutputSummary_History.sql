CREATE TABLE [dbo].[P_MonthlySewingOutputSummary_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[Fty] [varchar](8) NOT NULL,
	[Period] [varchar](6) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_MonthlySewingOutputSummary_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Output Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary_History', @level2type=N'COLUMN',@level2name=N'Fty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LastDatePerMonth的年月(YYYYMM)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary_History', @level2type=N'COLUMN',@level2name=N'Period'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO