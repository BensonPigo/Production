CREATE TABLE [dbo].[SubprocessLeadTime_Detail] (
    [ID]           SMALLINT     CONSTRAINT [DF_SubprocessLeadTime_Detail_ID] DEFAULT ((0)) NOT NULL,
    [SubprocessID] VARCHAR (10) CONSTRAINT [DF_SubprocessLeadTime_Detail_SubprocessID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SubprocessLeadTime_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [SubprocessID] ASC)
);

