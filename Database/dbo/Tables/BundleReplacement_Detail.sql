CREATE TABLE [dbo].[BundleReplacement_Detail] (
    [BundleNo]    VARCHAR (10)   NOT NULL,
    [Id]          VARCHAR (13)   NOT NULL,
    [BundleGroup] NUMERIC (5)    NULL,
    [Patterncode] VARCHAR (20)   NOT NULL,
    [PatternDesc] NVARCHAR (100) NOT NULL,
    [SizeCode]    VARCHAR (8)    NULL,
    [Qty]         NUMERIC (5)    NULL,
    [Parts]       NUMERIC (5)    NULL,
    [Farmin]      NUMERIC (5)    NULL,
    [FarmOut]     NUMERIC (5)    NULL,
    [PrintDate]   DATETIME       NULL,
    [IsPair]      BIT            NULL,
    [Location]    VARCHAR (1)    NOT NULL,
    CONSTRAINT [PK_BundleReplacement_Detail] PRIMARY KEY CLUSTERED ([BundleNo] ASC, [Id] ASC)
);

