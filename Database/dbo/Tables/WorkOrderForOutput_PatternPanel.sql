CREATE TABLE [dbo].[WorkOrderForOutput_PatternPanel] (
    [WorkOrderForOutputUkey] INT          CONSTRAINT [DF_WorkOrderForOutput_PatternPanel_WorkOrderForOutputUkey] DEFAULT ((0)) NOT NULL,
    [ID]                     VARCHAR (13) CONSTRAINT [DF_WorkOrderForOutput_PatternPanel_ID] DEFAULT ('') NOT NULL,
    [PatternPanel]           VARCHAR (2)  CONSTRAINT [DF_WorkOrderForOutput_PatternPanel_PatternPanel] DEFAULT ('') NOT NULL,
    [FabricPanelCode]        VARCHAR (2)  CONSTRAINT [DF_WorkOrderForOutput_PatternPanel_FabricPanelCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_WorkOrderForOutput_PatternPanel] PRIMARY KEY CLUSTERED ([WorkOrderForOutputUkey] ASC, [PatternPanel] ASC, [FabricPanelCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput_PatternPanel', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput_PatternPanel', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput_PatternPanel', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產出裁剪工單主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput_PatternPanel', @level2type = N'COLUMN', @level2name = N'WorkOrderForOutputUkey';

