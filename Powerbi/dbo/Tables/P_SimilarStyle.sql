Create Table P_SimilarStyle
(
OutputDate date not null,
FactoryID varchar(8) not null,
StyleID varchar(15) not null,
BrandID varchar(8) not null,
Remark nvarchar(200) not null,
RemarkSimilarStyle nvarchar(2000) not null,
Type varchar(10)
PRIMARY KEY (OutputDate, FactoryID, StyleID, BrandID) NOT NULL
)

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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'OutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顯示該款示最近一次產出之日期&最小的產線名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顯示該款示之相似母款, 以及最近一次產出之日期&最小的產線名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'RemarkSimilarStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'如果Remark一欄有值 & 且最後產出日期落在當日往前算三個月內 (動態計算), 或是Remark(Similar style) 一欄有值 & 且最後產出日期落在當日往前算三個月內 (動態計算),  顯示"Repeat", 否則顯示"New Style"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'Type'
GO