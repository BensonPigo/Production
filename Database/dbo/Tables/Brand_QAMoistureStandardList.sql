CREATE TABLE [dbo].[Brand_QAMoistureStandardList] (
    [BrandID]                      VARCHAR (8)    NOT NULL,
    [MaterialCompositionGrp]       VARCHAR (50)   NOT NULL,
    [MaterialCompositionItem]      VARCHAR (100)  NOT NULL,
    [MoistureStandardDesc]         NVARCHAR (10)  CONSTRAINT [DF_QAMoistureStandardList_MoistureStandardDesc] DEFAULT ('') NOT NULL,
    [MoistureStandard1]            NUMERIC (3, 1) CONSTRAINT [DF_QAMoistureStandardList_MoistureStandard1] DEFAULT ((0)) NOT NULL,
    [MoistureStandard1_Comparison] TINYINT        CONSTRAINT [DF_QAMoistureStandardList_MoistureStandard1_Comparison] DEFAULT ((0)) NOT NULL,
    [MoistureStandard2]            NUMERIC (3, 1) CONSTRAINT [DF_QAMoistureStandardList_MoistureStandard2] DEFAULT ((0)) NOT NULL,
    [MoistureStandard2_Comparison] TINYINT        CONSTRAINT [DF_QAMoistureStandardList_MoistureStandard2_Comparison] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_QAMoistureStandardList] PRIMARY KEY CLUSTERED ([BrandID] ASC, [MaterialCompositionGrp] ASC, [MaterialCompositionItem] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測 Pass 標準範圍的終值，判斷符號
（0 = 等於 EqualTo、3 = 小於 LessThan、4 = 小於等於 LessThanOrEqualTo）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList', @level2type = N'COLUMN', @level2name = N'MoistureStandard2_Comparison';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測 Pass 標準範圍的終值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList', @level2type = N'COLUMN', @level2name = N'MoistureStandard2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測 Pass 標準範圍的起始值，判斷符號
（0 = 等於 EqualTo、1 = 大於 GreaterThan、2 = 大於等於 GreaterThanOrEqualTo）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList', @level2type = N'COLUMN', @level2name = N'MoistureStandard1_Comparison';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測 Pass 標準範圍的起始值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList', @level2type = N'COLUMN', @level2name = N'MoistureStandard1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pass 判斷標準 - 此欄位主要是給人看的', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList', @level2type = N'COLUMN', @level2name = N'MoistureStandardDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'該布料的成分說明 - 細項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList', @level2type = N'COLUMN', @level2name = N'MaterialCompositionItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'該布料的成分說明 - 群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList', @level2type = N'COLUMN', @level2name = N'MaterialCompositionGrp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各品牌濕度檢測標準
0 = 等於 EqualTo
1 = 大於 GreaterThan
2 = 大於等於 GreaterThanOrEqualTo
3 = 小於 LessThan
4 = 小於等於 LessThanOrEqualTo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_QAMoistureStandardList';

