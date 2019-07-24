CREATE TABLE [dbo].[SampleGarmentTest_Detail_Apperance] (
    [ID]      BIGINT         NOT NULL,
    [No]      INT            NOT NULL,
    [Type]    VARCHAR (200)   NOT NULL,
    [Wash1]   VARCHAR (50)   DEFAULT ('N/A') NOT NULL,
    [Wash2]   VARCHAR (10)   DEFAULT ('N/A') NOT NULL,
    [Wash3]   VARCHAR (10)   DEFAULT ('N/A') NOT NULL,
    [Comment] NVARCHAR (500) NOT NULL DEFAULT (''),
    [Seq]     INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([ID],[No],[Seq])
);

