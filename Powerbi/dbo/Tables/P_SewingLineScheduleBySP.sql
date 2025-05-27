CREATE TABLE [dbo].[P_SewingLineScheduleBySP](
	[ID] [bigint] NOT NULL,
	[SewingLineID] [varchar](5) NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SPNo] [varchar](13) NOT NULL,
	[CustPONo] [varchar](30) NOT NULL,
	[Category] [varchar](1) NOT NULL,
	[ComboType] [varchar](1) NOT NULL,
	[SwitchToWorkorder] [varchar](12) NOT NULL,
	[Colorway] [nvarchar](max) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[CDCodeNew] [varchar](5) NOT NULL,
	[ProductType] [nvarchar](100) NOT NULL,
	[MatchFabric] [varchar](1) NOT NULL,
	[FabricType] [nvarchar](100) NOT NULL,
	[Lining] [varchar](20) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Construction] [nvarchar](100) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[OrderQty] [int] NOT NULL,
	[AlloQty] [int] NOT NULL,
	[CutQty] [numeric](11, 2) NOT NULL,
	[SewingQty] [int] NOT NULL,
	[ClogQty] [int] NOT NULL,
	[FirstCuttingOutputDate] [date] NULL,
	[InspectionDate] [nvarchar](100) NOT NULL,
	[TotalStandardOutput] [numeric](11, 6) NOT NULL,
	[WorkHour] [numeric](8, 6) NOT NULL,
	[StandardOutputPerHour] [int] NOT NULL,
	[Efficiency] [numeric](10, 2) NOT NULL,
	[KPILETA] [date] NULL,
	[PFRemark] [nvarchar](max) NOT NULL,
	[ActMTLETA] [date] NULL,
	[MTLExport] [varchar](2) NOT NULL,
	[CutInLine] [date] NULL,
	[Inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[SCIDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[CRDDate] [date] NULL,
	[CPU] [numeric](8, 3) NOT NULL,
	[SewingCPU] [numeric](12, 5) NOT NULL,
	[VASSHAS] [varchar](1) NOT NULL,
	[ShipModeList] [varchar](30) NOT NULL,
	[Destination] [varchar](30) NOT NULL,
	[Artwork] [nvarchar](max) NOT NULL,
	[Remarks] [nvarchar](max) NOT NULL,
	[TTL_PRINTING_PCS] [numeric](38, 6) NOT NULL,
	[TTL_PRINTING_PPU_PPU] [numeric](38, 6) NOT NULL,
	[SubCon] [nvarchar](20) NOT NULL,
	[SewETA] [date] NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
	[TransferDate] [datetime] NULL,
 CONSTRAINT [PK_P_SewingLineScheduleBySP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SPNo]  DEFAULT ('') FOR [SPNo]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_CustPONo]  DEFAULT ('') FOR [CustPONo]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SwitchToWorkorder]  DEFAULT ('') FOR [SwitchToWorkorder]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Colorway]  DEFAULT ('') FOR [Colorway]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_CDCodeNew]  DEFAULT ('') FOR [CDCodeNew]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_ProductType]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_MatchFabric]  DEFAULT ('') FOR [MatchFabric]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Lining]  DEFAULT ('') FOR [Lining]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Gender]  DEFAULT ('') FOR [Gender]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Construction]  DEFAULT ('') FOR [Construction]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_AlloQty]  DEFAULT ((0)) FOR [AlloQty]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_CutQty]  DEFAULT ((0)) FOR [CutQty]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SewingQty]  DEFAULT ((0)) FOR [SewingQty]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_ClogQty]  DEFAULT ((0)) FOR [ClogQty]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_InspectionDate]  DEFAULT ('') FOR [InspectionDate]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_TotalStandardOutput]  DEFAULT ((0)) FOR [TotalStandardOutput]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_WorkHour]  DEFAULT ((0)) FOR [WorkHour]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_StandardOutputPerHour]  DEFAULT ((0)) FOR [StandardOutputPerHour]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Efficiency]  DEFAULT ((0)) FOR [Efficiency]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_PFRemark]  DEFAULT ('') FOR [PFRemark]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_MTLExport]  DEFAULT ('') FOR [MTLExport]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_CPU]  DEFAULT ((0)) FOR [CPU]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SewingCPU]  DEFAULT ((0)) FOR [SewingCPU]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_VASSHAS]  DEFAULT ('') FOR [VASSHAS]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_ShipModeList]  DEFAULT ('') FOR [ShipModeList]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Destination]  DEFAULT ('') FOR [Destination]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Artwork]  DEFAULT ('') FOR [Artwork]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_Remarks]  DEFAULT ('') FOR [Remarks]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_TTL_PRINTING_PCS]  DEFAULT ((0)) FOR [TTL_PRINTING_PCS]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_TTL_PRINTING_PPU_PPU]  DEFAULT ((0)) FOR [TTL_PRINTING_PPU_PPU]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [CONSTRAINT_P_SewingLineScheduleBySP_SubCon]  DEFAULT ('') FOR [SubCon]
GO

ALTER TABLE [dbo].[P_SewingLineScheduleBySP] ADD  CONSTRAINT [DF_P_SewingLineScheduleBySP_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineScheduleBySP', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineScheduleBySP', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料轉入時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineScheduleBySP', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO