CREATE TABLE [dbo].[PackingErrorRecord] (
    [ID]                       BIGINT       IDENTITY (1, 1) NOT NULL,
    [PackID]                   VARCHAR (13) CONSTRAINT [DF_PackingErrorRecord_PackID] DEFAULT ('') NOT NULL,
    [CTN]                      VARCHAR (6)  CONSTRAINT [DF_PackingErrorRecord_CTN] DEFAULT ('') NOT NULL,
    [Line]                     VARCHAR (5)  CONSTRAINT [DF_PackingErrorRecord_Line] DEFAULT ('') NOT NULL,
    [Shift]                    VARCHAR (5)  CONSTRAINT [DF_PackingErrorRecord_Shift] DEFAULT ('') NOT NULL,
    [PackingReasonIDForTypeEG] VARCHAR (5)  DEFAULT ('') NOT NULL,
    [PackingReasonIDForTypeEO] VARCHAR (5)  DEFAULT ('') NOT NULL,
    [PackingReasonIDForTypeET] VARCHAR (5)  DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_PackingErrorRecord] PRIMARY KEY CLUSTERED ([ID] ASC)
);



