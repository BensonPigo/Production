CREATE TABLE [dbo].[P_DQSDefect_Summary_History] (
    [HistoryUkey]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstInspectDate] DATE           NOT NULL,
    [FactoryID]        VARCHAR (8000) NOT NULL,
    [SPNO]             VARCHAR (8000) NOT NULL,
    [Article]          VARCHAR (8000) NOT NULL,
    [SizeCode]         VARCHAR (8000) NOT NULL,
    [QCName]           VARCHAR (8000) NOT NULL,
    [Shift]            VARCHAR (8000) NOT NULL,
    [Line]             VARCHAR (8000) NOT NULL,
    [InspectionDate]   DATE           NOT NULL,
    [BIFactoryID]      VARCHAR (8000) NOT NULL,
    [BIInsertDate]     DATETIME       NOT NULL,
    [BIStatus]         VARCHAR (8000) CONSTRAINT [DF_P_DQSDefect_Summary_History_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_DQSDefect_Summary_History] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_DQSDefect_Summary_History', @level2type = N'COLUMN', @level2name = N'BIStatus';

