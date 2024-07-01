CREATE TABLE [dbo].[P_FabricInspAvgInspLTInPast7Days]
(
	[TransferDate] DATE NOT NULL  , 
    [ColumnNM] VARCHAR(8) NOT NULL DEFAULT ('') , 
    [AvgInspLTInPast7Days] NUMERIC(6, 2) NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_P_FabricInspAvgInspLTInPast7Days] PRIMARY KEY ([TransferDate])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Addate',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspAvgInspLTInPast7Days',
    @level2type = N'COLUMN',
    @level2name = N'TransferDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'生產廠的FactoryID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspAvgInspLTInPast7Days',
    @level2type = N'COLUMN',
    @level2name = N'ColumnNM'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'生產廠的FactoryID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspAvgInspLTInPast7Days',
    @level2type = N'COLUMN',
    @level2name = N'AvgInspLTInPast7Days'