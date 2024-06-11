CREATE TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture](
	[BrandID] [varchar](8) NOT NULL,
	[Category] [varchar](30) NOT NULL,
	[CategoryValue] [varchar](30) NOT NULL,
	[MoistureStandardDesc] [nvarchar](10) NOT NULL,
	[MoistureStandard1] [numeric](4, 0) NOT NULL,
	[MoistureStandard1_Comparison] [tinyint] NOT NULL,
	[MoistureStandard2] [numeric](4, 0) NOT NULL,
	[MoistureStandard2_Comparison] [tinyint] NOT NULL,
 CONSTRAINT [PK_Brand_QAMoistureStandardListByActMoisture] PRIMARY KEY CLUSTERED 
(
	[BrandID] ASC,
	[MoistureStandard1] ASC,
	[MoistureStandard1_Comparison] ASC,
	[MoistureStandard2] ASC,
	[MoistureStandard2_Comparison] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture] ADD  CONSTRAINT [DF_Brand_QAMoistureStandardListByActMoisture_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture] ADD  CONSTRAINT [DF_Brand_QAMoistureStandardListByActMoisture_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture] ADD  CONSTRAINT [DF_Brand_QAMoistureStandardListByActMoisture_CategoryValue]  DEFAULT ('') FOR [CategoryValue]
GO

ALTER TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture] ADD  CONSTRAINT [DF_Brand_QAMoistureStandardListByActMoisture_MoistureStandardDesc]  DEFAULT ('') FOR [MoistureStandardDesc]
GO

ALTER TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture] ADD  CONSTRAINT [DF_Brand_QAMoistureStandardListByActMoisture_MoistureStandard1]  DEFAULT ((0)) FOR [MoistureStandard1]
GO

ALTER TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture] ADD  CONSTRAINT [DF_Brand_QAMoistureStandardListByActMoisture_MoistureStandard1_Comparison]  DEFAULT ((0)) FOR [MoistureStandard1_Comparison]
GO

ALTER TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture] ADD  CONSTRAINT [DF_Brand_QAMoistureStandardListByActMoisture_MoistureStandard2]  DEFAULT ((0)) FOR [MoistureStandard2]
GO

ALTER TABLE [dbo].[Brand_QAMoistureStandardListByActMoisture] ADD  CONSTRAINT [DF_Brand_QAMoistureStandardListByActMoisture_MoistureStandard2_Comparison]  DEFAULT ((0)) FOR [MoistureStandard2_Comparison]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_QAMoistureStandardListByActMoisture', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'對應欄位名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_QAMoistureStandardListByActMoisture', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CategoryValue' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_QAMoistureStandardListByActMoisture', @level2type=N'COLUMN',@level2name=N'CategoryValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pass 判斷標準 - 此欄位主要是
給人看的' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_QAMoistureStandardListByActMoisture', @level2type=N'COLUMN',@level2name=N'MoistureStandardDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'濕度檢測 Pass 標準範圍的起始' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_QAMoistureStandardListByActMoisture', @level2type=N'COLUMN',@level2name=N'MoistureStandard1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'濕度檢測 Pass 標準範圍的起始
值，判斷符號
（0 = 等於 EqualTo、1 = 大於
GreaterThan、2 = 大於等於
GreaterThanOrEqualTo）
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_QAMoistureStandardListByActMoisture', @level2type=N'COLUMN',@level2name=N'MoistureStandard1_Comparison'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'濕度檢測 Pass 標準範圍的終值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_QAMoistureStandardListByActMoisture', @level2type=N'COLUMN',@level2name=N'MoistureStandard2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'濕度檢測 Pass 標準範圍的終值
，判斷符號
（0 = 等於 EqualTo、3 = 小於
LessThan、4 = 小於等於
LessThanOrEqualTo）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Brand_QAMoistureStandardListByActMoisture', @level2type=N'COLUMN',@level2name=N'MoistureStandard2_Comparison'
GO


