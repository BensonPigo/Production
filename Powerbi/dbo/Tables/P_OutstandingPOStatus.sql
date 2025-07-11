CREATE TABLE [dbo].[P_OutstandingPOStatus] (
    [Buyerdelivery]    DATE           NOT NULL,
    [FTYGroup]         VARCHAR (8000) NOT NULL,
    [TotalCMPQty]      INT            CONSTRAINT [DF_P_OutstandingPOStatus_TotalCMPQty_New] DEFAULT ((0)) NOT NULL,
    [TotalClogCtn]     INT            CONSTRAINT [DF_P_OutstandingPOStatus_TotalClogCtn_New] DEFAULT ((0)) NOT NULL,
    [NotYet3rdSPCount] INT            CONSTRAINT [DF_P_OutstandingPOStatus_NotYet3rdSPCount_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]      VARCHAR (8000) CONSTRAINT [DF_P_OutstandingPOStatus_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]     DATETIME       NULL,
    [BIStatus]         VARCHAR (8000) CONSTRAINT [DF_P_OutstandingPOStatus_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_OutstandingPOStatus] PRIMARY KEY CLUSTERED ([Buyerdelivery] ASC, [FTYGroup] ASC)
);



GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutstandingPOStatus', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutstandingPOStatus', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OutstandingPOStatus', @level2type = N'COLUMN', @level2name = N'BIStatus';

