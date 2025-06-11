CREATE TABLE [dbo].[FIR_Physical_QCTime](
	[FIR_PhysicalDetailUkey] [bigint] NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[MachineIoTUkey] [bigint] NOT NULL,
	[IsFirstInspection] [bit] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[StartYards] [numeric](11, 2) NOT NULL,
	[EndYards] [numeric](11, 2) NOT NULL,
 CONSTRAINT [PK_FIR_Physical_QCTime] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FIR_Physical_QCTime] ADD  CONSTRAINT [DF_FIR_Physical_QCTime_IsFirstInspection]  DEFAULT ((0)) FOR [IsFirstInspection]
GO

ALTER TABLE [dbo].[FIR_Physical_QCTime] ADD  CONSTRAINT [DF_FIR_Physical_QCTime_StartYards]  DEFAULT ((0)) FOR [StartYards]
GO

ALTER TABLE [dbo].[FIR_Physical_QCTime] ADD  CONSTRAINT [DF_FIR_Physical_QCTime_EndYards]  DEFAULT ((0)) FOR [EndYards]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'至 FIR_Physical 可以知道是哪捲布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_QCTime', @level2type=N'COLUMN',@level2name=N'FIR_PhysicalDetailUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IoT 機台 Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_QCTime', @level2type=N'COLUMN',@level2name=N'MachineIoTUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為該卷布第一次檢驗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_QCTime', @level2type=N'COLUMN',@level2name=N'IsFirstInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布機開始運作時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_QCTime', @level2type=N'COLUMN',@level2name=N'StartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布機停止時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_QCTime', @level2type=N'COLUMN',@level2name=N'EndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布機開始時的碼數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_QCTime', @level2type=N'COLUMN',@level2name=N'StartYards'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布機結束時的碼數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_QCTime', @level2type=N'COLUMN',@level2name=N'EndYards'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布機運作時間明細' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_Physical_QCTime'
GO

CREATE NONCLUSTERED INDEX [IDX_FIR_Physical_QCTime_FIR_PhysicalDetailUkey] ON [dbo].[FIR_Physical_QCTime]
(
	[FIR_PhysicalDetailUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO