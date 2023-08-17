CREATE TABLE [dbo].[LocalOrderInventory_Location]
(
	[LocalOrderInventoryUkey] BIGINT NOT NULL, 
    [MtlLocationID] VARCHAR(20) NOT NULL, 
    CONSTRAINT [PK_LocalOrderInventory_Location] PRIMARY KEY ([MtlLocationID], [LocalOrderInventoryUkey]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'儲位編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory_Location',
    @level2type = N'COLUMN',
    @level2name = N'MtlLocationID'