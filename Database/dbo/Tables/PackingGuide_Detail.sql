CREATE TABLE [dbo].[PackingGuide_Detail] (
    [Id]        VARCHAR (13)   CONSTRAINT [DF_PackingGuide_Detail_Id] DEFAULT ('') NOT NULL,
    [RefNo]     VARCHAR (21)   CONSTRAINT [DF_PackingGuide_Detail_RefNo] DEFAULT ('') NOT NULL,
    [Article]   VARCHAR (8)    CONSTRAINT [DF_PackingGuide_Detail_Article] DEFAULT ('') NOT NULL,
    [Color]     VARCHAR (6)    CONSTRAINT [DF_PackingGuide_Detail_Color] DEFAULT ('') NULL,
    [SizeCode]  VARCHAR (8)    CONSTRAINT [DF_PackingGuide_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [QtyPerCTN] SMALLINT       CONSTRAINT [DF_PackingGuide_Detail_QtyPerCTN] DEFAULT ((0)) NOT NULL,
    [ShipQty]   INT            CONSTRAINT [DF_PackingGuide_Detail_ShipQty] DEFAULT ((0)) NOT NULL,
    [NW]        NUMERIC (7, 3) CONSTRAINT [DF_PackingGuide_Detail_NW] DEFAULT ((0)) NULL,
    [NNW]       NUMERIC (7, 3) CONSTRAINT [DF_PackingGuide_Detail_NNW] DEFAULT ((0)) NULL,
    [GW]        NUMERIC (7, 3) CONSTRAINT [DF_PackingGuide_Detail_GW] DEFAULT ((0)) NULL,
    [RefNoForBalance] VARCHAR(21) NOT NULL DEFAULT (''), 
    [CombineBalance] BIT CONSTRAINT [DF_PackingGuide_Detail_CombineBalance] DEFAULT ((0)) Not NULL,
	PrePackQty int NOT NULL CONSTRAINT [DF_PackingGuide_Detail _PrePackQty]  DEFAULT(0), 
    CONSTRAINT [PK_PackingGuide_Detail] PRIMARY KEY CLUSTERED ([Id] ASC, [Article] ASC, [SizeCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing Guide Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料(箱子)編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'Color';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每箱數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'QtyPerCTN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NW', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'NW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NNW', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'NNW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GW', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide_Detail', @level2type = N'COLUMN', @level2name = N'GW';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'尾箱使用紙箱料號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingGuide_Detail',
    @level2type = N'COLUMN',
    @level2name = N'RefNoForBalance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單色單碼裝箱，尾箱數量過少時是否可以合併在同一 SKU 的最後一箱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingGuide_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CombineBalance'


EXEC sp_addextendedproperty @name = N'MS_Description',
	@value = N'混尺碼裝箱各色組尺寸 1 個塑膠袋裝入的件數',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'PackingGuide_Detail',
	@level2type = N'COLUMN',
	@level2name = N'PrePackQty'
GO