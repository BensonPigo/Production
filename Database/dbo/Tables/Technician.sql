CREATE TABLE [dbo].[Technician]
(
	[ID] VARCHAR(10) NOT NULL PRIMARY KEY, 
    [SignaturePic] NVARCHAR(60) CONSTRAINT [DF_Technician_SignaturePic] DEFAULT ('') NULL,  
    [SampleGarmentWash] BIT CONSTRAINT [DF_Technician_SampleGarmentWash] DEFAULT (0) NULL, 
    [MockupCrocking] BIT CONSTRAINT [DF_Technician_MockupCrocking] DEFAULT (0) NULL, 
    [MockupOven] BIT CONSTRAINT [DF_Technician_MockupOven] DEFAULT (0) NULL, 
    [MockupWash] BIT CONSTRAINT [DF_Technician_MockupWash] DEFAULT (0) NULL, 
    [Junk] BIT CONSTRAINT [DF_Technician_Junk] DEFAULT (0) NULL, 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_Technician_EditName] DEFAULT ('') NULL, 
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_Technician_AddName] DEFAULT ('') NULL
)

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Technician', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Technician';

