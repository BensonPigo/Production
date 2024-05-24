CREATE TABLE [dbo].[P_SubProInsReportDailyRate]
(
	[InspectionDate]    date                                                                NOT NULL, 
    [FactoryID]         VARCHAR(8)      CONSTRAINT [DF_P_SubProInsReportDailyRate_FactoryID]      DEFAULT ((''))      NOT NULL, 
    [SubprocessRate]    NUMERIC(5, 2)   CONSTRAINT [DF_P_SubProInsReportDailyRate_SubprocessRate]      DEFAULT (0)         NOT NULL, 
    [TotalPassQty]      INT             CONSTRAINT [DF_P_SubProInsReportDailyRate_TotalPassQty]      DEFAULT (0)         NOT NULL, 
    [TotalQty]          INT             CONSTRAINT [DF_P_SubProInsReportDailyRate_TotalQty]      DEFAULT (0)         NOT NULL, 
    CONSTRAINT [PK_P_SubProInsReportDailyRate] PRIMARY KEY ([InspectionDate],[FactoryID])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'InspectionDate',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportDailyRate',
    @level2type = N'COLUMN',
    @level2name = N'InspectionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FactoryID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportDailyRate',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SubprocessRate',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportDailyRate',
    @level2type = N'COLUMN',
    @level2name = N'SubprocessRate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'InspectionDate當天Pass的綁包總數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportDailyRate',
    @level2type = N'COLUMN',
    @level2name = N'TotalPassQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'InspectionDate當天綁包總數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportDailyRate',
    @level2type = N'COLUMN',
    @level2name = N'TotalQty'