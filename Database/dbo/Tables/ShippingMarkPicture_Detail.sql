


CREATE TABLE [dbo].ShippingMarkPicture_Detail(
	ShippingMarkPictureUkey [bigint] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_ShippingMarkPictureUkey] DEFAULT (0) ,
	ShippingMarkTypeUkey [bigint] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_ShippingMarkTypeUkey] DEFAULT (0) ,
	Seq [Int] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_Seq] DEFAULT (0) ,
	IsSSCC [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_IsSSCC]  DEFAULT (0),
	Side Varchar(5) NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_Side]  DEFAULT (''),
	FromRight [Int] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_FromRight] DEFAULT (0) ,
	FromBottom [Int] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_FromBottom] DEFAULT (0) ,
	StickerSizeID [bigInt] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_StickerSizeID] DEFAULT (0) ,
	Is2Side [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_Is2Side]  DEFAULT (0),
	IsHorizontal [Bit] NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_IsHorizontal]  DEFAULT (0),
	IsOverCtnHt bit NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_IsOverCtnHt]  DEFAULT 0,
	NotAutomate bit NOT NULL CONSTRAINT [DF_ShippingMarkPicture_Detail_NotAutomate]  DEFAULT(0),
	CONSTRAINT [PK_ShippingMarkPicture_Detail] PRIMARY KEY CLUSTERED 
	(		ShippingMarkPictureUkey ,ShippingMarkTypeUkey ASC	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO