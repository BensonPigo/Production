

	CREATE TABLE [dbo].ShippingMarkCombination_Detail(
		ShippingMarkCombinationUkey [Bigint] NOT NULL,
		Seq [Int] NOT NULL CONSTRAINT [DF_ShippingMarkCombination_Detail_Seq] DEFAULT (1),
		ShippingMarkTypeUkey [Bigint]  NOT NULL,
	 CONSTRAINT [PK_ShippingMarkCombination_Detail] PRIMARY KEY CLUSTERED 
	(		ShippingMarkCombinationUkey ,ShippingMarkTypeUkey ASC	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark 組合'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkCombination_Detail'
	, @level2type = N'COLUMN', @level2name = N'ShippingMarkCombinationUkey'
	GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark 順序'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkCombination_Detail'
	, @level2type = N'COLUMN', @level2name = N'Seq'
	GO
	;
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark 種類'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkCombination_Detail'
	, @level2type = N'COLUMN', @level2name = N'ShippingMarkTypeUkey'
	GO
	;
