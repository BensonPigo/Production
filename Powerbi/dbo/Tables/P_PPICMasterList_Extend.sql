CREATE TABLE [dbo].[P_PPICMasterList_Extend](
	[OrderID] [varchar](13) NOT NULL,
	[ColumnName] [varchar](50) NOT NULL,
	[ColumnValue] [numeric](38, 6) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_PPICMasterList_Extend] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_PPICMasterList_Extend] ADD  CONSTRAINT [DF_P_PPICMasterList_Extend_ColumnValue]  DEFAULT ((0)) FOR [ColumnValue]
GO

ALTER TABLE [dbo].[P_PPICMasterList_Extend] ADD  CONSTRAINT [DF_P_PPICMasterList_Extend_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_Extend', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_Extend', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO