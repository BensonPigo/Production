CREATE TABLE [dbo].[P_FabricInspDailyReport_Detail_History] (
    [HistoryUkey]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [POID]         VARCHAR (8000) NOT NULL,
    [ReceivingID]  VARCHAR (8000) NOT NULL,
    [SEQ]          VARCHAR (8000) NOT NULL,
    [Roll]         VARCHAR (8000) NOT NULL,
    [Dyelot]       VARCHAR (8000) NOT NULL,
    [InspSeq]      INT            NOT NULL,
    [BIFactoryID]  VARCHAR (8000) NOT NULL,
    [BIInsertDate] DATETIME       NOT NULL,
    [BIStatus]     VARCHAR (8000) NULL,
    CONSTRAINT [PK_P_FabricInspDailyReport_Detail_History] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);
