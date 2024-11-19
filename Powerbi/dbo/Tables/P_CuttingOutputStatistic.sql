CREATE TABLE [dbo].[P_CuttingOutputStatistic] (
    [TransferDate]     DATE            CONSTRAINT [DF_P_CuttingOutputStatistic_TransferDate] DEFAULT ('') NOT NULL,
    [FactoryID]        VARCHAR (8)     CONSTRAINT [DF_P_CuttingOutputStatistic_FactoryID] DEFAULT ('') NOT NULL,
    [CutRateByDate]    NUMERIC (5, 2)  CONSTRAINT [DF_P_CuttingOutputStatistic_CutRateByDate] DEFAULT ((0)) NOT NULL,
    [CutRateByMonth]   NUMERIC (5, 2)  CONSTRAINT [DF_P_CuttingOutputStatistic_CutRateByMonth] DEFAULT ((0)) NOT NULL,
    [CutOutputByDate]  NUMERIC (11, 4) CONSTRAINT [DF_P_CuttingOutputStatistic_CutOutputByDate] DEFAULT ((0)) NOT NULL,
    [CutOutputIn7Days] NUMERIC (11, 4) CONSTRAINT [DF_P_CuttingOutputStatistic_CutOutputIn7Days] DEFAULT ((0)) NOT NULL,
    [CutDelayIn7Days]  NUMERIC (11, 4) CONSTRAINT [DF_P_CuttingOutputStatistic_CutDelayIn7Days] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([TransferDate] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Delay計算BY周', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingOutputStatistic', @level2type = N'COLUMN', @level2name = N'CutDelayIn7Days';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Output計算BY周', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingOutputStatistic', @level2type = N'COLUMN', @level2name = N'CutOutputIn7Days';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Output計算BY天', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingOutputStatistic', @level2type = N'COLUMN', @level2name = N'CutOutputByDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Rate計算BY月', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingOutputStatistic', @level2type = N'COLUMN', @level2name = N'CutRateByMonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Rate計算BY天', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingOutputStatistic', @level2type = N'COLUMN', @level2name = N'CutRateByDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingOutputStatistic', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉換日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingOutputStatistic', @level2type = N'COLUMN', @level2name = N'TransferDate';

