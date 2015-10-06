CREATE TABLE [dbo].[Style_MarkerList_SizeQty] (
    [Style_MarkerListUkey] BIGINT      CONSTRAINT [DF_Style_MarkerList_SizeQty_Style_MarkerListUkey] DEFAULT ((0)) NOT NULL,
    [StyleUkey]            BIGINT      CONSTRAINT [DF_Style_MarkerList_SizeQty_StyleUkey] DEFAULT ((0)) NULL,
    [SizeCode]             VARCHAR (8) CONSTRAINT [DF_Style_MarkerList_SizeQty_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                  SMALLINT    CONSTRAINT [DF_Style_MarkerList_SizeQty_Qty] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Style_MarkerList_SizeQty] PRIMARY KEY CLUSTERED ([Style_MarkerListUkey] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式基本檔-馬克檔-Size & Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_SizeQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Marker的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_SizeQty', @level2type = N'COLUMN', @level2name = N'Style_MarkerListUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_SizeQty', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_SizeQty', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_SizeQty', @level2type = N'COLUMN', @level2name = N'Qty';

