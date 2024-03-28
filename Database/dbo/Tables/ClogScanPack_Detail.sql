CREATE TABLE [dbo].[ClogScanPack_Detail]
(
	[ClogScanPackUkey]      BIGINT      CONSTRAINT [DF_ClogScanPack_Detail_ClogScanPackUkey]       DEFAULT ((0)) Not null, 
    [OrderID]               VARCHAR(13) CONSTRAINT [DF_ClogScanPack_Detail_OrderID]    DEFAULT ('')  NOT NULL, 
    [Article]               VARCHAR(8)  CONSTRAINT [DF_ClogScanPack_Detail_Article]       DEFAULT ('')  Not null, 
    [SizeCode]              VARCHAR(8)  CONSTRAINT [DF_ClogScanPack_Detail_SizeCode]       DEFAULT ('')  Not null, 
    [ScanQty]               SMALLINT    CONSTRAINT [DF_ClogScanPack_Detail_ScanQty]       DEFAULT ((0)) Not null, 
    [LackingQty]            SMALLINT    CONSTRAINT [DF_ClogScanPack_Detail_LackingQty]       DEFAULT ((0)) Not null, 
    CONSTRAINT [PK_ClogScanPack_Detail] PRIMARY KEY ([ClogScanPackUkey], [Article], [SizeCode]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'掃描主鍵',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ClogScanPackUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Article'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'尺碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SizeCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'掃到的件數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ScanQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缺件的件數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack_Detail',
    @level2type = N'COLUMN',
    @level2name = N'LackingQty'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogScanPack_Detail',
    @level2type = N'COLUMN',
    @level2name = N'OrderID'
GO