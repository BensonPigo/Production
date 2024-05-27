	CREATE TABLE [dbo].[P_SubProInsReportMonthlyRate]
	(
		[Month]				INT																				DEFAULT (0)			NOT NULL , 
		[FactoryID]         VARCHAR(8)      CONSTRAINT [DF_P_SubProInsReportMonthlyRate_FactoryID]			DEFAULT ((''))      NOT NULL, 
		[SubprocessRate]    NUMERIC(5, 2)   CONSTRAINT [DF_P_SubProInsReportMonthlyRate_SubprocessRate]     DEFAULT (0)         NOT NULL, 
		[TotalPassQty]      INT             CONSTRAINT [DF_P_SubProInsReportMonthlyRate_TotalPassQty]		DEFAULT (0)         NOT NULL, 
		[TotalQty]          INT             CONSTRAINT [DF_P_SubProInsReportMonthlyRate_TotalQty]			DEFAULT (0)         NOT NULL, 
		CONSTRAINT [PK_P_SubProInsReportMonthlyRate] PRIMARY KEY ([Month],[FactoryID])
	)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'月份',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportMonthlyRate',
    @level2type = N'COLUMN',
    @level2name = N'Month'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FactoryID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportMonthlyRate',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SubprocessRate',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportMonthlyRate',
    @level2type = N'COLUMN',
    @level2name = N'SubprocessRate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'上個月Pass的綁包總數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportMonthlyRate',
    @level2type = N'COLUMN',
    @level2name = N'TotalPassQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'上個月的綁包總數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReportMonthlyRate',
    @level2type = N'COLUMN',
    @level2name = N'TotalQty'