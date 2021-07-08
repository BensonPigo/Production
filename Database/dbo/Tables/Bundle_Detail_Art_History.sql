CREATE TABLE [dbo].[Bundle_Detail_Art_History](
	[Bundleno] [varchar](10) NOT NULL,
	[SubprocessId] [varchar](15) NOT NULL,
	[PatternCode] [varchar](20) NOT NULL,
	[ID] [varchar](13) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[PostSewingSubProcess] [bit] NOT NULL,
	[NoBundleCardAfterSubprocess] [bit] NOT NULL,
 CONSTRAINT [PK_Bundle_Detail_Art_History] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bundle_Detail_Art_History] ADD  CONSTRAINT [DF_Bundle_Detail_Art_History_Bundleno]  DEFAULT ('') FOR [Bundleno]
GO

ALTER TABLE [dbo].[Bundle_Detail_Art_History] ADD  CONSTRAINT [DF_Bundle_Detail_Art_History_SubprocessId]  DEFAULT ('') FOR [SubprocessId]
GO

ALTER TABLE [dbo].[Bundle_Detail_Art_History] ADD  CONSTRAINT [DF_Bundle_Detail_Art_History_PatternCode]  DEFAULT ('') FOR [PatternCode]
GO

ALTER TABLE [dbo].[Bundle_Detail_Art_History] ADD  CONSTRAINT [DF_Bundle_Detail_Art_History_ID]  DEFAULT ((0)) FOR [ID]
GO

ALTER TABLE [dbo].[Bundle_Detail_Art_History] ADD  DEFAULT ((0)) FOR [PostSewingSubProcess]
GO

ALTER TABLE [dbo].[Bundle_Detail_Art_History] ADD  DEFAULT ((0)) FOR [NoBundleCardAfterSubprocess]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BundleNo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Art_History', @level2type=N'COLUMN',@level2name=N'Bundleno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工項目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Art_History', @level2type=N'COLUMN',@level2name=N'SubprocessId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版片名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Art_History', @level2type=N'COLUMN',@level2name=N'PatternCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Art_History', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原bundle2.artwork轉成table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Art_History'
GO

