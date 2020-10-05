CREATE TABLE [dbo].ShippingMarkType_Detail(
	ShippingMarkTypeUkey bigint		NOT NULL ,
	StickerSizeID		 bigint		NOT NULL ,
	TemplateName		 varchar(30) NOT NULL CONSTRAINT [DF_ShippingMarkType_Detail_TemplateName] DEFAULT(''),
	CONSTRAINT [PK_ShippingMarkType_Detail] PRIMARY KEY CLUSTERED 
(
	ShippingMarkTypeUkey ASC ,StickerSizeID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark 種類'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkType_Detail'
	, @level2type = N'COLUMN', @level2name = N'ShippingMarkTypeUkey';
;
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark 大小'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkType_Detail'
	, @level2type = N'COLUMN', @level2name = N'StickerSizeID';
;
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TFORMer 範本檔名'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkType_Detail'
	, @level2type = N'COLUMN', @level2name = N'TemplateName';
;
GO