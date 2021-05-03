CREATE TABLE [dbo].[SampleGarmentTest_Detail_FGPT] (
    [ID]                      BIGINT        NOT NULL,
    [No]                      INT           NOT NULL,
    [Location]                VARCHAR (1)   NOT NULL,
    [Type]                    VARCHAR (150) NOT NULL,
    [TestName]                VARCHAR (30)  CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_TestName] DEFAULT ('') NOT NULL,
    [TestDetail]              VARCHAR (10)  CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_TestDetail] DEFAULT ('') NOT NULL,
    [Criteria]                INT           NULL,
    [TestResult]              VARCHAR (10)  CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_3Test] DEFAULT ('') NOT NULL,
    [TestUnit]                VARCHAR (10)  CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_TestUnit] DEFAULT ('') NOT NULL,
    [TypeSelection_VersionID] INT           CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_TypeSelection_VersionID] DEFAULT ((0)) NOT NULL,
    [TypeSelection_Seq]       INT           CONSTRAINT [DF_SampleGarmentTest_Detail_FGPT_TypeSelection_Seq] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SampleGarmentTest_Detail_FGPT] PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC, [Location] ASC, [Type] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type 選項版本號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail_FGPT', @level2type = N'COLUMN', @level2name = N'TypeSelection_VersionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type 同版本下選擇的項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail_FGPT', @level2type = N'COLUMN', @level2name = N'TypeSelection_Seq';

