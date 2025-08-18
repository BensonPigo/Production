CREATE TABLE [dbo].[P_CartonScanRate] (
    [Date]                 DATE            NOT NULL,
    [FactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_CartonScanRate_FactoryID_New] DEFAULT ('') NOT NULL,
    [HaulingScanRate]      NUMERIC (38, 2) CONSTRAINT [DF_P_CartonScanRate_HaulingScanRate_New] DEFAULT ((0)) NOT NULL,
    [PackingAuditScanRate] NUMERIC (38, 2) CONSTRAINT [DF_P_CartonScanRate_PackingAuditScanRate_New] DEFAULT ((0)) NOT NULL,
    [MDScanRate]           NUMERIC (38, 2) CONSTRAINT [DF_P_CartonScanRate_MDScanRate_New] DEFAULT ((0)) NOT NULL,
    [ScanAndPackRate]      NUMERIC (38, 2) CONSTRAINT [DF_P_CartonScanRate_ScanAndPackRate_New] DEFAULT ((0)) NOT NULL,
    [PullOutRate]          NUMERIC (38, 2) CONSTRAINT [DF_P_CartonScanRate_PullOutRate_New] DEFAULT ((0)) NOT NULL,
    [ClogReceivedRate]     NUMERIC (38, 2) CONSTRAINT [DF_P_CartonScanRate_ClogReceivedRate_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_CartonScanRate_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_CartonScanRate_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_CartonScanRate] PRIMARY KEY CLUSTERED ([Date] ASC, [FactoryID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonScanRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonScanRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CartonScanRate', @level2type = N'COLUMN', @level2name = N'BIStatus';

