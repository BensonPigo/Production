CREATE TABLE [dbo].[WorkOrder_SizeRatioRevisedMarkerOriginalData] (
    [WorkOrderRevisedMarkerOriginalDataUkey] BIGINT       NOT NULL,
    [ID]                                     VARCHAR (13) NOT NULL,
    [SizeCode]                               VARCHAR (8)  NOT NULL,
    [Qty]                                    NUMERIC (5)  NOT NULL,
    CONSTRAINT [PK_WorkOrder_SizeRatioRevisedMarkerOriginalData] PRIMARY KEY CLUSTERED ([WorkOrderRevisedMarkerOriginalDataUkey] ASC, [SizeCode] ASC)
);

