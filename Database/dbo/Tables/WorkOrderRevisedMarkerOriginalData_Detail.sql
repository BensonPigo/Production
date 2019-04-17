CREATE TABLE [dbo].[WorkOrderRevisedMarkerOriginalData_Detail]
(
	[WorkorderUkeyRevisedMarkerOriginalUkey] BIGINT NOT NULL, 
    [WorkorderUkey] BIGINT NOT NULL,
    CONSTRAINT [PK_WorkOrderRevisedMarkerOriginalData_Detail] PRIMARY KEY CLUSTERED ([WorkorderUkey] ASC)
)
