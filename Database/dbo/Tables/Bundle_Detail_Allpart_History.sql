CREATE TABLE [dbo].[Bundle_Detail_Allpart_History](
	[ID] [varchar](13) NOT NULL,
	[Patterncode] [varchar](20) NOT NULL,
	[PatternDesc] [nvarchar](100) NULL,
	[parts] [numeric](5, 0) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[IsPair] [bit] NULL,
	[Location] [varchar](1) NOT NULL,
 CONSTRAINT [PK_Bundle_Detail_Allpart_History] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bundle_Detail_Allpart_History] ADD  CONSTRAINT [DF_Bundle_Detail_Allpart_History_ID]  DEFAULT ((0)) FOR [ID]
GO

ALTER TABLE [dbo].[Bundle_Detail_Allpart_History] ADD  CONSTRAINT [DF_Bundle_Detail_Allpart_History_Patterncode]  DEFAULT ('') FOR [Patterncode]
GO

ALTER TABLE [dbo].[Bundle_Detail_Allpart_History] ADD  CONSTRAINT [DF_Bundle_Detail_Allpart_History_PatternDesc]  DEFAULT ('') FOR [PatternDesc]
GO

ALTER TABLE [dbo].[Bundle_Detail_Allpart_History] ADD  CONSTRAINT [DF_Bundle_Detail_Allpart_History_parts]  DEFAULT ((0)) FOR [parts]
GO

ALTER TABLE [dbo].[Bundle_Detail_Allpart_History] ADD  DEFAULT ('') FOR [Location]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle 單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Allpart_History', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版片編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Allpart_History', @level2type=N'COLUMN',@level2name=N'Patterncode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版片敘述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Allpart_History', @level2type=N'COLUMN',@level2name=N'PatternDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Allpart_History', @level2type=N'COLUMN',@level2name=N'parts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle Detail Apll Part History' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_Allpart_History'
GO
