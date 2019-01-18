CREATE TABLE [dbo].[WorkOrder_DistributeRevisedMarkerOriginalData] (
    [WorkOrderRevisedMarkerOriginalDataUkey] BIGINT       NOT NULL,
    [ID]                                     VARCHAR (13) NOT NULL,
    [OrderID]                                VARCHAR (13) NOT NULL,
    [Article]                                VARCHAR (8)  NOT NULL,
    [SizeCode]                               VARCHAR (8)  NOT NULL,
    [Qty]                                    NUMERIC (6)  NOT NULL,
    CONSTRAINT [PK_WorkOrder_DistributeRevisedMarkerOriginalData] PRIMARY KEY CLUSTERED ([WorkOrderRevisedMarkerOriginalDataUkey] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC)
);

