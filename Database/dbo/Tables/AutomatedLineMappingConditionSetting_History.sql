CREATE TABLE [dbo].[AutomatedLineMappingConditionSetting_History]
(
	[Ukey] bigint NOT NULL IDENTITY, 
    [AutomatedLineMappingConditionSettingUkey] TINYINT NOT NULL, 
    [HisType] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_History_HisType] DEFAULT ('') NOT NULL, 
    [OldValue] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_History_OldValue] DEFAULT ('') NOT NULL, 
    [NewValue] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_History_NewValue] DEFAULT ('') NOT NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMappingConditionSetting_History_AddName] DEFAULT ('') NOT NULL, 
    [AddDate] DATETIME NULL,
    CONSTRAINT [PK_AutomatedLineMappingConditionSetting_History] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'上層主鍵',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMappingConditionSetting_History',
    @level2type = N'COLUMN',
    @level2name = N'AutomatedLineMappingConditionSettingUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'紀錄類型',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMappingConditionSetting_History',
    @level2type = N'COLUMN',
    @level2name = N'HisType'