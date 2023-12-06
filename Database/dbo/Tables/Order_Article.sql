CREATE TABLE [dbo].[Order_Article](
	[id] [varchar](13) NOT NULL,
	[Seq] [smallint] NULL,
	[Article] [varchar](8) NOT NULL,
	[TissuePaper] [bit] NULL,
	[CertificateNumber] [nvarchar](100) NOT NULL,
	[SecurityCode] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Order_Article] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[Article] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order_Article] ADD  CONSTRAINT [DF_Order_Article_id]  DEFAULT ('') FOR [id]
GO

ALTER TABLE [dbo].[Order_Article] ADD  CONSTRAINT [DF_Order_Article_Seq]  DEFAULT ((0)) FOR [Seq]
GO

ALTER TABLE [dbo].[Order_Article] ADD  CONSTRAINT [DF_Order_Article_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[Order_Article] ADD  CONSTRAINT [DF_Order_Article_TissuePaper]  DEFAULT ((0)) FOR [TissuePaper]
GO

ALTER TABLE [dbo].[Order_Article] ADD  CONSTRAINT [DF_Order_Article_CertificateNumber]  DEFAULT ('') FOR [CertificateNumber]
GO

ALTER TABLE [dbo].[Order_Article] ADD  CONSTRAINT [DF_Order_Article_SecurityCode]  DEFAULT ('') FOR [SecurityCode]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Article', @level2type=N'COLUMN',@level2name=N'id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Article', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Article', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'棉紙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Article', @level2type=N'COLUMN',@level2name=N'TissuePaper'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order Article' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Article'
GO
