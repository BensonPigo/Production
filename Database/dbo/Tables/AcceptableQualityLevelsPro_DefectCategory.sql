CREATE TABLE [dbo].[AcceptableQualityLevelsPro_DefectCategory](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](20) NOT NULL,
 CONSTRAINT [PK_AcceptableQualityLevelsPro_DefectCategory] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AcceptableQualityLevelsPro_DefectCategory] ADD  CONSTRAINT [DF_AcceptableQualityLevelsPro_DefectCategory_Description]  DEFAULT ('') FOR [Description]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Defect種類Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro_DefectCategory', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Defect種類描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AcceptableQualityLevelsPro_DefectCategory', @level2type=N'COLUMN',@level2name=N'Description'
GO

