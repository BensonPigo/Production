CREATE TABLE [dbo].[P_LineBalancingRate](
	[SewingDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Total SP Qty] [int] NOT NULL,
	[Total LBR] [numeric](7, 2) NOT NULL,
	[Avg. LBR] [numeric](7, 2) NOT NULL,
	[Total SP Qty In7Days] [int] NOT NULL,
	[Total LBR In7Days] [numeric](12, 2) NOT NULL,
	[Avg. LBR In7Days] [numeric](7, 2) NOT NULL,
 CONSTRAINT [PK_P_LineBalancingRate] PRIMARY KEY CLUSTERED 
(
	[SewingDate] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_LineBalancingRate] ADD  CONSTRAINT [DF_P_LineBalancingRate_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_LineBalancingRate] ADD  CONSTRAINT [DF_P_LineBalancingRate_Total SP Qty]  DEFAULT ((0)) FOR [Total SP Qty]
GO

ALTER TABLE [dbo].[P_LineBalancingRate] ADD  CONSTRAINT [DF_P_LineBalancingRate_Total LBR]  DEFAULT ((0)) FOR [Total LBR]
GO

ALTER TABLE [dbo].[P_LineBalancingRate] ADD  CONSTRAINT [DF_P_LineBalancingRate_Avg. LBR]  DEFAULT ((0)) FOR [Avg. LBR]
GO

ALTER TABLE [dbo].[P_LineBalancingRate] ADD  CONSTRAINT [DF_P_LineBalancingRate_Total SP Qty In7Days]  DEFAULT ((0)) FOR [Total SP Qty In7Days]
GO

ALTER TABLE [dbo].[P_LineBalancingRate] ADD  CONSTRAINT [DF_P_LineBalancingRate_Total LBR In7Days]  DEFAULT ((0)) FOR [Total LBR In7Days]
GO

ALTER TABLE [dbo].[P_LineBalancingRate] ADD  CONSTRAINT [DF_P_LineBalancingRate_Avg. LBR In7Days]  DEFAULT ((0)) FOR [Avg. LBR In7Days]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'SewingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'當日有在生產線上的訂單總數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Total SP Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'當日總LBR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Total LBR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'當日平均LBR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Avg. LBR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未來7天(不含當日)在生產線上的訂單總數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Total SP Qty In7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未來7天(不含當日)總LBR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Total LBR In7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未來7天(不含當日)平均LBR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Avg. LBR In7Days'
GO