CREATE TABLE [dbo].[ClogScanPack]
(
	[Ukey]                  bigint                  IDENTITY(1,1)               DEFAULT ((0)) NOT NULL, 
    [MDivisionID]           VARCHAR(8)              CONSTRAINT [DF_ClogScanPack_MDivisionID]    DEFAULT ('')       NOT NULL , 
    [PackingListID]         VARCHAR(13)             CONSTRAINT [DF_ClogScanPack_PackingListID]    DEFAULT ('')  NOT NULL, 
    [CTNStartNo]            VARCHAR(6)              CONSTRAINT [DF_ClogScanPack_CTNStartNo]    DEFAULT ('')  NOT NULL, 
    [SCICtnNo]              VARCHAR(16)             CONSTRAINT [DF_ClogScanPack_SCICtnNo]    DEFAULT ('')  NOT NULL, 
    [ScanQty]               SMALLINT                CONSTRAINT [DF_ClogScanPack_ScanQty]    DEFAULT ((0)) NOT NULL, 
    [LackingQty]            SMALLINT                CONSTRAINT [DF_ClogScanPack_LackingQty]    DEFAULT ((0)) NOT NULL, 
    [AddName]               VARCHAR(10)             CONSTRAINT [DF_ClogScanPack_AddName]    DEFAULT ('')  NOT NULL, 
    [AddDate]               DATETIME                CONSTRAINT [DF_ClogScanPack_AddDate]             NULL, 
    CONSTRAINT [PK_ClogScanPack] PRIMARY KEY ([Ukey])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'主鍵',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M Level',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裝箱清單號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'PackingListID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'箱號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'CTNStartNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SCI內部箱號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'SCICtnNo'

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'掃到的總件數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'ScanQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缺件的總件數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'LackingQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'掃描人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'掃描時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'