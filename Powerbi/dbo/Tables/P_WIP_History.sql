CREATE TABLE [dbo].[P_WIP_History] (
    [HistoryUkey]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [SPNO]         VARCHAR (8000) NOT NULL,
    [BIFactoryID]  VARCHAR (8000) NOT NULL,
    [BIInsertDate] DATETIME       NOT NULL,
    [BIStatus]     VARCHAR (8000) CONSTRAINT [DF_P_WIP_History_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_WIP_History] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_WIP_History', @level2type = N'COLUMN', @level2name = N'BIStatus';

