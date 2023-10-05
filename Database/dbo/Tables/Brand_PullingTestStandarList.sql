CREATE TABLE [dbo].[Brand_PullingTestStandarList](
	[BrandID] [varchar](8) NOT NULL,
	[TestItem] [varchar](20) NOT NULL,
	[PullForceUnit] [varchar](6) NOT NULL,
	[PullForce] [numeric](6, 2) NOT NULL,
	[Time] [int] NOT NULL,
	[StyleType] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Brand_PullingTestStandarList] PRIMARY KEY CLUSTERED 
(
	[BrandID] ASC,
	[TestItem] ASC,
	[PullForceUnit] ASC,
	[StyleType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Brand_PullingTestStandarList] ADD  CONSTRAINT [DF_Brand_PullingTestStandarList_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[Brand_PullingTestStandarList] ADD  CONSTRAINT [DF_Brand_PullingTestStandarList_TestItem]  DEFAULT ('') FOR [TestItem]
GO

ALTER TABLE [dbo].[Brand_PullingTestStandarList] ADD  CONSTRAINT [DF_Brand_PullingTestStandarList_PullForceUnit]  DEFAULT ('') FOR [PullForceUnit]
GO

ALTER TABLE [dbo].[Brand_PullingTestStandarList] ADD  CONSTRAINT [DF_Brand_PullingTestStandarList_PullForce]  DEFAULT ((0)) FOR [PullForce]
GO

ALTER TABLE [dbo].[Brand_PullingTestStandarList] ADD  CONSTRAINT [DF_Brand_PullingTestStandarList_Time]  DEFAULT ((0)) FOR [Time]
GO

ALTER TABLE [dbo].[Brand_PullingTestStandarList] ADD  CONSTRAINT [DF_Brand_PullingTestStandarList_StyleType]  DEFAULT ('') FOR [StyleType]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'測試項目 e.g. Snaps, Buttons, Rivet, ….' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_PullingTestStandarList', @level2type=N'COLUMN',@level2name=N'TestItem'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拉力單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_PullingTestStandarList', @level2type=N'COLUMN',@level2name=N'PullForceUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拉力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_PullingTestStandarList', @level2type=N'COLUMN',@level2name=N'PullForce'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'持續時間 (秒)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_PullingTestStandarList', @level2type=N'COLUMN',@level2name=N'Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value: Adults/ Youth' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_PullingTestStandarList', @level2type=N'COLUMN',@level2name=N'StyleType'
GO
