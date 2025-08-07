CREATE TABLE [dbo].[P_StationHourlyOutput_Detail] (
    [Ukey]                    INT            CONSTRAINT [DF_P_StationHourlyOutput_Detail_Ukey_New] DEFAULT ((0)) NOT NULL,
    [FactoryID]               VARCHAR (8000) CONSTRAINT [DF_P_StationHourlyOutput_Detail_FactoryID_New] DEFAULT ('') NOT NULL,
    [StationHourlyOutputUkey] BIGINT         CONSTRAINT [DF_P_StationHourlyOutput_Detail_StationHourlyOutputUkey_New] DEFAULT ((0)) NOT NULL,
    [Oclock]                  TINYINT        CONSTRAINT [DF_P_StationHourlyOutput_Detail_Oclock_New] DEFAULT ((0)) NOT NULL,
    [Qty]                     INT            CONSTRAINT [DF_P_StationHourlyOutput_Detail_Qty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]             VARCHAR (8000) CONSTRAINT [DF_P_StationHourlyOutput_Detail_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]            DATETIME       NULL,
    [BIStatus]                VARCHAR (8000) CONSTRAINT [DF_P_StationHourlyOutput_Detail_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_StationHourlyOutput_Detail] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [Ukey] ASC)
);



GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StationHourlyOutputUkey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'StationHourlyOutputUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間區間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Oclock'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣件數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_StationHourlyOutput_Detail', @level2type = N'COLUMN', @level2name = N'BIStatus';

