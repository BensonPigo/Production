CREATE TABLE [dbo].[TransExport] (
    [GroupID] INT         NOT NULL,
    [Seq]     INT         NOT NULL,
    [Name]    NCHAR (100) NULL,
    [TSQL]    NCHAR (500) NULL,
    CONSTRAINT [PK_TransExport] PRIMARY KEY CLUSTERED ([GroupID] ASC, [Seq] ASC)
);

