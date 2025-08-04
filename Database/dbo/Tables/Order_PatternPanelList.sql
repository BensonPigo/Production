CREATE TABLE [dbo].[Order_PatternPanelList]
(
	[CuttingSP] VARCHAR(13) CONSTRAINT [DF_Order_PatternPanelList_CuttingSP] DEFAULT ('') NOT NULL,
    [Article] VARCHAR(8) CONSTRAINT [DF_Order_PatternPanelList_Article] DEFAULT ('') NOT NULL,
    [SizeCode] VARCHAR(8) CONSTRAINT [DF_Order_PatternPanelList_SizeCode] DEFAULT ('') NOT NULL,
    [FabricPanelCode] VARCHAR(2) CONSTRAINT [DF_Order_PatternPanelList_FabricPanelCode] DEFAULT ('') NOT NULL,
    [PatternPanel] VARCHAR(2) CONSTRAINT [DF_Order_PatternPanelList_PatternPanel] DEFAULT ('') NOT NULL,
    [PatternUkey] BIGINT CONSTRAINT [DF_Order_PatternPanelList_PatternUkey] DEFAULT (0) NOT NULL,
    [ArticleGroup] VARCHAR(6) CONSTRAINT [DF_Order_PatternPanelList_ArticleGroup] DEFAULT ('') NOT NULL,
    [Ukey] BIGINT NOT NULL IDENTITY,
    CONSTRAINT [PK_Order_PatternPanelList] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裁剪母單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_PatternPanelList',
    @level2type = N'COLUMN',
    @level2name = N'CuttingSP'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_PatternPanelList',
    @level2type = N'COLUMN',
    @level2name = N'Article'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'尺寸',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_PatternPanelList',
    @level2type = N'COLUMN',
    @level2name = N'SizeCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'布種代號 + 裁片組合產生出的唯一值',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_PatternPanelList',
    @level2type = N'COLUMN',
    @level2name = N'FabricPanelCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裁片組合',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_PatternPanelList',
    @level2type = N'COLUMN',
    @level2name = N'PatternPanel'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'對應的 Pattern 版本',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_PatternPanelList',
    @level2type = N'COLUMN',
    @level2name = N'PatternUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色組對應的 F_Code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_PatternPanelList',
    @level2type = N'COLUMN',
    @level2name = N'ArticleGroup'
go

--index 成套計算
CREATE NONCLUSTERED INDEX [IX_Order_PatternPanelList_CuttingSPArticleSizeCode] ON [dbo].[Order_PatternPanelList]
(
    [CuttingSP] ASC,
    [Article] ASC,
    [SizeCode] ASC
)

go

--index BundleCard
CREATE NONCLUSTERED INDEX [IX_Order_PatternPanelList_CuttingSPArticleSizeCodeFabricPanelCodePatternPanel] ON [dbo].[Order_PatternPanelList]
(
    [CuttingSP] ASC,
    [Article] ASC,
    [SizeCode] ASC,
    [FabricPanelCode] ASC,
    [PatternPanel] ASC
)