
CREATE TABLE [dbo].[MockupWash_Detail](
	[ReportNo] [varchar](14) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[TypeofPrint] [varchar](30) NOT NULL,
	[Design] [varchar](100) NOT NULL,
	[ArtworkColor] [varchar](35) NOT NULL,
	[AccessoryRefno] [varchar](20) NULL,
	[FabricRefNo] [varchar](30) NOT NULL,
	[FabricColor] [varchar](35) NOT NULL,
	[Result] [varchar](4) NOT NULL,
	[Remark] [nvarchar](300) NOT NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_MockupWash_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_ReportNo]  DEFAULT ('') FOR [ReportNo]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_TypeofPrint]  DEFAULT ('') FOR [TypeofPrint]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_Design]  DEFAULT ('') FOR [Design]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_ArtworkColor]  DEFAULT ('') FOR [ArtworkColor]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_AccessoryRefno]  DEFAULT ('') FOR [AccessoryRefno]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_FabricRefNo]  DEFAULT ('') FOR [FabricRefNo]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_FabricColor]  DEFAULT ('') FOR [FabricColor]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_Result]  DEFAULT ('') FOR [Result]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[MockupWash_Detail] ADD  CONSTRAINT [DF_MockupWash_Detail_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'測試單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'ReportNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'ArtworkColor'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'熱轉印物料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'AccessoryRefno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'FabricRefNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'FabricColor'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'測試結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupWash_Detail', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

CREATE NONCLUSTERED INDEX [ReportNo] ON [dbo].[MockupWash_Detail]
(
	[ReportNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO