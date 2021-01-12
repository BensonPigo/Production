CREATE TABLE [dbo].[FIR_Physical_Defect] (
    [FIR_PhysicalDetailUKey] BIGINT       CONSTRAINT [DF_FIR_Physical_Defect_FIR_PhysicalDetailUKey] DEFAULT ((0)) NOT NULL,
    [DefectLocation]         VARCHAR (7)  CONSTRAINT [DF_FIR_Physical_Defect_DefectLocation] DEFAULT ('') NOT NULL,
    [DefectRecord]           VARCHAR (20) CONSTRAINT [DF_FIR_Physical_Defect_DefectRecord] DEFAULT ('') NULL,
    [Point]                  NUMERIC (3)  CONSTRAINT [DF_FIR_Physical_Defect_Point] DEFAULT ((0)) NULL,
    [ID]                     BIGINT       NOT NULL,
    [RealTimeInsert]         BIT          NULL,
    CONSTRAINT [PK_FIR_Physical_Defect] PRIMARY KEY CLUSTERED ([FIR_PhysicalDetailUKey] ASC, [DefectLocation] ASC, [ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Physical Defect Point Record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FIR_PhysicalDetailUKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect', @level2type = N'COLUMN', @level2name = N'FIR_PhysicalDetailUKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect', @level2type = N'COLUMN', @level2name = N'DefectLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵記錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect', @level2type = N'COLUMN', @level2name = N'DefectRecord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'點數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect', @level2type = N'COLUMN', @level2name = N'Point';

