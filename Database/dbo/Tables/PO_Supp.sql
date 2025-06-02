CREATE TABLE [dbo].[PO_Supp](
	[ID] [varchar](13) NOT NULL,
	[SEQ1] [varchar](3) NOT NULL,
	[SuppID] [varchar](6) NOT NULL,
	[Remark] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[CompanyID] [numeric](2, 0) NOT NULL,
 CONSTRAINT [PK_PO_Supp] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[SEQ1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PO_Supp] ADD  CONSTRAINT [DF_PO_Supp_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[PO_Supp] ADD  CONSTRAINT [DF_PO_Supp_SEQ1]  DEFAULT ('') FOR [SEQ1]
GO

ALTER TABLE [dbo].[PO_Supp] ADD  CONSTRAINT [DF_PO_Supp_SuppID]  DEFAULT ('') FOR [SuppID]
GO

ALTER TABLE [dbo].[PO_Supp] ADD  CONSTRAINT [DF_PO_Supp_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[PO_Supp] ADD  CONSTRAINT [DF_PO_Supp_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[PO_Supp] ADD  CONSTRAINT [DF_PO_Supp_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[PO_Supp] ADD  CONSTRAINT [DF_PO_Supp_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[PO_Supp] ADD  CONSTRAINT [DF_PO_Supp_CompanyID]  DEFAULT ((0)) FOR [CompanyID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大項編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'SEQ1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單公司別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp', @level2type=N'COLUMN',@level2name=N'CompanyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單-廠商' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO_Supp'
GO