CREATE TABLE [dbo].[P_FabricInspDailyReport_Detail_History]
(
	[HistoryUkey] bigint NOT NULL IDENTITY(1,1), 
    [InspDate] DATE NOT NULL DEFAULT (''), 
    [Inspector] VARCHAR(10) NOT NULL DEFAULT (''), 
    [POID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [SEQ] VARCHAR(6) NOT NULL DEFAULT (''), 
    [ATA] DATE NULL, 
    [Roll] VARCHAR(8) NOT NULL DEFAULT (''), 
    [Dyelot] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_FabricInspDailyReport_Detail_History] PRIMARY KEY ([HistoryUkey]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'Inspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'InspDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'採購單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項-小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'SEQ'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Arrive W/H Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'ATA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'捲號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'Roll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缸號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'Dyelot'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'