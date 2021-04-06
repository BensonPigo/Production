CREATE TABLE [dbo].[KHCustomsItem_Detail] (
    [KHCustomsItemUkey] BIGINT       NOT NULL,
    [Port]              VARCHAR (20) NOT NULL,
    [HSCode]            VARCHAR (14) NOT NULL,
    CONSTRAINT [PK_KHCustomsItem_Detail] PRIMARY KEY CLUSTERED ([KHCustomsItemUkey], [Port])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際商品統一分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem_Detail', @level2type = N'COLUMN', @level2name = N'HSCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem_Detail', @level2type = N'COLUMN', @level2name = N'Port';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KHCustomsItem Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem_Detail', @level2type = N'COLUMN', @level2name = N'KHCustomsItemUkey';

