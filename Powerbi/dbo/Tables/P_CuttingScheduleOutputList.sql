CREATE TABLE [dbo].[P_CuttingScheduleOutputList](
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Fabrication] [varchar](20) NOT NULL,
	[EstCuttingDate] [date] NULL,
	[ActCuttingDate] [date] NULL,
	[EarliestSewingInline] [date] NULL,
	[POID] [varchar](13) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[FabRef] [varchar](36) NOT NULL,
	[SwitchToWorkorderType] [varchar](11) NOT NULL,
	[CutRef] [varchar](10) NOT NULL,
	[CutNo] [numeric](6, 0) NOT NULL,
	[SpreadingNoID] [varchar](5) NOT NULL,
	[CutCell] [varchar](2) NOT NULL,
	[Combination] [varchar](2) NOT NULL,
	[Layers] [numeric](5, 0) NOT NULL,
	[LayersLevel] [varchar](10) NOT NULL,
	[LackingLayers] [numeric](10, 0) NOT NULL,
	[Ratio] [varchar](max) NOT NULL,
	[Consumption] [numeric](9, 4) NOT NULL,
	[ActConsOutput] [numeric](10, 4) NOT NULL,
	[BalanceCons] [numeric](10, 4) NOT NULL,
	[MarkerName] [varchar](20) NOT NULL,
	[MarkerNo] [varchar](10) NOT NULL,
	[MarkerLength] [varchar](15) NOT NULL,
	[CuttingPerimeter] [nvarchar](15) NOT NULL,
	[StraightLength] [varchar](15) NOT NULL,
	[CurvedLength] [varchar](15) NOT NULL,
	[DelayReason] [nvarchar](100) NOT NULL,
	[Remark] [nvarchar](max) NOT NULL,
	[WorkOrderUkey] [bigint] NOT NULL,
	[BIFactoryID] [varchar](50) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_CuttingScheduleOutputList] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[WorkOrderUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_Fabrication]  DEFAULT ('') FOR [Fabrication]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_FabRef]  DEFAULT ('') FOR [FabRef]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_SwitchToWorkorderType]  DEFAULT ('') FOR [SwitchToWorkorderType]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_CutRef]  DEFAULT ('') FOR [CutRef]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_CutNo]  DEFAULT ((0)) FOR [CutNo]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_SpreadingNoID]  DEFAULT ('') FOR [SpreadingNoID]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_CutCell]  DEFAULT ('') FOR [CutCell]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_Combination]  DEFAULT ('') FOR [Combination]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_Layers]  DEFAULT ((0)) FOR [Layers]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_LayersLevel]  DEFAULT ('') FOR [LayersLevel]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_LackingLayers]  DEFAULT ((0)) FOR [LackingLayers]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_Consumption]  DEFAULT ((0)) FOR [Consumption]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_ActConsOutput]  DEFAULT ((0)) FOR [ActConsOutput]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_BalanceCons]  DEFAULT ((0)) FOR [BalanceCons]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_MarkerName]  DEFAULT ('') FOR [MarkerName]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_MarkerNo]  DEFAULT ('') FOR [MarkerNo]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_MarkerLength]  DEFAULT ('') FOR [MarkerLength]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_CuttingPerimeter]  DEFAULT ('') FOR [CuttingPerimeter]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_StraightLength]  DEFAULT ('') FOR [StraightLength]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_CurvedLength]  DEFAULT ('') FOR [CurvedLength]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_DelayReason]  DEFAULT ('') FOR [DelayReason]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[P_CuttingScheduleOutputList] ADD  DEFAULT ('') FOR [BIFactoryID]
GO