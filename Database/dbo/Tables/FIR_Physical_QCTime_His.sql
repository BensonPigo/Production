CREATE TABLE [dbo].[FIR_Physical_QCTime_His] (
    [Ukey] BIGINT NOT NULL,
    [InspSeq] INT NOT NULL,
    [FIR_PhysicalDetailUKey] BIGINT NOT NULL,
    [MachineIoTUkey] BIGINT NOT NULL,
    [IsFirstInspection] BIT NOT NULL CONSTRAINT [DF_FIR_Physical_QCTime_His_IsFirstInspection] DEFAULT (0),
    [StartTime] DATETIME NULL,
    [EndTime] DATETIME NULL,
    [StartYards] NUMERIC(11,2) NOT NULL CONSTRAINT [DF_FIR_Physical_QCTime_His_StartYards] DEFAULT (0),
    [EndYards] NUMERIC(11,2) NOT NULL CONSTRAINT [DF_FIR_Physical_QCTime_His_EndYards] DEFAULT (0),
    CONSTRAINT [PK_FIR_Physical_QCTime_His] PRIMARY KEY CLUSTERED ([Ukey], [InspSeq])
);
GO

-- Extended Properties (欄位描述)
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'NULL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'Ukey';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗流水號 By FIRID, Roll, Dyelot', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'InspSeq';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'至FIR_Physical可以知道是哪條布', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'FIR_PhysicalDetailUKey';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'IoT機台Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'MachineIoTUkey';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否為該卷第一次檢驗', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'IsFirstInspection';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'驗布機開始運作時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'StartTime';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'驗布機停止時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'EndTime';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'驗布機開始時的碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'StartYards';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'驗布機結束時的碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_QCTime_His', @level2type = N'COLUMN', @level2name = N'EndYards';
GO

CREATE NONCLUSTERED INDEX [IDX_FIR_Physical_QCTime_His_FIR_PhysicalDetailUkey] ON [dbo].[FIR_Physical_QCTime_His]
(
	[FIR_PhysicalDetailUkey], [InspSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO