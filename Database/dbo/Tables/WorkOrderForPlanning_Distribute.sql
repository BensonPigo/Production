CREATE TABLE [dbo].[WorkOrderForPlanning_Distribute] (
    [WorkOrderForPlanningUkey] INT          CONSTRAINT [DF_WorkOrderForPlanning_Distribute_WorkOrderForPlanningUkey] DEFAULT ((0)) NOT NULL,
    [ID]                       VARCHAR (13) CONSTRAINT [DF_WorkOrderForPlanning_Distribute_ID] DEFAULT ('') NOT NULL,
    [OrderID]                  VARCHAR (13) CONSTRAINT [DF_WorkOrderForPlanning_Distribute_OrderID] DEFAULT ('') NOT NULL,
    [Article]                  VARCHAR (8)  CONSTRAINT [DF_WorkOrderForPlanning_Distribute_Article] DEFAULT ('') NOT NULL,
    [SizeCode]                 VARCHAR (8)  CONSTRAINT [DF_WorkOrderForPlanning_Distribute_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                      NUMERIC (6)  CONSTRAINT [DF_WorkOrderForPlanning_Distribute_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrderForPlanning_Distribute] PRIMARY KEY CLUSTERED ([WorkOrderForPlanningUkey] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForPlanning_Distribute', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForPlanning_Distribute', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForPlanning_Distribute', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForPlanning_Distribute', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForPlanning_Distribute', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料裁剪工單主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForPlanning_Distribute', @level2type = N'COLUMN', @level2name = N'WorkOrderForPlanningUkey';

