CREATE TABLE [dbo].[Door2DoorDelivery]
(
	[ExportPort] VARCHAR(20) NOT NULL , 
    [ExportCountry] VARCHAR(2) NOT NULL, 
    [ImportCountry] VARCHAR(2) NOT NULL, 
    [ShipModeID] VARCHAR(10) NOT NULL, 
    [Vessel] VARCHAR(30) NOT NULL, 
    PRIMARY KEY ([ExportPort], [Vessel], [ImportCountry], [ExportCountry], [ShipModeID])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出口港口',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Door2DoorDelivery',
    @level2type = N'COLUMN',
    @level2name = N'ExportPort'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出口國別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Door2DoorDelivery',
    @level2type = N'COLUMN',
    @level2name = N'ExportCountry'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'目的地',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Door2DoorDelivery',
    @level2type = N'COLUMN',
    @level2name = N'ImportCountry'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'貨運方式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Door2DoorDelivery',
    @level2type = N'COLUMN',
    @level2name = N'ShipModeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'船名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Door2DoorDelivery',
    @level2type = N'COLUMN',
    @level2name = N'Vessel'