CREATE TABLE [dbo].[QABrandSetting] (
    [BrandID]                   VARCHAR (8) NOT NULL,
    [PullingTest_PullForceUnit] VARCHAR (6) NOT NULL,
    CONSTRAINT [PK_QABrandSetting] PRIMARY KEY CLUSTERED ([BrandID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉力單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QABrandSetting', @level2type = N'COLUMN', @level2name = N'PullingTest_PullForceUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QABrandSetting', @level2type = N'COLUMN', @level2name = N'BrandID';

