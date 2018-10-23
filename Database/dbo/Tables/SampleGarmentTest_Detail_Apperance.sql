CREATE TABLE [dbo].[SampleGarmentTest_Detail_Apperance] (
    [ID]      BIGINT         NOT NULL,
    [No]      INT            NOT NULL,
    [Type]    VARCHAR (50)   NOT NULL,
    [Wash1]   VARCHAR (50)   DEFAULT ('N/A') NOT NULL,
    [Wash2]   VARCHAR (10)   DEFAULT ('N/A') NOT NULL,
    [Wash3]   VARCHAR (10)   DEFAULT ('N/A') NOT NULL,
    [Comment] NVARCHAR (100) NULL,
    [Seq]     INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC, [Type] ASC)
);

