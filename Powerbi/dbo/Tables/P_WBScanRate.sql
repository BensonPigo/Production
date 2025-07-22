CREATE TABLE [dbo].[P_WBScanRate] (
    [Date]                DATE            NOT NULL,
    [FactoryID]           VARCHAR (8000)  NOT NULL,
    [WBScanRate]          NUMERIC (38, 2) CONSTRAINT [DF_P_WBScanRate_WBScanRate_New] DEFAULT ((0)) NOT NULL,
    [TTLRFIDSewInlineQty] INT             CONSTRAINT [DF_P_WBScanRate_TTLRFIDSewInlineQty_New] DEFAULT ((0)) NOT NULL,
    [TTLSewQty]           INT             CONSTRAINT [DF_P_WBScanRate_TTLSewQty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]         VARCHAR (8000)  CONSTRAINT [DF_P_WBScanRate_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]        DATETIME        NULL,
    [BIStatus]            VARCHAR (8000)  CONSTRAINT [DF_P_WBScanRate_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_WBScanRate] PRIMARY KEY CLUSTERED ([Date] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_WBScanRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_WBScanRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_WBScanRate', @level2type = N'COLUMN', @level2name = N'BIStatus';

