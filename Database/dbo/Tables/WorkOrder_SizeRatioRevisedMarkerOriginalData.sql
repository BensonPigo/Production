CREATE TABLE [dbo].[WorkOrder_SizeRatioRevisedMarkerOriginalData] (
    [WorkOrderRevisedMarkerOriginalDataUkey] BIGINT       CONSTRAINT [DF_WorkOrder_SizeRatioRevisedMarkerOriginalData_WorkOrderRevisedMarkerOriginalDataUkey] DEFAULT ((0)) NOT NULL,
    [ID]                                     VARCHAR (13) CONSTRAINT [DF_WorkOrder_SizeRatioRevisedMarkerOriginalData_ID] DEFAULT ('') NOT NULL,
    [SizeCode]                               VARCHAR (8)  CONSTRAINT [DF_WorkOrder_SizeRatioRevisedMarkerOriginalData_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                                    NUMERIC (5)  CONSTRAINT [DF_WorkOrder_SizeRatioRevisedMarkerOriginalData_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrder_SizeRatioRevisedMarkerOriginalData] PRIMARY KEY CLUSTERED ([WorkOrderRevisedMarkerOriginalDataUkey] ASC, [SizeCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder SizeRatio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatioRevisedMarkerOriginalData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrderRevisedMarkerOriginalDataUkey                                                WorkOrderRevisedMarkerOriginalDataUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatioRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'WorkOrderRevisedMarkerOriginalDataUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatioRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatioRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_SizeRatioRevisedMarkerOriginalData', @level2type = N'COLUMN', @level2name = N'ID';

