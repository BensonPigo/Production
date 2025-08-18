CREATE TABLE [dbo].[P_RecevingInfoTrackingSummary] (
    [TransferDate]    DATE            NOT NULL,
    [FactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_FactoryID_New] DEFAULT ('') NOT NULL,
    [WHReceivingLT]   NUMERIC (38, 2) CONSTRAINT [DF_P_RecevingInfoTrackingSummary_WHReceivingLT_New] DEFAULT ((0)) NOT NULL,
    [UnloaderTtlRoll] INT             CONSTRAINT [DF_P_RecevingInfoTrackingSummary_UnloaderTtlRoll_New] DEFAULT ((0)) NOT NULL,
    [UnloaderTtlKG]   NUMERIC (38, 2) CONSTRAINT [DF_P_RecevingInfoTrackingSummary_UnloaderTtlKG_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]     VARCHAR (8000)  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]    DATETIME        NULL,
    [BIStatus]        VARCHAR (8000)  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_RecevingInfoTrackingSummary] PRIMARY KEY CLUSTERED ([TransferDate] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卸貨總卷數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'UnloaderTtlRoll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卸貨總重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'UnloaderTtlKG'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RecevingInfoTrackingSummary', @level2type = N'COLUMN', @level2name = N'BIStatus';

