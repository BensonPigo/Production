CREATE TABLE [dbo].[SampleGarmentTest_Detail_Shrinkage] (
    [ID]         BIGINT          NOT NULL,
    [No]         INT             NOT NULL,
    [Location]   VARCHAR (2)     NOT NULL,
    [Type]       VARCHAR (150)   NOT NULL,
    [BeforeWash] NUMERIC (11, 2) NULL,
    [SizeSpec]   NUMERIC (11, 2) NULL,
    [AfterWash1] NUMERIC (11, 2) NULL,
    [Shrinkage1] NUMERIC (11, 2) NULL,
    [AfterWash2] NUMERIC (11, 2) NULL,
    [Shrinkage2] NUMERIC (11, 2) NULL,
    [AfterWash3] NUMERIC (11, 2) NULL,
    [Shrinkage3] NUMERIC (11, 2) NULL,
    [Seq]        INT             NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC, [Location] ASC, [Type] ASC)
);



