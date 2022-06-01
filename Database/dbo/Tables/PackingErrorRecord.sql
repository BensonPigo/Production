CREATE TABLE [dbo].[PackingErrorRecord] (
    [ID]                    BIGINT       IDENTITY (1, 1) NOT NULL,
    [PackID]                VARCHAR (13) CONSTRAINT [DF_PackingErrorRecord_PackID] DEFAULT ('') NOT NULL,
    [CTN]                   VARCHAR (6)  CONSTRAINT [DF_PackingErrorRecord_CTN] DEFAULT ('') NOT NULL,
    [Line]                  VARCHAR (5)  CONSTRAINT [DF_PackingErrorRecord_Line] DEFAULT ('') NOT NULL,
    [Shift]                 VARCHAR (5)  CONSTRAINT [DF_PackingErrorRecord_Shift] DEFAULT ('') NOT NULL,
    [ReasonforGarmentSound] VARCHAR (50) CONSTRAINT [DF_PackingErrorRecord_ReasonforGarmentSound] DEFAULT ('') NOT NULL,
    [AreaOperation]         VARCHAR (50) CONSTRAINT [DF_PackingErrorRecord_AreaOperation] DEFAULT ('') NOT NULL,
    [ActionTaken]           VARCHAR (100) CONSTRAINT [DF_PackingErrorRecord_ActionTaken] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_PackingErrorRecord] PRIMARY KEY CLUSTERED ([ID] ASC)
);

