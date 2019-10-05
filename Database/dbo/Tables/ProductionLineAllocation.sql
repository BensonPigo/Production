CREATE TABLE [dbo].[ProductionLineAllocation](
	[FactoryID] [varchar](8) NOT NULL,
	[ProductionDate] [date] NOT NULL,
 CONSTRAINT [PK_ProductionLineAllocation] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[ProductionDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation', @level2type=N'COLUMN',@level2name=N'ProductionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線配置主檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductionLineAllocation'
GO