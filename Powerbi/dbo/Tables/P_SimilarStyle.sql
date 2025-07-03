CREATE TABLE [dbo].[P_SimilarStyle](
	[OutputDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[Remark] [nvarchar](200) NOT NULL,
	[RemarkSimilarStyle] [nvarchar](2000) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[OutputDate] ASC,
	[FactoryID] ASC,
	[StyleID] ASC,
	[BrandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_SimilarStyle] ADD  CONSTRAINT [DF_P_SimilarStyle_OutputDate]  DEFAULT ('') FOR [OutputDate]
GO

ALTER TABLE [dbo].[P_SimilarStyle] ADD  CONSTRAINT [DF_P_SimilarStyle_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SimilarStyle] ADD  CONSTRAINT [DF_P_SimilarStyle_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_SimilarStyle] ADD  CONSTRAINT [DF_P_SimilarStyle_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_SimilarStyle] ADD  CONSTRAINT [DF_P_SimilarStyle_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[P_SimilarStyle] ADD  CONSTRAINT [DF_P_SimilarStyle_RemarkSimilarStyle]  DEFAULT ('') FOR [RemarkSimilarStyle]
GO

ALTER TABLE [dbo].[P_SimilarStyle] ADD  CONSTRAINT [DF_P_SimilarStyle_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[P_SimilarStyle] ADD  CONSTRAINT [DF_P_SimilarStyle_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'OutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顯示該款示三個月內最近一次產出之日期&最小的產線名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顯示該款示之相似母款, 以及最近三個月內最後一次產出之日期&最小的產線名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'RemarkSimilarStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'如果Remark一欄有值 & 且最後產出日期落在當日往前算三個月內 (動態計算), 或是Remark(Similar style) 一欄有值 & 且最後產出日期落在當日往前算三個月內 (動態計算),  顯示"Repeat", 否則顯示"New Style"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO