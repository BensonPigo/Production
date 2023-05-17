CREATE TABLE [dbo].[CTNHauling]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [HaulingDate] DATETIME NOT NULL, 
    [MDivisionID] VARCHAR(8) NOT NULL DEFAULT '', 
    [OrderID] VARCHAR(13) NOT NULL DEFAULT '', 
    [PackingListID] VARCHAR(13) NOT NULL DEFAULT '', 
    [CTNStartNo] VARCHAR(6) NOT NULL DEFAULT '', 
    [SCICtnNo] VARCHAR(16) NOT NULL DEFAULT '', 
    [AddName] VARCHAR(10) NOT NULL DEFAULT '', 
    [AddDate] DATETIME NOT NULL DEFAULT GetDate(), 
    [Status] VARCHAR(6) not null constraint [DF_CTNHauling_Status] DEFAULT '', 
    [Remark] NVARCHAR(MAX) not null constraint [DF_CTNHauling_Remark] DEFAULT ''
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Hauling狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CTNHauling',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'退回原因備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CTNHauling',
    @level2type = N'COLUMN',
    @level2name = N'Remark'