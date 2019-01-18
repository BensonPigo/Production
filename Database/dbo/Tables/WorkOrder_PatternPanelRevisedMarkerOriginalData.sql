CREATE TABLE [dbo].[WorkOrder_PatternPanelRevisedMarkerOriginalData] (
    [ID]                                     VARCHAR (13) NULL,
    [WorkOrderRevisedMarkerOriginalDataUkey] BIGINT       NOT NULL,
    [PatternPanel]                           VARCHAR (2)  NOT NULL,
    [FabricPanelCode]                        VARCHAR (2)  NOT NULL,
    CONSTRAINT [PK_WorkOrder_PatternPanelRevisedMarkerOriginalData] PRIMARY KEY CLUSTERED ([WorkOrderRevisedMarkerOriginalDataUkey] ASC, [PatternPanel] ASC, [FabricPanelCode] ASC)
);

