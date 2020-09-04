CREATE TABLE [dbo].[SampleGarmentTest_Detail_FGPT] (
    [ID]         BIGINT        NOT NULL,
    [No]         INT           NOT NULL,
    [Location]   VARCHAR (1)   NOT NULL,
    [Type]       VARCHAR (150) NOT NULL,
    [TestName]   VARCHAR (30)  CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_TestName] DEFAULT ('') NOT NULL,
    [TestDetail] VARCHAR (10)  CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_TestDetail] DEFAULT ('') NOT NULL,
    [Criteria]   INT           NULL,
    [TestResult] VARCHAR (10)  CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_3Test] DEFAULT ('') NOT NULL,
    [TestUnit]   VARCHAR (10)  CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_TestUnit] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SampleGarmentTest_Detail_FGPT] PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC, [Location] ASC, [Type] ASC)
);

