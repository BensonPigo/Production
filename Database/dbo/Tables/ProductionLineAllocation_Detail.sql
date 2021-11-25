--ProductionLineAllocation_Detail
CREATE TABLE [dbo].[ProductionLineAllocation_Detail] (
    [FactoryID]      VARCHAR (8) NOT NULL,
    [ProductionDate] DATE        NOT NULL,
    [LineLocationID] VARCHAR (2) NOT NULL,
    [SewingLineID]   VARCHAR (5) NOT NULL,
    [Team]           VARCHAR (5) NOT NULL,
    CONSTRAINT [PK_ProductionLineAllocation_Detail] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [ProductionDate] ASC, [LineLocationID] ASC, [SewingLineID] ASC, [Team] ASC)
);



GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation_Detail', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation_Detail', @level2type=N'COLUMN',@level2name=N'ProductionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation_Detail', @level2type=N'COLUMN',@level2name=N'LineLocationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Line ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation_Detail', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation_Detail', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線配置細節檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation_Detail'
GO