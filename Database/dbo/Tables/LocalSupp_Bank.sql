
CREATE TABLE [dbo].[LocalSupp_Bank](
	[ID] [varchar](8) NOT NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[PKey] [bigint] IDENTITY(1,1) NOT NULL,
	[ByCheck] [bit] NOT NULL,
	[Status] [varchar](15) NOT NULL,
	[ApproveName] [varchar](15) NOT NULL,
	[ApproveDate] [datetime] NULL,
 CONSTRAINT [PK_LocalSupp_Bank] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[PKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LocalSupp_Bank] ADD  CONSTRAINT [DF_LocalSupp_Bank_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[LocalSupp_Bank] ADD  CONSTRAINT [DF_LocalSupp_Bank_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[LocalSupp_Bank] ADD  CONSTRAINT [DF_LocalSupp_Bank_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[LocalSupp_Bank] ADD  DEFAULT ((0)) FOR [ByCheck]
GO

ALTER TABLE [dbo].[LocalSupp_Bank] ADD  DEFAULT ('') FOR [Status]
GO

ALTER TABLE [dbo].[LocalSupp_Bank] ADD  DEFAULT ('') FOR [ApproveName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalSupp_Bank', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalSupp_Bank', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalSupp_Bank', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalSupp_Bank', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalSupp_Bank', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Supplier_bank' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalSupp_Bank'
GO


