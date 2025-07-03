CREATE TABLE [dbo].[P_SubprocessWIP](
	[Bundleno] [varchar](12) NOT NULL,
	[RFIDProcessLocationID] [varchar](15) NOT NULL,
	[EXCESS] [varchar](5) NOT NULL,
	[FabricKind] [varchar](250) NOT NULL,
	[CutRef] [varchar](10) NOT NULL,
	[Sp] [varchar](250) NOT NULL,
	[MasterSP] [varchar](15) NOT NULL,
	[M] [varchar](8) NOT NULL,
	[FactoryID] [varchar](10) NOT NULL,
	[Category] [varchar](1) NOT NULL,
	[Program] [nvarchar](50) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[Season] [varchar](10) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[Comb] [varchar](4) NOT NULL,
	[CutNo] [numeric](6, 0) NOT NULL,
	[FabPanelCode] [varchar](4) NOT NULL,
	[Article] [varchar](15) NOT NULL,
	[Color] [varchar](10) NOT NULL,
	[ScheduledLineID] [varchar](10) NOT NULL,
	[ScannedLineID] [varchar](10) NOT NULL,
	[Cell] [varchar](10) NOT NULL,
	[Pattern] [varchar](20) NOT NULL,
	[PtnDesc] [nvarchar](200) NOT NULL,
	[Group] [numeric](7, 0) NOT NULL,
	[Size] [varchar](8) NOT NULL,
	[Artwork] [nvarchar](500) NOT NULL,
	[Qty] [numeric](9, 0) NOT NULL,
	[SubprocessID] [varchar](50) NOT NULL,
	[PostSewingSubProcess] [varchar](15) NOT NULL,
	[NoBundleCardAfterSubprocess] [varchar](15) NOT NULL,
	[Location] [varchar](10) NOT NULL,
	[BundleCreateDate] [date] NULL,
	[BuyerDeliveryDate] [date] NULL,
	[SewingInline] [date] NULL,
	[SubprocessQAInspectionDate] [date] NULL,
	[InTime] [datetime] NULL,
	[OutTime] [datetime] NULL,
	[POSupplier] [nvarchar](500) NOT NULL,
	[AllocatedSubcon] [nvarchar](500) NOT NULL,
	[AvgTime] [numeric](20, 9) NOT NULL,
	[TimeRange] [nvarchar](50) NOT NULL,
	[EstimatedCutDate] [date] NULL,
	[CuttingOutputDate] [date] NULL,
	[Item] [varchar](50) NOT NULL,
	[PanelNo] [varchar](24) NOT NULL,
	[CutCellID] [varchar](10) NOT NULL,
	[SpreadingNo] [nvarchar](500) NOT NULL,
	[LastSewDate] [date] NULL,
	[SewQty] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_SubprocessWIP] PRIMARY KEY CLUSTERED 
(
	[Bundleno] ASC,
	[RFIDProcessLocationID] ASC,
	[Sp] ASC,
	[Pattern] ASC,
	[SubprocessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Bundleno]  DEFAULT ('') FOR [Bundleno]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_RFIDProcessLocationID]  DEFAULT ('') FOR [RFIDProcessLocationID]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_EXCESS]  DEFAULT ('') FOR [EXCESS]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_FabricKind]  DEFAULT ('') FOR [FabricKind]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_CutRef]  DEFAULT ('') FOR [CutRef]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Sp]  DEFAULT ('') FOR [Sp]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_MasterSP]  DEFAULT ('') FOR [MasterSP]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_M]  DEFAULT ('') FOR [M]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Factory]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Program]  DEFAULT ('') FOR [Program]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Brand]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Comb]  DEFAULT ('') FOR [Comb]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_CutNo]  DEFAULT ((0)) FOR [CutNo]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_FabPanelCode]  DEFAULT ('') FOR [FabPanelCode]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_ScheduledLineID]  DEFAULT ('') FOR [ScheduledLineID]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_ScannedLineID]  DEFAULT ('') FOR [ScannedLineID]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Cell]  DEFAULT ('') FOR [Cell]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Pattern]  DEFAULT ('') FOR [Pattern]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_PtnDesc]  DEFAULT ('') FOR [PtnDesc]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Group]  DEFAULT ((0)) FOR [Group]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Size]  DEFAULT ('') FOR [Size]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Artwork]  DEFAULT ('') FOR [Artwork]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Qty]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_SubprocessID]  DEFAULT ('') FOR [SubprocessID]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_PostSewingSubProcess]  DEFAULT ('') FOR [PostSewingSubProcess]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_NoBundleCardAfterSubprocess]  DEFAULT ('') FOR [NoBundleCardAfterSubprocess]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Location]  DEFAULT ('') FOR [Location]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_POSupplier]  DEFAULT ('') FOR [POSupplier]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_AllocatedSubcon]  DEFAULT ('') FOR [AllocatedSubcon]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_AvgTime]  DEFAULT ((0)) FOR [AvgTime]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_TimeRange]  DEFAULT ('') FOR [TimeRange]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_Item]  DEFAULT ('') FOR [Item]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_PanelNo]  DEFAULT ('') FOR [PanelNo]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_CutCellID]  DEFAULT ('') FOR [CutCellID]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_SpreadingNo]  DEFAULT ('') FOR [SpreadingNo]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_SewQty]  DEFAULT ((0)) FOR [SewQty]
GO

ALTER TABLE [dbo].[P_SubprocessWIP] ADD  CONSTRAINT [DF_P_SubprocessWIP_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessWIP', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessWIP', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO