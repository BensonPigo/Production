CREATE TABLE [dbo].[AcceptableQualityLevelsPro](
	[ProUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[InspectionLevels] [varchar](5) NOT NULL,
	[LotSize_Start] [int] NOT NULL,
	[LotSize_End] [int] NOT NULL,
	[SampleSize] [int] NOT NULL,
	[AQLType] [numeric](2, 1) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[Category] [varchar](15) NOT NULL,
	[Junk] [bit] NOT NULL,
 CONSTRAINT [PK_AcceptableQualityLevelsPro] PRIMARY KEY CLUSTERED 
(
	[ProUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_InspectionLevels]  DEFAULT ('') FOR [InspectionLevels]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_LotSize_Start]  DEFAULT ((0)) FOR [LotSize_Start]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_LotSize_End]  DEFAULT ((0)) FOR [LotSize_End]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_SampleSize]  DEFAULT ((0)) FOR [SampleSize]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_AQLType]  DEFAULT ((0)) FOR [AQLType]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_Junk]  DEFAULT ((0)) FOR [Junk]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'抽樣計劃' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro', @level2type=N'COLUMN',@level2name=N'InspectionLevels'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產品總數 - 判斷範圍起始值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro', @level2type=N'COLUMN',@level2name=N'LotSize_Start'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產品總數 - 判斷範圍結束值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro', @level2type=N'COLUMN',@level2name=N'LotSize_End'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'抽樣數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro', @level2type=N'COLUMN',@level2name=N'SampleSize'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AQL類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro', @level2type=N'COLUMN',@level2name=N'AQLType'
GO


