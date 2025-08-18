CREATE TABLE [dbo].[P_ProductionKitsTracking] (
    [BrandID]                  VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]                  VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_StyleID_New] DEFAULT ('') NOT NULL,
    [SeasonID]                 VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_SeasonID_New] DEFAULT ('') NOT NULL,
    [Article]                  NVARCHAR (1000) CONSTRAINT [DF_P_ProductionKitsTracking_Article_New] DEFAULT ('') NOT NULL,
    [Mdivision]                VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_Mdivision_New] DEFAULT ('') NOT NULL,
    [FactoryID]                VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_FactoryID_New] DEFAULT ('') NOT NULL,
    [Doc]                      NVARCHAR (1000) CONSTRAINT [DF_P_ProductionKitsTracking_Doc_New] DEFAULT ('') NOT NULL,
    [TWSendDate]               DATE            NULL,
    [FtyMRRcvDate]             DATE            NULL,
    [FtySendtoQADate]          DATE            NULL,
    [QARcvDate]                DATE            NULL,
    [UnnecessaryToSend]        VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_UnnecessaryToSend_New] DEFAULT ('') NOT NULL,
    [ProvideDate]              DATE            NULL,
    [SPNo]                     VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_SPNo_New] DEFAULT ('') NOT NULL,
    [SCIDelivery]              DATE            NULL,
    [BuyerDelivery]            DATE            NULL,
    [Pullforward]              VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_Pullforward_New] DEFAULT ('') NOT NULL,
    [Handle]                   VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_Handle_New] DEFAULT ('') NOT NULL,
    [MRHandle]                 VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_MRHandle_New] DEFAULT ('') NOT NULL,
    [SMR]                      VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_SMR_New] DEFAULT ('') NOT NULL,
    [POHandle]                 VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_POHandle_New] DEFAULT ('') NOT NULL,
    [POSMR]                    VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_POSMR_New] DEFAULT ('') NOT NULL,
    [FtyHandle]                VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_FtyHandle_New] DEFAULT ('') NOT NULL,
    [ProductionKitsGroup]      VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_ProductionKitsGroup_New] DEFAULT ('') NOT NULL,
    [AddDate]                  DATETIME        NULL,
    [EditDate]                 DATETIME        NULL,
    [Reject]                   VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_Reject_New] DEFAULT ('') NOT NULL,
    [AWBNO]                    VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_AWBNO_New] DEFAULT ('') NOT NULL,
    [Style_ProductionKitsUkey] BIGINT          CONSTRAINT [DF_P_ProductionKitsTracking_StyleProductionKitsUkey_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]              VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]             DATETIME        NULL,
    [BIStatus]                 VARCHAR (8000)  CONSTRAINT [DF_P_ProductionKitsTracking_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ProductionKitsTracking] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [Style_ProductionKitsUkey] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.ProductionKitsGroup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'ProductionKitsGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ProductionKitsTracking', @level2type = N'COLUMN', @level2name = N'BIStatus';

