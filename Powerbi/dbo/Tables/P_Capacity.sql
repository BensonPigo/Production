CREATE TABLE [dbo].[P_Capacity](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[ID] [varchar](12) NULL,
	[FTY] [varchar](8) NULL,
	[MDivision] [varchar](8) NULL,
	[Year] [int] NULL,
	[Month] [int] NULL,
	[Key] [varchar](7) NULL,
	[ArtworkType] [varchar](50) NULL,
	[WorkDays] [int] NULL,
	[WorkingHourDaily] [numeric](5, 2) NULL,
	[TotalIndirectManpower] [int] NULL,
	[Noofcells] [numeric](6, 2) NULL,
	[NoofSewerCell] [numeric](6, 2) NULL,
	[NoofSewers] [int] NULL,
	[AbsentRate] [numeric](6, 2) NULL,
	[TotalAvailableSewers] [int] NULL,
	[AverageProductivity] [numeric](5, 2) NULL,
	[FTYCPU] [numeric](10, 3) NULL,
	[SubconCPU] [numeric](10, 3) NULL,
	[TTLCPU] [numeric](10, 3) NULL,
	[RemarkDayOffDate] [nvarchar](max) NULL,
	[MachineAvailableUnits] [int] NULL,
	[TTLPrinter] [int] NULL,
	[AverageAttendance] [numeric](5, 2) NULL,
	[AverageOutputPerHour] [numeric](6, 2) NULL,
	[OvalMachineOutputPerDayPPU] [numeric](6, 2) NULL,
	[AverageStitchesPerHour1000Stiches] [numeric](6, 2) NULL,
	[SubconOut1000StichesMins] [numeric](10, 3) NULL,
	[SubconOutPcs] [numeric](10, 3) NULL,
	[ShiftDayandNight] [int] NULL,
	[MachineCapacity] [numeric](10, 3) NULL,
	[Unit] [varchar](8) NULL,
	[ApprovedDate] [varchar](10) NULL,
	[AverageEfficiency] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_P_Capacity] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_FTY]  DEFAULT ('') FOR [FTY]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_MDivision]  DEFAULT ('') FOR [MDivision]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_Year]  DEFAULT ((0)) FOR [Year]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_Month]  DEFAULT ((0)) FOR [Month]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_Key]  DEFAULT ('') FOR [Key]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_ArtworkType]  DEFAULT ('') FOR [ArtworkType]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_WorkDays]  DEFAULT ((0)) FOR [WorkDays]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_WorkingHourDaily]  DEFAULT ((0)) FOR [WorkingHourDaily]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_TotalIndirectManpower]  DEFAULT ((0)) FOR [TotalIndirectManpower]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_Noofcells]  DEFAULT ((0)) FOR [Noofcells]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_NoofSewerCell]  DEFAULT ((0)) FOR [NoofSewerCell]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_NoofSewers]  DEFAULT ((0)) FOR [NoofSewers]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_AbsentRate]  DEFAULT ((0)) FOR [AbsentRate]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_TotalAvailableSewers]  DEFAULT ((0)) FOR [TotalAvailableSewers]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_AverageProductivity]  DEFAULT ((0)) FOR [AverageProductivity]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_FTYCPU]  DEFAULT ((0)) FOR [FTYCPU]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_SubconCPU]  DEFAULT ((0)) FOR [SubconCPU]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_TTLCPU]  DEFAULT ((0)) FOR [TTLCPU]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_RemarkDayOffDate]  DEFAULT ('') FOR [RemarkDayOffDate]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_MachineAvailableUnits]  DEFAULT ((0)) FOR [MachineAvailableUnits]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_TTLPrinter]  DEFAULT ((0)) FOR [TTLPrinter]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_AverageAttendance]  DEFAULT ((0)) FOR [AverageAttendance]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_AverageOutputPerHour]  DEFAULT ((0)) FOR [AverageOutputPerHour]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_OvalMachineOutputPerDayPPU]  DEFAULT ((0)) FOR [OvalMachineOutputPerDayPPU]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_AverageStitchesPerHour1000Stiches]  DEFAULT ((0)) FOR [AverageStitchesPerHour1000Stiches]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_SubconOut1000StichesMins]  DEFAULT ((0)) FOR [SubconOut1000StichesMins]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_SubconOutPcs]  DEFAULT ((0)) FOR [SubconOutPcs]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_ShiftDayandNight]  DEFAULT ((0)) FOR [ShiftDayandNight]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_MachineCapacity]  DEFAULT ((0)) FOR [MachineCapacity]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_Unit]  DEFAULT ('') FOR [Unit]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_PBIReportData_ApprovedDate]  DEFAULT ('') FOR [ApprovedDate]
GO

ALTER TABLE [dbo].[P_Capacity] ADD  CONSTRAINT [DF_P_Capacity_AverageEfficiency]  DEFAULT ((0)) FOR [AverageEfficiency]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'FTY'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年+月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'Key'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Artwork類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'ArtworkType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總工作日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'WorkDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一天工作幾小時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'WorkingHourDaily'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直接人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'TotalIndirectManpower'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'多少cell' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'Noofcells'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一個cell有幾個車縫工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'NoofSewerCell'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'共多少車縫工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'NoofSewers'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缺席率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'AbsentRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'共多少可用車縫工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'TotalAvailableSewers'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均生產力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'AverageProductivity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'FTYCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'SubconCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總共CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'TTLCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註(當月休假日期)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'RemarkDayOffDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機器可用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'MachineAvailableUnits'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'共多少打印機' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'TTLPrinter'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均出席率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'AverageAttendance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均每小時多少產出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'AverageOutputPerHour'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'OvalMachineOutputPerDayPPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均每小時多少縫線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'AverageStitchesPerHour1000Stiches'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發(1000 stiches) (mins) ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'SubconOut1000StichesMins'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發(pcs)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'SubconOutPcs'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'早、晚班' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'ShiftDayandNight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Machine產能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'MachineCapacity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'Unit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Capacity', @level2type=N'COLUMN',@level2name=N'ApprovedDate'
GO


