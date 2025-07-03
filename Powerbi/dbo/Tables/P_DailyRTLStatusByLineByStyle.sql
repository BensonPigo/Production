CREATE TABLE [dbo].[P_DailyRTLStatusByLineByStyle](
	[TransferDate] [date] NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[APSNo] [int] NOT NULL,
	[SewingLineID] [varchar](5) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[CurrentWIP] [int] NOT NULL,
	[StdQty] [int] NOT NULL,
	[WIP] [numeric](5, 2) NOT NULL,
	[nWIP] [numeric](5, 2) NOT NULL,
	[InLine] [date] NOT NULL,
	[OffLine] [date] NOT NULL,
	[NewCdCode] [varchar](5) NOT NULL,
	[ProductType] [nvarchar](500) NOT NULL,
	[FabricType] [nvarchar](500) NOT NULL,
	[AlloQty] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_DailyRTLStatusByLineByStyle] PRIMARY KEY CLUSTERED 
(
	[TransferDate] ASC,
	[FactoryID] ASC,
	[APSNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_APSNo]  DEFAULT ((0)) FOR [APSNo]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_CurrentWIP]  DEFAULT ((0)) FOR [CurrentWIP]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_StdQty]  DEFAULT ((0)) FOR [StdQty]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_WIP]  DEFAULT ((0)) FOR [WIP]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_nWIP]  DEFAULT ((0)) FOR [nWIP]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_NewCdCode]  DEFAULT ('') FOR [NewCdCode]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_ProductType]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_AlloQty]  DEFAULT ((0)) FOR [AlloQty]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyRTLStatusByLineByStyle', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyRTLStatusByLineByStyle', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
