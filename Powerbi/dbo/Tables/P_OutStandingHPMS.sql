CREATE TABLE [dbo].[P_OutStandingHPMS] (
    [BuyerDelivery]    DATE           NOT NULL,
    [FactoryID]        VARCHAR (8000) NOT NULL,
    [OSTInHauling]     INT            CONSTRAINT [DF_P_OutStandingHPMS_OSTInHauling_New] DEFAULT ((0)) NOT NULL,
    [OSTInScanAndPack] INT            CONSTRAINT [DF_P_OutStandingHPMS_OSTInScanAndPack_New] DEFAULT ((0)) NOT NULL,
    [OSTInCFA]         INT            CONSTRAINT [DF_P_OutStandingHPMS_OSTInCFA_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]      VARCHAR (8000) CONSTRAINT [DF_P_OutStandingHPMS_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]     DATETIME       NULL,
    [BIStatus]         VARCHAR (8000) CONSTRAINT [DF_P_OutStandingHPMS_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_OutStandingHPMS] PRIMARY KEY CLUSTERED ([BuyerDelivery] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OutStandingHPMS', @level2type = N'COLUMN', @level2name = N'BIStatus';

