CREATE TABLE [dbo].[P_PPICMasterList_Extend_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnName] [varchar](50) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_PPICMasterList_Extend_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'欄位名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_Extend_History', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_Extend_History', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_Extend_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_Extend_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO