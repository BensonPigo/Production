CREATE TABLE [dbo].[SeasonSCI](
	[ID] [varchar](10) NOT NULL,
	[Month] [varchar](7) NOT NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[Junk] [bit] NULL,
 CONSTRAINT [PK_SeasonSCI] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SeasonSCI] ADD  CONSTRAINT [DF_SeasonSCI_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[SeasonSCI] ADD  CONSTRAINT [DF_SeasonSCI_Month]  DEFAULT ('') FOR [Month]
GO

ALTER TABLE [dbo].[SeasonSCI] ADD  CONSTRAINT [DF_SeasonSCI_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[SeasonSCI] ADD  CONSTRAINT [DF_SeasonSCI_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[SeasonSCI] ADD  CONSTRAINT [DF_SeasonSCI_Junk]  DEFAULT ((0)) FOR [Junk]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI的Season' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeasonSCI', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI Season的起始年月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeasonSCI', @level2type=N'COLUMN',@level2name=N'Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeasonSCI', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeasonSCI', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeasonSCI', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeasonSCI', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeasonSCI', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI的季別基本檔 (SeasonGroup)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeasonSCI'
GO