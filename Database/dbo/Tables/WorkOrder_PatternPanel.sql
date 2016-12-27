CREATE TABLE [dbo].[WorkOrder_PatternPanel] (
    [ID]            VARCHAR (13) CONSTRAINT [DF_WorkOrder_PatternPanel_ID] DEFAULT ('') NULL,
    [WorkOrderUkey] BIGINT       CONSTRAINT [DF_WorkOrder_PatternPanel_WorkOrderUkey] DEFAULT ((0)) NOT NULL,
    [PatternPanel]  VARCHAR (2)  CONSTRAINT [DF_WorkOrder_PatternPanel_PatternPanel] DEFAULT ('') NOT NULL,
    [LectraCode]    VARCHAR (2)  CONSTRAINT [DF_WorkOrder_PatternPanel_LectraCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_WorkOrder_PatternPanel] PRIMARY KEY CLUSTERED ([WorkOrderUkey] ASC, [PatternPanel] ASC, [LectraCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Panel Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanel', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanel', @level2type = N'COLUMN', @level2name = N'WorkOrderUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Panel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanel', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lectra Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanel', @level2type = N'COLUMN', @level2name = N'LectraCode';

