CREATE TABLE [dbo].[Supp_BrandSuppCode](
	[ID] [varchar](6) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[SuppCode] [varchar](6) NOT NULL,
	[T2LO] [varchar](50) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[SuppName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Supp_BrandSuppCode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[BrandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Supp_BrandSuppCode] ADD  CONSTRAINT [DF_Supp_BrandSuppCode_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[Supp_BrandSuppCode] ADD  CONSTRAINT [DF_Supp_BrandSuppCode_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[Supp_BrandSuppCode] ADD  CONSTRAINT [DF_Supp_BrandSuppCode_SuppCode]  DEFAULT ('') FOR [SuppCode]
GO

ALTER TABLE [dbo].[Supp_BrandSuppCode] ADD  CONSTRAINT [DF_Supp_BrandSuppCode_T2LO]  DEFAULT ('') FOR [T2LO]
GO

ALTER TABLE [dbo].[Supp_BrandSuppCode] ADD  CONSTRAINT [DF_Supp_BrandSuppCode_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Supp_BrandSuppCode] ADD  CONSTRAINT [DF_Supp_BrandSuppCode_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Supplier Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Brand' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Brand Supplier Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'SuppCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'T2 LO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'T2LO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Brand Supplier name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode', @level2type=N'COLUMN',@level2name=N'SuppName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Brand Supplier Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Supp_BrandSuppCode'
GO