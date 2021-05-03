CREATE TABLE [dbo].[AcceptableQualityLevels] (
    [InspectionLevels] VARCHAR (2) CONSTRAINT [DF_AcceptableQualityLevels_InspectionLevels] DEFAULT ((0)) NOT NULL,
    [LotSize_Start]    INT         CONSTRAINT [DF_AcceptableQualityLevels_LotSize_Start] DEFAULT ((0)) NOT NULL,
    [LotSize_End]      INT         CONSTRAINT [DF_AcceptableQualityLevels_LotSize_End] DEFAULT ((0)) NOT NULL,
    [SampleSize]       INT         CONSTRAINT [DF_AcceptableQualityLevels_SampleSize] DEFAULT ((0)) NOT NULL,
    [Ukey]             BIGINT      IDENTITY (1, 1) NOT NULL,
    [Junk]             BIT         CONSTRAINT [DF_AcceptableQualityLevels_Junk] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AcceptableQualityLevels] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'抽樣數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'SampleSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產品總數 - 判斷範圍結束值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'LotSize_End';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產品總數 - 判斷範圍起始值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'LotSize_Start';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'抽樣計劃', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'InspectionLevels';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'正常檢驗單次抽樣質量允收標準 (AQL)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels';

