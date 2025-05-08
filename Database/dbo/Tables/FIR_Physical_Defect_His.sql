CREATE TABLE [dbo].[FIR_Physical_Defect_His] (
    [FIR_PhysicalDetailUKey] BIGINT NOT NULL CONSTRAINT [DF_FIR_Physical_Defect_His_FIR_PhysicalDetailUKey] DEFAULT (0),
    [DefectLocation] VARCHAR(7) NOT NULL CONSTRAINT [DF_FIR_Physical_Defect_His_DefectLocation] DEFAULT (''),
    [ID] BIGINT NOT NULL,
    [InspSeq] INT NOT NULL,
    [DefectRecord] VARCHAR(30) NOT NULL CONSTRAINT [DF_FIR_Physical_Defect_His_DefectRecord] DEFAULT (''),
    [Point] NUMERIC(3,0) NOT NULL CONSTRAINT [DF_FIR_Physical_Defect_His_Point] DEFAULT (0),
    [RealTimeInsert] BIT NULL
    CONSTRAINT [PK_FIR_Physical_Defect_His] PRIMARY KEY CLUSTERED ([FIR_PhysicalDetailUKey], [DefectLocation], [ID], [InspSeq])
);
GO

-- Extended Properties (欄位描述)
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'FIR_PhysicalDetailUKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_His', @level2type = N'COLUMN', @level2name = N'FIR_PhysicalDetailUKey';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_His', @level2type = N'COLUMN', @level2name = N'DefectLocation';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'NULL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_His', @level2type = N'COLUMN', @level2name = N'ID';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗流水號 By FIRID, Roll, Dyelot', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_His', @level2type = N'COLUMN', @level2name = N'InspSeq';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵記錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_His', @level2type = N'COLUMN', @level2name = N'DefectRecord';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'點數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_His', @level2type = N'COLUMN', @level2name = N'Point';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'NULL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_His', @level2type = N'COLUMN', @level2name = N'RealTimeInsert';
GO
