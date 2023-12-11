CREATE TABLE [dbo].[PO_Supp_Detail_Keyword](
	[ID] [varchar](13) NOT NULL,
	[Seq1] [varchar](3) NOT NULL,
	[Seq2] [varchar](2) NOT NULL,
	[KeywordField] [varchar](30) NOT NULL,
	[KeywordValue] [nvarchar](200) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_PO_Supp_Detail_Keyword] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Seq1] ASC,
	[Seq2] ASC,
	[KeywordField] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PO_Supp_Detail_Keyword] ADD  CONSTRAINT [DF_PO_Supp_Detail_Keyword_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[PO_Supp_Detail_Keyword] ADD  CONSTRAINT [DF_PO_Supp_Detail_Keyword_Seq1]  DEFAULT ('') FOR [Seq1]
GO

ALTER TABLE [dbo].[PO_Supp_Detail_Keyword] ADD  CONSTRAINT [DF_PO_Supp_Detail_Keyword_Seq2]  DEFAULT ('') FOR [Seq2]
GO

ALTER TABLE [dbo].[PO_Supp_Detail_Keyword] ADD  CONSTRAINT [DF_Table_1_SpecColumnID]  DEFAULT ('') FOR [KeywordField]
GO

ALTER TABLE [dbo].[PO_Supp_Detail_Keyword] ADD  CONSTRAINT [DF_PO_Supp_Detail_Keyword_KeywordValue]  DEFAULT ('') FOR [KeywordValue]
GO

ALTER TABLE [dbo].[PO_Supp_Detail_Keyword] ADD  CONSTRAINT [DF_PO_Supp_Detail_Keyword_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[PO_Supp_Detail_Keyword] ADD  CONSTRAINT [DF_PO_Supp_Detail_Keyword_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp_Detail_Keyword', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大項編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp_Detail_Keyword', @level2type=N'COLUMN',@level2name=N'Seq1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp_Detail_Keyword', @level2type=N'COLUMN',@level2name=N'Seq2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp_Detail_Keyword', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp_Detail_Keyword', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp_Detail_Keyword', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp_Detail_Keyword', @level2type=N'COLUMN',@level2name=N'EditDate'
GO
