CREATE TABLE [dbo].[P_OustandingPO_History] (
    [HistoryUkey]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [FactoryID]    VARCHAR (8000) NOT NULL,
    [OrderID]      VARCHAR (8000) NOT NULL,
    [Seq]          VARCHAR (8000) NOT NULL,
    [BIFactoryID]  VARCHAR (8000) NOT NULL,
    [BIInsertDate] DATETIME       NOT NULL,
    [BIStatus]     VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_History_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_OustandingPO_History] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);



GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OustandingPO_History', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OustandingPO_History', @level2type = N'COLUMN', @level2name = N'OrderID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'順序編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OustandingPO_History', @level2type = N'COLUMN', @level2name = N'Seq';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'記錄哪間工廠的資料，ex PH1, PH2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OustandingPO_History', @level2type = N'COLUMN', @level2name = N'BIFactoryID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'時間戳記，紀錄寫入table時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OustandingPO_History', @level2type = N'COLUMN', @level2name = N'BIInsertDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OustandingPO_History', @level2type = N'COLUMN', @level2name = N'BIStatus';

