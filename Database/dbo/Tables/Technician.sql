CREATE TABLE [dbo].[Technician] (
    [ID]                 VARCHAR (10)    NOT NULL,
    [SignaturePic]       NVARCHAR (60)   CONSTRAINT [DF_Technician_SignaturePic] DEFAULT ('') NULL,
    [SampleGarmentWash]  BIT             CONSTRAINT [DF_Technician_SampleGarmentWash] DEFAULT ((0)) NOT NULL,
    [MockupCrocking]     BIT             CONSTRAINT [DF_Technician_MockupCrocking] DEFAULT ((0)) NOT NULL,
    [MockupOven]         BIT             CONSTRAINT [DF_Technician_MockupOven] DEFAULT ((0)) NOT NULL,
    [MockupWash]         BIT             CONSTRAINT [DF_Technician_MockupWash] DEFAULT ((0)) NOT NULL,
    [GarmentTest]        BIT             CONSTRAINT [DF_Technicia_GarmentTest] DEFAULT ((0)) NOT NULL,
    [BulkMockupCrocking] BIT             CONSTRAINT [DF_Technician_BulkMockupCrocking] DEFAULT ((0)) NOT NULL,
    [BulkMockupOven]     BIT             CONSTRAINT [DF_Technician_BulkMockupOven] DEFAULT ((0)) NOT NULL,
    [BulkMockWash]       BIT             CONSTRAINT [DF_Technician_BulkMockWash] DEFAULT ((0)) NOT NULL,
    [Signature]          VARBINARY (MAX) NULL,
    [Junk]               BIT             CONSTRAINT [DF_Technician_Junk] DEFAULT ((0)) NULL,
    [EditDate]           DATETIME        NULL,
    [EditName]           VARCHAR (10)    CONSTRAINT [DF_Technician_EditName] DEFAULT ('') NULL,
    [AddDate]            DATETIME        NULL,
    [AddName]            VARCHAR (10)    CONSTRAINT [DF_Technician_AddName] DEFAULT ('') NULL,
    CONSTRAINT [PK_Technici_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'簽名圖檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'Signature';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bulk Stage - Mockup Wash Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'BulkMockWash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bulk Stage - Mockup Oven Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'BulkMockupOven';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bulk Stage - Mockup Crocking Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'BulkMockupCrocking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bulk Stage - Garment Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'GarmentTest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample Stage - Mockup Wash Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'MockupWash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample Stage - Mockup Oven Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'MockupOven';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample Stage - Mockup Crocking Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'MockupCrocking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample Stage - Garment Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician', @level2type = N'COLUMN', @level2name = N'SampleGarmentWash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Technician', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician';

