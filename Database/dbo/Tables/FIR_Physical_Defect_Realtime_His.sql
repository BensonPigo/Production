CREATE TABLE [dbo].[FIR_Physical_Defect_Realtime_His] (
    [Id] BIGINT NOT NULL,
    [InspSeq] INT NOT NULL,
    [FIR_PhysicalDetailUKey] BIGINT NULL CONSTRAINT [DF_FIR_Physical_Defect_Realtime_His_FIR_PhysicalDetailUKey] DEFAULT (0),
    [Yards] NUMERIC(9,3) NULL CONSTRAINT [DF_FIR_Physical_Defect_Realtime_His_Yards] DEFAULT (0),
    [FabricdefectID] VARCHAR(2) NULL CONSTRAINT [DF_FIR_Physical_Defect_Realtime_His_FabricdefectID] DEFAULT (''),
    [AddDate] DATETIME NULL,
    [T2] BIT NOT NULL CONSTRAINT [DF_FIR_Physical_Defect_Realtime_His_T2] DEFAULT (0),
    [MachineIoTUkey] BIGINT NULL CONSTRAINT [DF_FIR_Physical_Defect_Realtime_His_MachineIoTUkey] DEFAULT (0),
    CONSTRAINT [PK_FIR_Physical_Defect_Realtime_His] PRIMARY KEY CLUSTERED ([Id], [InspSeq])
);
GO

-- Extended Properties (欄位描述)
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'Identity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime_His', @level2type = N'COLUMN', @level2name = N'Id';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗流水號 By FIRID, Roll, Dyelot', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime_His', @level2type = N'COLUMN', @level2name = N'InspSeq';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'DetailUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime_His', @level2type = N'COLUMN', @level2name = N'FIR_PhysicalDetailUKey';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime_His', @level2type = N'COLUMN', @level2name = N'Yards';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime_His', @level2type = N'COLUMN', @level2name = N'FabricdefectID';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime_His', @level2type = N'COLUMN', @level2name = N'AddDate';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'T2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime_His', @level2type = N'COLUMN', @level2name = N'T2';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'IoT機台Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime_His', @level2type = N'COLUMN', @level2name = N'MachineIoTUkey';
GO

CREATE NONCLUSTERED INDEX [IDX_FIR_Physical_Defect_Realtime_His_FIR_PhysicalDetailUkey] ON [dbo].[FIR_Physical_Defect_Realtime_His]
(
	[FIR_PhysicalDetailUkey], [InspSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO