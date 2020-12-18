CREATE TABLE [dbo].[Consignee](
	[ID] [varchar](8) NOT NULL,
	[Junk] [bit] NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Consignee] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Consignee] ADD  CONSTRAINT [DF_Consignee_Junk]  DEFAULT ((0)) FOR [Junk]
GO

ALTER TABLE [dbo].[Consignee] ADD  CONSTRAINT [DF_Consignee_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Consignee] ADD  CONSTRAINT [DF_Consignee_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Consignee', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Consignee', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Consignee', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Consignee', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Consignee', @level2type=N'COLUMN',@level2name=N'EditDate'
GO