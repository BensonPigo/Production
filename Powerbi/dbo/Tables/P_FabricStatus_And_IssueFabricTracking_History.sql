CREATE TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking_History] (
    [HistoryUkey]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReplacementID] VARCHAR (8000) NOT NULL,
    [SP]            VARCHAR (8000) NOT NULL,
    [Seq]           VARCHAR (8000) NOT NULL,
    [RefNo]         VARCHAR (8000) NOT NULL,
    [BIFactoryID]   VARCHAR (8000) NOT NULL,
    [BIInsertDate]  DATETIME       NOT NULL,
    [BIStatus]      VARCHAR (8000) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_History_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricStatus_And_IssueFabricTracking_History] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);



GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Replacement 號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'ReplacementID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'RefNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricStatus_And_IssueFabricTracking_History', @level2type = N'COLUMN', @level2name = N'BIStatus';

