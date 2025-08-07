CREATE TABLE [dbo].[P_PPICMasterList_ArtworkType] (
    [Ukey]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [SP#]             VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_SP#_New] DEFAULT ('') NOT NULL,
    [FactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_FactoryID_New] DEFAULT ('') NOT NULL,
    [ArtworkTypeNo]   VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_ArtworkTypeNo_New] DEFAULT ('') NOT NULL,
    [ArtworkType]     VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_ArtworkType_New] DEFAULT ('') NOT NULL,
    [Value]           NUMERIC (38, 6) CONSTRAINT [DF_P_PPICMasterList_ArtworkType_Value_New] DEFAULT ((0)) NOT NULL,
    [TotalValue]      NUMERIC (38, 6) CONSTRAINT [DF_P_PPICMasterList_ArtworkType_TotalValue_New] DEFAULT ((0)) NOT NULL,
    [ArtworkTypeUnit] VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_ArtworkTypeUnit_New] DEFAULT ('') NOT NULL,
    [SubconInTypeID]  VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_SubconInTypeID_New] DEFAULT ('') NOT NULL,
    [ArtworkTypeKey]  VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_ArtworkTypeKey_New] DEFAULT ('') NOT NULL,
    [OrderDataKey]    VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_OrderDataKey_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]     VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]    DATETIME        NULL,
    [BIStatus]        VARCHAR (8000)  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_PPICMasterList_ArtworkType] PRIMARY KEY CLUSTERED ([SP#] ASC, [SubconInTypeID] ASC, [ArtworkTypeKey] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'SP#'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkTypeNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成本資訊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成本總計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'TotalValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工成本單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkTypeUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代工類型代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'SubconInTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ArtworkTypeID+Unit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkTypeKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderID+SubconInType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'OrderDataKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_PPICMasterList_ArtworkType', @level2type = N'COLUMN', @level2name = N'BIStatus';

