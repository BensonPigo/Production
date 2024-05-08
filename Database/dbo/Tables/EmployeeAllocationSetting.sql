CREATE TABLE [dbo].[EmployeeAllocationSetting]
(
	[ID]        VARCHAR(5)                  CONSTRAINT [DF_EmployeeAllocationSetting_ID]     NOT NULL  DEFAULT (''), 
    [FactoryID] VARCHAR(8)                  CONSTRAINT [DF_EmployeeAllocationSetting_FactoryID]     NOT NULL DEFAULT (''), 
    [Dept]      NVARCHAR(200)               CONSTRAINT [DF_EmployeeAllocationSetting_Dept]     NOT NULL DEFAULT (''), 
    [Position]  NVARCHAR(200)               CONSTRAINT [DF_EmployeeAllocationSetting_Position]     NOT NULL DEFAULT (''), 
    [Junk]      BIT                         CONSTRAINT [DF_EmployeeAllocationSetting_Junk]     NOT NULL DEFAULT ((0)), 
    [P03]       BIT                         CONSTRAINT [DF_EmployeeAllocationSetting_P03]     NOT NULL DEFAULT ((0)), 
    [P06]       BIT                         CONSTRAINT [DF_EmployeeAllocationSetting_P06]     NOT NULL DEFAULT ((0)), 
    [AddName]   VARCHAR(10)                 CONSTRAINT [DF_EmployeeAllocationSetting_AddName]     NOT NULL DEFAULT (''), 
    [AddDate]   DATETIME                    CONSTRAINT [DF_EmployeeAllocationSetting_AddDate]     NULL, 
    [EditName]  VARCHAR(10)                 CONSTRAINT [DF_EmployeeAllocationSetting_EditName]     NOT NULL DEFAULT (''), 
    [EditDate]  DATETIME                    CONSTRAINT [DF_EmployeeAllocationSetting_EditDate]     NULL, 
    CONSTRAINT [PK_EmployeeAllocationSetting] PRIMARY KEY ([FactoryID],[Dept],[Position])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IE.P03',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'EmployeeAllocationSetting',
    @level2type = N'COLUMN',
    @level2name = N'P03'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IE.P06',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'EmployeeAllocationSetting',
    @level2type = N'COLUMN',
    @level2name = N'P06'