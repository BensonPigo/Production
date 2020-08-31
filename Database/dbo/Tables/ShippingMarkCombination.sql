

	CREATE TABLE [dbo].ShippingMarkCombination(
		ID [varchar](30) NOT NULL CONSTRAINT [DF_ShippingMarkCombination_ID]  DEFAULT ('') ,
		Category [varchar](10) NOT NULL CONSTRAINT [DF_ShippingMarkCombination_Category] DEFAULT ('') ,
		BrandID [varchar](8) NOT NULL CONSTRAINT [DF_ShippingMarkCombination_BrandID] DEFAULT ('') ,
		IsMixPack [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkCombination_IsMixPack]  DEFAULT (0),
		IsDefault [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkCombination_IsDefault]  DEFAULT (0),
		Ukey [Bigint] IDENTITY(1,1) NOT NULL,
		Junk [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkCombination_Junk] DEFAULT (0),
		AddDate [datetime] NULL,
		AddName [varchar](10) NOT NULL CONSTRAINT DF_ShippingMarkCombination_AddName DEFAULT ('') ,
		EditDate [datetime] NULL,
		EditName [varchar](10) NOT NULL CONSTRAINT DF_ShippingMarkCombination_EditName DEFAULT ('') ,
	 CONSTRAINT [PK_ShippingMarkCombination] PRIMARY KEY CLUSTERED 
	(		Ukey ASC	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark組合; 同品牌 ID 不可重複'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkCombination'
	, @level2type = N'COLUMN', @level2name = N'ID';
	GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別 : 貼標 Sticker(PIC) 或 噴碼 Stamp(HTML)'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkCombination'
	, @level2type = N'COLUMN', @level2name = N'Category';
	GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'混尺碼裝箱'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkCombination'
	, @level2type = N'COLUMN', @level2name = N'IsMixPack';
	GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為CustCD預設'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkCombination'
	, @level2type = N'COLUMN', @level2name = N'IsDefault';
	GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkCombination'
	, @level2type = N'COLUMN', @level2name = N'BrandID'
	GO
	;	
	;