CREATE TABLE [dbo].[BundleReplacement_Detail_Allpart] (
    [ID]          VARCHAR (13)   NOT NULL,
    [Patterncode] VARCHAR (20)   NOT NULL,
    [PatternDesc] NVARCHAR (100) NULL,
    [parts]       NUMERIC (5)    NOT NULL,
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [IsPair]      BIT            NULL,
    [Location]    VARCHAR (1)    NOT NULL,
    CONSTRAINT [PK_BundleReplacement_Detail_Allpart] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

