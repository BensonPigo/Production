CREATE TABLE [dbo].[Brand_PullingTestStandarList] (
    [BrandID]       VARCHAR (8)    NOT NULL,
    [TestItem]      VARCHAR (20)   NOT NULL,
    [PullForceUnit] VARCHAR (6)    NOT NULL,
    [PullForce]     NUMERIC (6, 2) NOT NULL,
    [Time]          INT            NOT NULL,
    CONSTRAINT [PK_Brand_PullingTestStandarList] PRIMARY KEY CLUSTERED ([BrandID] ASC, [TestItem] ASC, [PullForceUnit] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'持續時間 (秒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_PullingTestStandarList', @level2type = N'COLUMN', @level2name = N'Time';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉力', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_PullingTestStandarList', @level2type = N'COLUMN', @level2name = N'PullForce';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉力單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_PullingTestStandarList', @level2type = N'COLUMN', @level2name = N'PullForceUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試項目 
e.g. Snaps, Buttons, Rivet, ….', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_PullingTestStandarList', @level2type = N'COLUMN', @level2name = N'TestItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_PullingTestStandarList', @level2type = N'COLUMN', @level2name = N'BrandID';

