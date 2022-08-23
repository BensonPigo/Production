CREATE TABLE [dbo].[ClogReceiveCFA]
(
	[ID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ReceiveDate] DATE NOT NULL, 
    [MDivisionID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [OrderID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [PackingListID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [CTNStartNo] VARCHAR(6) NOT NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [AddDate] DATETIME NOT NULL, 
    [SCICtnNo] VARCHAR(16) CONSTRAINT [DF_ClogReceiveCFA_SCICtnNo] DEFAULT ('') NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identity',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReceiveCFA',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'移轉日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReceiveCFA',
    @level2type = N'COLUMN',
    @level2name = N'ReceiveDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReceiveCFA',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReceiveCFA',
    @level2type = N'COLUMN',
    @level2name = N'OrderID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PackID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReceiveCFA',
    @level2type = N'COLUMN',
    @level2name = N'PackingListID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'箱號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReceiveCFA',
    @level2type = N'COLUMN',
    @level2name = N'CTNStartNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReceiveCFA',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReceiveCFA',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'