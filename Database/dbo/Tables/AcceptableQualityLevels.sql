﻿CREATE TABLE [dbo].[AcceptableQualityLevels] (
    [InspectionLevels] VARCHAR (2) CONSTRAINT [DF_AcceptableQualityLevels_InspectionLevels] DEFAULT ((0)) NOT NULL,
    [LotSize_Start]    INT         CONSTRAINT [DF_AcceptableQualityLevels_LotSize_Start] DEFAULT ((0)) NOT NULL,
    [LotSize_End]      INT         CONSTRAINT [DF_AcceptableQualityLevels_LotSize_End] DEFAULT ((0)) NOT NULL,
    [SampleSize]       INT         CONSTRAINT [DF_AcceptableQualityLevels_SampleSize] DEFAULT ((0)) NOT NULL,
    [Ukey]             BIGINT      IDENTITY (1, 1) NOT NULL,
    [Junk]             BIT         CONSTRAINT [DF_AcceptableQualityLevels_Junk] DEFAULT ((0)) NOT NULL,
	[AQLType]          numeric(2,1)         CONSTRAINT [DF_AcceptableQualityLevels_AQLType] DEFAULT ((0)) NOT NULL,
	[AcceptedQty]      int         CONSTRAINT [DF_AcceptableQualityLevels_AcceptedQty] DEFAULT ((0)) NULL,
    BrandID varchar(8) not null CONSTRAINT [DF_AcceptableQualityLevels_BrandID] default '',
    Category varchar(15) not null CONSTRAINT [DF_AcceptableQualityLevels_Category] default ''
    CONSTRAINT [PK_AcceptableQualityLevels] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'抽樣數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'SampleSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產品總數 - 判斷範圍結束值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'LotSize_End';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AQL類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'AQLType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'可容忍檢驗失敗數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'AcceptedQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產品總數 - 判斷範圍起始值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'LotSize_Start';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'抽樣計劃', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels', @level2type = N'COLUMN', @level2name = N'InspectionLevels';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'正常檢驗單次抽樣質量允收標準 (AQL)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AcceptableQualityLevels';

