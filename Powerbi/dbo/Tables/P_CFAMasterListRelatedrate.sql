CREATE TABLE [dbo].[P_CFAMasterListRelatedrate] (
    [Buyerdelivery]     DATE            NOT NULL,
    [FactoryID]         VARCHAR (8000)  NOT NULL,
    [FinalRate]         NUMERIC (38, 2) CONSTRAINT [DF_P_CFAMasterListRelatedrate_FinalRate_New] DEFAULT ((0)) NOT NULL,
    [FinalInspectionSP] INT             CONSTRAINT [DF_P_CFAMasterListRelatedrate_FinalInspectionSP_New] DEFAULT ((0)) NOT NULL,
    [TotalSP]           INT             CONSTRAINT [DF_P_CFAMasterListRelatedrate_TotalSP_New] DEFAULT ((0)) NOT NULL,
    [PassRate]          NUMERIC (38, 2) CONSTRAINT [DF_P_CFAMasterListRelatedrate_PassRate_New] DEFAULT ((0)) NOT NULL,
    [PassSP]            INT             CONSTRAINT [DF_P_CFAMasterListRelatedrate_PassSP_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_CFAMasterListRelatedrate_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]      DATETIME        NULL,
    [BIStatus]          VARCHAR (8000)  CONSTRAINT [DF_P_CFAMasterListRelatedrate_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_CFAMasterListRelatedrate] PRIMARY KEY CLUSTERED ([Buyerdelivery] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO


GO


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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CFAMasterListRelatedrate', @level2type = N'COLUMN', @level2name = N'BIStatus';

