	CREATE TABLE [dbo].[Style_SpecialMark](
		[ID] [varchar](5) NOT NULL,
		[BrandID] [varchar](8) NOT NULL,
		[Name] [varchar](50) NOT NULL,
		[Remark] [varchar](500) NOT NULL,
		[Junk] [bit] NOT NULL,
		[AddName] [varchar](10) NOT NULL,
		[AddDate] [datetime] NULL,
		[EditName] [varchar](10) NOT NULL,
		[EditDate] [datetime] NULL,
	 CONSTRAINT [PK_Style_SpecialMark] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[BrandID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	ALTER TABLE [dbo].[Style_SpecialMark] ADD  CONSTRAINT [DF_Style_SpecialMark_ID]  DEFAULT ('') FOR [ID]
	ALTER TABLE [dbo].[Style_SpecialMark] ADD  CONSTRAINT [DF_Style_SpecialMark_BrandID]  DEFAULT ('') FOR [BrandID]
	ALTER TABLE [dbo].[Style_SpecialMark] ADD  CONSTRAINT [DF_Style_SpecialMark_Name]  DEFAULT ('') FOR [Name]
	ALTER TABLE [dbo].[Style_SpecialMark] ADD  CONSTRAINT [DF_Style_SpecialMark_Remark]  DEFAULT ('') FOR [Remark]
	ALTER TABLE [dbo].[Style_SpecialMark] ADD  CONSTRAINT [DF_Style_SpecialMark_Junk]  DEFAULT ((0)) FOR [Junk]
	ALTER TABLE [dbo].[Style_SpecialMark] ADD  CONSTRAINT [DF_Style_SpecialMark_AddName]  DEFAULT ('') FOR [AddName]
	ALTER TABLE [dbo].[Style_SpecialMark] ADD  CONSTRAINT [DF_Style_SpecialMark_EditName]  DEFAULT ('') FOR [EditName]