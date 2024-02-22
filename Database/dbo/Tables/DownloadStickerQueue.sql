CREATE TABLE [dbo].[DownloadStickerQueue]
(
	[PackingID] Varchar (13) NOT NULL, 
    [ErrorMsg] VARCHAR(1000) CONSTRAINT [DF_DownloadStickerQueue_ErrorMsg] DEFAULT ('') NOT NULL,
    [AddDate] DATETIME NULL, 
    [UpdateDate] DATETIME NULL, 
    [Processing] BIT CONSTRAINT [DF_DownloadStickerQueue_Processing] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_DownloadStickerQueue] PRIMARY KEY CLUSTERED ([PackingID] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'待下載的裝箱單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DownloadStickerQueue',
    @level2type = N'COLUMN',
    @level2name = N'PackingID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'下載期間出現的問題',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DownloadStickerQueue',
    @level2type = N'COLUMN',
    @level2name = N'ErrorMsg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DownloadStickerQueue',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後更新日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DownloadStickerQueue',
    @level2type = N'COLUMN',
    @level2name = N'UpdateDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否正在執行',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DownloadStickerQueue',
    @level2type = N'COLUMN',
    @level2name = N'Processing'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'下載貼標檔案佇列',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'DownloadStickerQueue',
    @level2type = NULL,
    @level2name = NULL