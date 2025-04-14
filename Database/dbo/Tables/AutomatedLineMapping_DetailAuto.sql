CREATE TABLE [dbo].[AutomatedLineMapping_DetailAuto]
(
    [Ukey] bigint not null identity,
	[ID] INT NOT NULL , 
    [No] VARCHAR(2) NOT NULL , 
    [SewerManpower] tinyint NOT NULL , 
    [Seq] SMALLINT CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_Seq] DEFAULT (0) NOT NULL, 
    [Location] VARCHAR(20) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_Location] DEFAULT ('') NOT NULL, 
    [PPA] VARCHAR CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_PPA] DEFAULT ('') NOT NULL, 
    [MachineTypeID] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_MachineTypeID] DEFAULT ('') NOT NULL, 
    [MasterPlusGroup] VARCHAR(4) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_MasterPlusGroup] DEFAULT ('') NOT NULL, 
    [OperationID] VARCHAR(20) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_OperationID] DEFAULT ('') NOT NULL, 
    [Annotation] NVARCHAR(200) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_Annotation] DEFAULT ('') NOT NULL, 
    [Attachment] VARCHAR(100) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_Attachment] DEFAULT ('') NOT NULL, 
    [SewingMachineAttachmentID] VARCHAR(200) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_SewingMachineAttachmentID] DEFAULT ('') NOT NULL, 
    [Template] VARCHAR(100) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_Template] DEFAULT ('') NOT NULL, 
    [GSD] NUMERIC(12, 2) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_GSD] DEFAULT (0) NOT NULL, 
    [SewerDiffPercentage] NUMERIC(3, 2) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_SewerDiffPercentage] DEFAULT (0) NOT NULL, 
    [DivSewer] NUMERIC(5, 4) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_DivSewer] DEFAULT (0) NOT NULL, 
    [OriSewer] NUMERIC(5, 4) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_OriSewer] DEFAULT (0) NOT NULL, 
    [TimeStudyDetailUkey] BIGINT CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_TimeStudyDetailUkey] DEFAULT (0) NOT NULL, 
    [ThreadComboID] VARCHAR(10) CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_ThreadComboID] DEFAULT ('') NOT NULL, 
    [IsNonSewingLine] BIT CONSTRAINT [DF_AutomatedLineMapping_DetailAuto_IsNonSewingLine] DEFAULT (0) NOT NULL, 
    CONSTRAINT [PK_AutomatedLineMapping_DetailAuto] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'站位編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'No'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'排序',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'Seq'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工段分類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'Location'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車縫機器代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'MachineTypeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車縫機器分類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'MasterPlusGroup'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工段代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'OperationID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工段註解',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'Annotation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'模具附屬物代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'Attachment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'輔具規格',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'SewingMachineAttachmentID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'模具模版代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'Template'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'General Sewing Data時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'GSD'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車工差異百分比',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'SewerDiffPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'配分車工數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'DivSewer'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'原始車工數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'OriSewer'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyDetailUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'線組合代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'ThreadComboID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否不在產線車縫',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'IsNonSewingLine'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車工人力',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AutomatedLineMapping_DetailAuto',
    @level2type = N'COLUMN',
    @level2name = N'SewerManpower'