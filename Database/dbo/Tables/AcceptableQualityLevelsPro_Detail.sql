CREATE TABLE [dbo].[AcceptableQualityLevelsPro_Detail](
	[ProUkey] [bigint] NOT NULL,
	[AQLDefectCategoryUkey] [bigint] NOT NULL,
	[AcceptedQty] [int] NOT NULL,
 CONSTRAINT [PK_AcceptableQualityLevelsPro_Detail] PRIMARY KEY CLUSTERED 
(
	[ProUkey] ASC,
	[AQLDefectCategoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro_Detail] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_Detail_ProUkey]  DEFAULT ((0)) FOR [ProUkey]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro_Detail] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_Detail_AQLDefectCategoryUkey]  DEFAULT ((0)) FOR [AQLDefectCategoryUkey]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro_Detail] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_Detail_AcceptedQty]  DEFAULT ((0)) FOR [AcceptedQty]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AcceptableQualityLevelsPro的Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro_Detail', @level2type=N'COLUMN',@level2name=N'ProUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AcceptableQualityLevelsPro_DefectCategory的Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro_Detail', @level2type=N'COLUMN',@level2name=N'AQLDefectCategoryUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該Defect種類可容忍檢驗失敗數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro_Detail', @level2type=N'COLUMN',@level2name=N'AcceptedQty'
GO


