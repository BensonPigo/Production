CREATE TABLE [dbo].[WorkOrder_DistributeRevisedMarkerOriginalData] (
    [WorkOrderRevisedMarkerOriginalDataUkey] BIGINT       CONSTRAINT [DF_WorkOrder_DistributeRevisedMarkerOriginalData_WorkOrderRevisedMarkerOriginalDataUkey] DEFAULT ((0)) NOT NULL,
    [ID]                                     VARCHAR (13) CONSTRAINT [DF_WorkOrder_DistributeRevisedMarkerOriginalData_ID] DEFAULT ('') NOT NULL,
    [OrderID]                                VARCHAR (13) CONSTRAINT [DF_WorkOrder_DistributeRevisedMarkerOriginalData_OrderID] DEFAULT ('') NOT NULL,
    [Article]                                VARCHAR (8)  CONSTRAINT [DF_WorkOrder_DistributeRevisedMarkerOriginalData_Article] DEFAULT ('') NOT NULL,
    [SizeCode]                               VARCHAR (8)  CONSTRAINT [DF_WorkOrder_DistributeRevisedMarkerOriginalData_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                                    NUMERIC (6)  CONSTRAINT [DF_WorkOrder_DistributeRevisedMarkerOriginalData_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrder_DistributeRevisedMarkerOriginalData] PRIMARY KEY CLUSTERED ([WorkOrderRevisedMarkerOriginalDataUkey] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder Distribute', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_DistributeRevisedMarkerOriginalData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrderRevisedMarkerOriginalDataUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_DistributeRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'WorkOrderRevisedMarkerOriginalDataUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_DistributeRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_DistributeRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_DistributeRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁簡母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_DistributeRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_DistributeRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'Article';

