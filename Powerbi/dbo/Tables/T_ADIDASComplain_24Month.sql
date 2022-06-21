CREATE TABLE [dbo].[T_ADIDASComplain_24Month](
	[Year] [varchar](4) NOT NULL,
	[Month] [varchar](2) NOT NULL,
	[BrandFtyCode] [varchar](10) NOT NULL,
	[FactoryName] [varchar](40) NOT NULL,
	[KPILO] [varchar](30) NOT NULL,
	[PV_RAW_24Month] [numeric](18, 4) NOT NULL,
	[SV_RAW_24Month] [numeric](18, 4) NOT NULL,
	[WHC] [numeric](18, 4) NOT NULL,
	[Defective_Return] [numeric](18, 4) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_T_ADIDASComplain_24Month] PRIMARY KEY CLUSTERED 
(
	[Year] ASC,
	[Month] ASC,
	[BrandFtyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_Year]  DEFAULT ('') FOR [Year]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_Month]  DEFAULT ('') FOR [Month]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_BrandFtyCode]  DEFAULT ('') FOR [BrandFtyCode]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_FactoryName]  DEFAULT ('') FOR [FactoryName]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_KPILO]  DEFAULT ('') FOR [KPILO]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_PV_RAW_24Month]  DEFAULT ((0)) FOR [PV_RAW_24Month]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_SV_RAW_24Month]  DEFAULT ((0)) FOR [SV_RAW_24Month]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_WHC]  DEFAULT ((0)) FOR [WHC]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_Defective_Return]  DEFAULT ((0)) FOR [Defective_Return]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[T_ADIDASComplain_24Month] ADD  CONSTRAINT [DF_T_ADIDASComplain_24Month_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ADIDASComplain_24Month', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ADIDASComplain_24Month', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ADIDASComplain_24Month', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ADIDASComplain_24Month', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

