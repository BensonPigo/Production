CREATE TABLE [dbo].[Style_Article](
	[StyleUkey] [bigint] NOT NULL,
	[Seq] [smallint] NULL,
	[Article] [varchar](8) NOT NULL,
	[TissuePaper] [bit] NULL,
	[ArticleName] [nvarchar](100) NULL,
	[Contents] [nvarchar](max) NULL,
	[GarmentLT] [numeric](3, 0) NULL,
	[SourceFile] [nvarchar](266) NOT NULL,
	[Description] [nvarchar](60) NOT NULL,
	[FDUploadDate] [datetime] NULL,
	[BuyReadyDate] [date] NULL,
	[CertificateNumber] [nvarchar](100) NOT NULL,
	[SecurityCode] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Style_Article] PRIMARY KEY CLUSTERED 
(
	[StyleUkey] ASC,
	[Article] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Style_Article] ADD  CONSTRAINT [DF_Style_Article_StyleUkey]  DEFAULT ((0)) FOR [StyleUkey]
GO

ALTER TABLE [dbo].[Style_Article] ADD  CONSTRAINT [DF_Style_Article_Seq]  DEFAULT ((0)) FOR [Seq]
GO

ALTER TABLE [dbo].[Style_Article] ADD  CONSTRAINT [DF_Style_Article_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[Style_Article] ADD  CONSTRAINT [DF_Style_Article_TissuePaper]  DEFAULT ((0)) FOR [TissuePaper]
GO

ALTER TABLE [dbo].[Style_Article] ADD  CONSTRAINT [DF_Style_Article_ArticleName]  DEFAULT ('') FOR [ArticleName]
GO

ALTER TABLE [dbo].[Style_Article] ADD  CONSTRAINT [DF_Style_Article_Content]  DEFAULT ('') FOR [Contents]
GO

ALTER TABLE [dbo].[Style_Article] ADD  DEFAULT ((0)) FOR [GarmentLT]
GO

ALTER TABLE [dbo].[Style_Article] ADD  DEFAULT ('') FOR [SourceFile]
GO

ALTER TABLE [dbo].[Style_Article] ADD  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[Style_Article] ADD  CONSTRAINT [DF_Style_Article_CertificateNumber]  DEFAULT ('') FOR [CertificateNumber]
GO

ALTER TABLE [dbo].[Style_Article] ADD  CONSTRAINT [DF_Style_Article_SecurityCode]  DEFAULT ('') FOR [SecurityCode]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式的唯一值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'StyleUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'棉紙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'TissuePaper'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色組名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'ArticleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣成份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'Contents'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment L/T' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'GarmentLT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FD 檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'SourceFile'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FD 描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FD 上傳日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article', @level2type=N'COLUMN',@level2name=N'FDUploadDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style Article' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_Article'
GO


