CREATE TABLE [dbo].[SampleGarmentTest]
(
	[ID] BIGINT NOT NULL  IDENTITY, 
    [BrandID] VARCHAR(8) NOT NULL, 
    [StyleID] VARCHAR(15) NOT NULL, 
    [SeasonID] VARCHAR(10) NOT NULL, 
    [Article] VARCHAR(8) NOT NULL, 
	[Result] VARCHAR(5) CONSTRAINT [DF_SampleGarmentTest_Result] DEFAULT ('') NULL, 
    [ReceivedDate] DATETIME NULL, 
    [ReleasedDate] DATETIME NULL, 
    [Deadline] DATETIME NULL, 
    [InspDate] DATE NULL, 
    [Remark] NVARCHAR(MAX) CONSTRAINT [DF_SampleGarmentTest_Remark] DEFAULT ('') NULL, 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SampleGarmentTest_EditName] DEFAULT ('') NULL,  
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_SampleGarmentTest_AddName] DEFAULT ('') NULL, 
	CONSTRAINT [PK_SampleGarmentTest] PRIMARY KEY CLUSTERED ([BrandID],[StyleID],[SeasonID],[Article] ASC)
)


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample Order Garment Wash', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'ID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BrandID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'BrandID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'StyleID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'StyleID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SeasonID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'SeasonID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'Article';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ReceiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = 'ReceivedDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ReleasedDate ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'ReleasedDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deadline', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'Deadline';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'InspDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'InspDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleGarmentTest', @level2type = N'COLUMN', @level2name = N'Remark';

