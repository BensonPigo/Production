CREATE TABLE [dbo].[Style_Artwork_Quot] (
    [Ukey]        BIGINT          CONSTRAINT [DF_Style_Artwork_Quot_Ukey] DEFAULT ((0)) NOT NULL,
    [LocalSuppId] VARCHAR (6)     CONSTRAINT [DF_Style_Artwork_Quot_LocalSuppId] DEFAULT ('') NOT NULL,
    [CurrencyId]  VARCHAR (3)     CONSTRAINT [DF_Style_Artwork_Quot_CurrencyId] DEFAULT ('') NOT NULL,
    [Price]       NUMERIC (12, 4) CONSTRAINT [DF_Style_Artwork_Quot_Price] DEFAULT ((0)) NULL,
    [Oven]        DATE            NULL,
    [Wash]        DATE            NULL,
    [Mockup]      DATE            NULL,
    [PriceApv]    VARCHAR (1)     CONSTRAINT [DF_Style_Artwork_Quot_PriceApv] DEFAULT ('') NULL,
    [StyleUkey]   BIGINT          NULL,
    [SizeCode] VARCHAR(8) NOT NULL CONSTRAINT [DF_Style_Artwork_Quot_SizeCode] DEFAULT (''), 
    CONSTRAINT [PK_Style_Artwork_Quot] PRIMARY KEY CLUSTERED ([Ukey] ASC, [LocalSuppId] ASC, [SizeCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Quotation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Artwork Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot', @level2type = N'COLUMN', @level2name = N'LocalSuppId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot', @level2type = N'COLUMN', @level2name = N'CurrencyId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱測試日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot', @level2type = N'COLUMN', @level2name = N'Oven';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗測試日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot', @level2type = N'COLUMN', @level2name = N'Wash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打樣日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot', @level2type = N'COLUMN', @level2name = N'Mockup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Artwork_Quot', @level2type = N'COLUMN', @level2name = N'PriceApv';

go
CREATE NONCLUSTERED INDEX [IDX_Style_Artwork_Quot_Price_PriceApv] ON [dbo].[Style_Artwork_Quot]
(
	[Price] ASC,
	[PriceApv] ASC
)