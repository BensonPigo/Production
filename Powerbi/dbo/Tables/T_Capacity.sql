CREATE TABLE [dbo].[T_Capacity](
	[FactoryID] [varchar](10) NOT NULL,
	[MDivisionID] [varchar](10) NOT NULL,
	[Year] [varchar](11) NOT NULL,
	[Type] [varchar](15) NOT NULL,
	[Month] [varchar](2) NOT NULL,
	[CPU] [decimal](18, 2) NOT NULL,
	[TtlWorkHour] [decimal](18, 2) NOT NULL,
	[HalfMonthWorkDate1] [decimal](18, 2) NOT NULL,
	[HalfMonthWorkDate2] [decimal](18, 2) NOT NULL,
	[Workdays] [decimal](18, 2) NOT NULL,
	[WorkingHour] [decimal](18, 2) NOT NULL,
	[Cells] [decimal](18, 2) NOT NULL,
	[SewersPerCell] [decimal](18, 2) NOT NULL,
	[AbsentRate] [decimal](18, 2) NOT NULL,
	[TotalSewer] [decimal](18, 2) NOT NULL,
	[ActualSewer] [decimal](18, 2) NOT NULL,
	[Efficiency] [decimal](18, 2) NOT NULL,
	[NetWorkHour] [decimal](18, 2) NOT NULL,
	[HalfMonthNetWorkHour1] [decimal](18, 4) NOT NULL,
	[HalfMonthNetCapacity1] [decimal](18, 4) NOT NULL,
	[HalfMonthNetWorkHour2] [decimal](18, 4) NOT NULL,
	[HalfMonthNetCapacity2] [decimal](18, 4) NOT NULL,
	[SubconCPU] [decimal](18, 2) NOT NULL,
	[TtlManpower] [decimal](18, 2) NOT NULL,
	[TtlInderectManpower] [decimal](18, 2) NOT NULL,
	[Ukey] [bigint] NULL,
 CONSTRAINT [PK_T_Capacity] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[MDivisionID] ASC,
	[Year] ASC,
	[Type] ASC,
	[Month] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_Year]  DEFAULT ('') FOR [Year]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_CPU]  DEFAULT ((0)) FOR [CPU]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_TtlWorkHour]  DEFAULT ((0)) FOR [TtlWorkHour]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_HalfMonthWorkDate1]  DEFAULT ((0)) FOR [HalfMonthWorkDate1]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_HalfMonthWorkDate2]  DEFAULT ((0)) FOR [HalfMonthWorkDate2]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_Workdays]  DEFAULT ((0)) FOR [Workdays]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_WorkingHour]  DEFAULT ((0)) FOR [WorkingHour]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_Cells]  DEFAULT ((0)) FOR [Cells]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_SewersPerCell]  DEFAULT ((0)) FOR [SewersPerCell]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_AbsentRate]  DEFAULT ((0)) FOR [AbsentRate]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_TotalSewer]  DEFAULT ((0)) FOR [TotalSewer]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_ActualSewer]  DEFAULT ((0)) FOR [ActualSewer]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_Efficiency]  DEFAULT ((0)) FOR [Efficiency]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_NetWorkHour]  DEFAULT ((0)) FOR [NetWorkHour]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_HalfMonthNetWorkHour1]  DEFAULT ((0)) FOR [HalfMonthNetWorkHour1]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_HalfMonthNetCapacity1]  DEFAULT ((0)) FOR [HalfMonthNetCapacity1]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_HalfMonthNetWorkHour2]  DEFAULT ((0)) FOR [HalfMonthNetWorkHour2]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_HalfMonthNetCapacity2]  DEFAULT ((0)) FOR [HalfMonthNetCapacity2]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_SubconCPU]  DEFAULT ((0)) FOR [SubconCPU]
GO

ALTER TABLE [dbo].[T_Capacity] ADD  CONSTRAINT [DF_T_Capacity_TtlManpower]  DEFAULT ((0)) FOR [TtlManpower]
GO

