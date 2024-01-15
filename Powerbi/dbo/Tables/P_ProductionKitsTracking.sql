CREATE TABLE [dbo].[P_ProductionKitsTracking](
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[Article] [nvarchar](1000) NOT NULL,
	[Mdivision] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Doc] [nvarchar](506) NOT NULL,
	[TWSendDate] [date] NULL,
	[FtyMRRcvDate] [date] NULL,
	[FtySendtoQADate] [date] NULL,
	[QARcvDate] [date] NULL,
	[UnnecessaryToSend] [varchar](1) NOT NULL,
	[ProvideDate] [date] NULL,
	[SPNo] [varchar](13) NOT NULL,
	[SCIDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[Pullforward] [varchar](1) NOT NULL,
	[Handle] [varchar](61) NOT NULL,
	[MRHandle] [varchar](61) NOT NULL,
	[SMR] [varchar](61) NOT NULL,
	[POHandle] [varchar](61) NOT NULL,
	[POSMR] [varchar](61) NOT NULL,
	[FtyHandle] [varchar](41) NOT NULL,
	[ProductionKitsGroup] [varchar](8) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_P_ProductionKitsTracking] PRIMARY KEY CLUSTERED 
(
	[Article] ASC,
	[FactoryID] ASC,
	[Doc] ASC,
	[SPNo] ASC,
	[ProductionKitsGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_Mdivision]  DEFAULT ('') FOR [Mdivision]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_Doc]  DEFAULT ('') FOR [Doc]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_UnnecessaryToSend]  DEFAULT ('') FOR [UnnecessaryToSend]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_SPNo]  DEFAULT ('') FOR [SPNo]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_Pullforward]  DEFAULT ('') FOR [Pullforward]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_Handle]  DEFAULT ('') FOR [Handle]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_MRHandle]  DEFAULT ('') FOR [MRHandle]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_SMR]  DEFAULT ('') FOR [SMR]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_POHandle]  DEFAULT ('') FOR [POHandle]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_POSMR]  DEFAULT ('') FOR [POSMR]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_FtyHandle]  DEFAULT ('') FOR [FtyHandle]
GO

ALTER TABLE [dbo].[P_ProductionKitsTracking] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_ProductionKitsGroup]  DEFAULT ('') FOR [ProductionKitsGroup]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.ProductionKitsGroup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'ProductionKitsGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'EditDate'
GO