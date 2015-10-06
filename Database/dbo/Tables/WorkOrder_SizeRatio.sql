CREATE TABLE [dbo].[WorkOrder_SizeRatio] (
    [WorkOrderUkey] BIGINT       CONSTRAINT [DF_WorkOrder_SizeRatio_WorkOrderUkey] DEFAULT ((0)) NOT NULL,
    [ID]            VARCHAR (13) CONSTRAINT [DF_WorkOrder_SizeRatio_ID] DEFAULT ('') NOT NULL,
    [SizeCode]      VARCHAR (8)  CONSTRAINT [DF_WorkOrder_SizeRatio_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]           NUMERIC (5)  CONSTRAINT [DF_WorkOrder_SizeRatio_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrder_SizeRatio] PRIMARY KEY CLUSTERED ([WorkOrderUkey] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder SizeRatio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrderUkey                                                WorkOrderUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatio', @level2type = N'COLUMN', @level2name = N'WorkOrderUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatio', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatio', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatio', @level2type = N'COLUMN', @level2name = N'Qty';

