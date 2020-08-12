CREATE TABLE [dbo].[SampleGarmentTest_Detail]
(
	[ID] BIGINT NOT NULL , 
    [No] INT NOT NULL, 
    [Result] VARCHAR(5) CONSTRAINT [DF_SampleGarmentTest_Detail_Result] DEFAULT ('') NULL, 
    [InspDate] DATE NULL, 
    [Technician] VARCHAR(10) CONSTRAINT [DF_SampleGarmentTest_Detail_Technician] DEFAULT ('') NULL,
    [Remark] NVARCHAR(MAX) CONSTRAINT [DF_SampleGarmentTest_Detail_Remark] DEFAULT ('') NULL,
    [Sender] VARCHAR(10) CONSTRAINT [DF_SampleGarmentTest_Detail_Sender] DEFAULT ('') NULL, 
    [SendDate] DATETIME NULL, 
    [Receiver] VARCHAR(10) CONSTRAINT [DF_SampleGarmentTest_Detail_Receiver] DEFAULT ('') NULL, 
    [ReceivedDate] DATETIME NULL, 
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_SampleGarmentTest_Detail_AddName] DEFAULT ('') NULL, 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SampleGarmentTest_Detail_EditName] DEFAULT ('') NULL,
	[ReportDate] DATE NULL, 
    [Colour] VARCHAR(100) NULL, 
    [SizeCode] VARCHAR(8) NULL, 
    [LineDry] BIT NOT NULL DEFAULT(0), 
    [Temperature] INT NOT NULL DEFAULT (0), 
    [TumbleDry] BIT NOT NULL DEFAULT (0), 
    [Machine] VARCHAR(10) NULL, 
    [HandWash] BIT NOT NULL, 
    [Composition] VARCHAR(50) NULL, 
    [Neck] BIT NOT NULL DEFAULT (1), 
    [Status] VARCHAR(15) NULL, 
    [ReportNo] VARCHAR(13) NOT NULL DEFAULT '', 
    [LOtoFactory] VARCHAR(100) NOT NULL  CONSTRAINT DF_SampleGarmentTest_Detail_LOtoFactory DEFAULT('') , 
    [FGWTMtlTypeID] VARCHAR(20) NOT NULL  CONSTRAINT DF_SampleGarmentTest_Detail_FGWTMtlTypeID DEFAULT('') , 
    CONSTRAINT [PK_SampleGarmentTest_Detail] PRIMARY KEY CLUSTERED ([ID],[No] ASC)
)

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample Order Garment Wash', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'No';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Result', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'Result';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'InspDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'InspDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Technician', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'Technician';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'Remark';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sender', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'Sender';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SendDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'SendDate';
