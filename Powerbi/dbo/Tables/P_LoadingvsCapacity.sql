CREATE TABLE [dbo].[P_LoadingvsCapacity] (
    [MDivisionID]    VARCHAR (8000)  CONSTRAINT [DF_P_LoadingvsCapacity_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]      VARCHAR (8000)  CONSTRAINT [DF_P_LoadingvsCapacity_FactoryID_New] DEFAULT ('') NOT NULL,
    [Key]            VARCHAR (8000)  CONSTRAINT [DF_P_LoadingvsCapacity_Key_New] DEFAULT ('') NOT NULL,
    [Halfkey]        VARCHAR (8000)  CONSTRAINT [DF_P_LoadingvsCapacity_Halfkey_New] DEFAULT ('') NOT NULL,
    [ArtworkTypeID]  VARCHAR (8000)  CONSTRAINT [DF_P_LoadingvsCapacity_ArtworkTypeID_New] DEFAULT ('') NOT NULL,
    [Capacity(CPU)]  NUMERIC (38, 6) CONSTRAINT [DF_P_LoadingvsCapacity_Capacity(CPU)_New] DEFAULT ((0)) NOT NULL,
    [Loading(CPU)]   NUMERIC (38, 6) CONSTRAINT [DF_P_LoadingvsCapacity_Loading(CPU)_New] DEFAULT ((0)) NOT NULL,
    [TransferBIDate] DATETIME        NULL,
    [Ukey]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [BIFactoryID]    VARCHAR (8000)  CONSTRAINT [DF_P_LoadingvsCapacity_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]   DATETIME        NULL,
    [BIStatus]       VARCHAR (8000)  CONSTRAINT [DF_P_LoadingvsCapacity_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_LoadingvsCapacity] PRIMARY KEY CLUSTERED ([MDivisionID] ASC, [Key] ASC, [FactoryID] ASC, [Halfkey] ASC, [ArtworkTypeID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組織代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠kpi統計群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'Key'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'半月份代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'Halfkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'ArtworkTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Capacity(CPU)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'Capacity(CPU)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loading (CPU)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'Loading(CPU)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_LoadingvsCapacity', @level2type = N'COLUMN', @level2name = N'BIStatus';

