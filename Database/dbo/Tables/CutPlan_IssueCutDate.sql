CREATE TABLE [dbo].[CutPlan_IssueCutDate]
(
	[ID]                VARCHAR(13)     CONSTRAINT [DF_CutPlan_IssueCutDate_Id]                 NOT NULL DEFAULT (('')), 
    [Refno]             VARCHAR(36)     CONSTRAINT [DF_CutPlan_IssueCutDate_Refno]              NOT NULL DEFAULT (('')), 
    [Colorid]           VARCHAR(6)      CONSTRAINT [DF_CutPlan_IssueCutDate_Colorid]            NOT NULL DEFAULT (('')), 
    [EstCutDate]        DATE            CONSTRAINT [DF_CutPlan_IssueCutDate_EstCutDate]         NULL, 
    [Reason]            VARCHAR(5)      CONSTRAINT [DF_CutPlan_IssueCutDate_Reason]             NOT NULL DEFAULT (('')), 
    [FabricIssued]      BIT             CONSTRAINT [DF_CutPlan_IssueCutDate_FabricIssued]       NOT NULL DEFAULT (('')), 
    [RequestorRemark]   NVARCHAR(MAX)   CONSTRAINT [DF_CutPlan_IssueCutDate_RequestorRemark]    NOT NULL DEFAULT (('')), 
    [EditName] VARCHAR(10) NOT NULL DEFAULT (('')), 
    [EditDate] DATETIME NULL, 
    CONSTRAINT [PK_CutPlan_IssueCutDate] PRIMARY KEY ([Colorid], [ID], [Refno])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'客戶物料編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'Refno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'Colorid'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預計裁剪日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'EstCutDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'理由',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'Reason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否已透過WH-P10發料',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'FabricIssued'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Tone備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'RequestorRemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編輯時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_IssueCutDate',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'