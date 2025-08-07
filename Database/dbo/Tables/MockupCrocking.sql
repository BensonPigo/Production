
CREATE TABLE [dbo].[MockupCrocking](
	[ReportNo] [varchar](14) NOT NULL,
	[POID] [varchar](13) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SeasonID] [varchar](8) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[ArtworkTypeID] [varchar](20) NOT NULL,
	[Remark] [nvarchar](300) NOT NULL,
	[T1Subcon] [varchar](8) NOT NULL,
	[TestDate] [date] NULL,
	[ReceivedDate] [date] NULL,
	[ReleasedDate] [date] NULL,
	[Result] [varchar](4) NOT NULL,
	[Technician] [varchar](10) NOT NULL,
	[MR] [varchar](10) NOT NULL,
	[Type] [varchar](1) NOT NULL,
	[AddDate] [datetime] NULL,
	[AddName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
 [Approver] VARCHAR(10)  CONSTRAINT [DF_MockupCrocking_Approver] NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_MockupCrocking] PRIMARY KEY CLUSTERED 
(
	[ReportNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_POID]  DEFAULT ('') FOR [POID]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_StyleID]  DEFAULT ('') FOR [StyleID]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_SeasonID]  DEFAULT ('') FOR [SeasonID]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_BrandID]  DEFAULT ('') FOR [BrandID]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_Article]  DEFAULT ('') FOR [Article]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_ArtworkTypeID]  DEFAULT ('') FOR [ArtworkTypeID]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_Remark]  DEFAULT ('') FOR [Remark]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_T1Subcon]  DEFAULT ('') FOR [T1Subcon]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_Result]  DEFAULT ('') FOR [Result]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_Technician]  DEFAULT ('') FOR [Technician]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_MR]  DEFAULT ('') FOR [MR]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_Type]  DEFAULT ('') FOR [Type]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_AddName]  DEFAULT ('') FOR [AddName]

GO
ALTER TABLE [dbo].[MockupCrocking] ADD  CONSTRAINT [DF_MockupCrocking_EditName]  DEFAULT ('') FOR [EditName]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'測試單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'ReportNo'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'POID'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'StyleID'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'SeasonID'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'BrandID'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'Article'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'ArtworkTypeID'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'Remark'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'T1 廠商' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'T1Subcon'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'測試日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'TestDate'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'Result'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'技術人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'Technician'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'業務' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'MR'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'區分大貨階段 (B) 與開發階段 (S)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'Type'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'AddDate'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'AddName'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'EditDate'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MockupCrocking', @level2type=N'COLUMN',@level2name=N'EditName'

GO




EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Approver',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MockupCrocking',
    @level2type = N'COLUMN',
    @level2name = N'Approver'