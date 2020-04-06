CREATE TABLE [dbo].[BundleReplacement_Detail] (
    [BundleNo]    VARCHAR (10)   DEFAULT ('') NOT NULL,
    [Id]          VARCHAR (13)   DEFAULT ('') NOT NULL,
    [BundleGroup] NUMERIC (5)    NULL,
    [Patterncode] VARCHAR (20)   DEFAULT ('') NOT NULL,
    [PatternDesc] NVARCHAR (100) DEFAULT ('') NOT NULL,
    [SizeCode]    VARCHAR (8)    NULL,
    [Qty]         NUMERIC (5)    NULL,
    [Parts]       NUMERIC (5)    NULL,
    [Farmin]      NUMERIC (5)    NULL,
    [FarmOut]     NUMERIC (5)    NULL,
    [PrintDate]   DATETIME       NULL,
    [IsPair]      BIT            NULL,
    [Location]    VARCHAR (1)    DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_BundleReplacement_Detail] PRIMARY KEY CLUSTERED ([BundleNo] ASC, [Id] ASC),
    CONSTRAINT [UK_BundleReplacement_Detail] UNIQUE NONCLUSTERED ([BundleNo] ASC)
);





