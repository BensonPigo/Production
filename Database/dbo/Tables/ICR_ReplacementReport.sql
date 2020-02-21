CREATE TABLE [dbo].[ICR_ReplacementReport] (
    [ID]            VARCHAR (13) DEFAULT ('') NOT NULL,
    [ReplacementNo] VARCHAR (20) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ICR_ReplacementReport] PRIMARY KEY CLUSTERED ([ID] ASC, [ReplacementNo] ASC)
);

