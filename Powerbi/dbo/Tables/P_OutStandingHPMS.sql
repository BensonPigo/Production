CREATE TABLE [dbo].[P_OutStandingHPMS](
	[BuyerDelivery] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[OSTInHauling] [int] NOT NULL,
	[OSTInScanAndPack] [int] NOT NULL,
	[OSTInCFA] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_OutStandingHPMS] PRIMARY KEY CLUSTERED 
(
	[BuyerDelivery] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_OutStandingHPMS] ADD  CONSTRAINT [DF_P_OutStandingHPMS_OSTInHauling]  DEFAULT ((0)) FOR [OSTInHauling]
GO

ALTER TABLE [dbo].[P_OutStandingHPMS] ADD  CONSTRAINT [DF_P_OutStandingHPMS_OSTInScanAndPack]  DEFAULT ((0)) FOR [OSTInScanAndPack]
GO

ALTER TABLE [dbo].[P_OutStandingHPMS] ADD  CONSTRAINT [DF_P_OutStandingHPMS_OSTInCFA]  DEFAULT ((0)) FOR [OSTInCFA]
GO

ALTER TABLE [dbo].[P_OutStandingHPMS] ADD  CONSTRAINT [DF_P_OutStandingHPMS_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P_CartonStatusTrackingList.BuyerDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutStandingHPMS', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Facotry.FtyGroup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutStandingHPMS', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P_CartonStatusTrackingList.Status = ''Hauling''的統計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutStandingHPMS', @level2type=N'COLUMN',@level2name=N'OSTInHauling'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P_CartonStatusTrackingList.Status = ''Scan And Pack''的統計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutStandingHPMS', @level2type=N'COLUMN',@level2name=N'OSTInScanAndPack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P_CartonStatusTrackingList.Status = ''CFA''的統計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutStandingHPMS', @level2type=N'COLUMN',@level2name=N'OSTInCFA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutStandingHPMS', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutStandingHPMS', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
