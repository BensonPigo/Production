CREATE TABLE [dbo].[P_PPICMasterList_ArtworkType](
	[Ukey] [bigint] NOT NULL IDENTITY,
	[SP#] [varchar](13) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[ArtworkTypeNo] [varchar](4) NOT NULL,
	[ArtworkType] [varchar](20) NOT NULL,
	[Value] [numeric](38, 6) NOT NULL,
	[TotalValue] [numeric](38, 6) NOT NULL,
	[ArtworkTypeUnit] [varchar](10) NOT NULL,
	[SubconInTypeID] [varchar](2) NOT NULL,
	[ArtworkTypeKey] [varchar](35) NOT NULL,
	[OrderDataKey] [varchar](22) NOT NULL,
 CONSTRAINT [PK_P_PPICMasterList_ArtworkType] PRIMARY KEY CLUSTERED 
(
	[SP#] ASC,
	[FactoryID] ASC,
	[SubconInTypeID] ASC,
	[ArtworkTypeKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_SP#]  DEFAULT ('') FOR [SP#]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_ArtworkTypeNo]  DEFAULT ('') FOR [ArtworkTypeNo]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_ArtworkType]  DEFAULT ('') FOR [ArtworkType]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_Value]  DEFAULT ((0)) FOR [Value]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_TotalValue]  DEFAULT ((0)) FOR [TotalValue]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_ArtworkTypeUnit]  DEFAULT ('') FOR [ArtworkTypeUnit]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_SubconInTypeID]  DEFAULT ('') FOR [SubconInTypeID]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_ArtworkTypeKey]  DEFAULT ('') FOR [ArtworkTypeKey]
GO

ALTER TABLE [dbo].[P_PPICMasterList_ArtworkType] ADD  CONSTRAINT [DF_P_PPICMasterList_ArtworkType_OrderDataKey]  DEFAULT ('') FOR [OrderDataKey]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'SP#'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkTypeNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成本資訊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成本總計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'TotalValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工成本單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkTypeUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代工類型代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'SubconInTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ArtworkTypeID+Unit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkTypeKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderID+SubconInType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMasterList_ArtworkType', @level2type=N'COLUMN',@level2name=N'OrderDataKey'
GO


