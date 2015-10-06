CREATE TABLE [dbo].[LearnCurve_Detail] (
    [ID]         INT CONSTRAINT [DF_LearnCurve_Detail_ID] DEFAULT ((0)) NOT NULL,
    [Day]        INT CONSTRAINT [DF_LearnCurve_Detail_Day] DEFAULT ((0)) NOT NULL,
    [Efficiency] INT CONSTRAINT [DF_LearnCurve_Detail_Efficiency] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LearnCurve_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Day] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'學習曲線明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'學習曲線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'學習天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve_Detail', @level2type = N'COLUMN', @level2name = N'Day';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'效率(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve_Detail', @level2type = N'COLUMN', @level2name = N'Efficiency';

