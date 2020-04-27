CREATE TABLE [dbo].[WorkOrder_Distribute] (
    [WorkOrderUkey] BIGINT       CONSTRAINT [DF_WorkOrder_Distribute_WorkOrderUkey] DEFAULT ((0)) NOT NULL,
    [ID]            VARCHAR (13) CONSTRAINT [DF_WorkOrder_Distribute_ID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13) CONSTRAINT [DF_WorkOrder_Distribute_OrderID] DEFAULT ('') NOT NULL,
    [Article]       VARCHAR (8)  CONSTRAINT [DF_WorkOrder_Distribute_Article] DEFAULT ('') NOT NULL,
    [SizeCode]      VARCHAR (8)  CONSTRAINT [DF_WorkOrder_Distribute_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]           NUMERIC (6)  CONSTRAINT [DF_WorkOrder_Distribute_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrder_Distribute] PRIMARY KEY CLUSTERED ([WorkOrderUkey] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder Distribute', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Distribute';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrderUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Distribute', @level2type = N'COLUMN', @level2name = N'WorkOrderUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁簡母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Distribute', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Distribute', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Distribute', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Distribute', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Distribute', @level2type = N'COLUMN', @level2name = N'Qty';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[WorkOrder_Distribute]([ID] ASC)
    INCLUDE([WorkOrderUkey], [OrderID], [Article], [SizeCode], [Qty]);
GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrder_Distribute_OrderID] ON [dbo].[WorkOrder_Distribute]
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
;
GO
