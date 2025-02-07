CREATE TABLE [dbo].[P_CMPByDate](
	[FactoryID] [varchar](8) NOT NULL,
	[OutputDate] [date] NOT NULL,
	[GPHCPU] [decimal](9, 3) NOT NULL,
	[SPHCPU] [decimal](9, 3) NOT NULL,
	[VPHCPU] [decimal](9, 3) NOT NULL,
	[GPHManhours] [decimal](9, 3) NOT NULL,
	[SPHManhours] [decimal](9, 3) NOT NULL,
	[VPHManhours] [decimal](9, 3) NOT NULL,
	[GPH] [decimal](6, 2) NOT NULL,
	[SPH] [decimal](6, 2) NOT NULL,
	[VPH] [decimal](6, 2) NOT NULL,
	[ManhoursRatio] [decimal](6, 2) NOT NULL,
	[TotalActiveHeadcount] [decimal](9, 3) NOT NULL,
	[RevenumDeptHeadcount] [decimal](9, 3) NOT NULL,
	[ManpowerRatio] [decimal](6, 2) NOT NULL,
 CONSTRAINT [PK_P_CMPByDate] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[OutputDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_GPHCPU]  DEFAULT ((0)) FOR [GPHCPU]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_SPHCPU]  DEFAULT ((0)) FOR [SPHCPU]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_VPHCPU]  DEFAULT ((0)) FOR [VPHCPU]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_GPHManhours]  DEFAULT ((0)) FOR [GPHManhours]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_SPHManhours]  DEFAULT ((0)) FOR [SPHManhours]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_VPHManhours]  DEFAULT ((0)) FOR [VPHManhours]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_GPH]  DEFAULT ((0)) FOR [GPH]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_SPH]  DEFAULT ((0)) FOR [SPH]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_VPH]  DEFAULT ((0)) FOR [VPH]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_ManhoursRatio]  DEFAULT ((0)) FOR [ManhoursRatio]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_TotalActiveHeadcount]  DEFAULT ((0)) FOR [TotalActiveHeadcount]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_RevenumDeptHeadcount]  DEFAULT ((0)) FOR [RevenumDeptHeadcount]
GO

ALTER TABLE [dbo].[P_CMPByDate] ADD  CONSTRAINT [DF_P_CMPByDate_ManpowerRatio]  DEFAULT ((0)) FOR [ManpowerRatio]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment per hour' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'GPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subprocess per hour ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'SPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value per hour' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'VPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工時比率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'ManhoursRatio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收入部門人頭數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'RevenumDeptHeadcount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人力比率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'ManpowerRatio'
GO