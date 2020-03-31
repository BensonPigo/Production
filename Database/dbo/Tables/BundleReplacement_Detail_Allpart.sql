CREATE TABLE [dbo].[BundleReplacement_Detail_Allpart] (
    [ID]          VARCHAR (13)   DEFAULT ('') NOT NULL,
    [Patterncode] VARCHAR (20)   DEFAULT ('') NOT NULL,
    [PatternDesc] NVARCHAR (100) NULL,
    [parts]       NUMERIC (5)    DEFAULT ((0)) NOT NULL,
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [IsPair]      BIT            NULL,
    [Location]    VARCHAR (1)    DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_BundleReplacement_Detail_Allpart] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



