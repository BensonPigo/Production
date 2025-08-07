CREATE TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn_History] (
    [HistoryUkey]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [POID]         VARCHAR (8000) NOT NULL,
    [SEQ]          VARCHAR (8000) NOT NULL,
    [ReceivingID]  VARCHAR (8000) NOT NULL,
    [Dyelot]       VARCHAR (8000) NOT NULL,
    [Roll]         VARCHAR (8000) NOT NULL,
    [DefectCode]   VARCHAR (8000) NOT NULL,
    [BIFactoryID]  VARCHAR (8000) NOT NULL,
    [BIInsertDate] DATETIME       NOT NULL,
    [BIStatus]     VARCHAR (8000) CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_History_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricInspReport_ReceivingTransferIn_History] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);



GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'母單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Receiving 單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricInspReport_ReceivingTransferIn_History', @level2type = N'COLUMN', @level2name = N'BIStatus';

