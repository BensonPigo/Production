CREATE TABLE [dbo].[WorkOrder_PatternPanelRevisedMarkerOriginalData] (
    [ID]                                     VARCHAR (13) CONSTRAINT [DF_WorkOrder_PatternPanelRevisedMarkerOriginalData_ID] DEFAULT ('') NULL,
    [WorkOrderRevisedMarkerOriginalDataUkey] BIGINT       CONSTRAINT [DF_WorkOrder_PatternPanelRevisedMarkerOriginalData_WorkOrderRevisedMarkerOriginalDataUkey] DEFAULT ((0)) NOT NULL,
    [PatternPanel]                           VARCHAR (2)  CONSTRAINT [DF_WorkOrder_PatternPanelRevisedMarkerOriginalData_PatternPanel] DEFAULT ('') NOT NULL,
    [FabricPanelCode]                        VARCHAR (2)  CONSTRAINT [DF_WorkOrder_PatternPanelRevisedMarkerOriginalData_FabricPanelCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_WorkOrder_PatternPanelRevisedMarkerOriginalData] PRIMARY KEY CLUSTERED ([WorkOrderRevisedMarkerOriginalDataUkey] ASC, [PatternPanel] ASC, [FabricPanelCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Panel Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanelRevisedMarkerOriginalData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanelRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'WorkOrderRevisedMarkerOriginalDataUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Panel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanelRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanelRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FabricPanelCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_PatternPanelRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';

