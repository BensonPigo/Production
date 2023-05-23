CREATE TABLE [dbo].[AutomatedLineMappingConditionSetting]
(
	[Ukey] tinyint NOT NULL IDENTITY, 
    [MDivisionID] VARCHAR(8) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_MDivisionID] DEFAULT ('') NOT NULL, 
    [FactoryID] VARCHAR(8) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_FactoryID] DEFAULT ('') NOT NULL, 
    [Functions] VARCHAR(50) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_FunctionIE] DEFAULT ('') NOT NULL, 
    [Verify] VARCHAR(50) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_Verify] DEFAULT ('') NOT NULL, 
    [Condition1] NUMERIC(3) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_Condition1] DEFAULT (0) NOT NULL, 
    [Condition2] NUMERIC(3) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_Condition2] DEFAULT (0) NOT NULL, 
    [Condition3] NUMERIC(3) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_Condition3] DEFAULT (0) NOT NULL, 
    [Junk] BIT CONSTRAINT [DF_AutomatedLineMappingConditionSetting_Junk] DEFAULT (0) NOT NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_AddName] DEFAULT ('') NOT NULL, 
    [AddDate] DATETIME NOT NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_EditName] DEFAULT ('') NOT NULL, 
    [EditDate] DATETIME NULL,
    CONSTRAINT [PK_AutomatedLineMappingConditionSetting] PRIMARY KEY CLUSTERED ([FactoryID], [Functions], [Verify] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'驗證欄位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMappingConditionSetting',
    @level2type = N'COLUMN',
    @level2name = N'Verify'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'條件1值',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMappingConditionSetting',
    @level2type = N'COLUMN',
    @level2name = N'Condition1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'條件2值',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMappingConditionSetting',
    @level2type = N'COLUMN',
    @level2name = N'Condition2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'條件3值',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMappingConditionSetting',
    @level2type = N'COLUMN',
    @level2name = N'Condition3'