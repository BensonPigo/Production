CREATE TABLE [dbo].[P_StationHourlyOutput_Detail_History]
(
		[HistoryUkey]   BIGINT NOT NULL IDENTITY(1,1), 
		[FactoryID]     VARCHAR(13) CONSTRAINT [DF_P_StationHourlyOutput_Detail_History_FactoryID]  NOT NULL DEFAULT (''), 
		[Ukey]		    int CONSTRAINT [DF_P_StationHourlyOutput_Detail_History_Ukey]  NOT NULL DEFAULT (0), 
		[BIFactoryID]   VARCHAR(8) CONSTRAINT [DF_P_StationHourlyOutput_Detail_History_BIFactoryID] NOT NULL DEFAULT (''), 
		[BIInsertDate]  DATETIME NULL, 
    CONSTRAINT [PK_P_StationHourlyOutput_Detail_History] PRIMARY KEY ([HistoryUkey]), 
)
GO
	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_StationHourlyOutput_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
    GO
	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_StationHourlyOutput_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
    GO
	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_StationHourlyOutput_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'
    GO
	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_StationHourlyOutput_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
    GO
	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'HistoryUkey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_StationHourlyOutput_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'HistoryUkey'
    GO