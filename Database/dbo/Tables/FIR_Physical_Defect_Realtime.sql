CREATE TABLE [dbo].[FIR_Physical_Defect_Realtime](
	[FIR_PhysicalDetailUkey] [bigint] NULL,
	[Yards] [numeric](9, 3) NULL,
	[FabricdefectID] [varchar](2) NULL,
	[AddDate] [datetime] NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[T2] [bit] NOT NULL,
	[MachineIoTUkey] [bigint] NULL,
 CONSTRAINT [PK_FIR_Physical_Defect_Realtime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IDX_FIR_Physical_Defect_Realtime_FIR_PhysicalDetailUkey] ON [dbo].[FIR_Physical_Defect_Realtime]
(
	[FIR_PhysicalDetailUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



ALTER TABLE [dbo].[FIR_Physical_Defect_Realtime] ADD  CONSTRAINT [DF_FIR_Physical_Defect_Realtime_FIR_PhysicalDetailUkey]  DEFAULT ((0)) FOR [FIR_PhysicalDetailUkey]
GO

ALTER TABLE [dbo].[FIR_Physical_Defect_Realtime] ADD  CONSTRAINT [DF_FIR_Physical_Defect_Realtime_Yards]  DEFAULT ((0)) FOR [Yards]
GO

ALTER TABLE [dbo].[FIR_Physical_Defect_Realtime] ADD  CONSTRAINT [DF_FIR_Physical_Defect_Realtime_FabricdefectID]  DEFAULT ('') FOR [FabricdefectID]
GO

ALTER TABLE [dbo].[FIR_Physical_Defect_Realtime] ADD  DEFAULT ((0)) FOR [T2]
GO

ALTER TABLE [dbo].[FIR_Physical_Defect_Realtime] ADD  CONSTRAINT [DF_FIR_Physical_Defect_Realtime_MachineIoTUkey]  DEFAULT ((0)) FOR [MachineIoTUkey]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DetailUkey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_Defect_Realtime', @level2type=N'COLUMN',@level2name=N'FIR_PhysicalDetailUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵碼數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_Defect_Realtime', @level2type=N'COLUMN',@level2name=N'Yards'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_Defect_Realtime', @level2type=N'COLUMN',@level2name=N'FabricdefectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_Defect_Realtime', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_Defect_Realtime', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'T2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_Defect_Realtime', @level2type=N'COLUMN',@level2name=N'T2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IoT 機台 Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_Defect_Realtime', @level2type=N'COLUMN',@level2name=N'MachineIoTUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_Defect_Realtime'
GO