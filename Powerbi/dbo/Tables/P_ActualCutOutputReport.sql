CREATE TABLE [dbo].[P_ActualCutOutputReport](
	[FactoryID] [varchar](8) NOT NULL,
	[EstCutDate] [date] NULL,
	[ActCutDate] [date] NULL,
	[CutCellid] [varchar](2) NOT NULL,
	[SpreadingNoID] [varchar](5) NOT NULL,
	[CutplanID] [varchar](13) NOT NULL,
	[CutRef] [varchar](10) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[SubSP] [nvarchar](max) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[Size] [nvarchar](100) NOT NULL,
	[noEXCESSqty] [numeric](10, 0) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[WeaveTypeID] [varchar](20) NOT NULL,
	[FabricCombo] [varchar](2) NOT NULL,
	[MarkerLength] [numeric](10, 4) NOT NULL,
	[PerimeterYd] [nvarchar](10) NOT NULL,
	[Layer] [numeric](5, 0) NOT NULL,
	[SizeCode] [nvarchar](max) NOT NULL,
	[Cons] [numeric](12, 4) NOT NULL,
	[EXCESSqty] [numeric](10, 0) NOT NULL,
	[NoofRoll] [int] NOT NULL,
	[DyeLot] [int] NOT NULL,
	[NoofWindow] [numeric](9, 4) NOT NULL,
	[CuttingSpeed] [numeric](5, 3) NOT NULL,
	[PreparationTime] [numeric](8, 3) NOT NULL,
	[ChangeoverTime] [numeric](8, 3) NOT NULL,
	[SpreadingSetupTime] [numeric](8, 3) NOT NULL,
	[MachSpreadingTime] [numeric](8, 3) NOT NULL,
	[SeparatorTime] [numeric](8, 3) NOT NULL,
	[ForwardTime] [numeric](8, 3) NOT NULL,
	[CuttingSetupTime] [numeric](8, 3) NOT NULL,
	[MachCuttingTime] [numeric](20, 4) NOT NULL,
	[WindowTime] [numeric](8, 3) NOT NULL,
	[TotalSpreadingTime] [numeric](8, 3) NOT NULL,
	[TotalCuttingTime] [numeric](8, 3) NOT NULL,
	[UKey] [bigint] IDENTITY(1,1) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_ActualCutOutputReport] PRIMARY KEY CLUSTERED 
(
	[UKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_CutCellid]  DEFAULT ('') FOR [CutCellid]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_SpreadingNoID]  DEFAULT ('') FOR [SpreadingNoID]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_CutplanID]  DEFAULT ('') FOR [CutplanID]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_CutRef]  DEFAULT ('') FOR [CutRef]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_SP]  DEFAULT ('') FOR [SP]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_SubSP]  DEFAULT ('') FOR [SubSP]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_Size]  DEFAULT ('') FOR [Size]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_noEXCESSqty]  DEFAULT ((0)) FOR [noEXCESSqty]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_WeaveTypeID]  DEFAULT ('') FOR [WeaveTypeID]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_FabricCombo]  DEFAULT ('') FOR [FabricCombo]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_MarkerLength]  DEFAULT ((0)) FOR [MarkerLength]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_PerimeterYd]  DEFAULT ('') FOR [PerimeterYd]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_Layer]  DEFAULT ((0)) FOR [Layer]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_SizeCode]  DEFAULT ('') FOR [SizeCode]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_Cons]  DEFAULT ((0)) FOR [Cons]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_EXCESSqty]  DEFAULT ((0)) FOR [EXCESSqty]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_NoofRoll]  DEFAULT ((0)) FOR [NoofRoll]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_DyeLot]  DEFAULT ((0)) FOR [DyeLot]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_NoofWindow]  DEFAULT ((0)) FOR [NoofWindow]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_CuttingSpeed]  DEFAULT ((0)) FOR [CuttingSpeed]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_PreparationTime]  DEFAULT ((0)) FOR [PreparationTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_ChangeoverTime]  DEFAULT ((0)) FOR [ChangeoverTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_SpreadingSetupTime]  DEFAULT ((0)) FOR [SpreadingSetupTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_MachSpreadingTime]  DEFAULT ((0)) FOR [MachSpreadingTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_SeparatorTime]  DEFAULT ((0)) FOR [SeparatorTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_ForwardTime]  DEFAULT ((0)) FOR [ForwardTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_CuttingSetupTime]  DEFAULT ((0)) FOR [CuttingSetupTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_MachCuttingTime]  DEFAULT ((0)) FOR [MachCuttingTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_WindowTime]  DEFAULT ((0)) FOR [WindowTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_TotalSpreadingTime]  DEFAULT ((0)) FOR [TotalSpreadingTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_TotalCuttingTime]  DEFAULT ((0)) FOR [TotalCuttingTime]
GO

ALTER TABLE [dbo].[P_ActualCutOutputReport] ADD  CONSTRAINT [DF_P_ActualCutOutputReport_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ActualCutOutputReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ActualCutOutputReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO