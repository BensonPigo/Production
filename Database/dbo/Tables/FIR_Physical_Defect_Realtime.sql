CREATE TABLE [dbo].[FIR_Physical_Defect_Realtime] (
    [FIR_PhysicalDetailUkey] BIGINT         CONSTRAINT [DF_FIR_Physical_Defect_Realtime_FIR_PhysicalDetailUkey] DEFAULT ((0)) NULL,
    [Yards]                  NUMERIC (9, 3) CONSTRAINT [DF_FIR_Physical_Defect_Realtime_Yards] DEFAULT ((0)) NULL,
    [FabricdefectID]         VARCHAR (2)    CONSTRAINT [DF_FIR_Physical_Defect_Realtime_FabricdefectID] DEFAULT ('') NULL,
    [AddDate]                DATETIME       NULL,
    [Id]                     BIGINT         IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_FIR_Physical_Defect_Realtime] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DetailUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime', @level2type = N'COLUMN', @level2name = N'FIR_PhysicalDetailUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime', @level2type = N'COLUMN', @level2name = N'Yards';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime', @level2type = N'COLUMN', @level2name = N'FabricdefectID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_Realtime', @level2type = N'COLUMN', @level2name = N'Id';

