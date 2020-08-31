USE Production
GO



CREATE TABLE [dbo].ShippingMarkType(
	ID [varchar](30) NOT NULL CONSTRAINT [DF_ShippingMarkType_ID]  DEFAULT ('') ,
	Category [varchar](10) NOT NULL CONSTRAINT [DF_ShippingMarkType_Category] DEFAULT ('') ,
	BrandID [varchar](8) NOT NULL CONSTRAINT [DF_ShippingMarkType_BrandID] DEFAULT ('') ,
	IsSSCC [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkType_IsSSCC]  DEFAULT (0),
	Ukey [Bigint] IDENTITY(1,1) NOT NULL,
	Junk [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkType_Junk] DEFAULT (0),
	AddDate [datetime] NULL,
	AddName [varchar](10) NOT NULL CONSTRAINT DF_ShippingMarkType_AddName DEFAULT ('') ,
	EditDate [datetime] NULL,
	EditName [varchar](10) NOT NULL CONSTRAINT DF_ShippingMarkType_EditName DEFAULT ('') ,
	CONSTRAINT [PK_ShippingMarkType] PRIMARY KEY CLUSTERED 
(		Ukey ASC	)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼紙的種類；同品牌 ID 不可重複'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkType'
	, @level2type = N'COLUMN', @level2name = N'ID';
GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別 : 貼標 Sticker(PIC) 或 噴碼 Stamp(HTML)'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkType'
	, @level2type = N'COLUMN', @level2name = N'Category';
GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為 SSCC 的貼紙；供廠商系統判斷哪一面有貼 SSCC，機械手臂會將紙箱有貼 SSCC 的那一面面向掃描器'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkType'
	, @level2type = N'COLUMN', @level2name = N'IsSSCC';
GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkType'
	, @level2type = N'COLUMN', @level2name = N'BrandID';
GO
	;