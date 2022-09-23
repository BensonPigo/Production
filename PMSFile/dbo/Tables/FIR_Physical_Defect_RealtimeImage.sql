CREATE TABLE [dbo].[FIR_Physical_Defect_RealtimeImage] (
    [FIRPhysicalDefectRealtimeID] BIGINT          CONSTRAINT [DF_FIR_Physical_Defect_RealtimeImage_FIRPhysicalDefectRealtimeID] DEFAULT ((0)) NOT NULL,
    [Seq]                         INT             CONSTRAINT [DF_FIR_Physical_Defect_RealtimeImage_Seq] DEFAULT ((0)) NOT NULL,
    [Description]                 NVARCHAR (60)   CONSTRAINT [DF_FIR_Physical_Defect_RealtimeImage_Description] DEFAULT ('') NULL,
    [Image]                       VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_FIR_Physical_Defect_RealtimeImage] PRIMARY KEY CLUSTERED ([FIRPhysicalDefectRealtimeID] ASC, [Seq] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'照片', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_RealtimeImage', @level2type = N'COLUMN', @level2name = N'Image';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'詳細描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_RealtimeImage', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'第幾個defect照片', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_RealtimeImage', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FinalInspection_DetailImage ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_Defect_RealtimeImage', @level2type = N'COLUMN', @level2name = N'FIRPhysicalDefectRealtimeID';

