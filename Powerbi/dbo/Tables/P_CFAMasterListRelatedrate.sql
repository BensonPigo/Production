CREATE TABLE [dbo].[P_CFAMasterListRelatedrate](
	[Buyerdelivery] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[FinalRate] [numeric](5, 2) NOT NULL,
	[FinalInspectionSP] [int] NOT NULL,
	[TotalSP] [int] NOT NULL,
	[PassRate] [numeric](5, 2) NOT NULL,
	[PassSP] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_CFAMasterListRelatedrate] PRIMARY KEY CLUSTERED 
(
	[Buyerdelivery] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_CFAMasterListRelatedrate] ADD  CONSTRAINT [DF_P_CFAMasterListRelatedrate_FinalRate]  DEFAULT ((0)) FOR [FinalRate]
GO

ALTER TABLE [dbo].[P_CFAMasterListRelatedrate] ADD  CONSTRAINT [DF_P_CFAMasterListRelatedrate_FinalInspectionSP]  DEFAULT ((0)) FOR [FinalInspectionSP]
GO

ALTER TABLE [dbo].[P_CFAMasterListRelatedrate] ADD  CONSTRAINT [DF_P_CFAMasterListRelatedrate_TotalSP]  DEFAULT ((0)) FOR [TotalSP]
GO

ALTER TABLE [dbo].[P_CFAMasterListRelatedrate] ADD  CONSTRAINT [DF_P_CFAMasterListRelatedrate_PassRate]  DEFAULT ((0)) FOR [PassRate]
GO

ALTER TABLE [dbo].[P_CFAMasterListRelatedrate] ADD  CONSTRAINT [DF_P_CFAMasterListRelatedrate_PassSP]  DEFAULT ((0)) FOR [PassSP]
GO

ALTER TABLE [dbo].[P_CFAMasterListRelatedrate] ADD  CONSTRAINT [DF_P_CFAMasterListRelatedrate_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'Buyerdelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產廠的FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FinalRate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'FinalRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Buyerdelivery當天已完成檢驗的數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'FinalInspectionSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Buyerdelivery當天的訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'TotalSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pass Rate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'PassRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Buyerdelivery當天 已完成檢驗且 ''Pass'' 的數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'PassSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAMasterListRelatedrate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO