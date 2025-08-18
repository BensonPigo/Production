CREATE TABLE [dbo].[P_FabricPhysicalInspectionList_History] (
    [HistoryUkey]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [FactoryID]    VARCHAR (8000) NOT NULL,
    [SP]           VARCHAR (8000) NOT NULL,
    [SEQ]          VARCHAR (8000) NOT NULL,
    [ReceivingID]  VARCHAR (8000) NOT NULL,
    [Roll]         VARCHAR (8000) NOT NULL,
    [Dyelot]       VARCHAR (8000) NOT NULL,
    [BIFactoryID]  VARCHAR (8000) NOT NULL,
    [BIInsertDate] DATETIME       NOT NULL,
    [BIStatus]     VARCHAR (8000) CONSTRAINT [DF_P_FabricPhysicalInspectionList_History_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricPhysicalInspectionList_History] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricPhysicalInspectionList_History', @level2type = N'COLUMN', @level2name = N'BIStatus';

