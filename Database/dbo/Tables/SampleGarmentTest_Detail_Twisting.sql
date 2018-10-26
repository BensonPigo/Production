CREATE TABLE [dbo].[SampleGarmentTest_Detail_Twisting] (
    [ID]       BIGINT          NOT NULL,
    [No]       INT             NOT NULL,
    [Location] VARCHAR (10)    NOT NULL,
    [S1]       NUMERIC (11, 2) DEFAULT ((0.00)) NOT NULL,
    [S2]       NUMERIC (11, 2) DEFAULT ((0.00)) NOT NULL,
    [L]        NUMERIC (11, 2) DEFAULT ((0.00)) NOT NULL,
    [Twisting] NUMERIC (11, 2) DEFAULT ((0.00)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC, [Location] ASC)
);

