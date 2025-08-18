CREATE TABLE [dbo].[P_FabricInspAvgInspLTInPast7Days] (
    [TransferDate]         DATE            NOT NULL,
    [FactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspAvgInspLTInPast7Days_FactoryID_New] DEFAULT ('') NOT NULL,
    [AvgInspLTInPast7Days] NUMERIC (38, 2) CONSTRAINT [DF_P_FabricInspAvgInspLTInPast7Days_AvgInspLTInPast7Days_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspAvgInspLTInPast7Days_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspAvgInspLTInPast7Days_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricInspAvgInspLTInPast7Days] PRIMARY KEY CLUSTERED ([TransferDate] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Addate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產廠的FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產廠的FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'AvgInspLTInPast7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricInspAvgInspLTInPast7Days', @level2type = N'COLUMN', @level2name = N'BIStatus';

