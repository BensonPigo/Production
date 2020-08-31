CREATE TABLE [dbo].ShippingMarkPicture(
	BrandID [varchar](8) NOT NULL CONSTRAINT [DF_ShippingMarkPicture_BrandID] DEFAULT ('') ,
	Category [varchar](10) NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Category] DEFAULT ('') ,
	ShippingMarkCombinationUkey [bigint] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_ShippingMarkCombinationUkey] DEFAULT (0) ,
	CTNRefno [varchar](21) NOT NULL CONSTRAINT [DF_ShippingMarkPicture_CTNRefno] DEFAULT ('') ,
	AddDate [datetime] NULL,
	AddName [varchar](10) NOT NULL CONSTRAINT DF_ShippingMarkPicture_AddName DEFAULT ('') ,
	EditDate [datetime] NULL,
	EditName [varchar](10) NOT NULL CONSTRAINT DF_ShippingMarkPicture_EditName DEFAULT ('') ,
	Junk [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Junk] DEFAULT (0),
	Ukey [Bigint] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_ShippingMarkPicture] PRIMARY KEY CLUSTERED 
(		Ukey ASC	)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

