CREATE TABLE [dbo].[CutInsRecord_RollDyelot] (
    [Ukey]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [CutInsRecordUkey] BIGINT         NOT NULL,
    [TicketYds]        NUMERIC (8, 2) NULL,
    [Roll]             VARCHAR (8)    NULL,
    [Dyelot]           VARCHAR (8)    NULL,
    CONSTRAINT [PK_CutInsRecord_RollDyelot] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

