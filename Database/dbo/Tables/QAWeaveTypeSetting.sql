CREATE TABLE [dbo].[QAWeaveTypeSetting]
(
	[WeaveTypeID] VARCHAR(20) NOT NULL, 
    [NonPhysical] BIT CONSTRAINT [DF_QAWeaveTypeSetting_NonPhysical] DEFAULT ((0)) not NULL,
    [NonWeight] BIT CONSTRAINT [DF_QAWeaveTypeSetting_NonWeight] DEFAULT ((0)) not NULL,
    [NonShadebond] BIT CONSTRAINT [DF_QAWeaveTypeSetting_NonShadebond] DEFAULT ((0)) not NULL,
    [NonContinuity] BIT CONSTRAINT [DF_QAWeaveTypeSetting_NonContinuity] DEFAULT ((0)) not NULL, 
    [NonOdor] BIT CONSTRAINT [DF_QAWeaveTypeSetting_NonOdor] DEFAULT ((0)) not NULL, 
    [NonMoisture] BIT CONSTRAINT [DF_QAWeaveTypeSetting_NonMoisture] DEFAULT ((0)) not NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_QAWeaveTypeSetting_AddName] DEFAULT ('') not NULL, 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_QAWeaveTypeSetting_EditName] DEFAULT ('') not NULL, 
    [EditDate] DATETIME NULL,
	CONSTRAINT [PK_QAWeaveTypeSetting] PRIMARY KEY CLUSTERED (
	[WeaveTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'織法', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAWeaveTypeSetting', @level2type = N'COLUMN', @level2name = N'WeaveTypeID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢驗 Physical', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAWeaveTypeSetting', @level2type = N'COLUMN', @level2name = N'NonPhysical';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢驗重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAWeaveTypeSetting', @level2type = N'COLUMN', @level2name = N'NonWeight';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢驗 Shade Bon', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAWeaveTypeSetting', @level2type = N'COLUMN', @level2name = N'NonShadebond';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢驗漸進色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAWeaveTypeSetting', @level2type = N'COLUMN', @level2name = N'NonContinuity';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢驗氣味', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAWeaveTypeSetting', @level2type = N'COLUMN', @level2name = N'NonOdor';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢測濕度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAWeaveTypeSetting', @level2type = N'COLUMN', @level2name = N'NonMoisture';
