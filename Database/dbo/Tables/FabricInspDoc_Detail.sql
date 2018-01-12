CREATE TABLE [dbo].[FabricInspDoc_Detail]
(
	[ID] VARCHAR(13) NOT NULL  DEFAULT (''), 
    [Seq1] VARCHAR(3) NOT NULL DEFAULT (''), 
    [Seq2] VARCHAR(2) NOT NULL DEFAULT (''), 
    [Refno] VARCHAR(20) NULL DEFAULT (''), 
    [ColorID] VARCHAR(6) NULL DEFAULT (''), 
    [SuppID] VARCHAR(6) NULL DEFAULT (''), 
    [TestReport] BIT NULL DEFAULT ((0)), 
    [InspReport] BIT NULL DEFAULT ((0)), 
    [ContinuityCard] BIT NULL DEFAULT ((0)), 
    [BulkDyelot] BIT NULL DEFAULT ((0)), 
    [SeasonID] VARCHAR(10) NULL DEFAULT (''), 
    PRIMARY KEY ([ID], [Seq1], [Seq2])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'採購大項編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物料號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Refno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ColorID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'供應商代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SuppID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否收到檢驗報告',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'TestReport'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否收到測試報告',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InspReport'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否收到色差卡',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ContinuityCard'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否收到染缸卡',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'BulkDyelot'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'季別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SeasonID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物料號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FabricInspDoc_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Refno'