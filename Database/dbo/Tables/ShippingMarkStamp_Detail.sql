CREATE TABLE [dbo].ShippingMarkStamp_Detail(
	PackingListID	varchar(15)	NOT NULL ,
	SCICtnNo	varchar(15)	NOT NULL ,
	ShippingMarkCombinationUkey bigint	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_ShippingMarkCombinationUkey] DEFAULT(0),
	ShippingMarkTypeUkey bigint	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_ShippingMarkTypeUkey] DEFAULT(0),
	FilePath	varchar(150)	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_FilePath] DEFAULT(''),
	FileName	varchar(30)	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_FileName] DEFAULT(''),
	Image	varbinary(max)	NULL ,
	Side	varchar(5)	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_Side] DEFAULT(''),
	Seq		int	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_Seq] DEFAULT(0),
	FromRight		numeric(8, 2)	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_FromRight] DEFAULT(0),
	FromBottom		numeric(8, 2)	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_FromBottom] DEFAULT(0),
	Width		int	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_Width] DEFAULT(0),
	Length		int	NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_Length] DEFAULT(0),
	[DPI] INT NOT NULL CONSTRAINT [DF_ShippingMarkStamp_Detail_DPI] DEFAULT 0, 
	CONSTRAINT [PK_ShippingMarkStamp_Detail] PRIMARY KEY CLUSTERED 
(
	PackingListID ASC ,SCICtnNo ASC ,ShippingMarkTypeUkey ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝箱清單'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'PackingListID';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI箱號'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'SCICtnNo';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark 組合'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'ShippingMarkCombinationUkey';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark 種類'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'ShippingMarkTypeUkey';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HTML 位置'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'FilePath';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HTML 名稱'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'FileName';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BMP圖片二進位制資料'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'Image';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼面, 上下左右前後'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'Side';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'Seq';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離右邊的位置(mm)'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'FromRight';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離下面的位置(mm)'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'FromBottom';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標籤寬度'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'Width';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標籤長度'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'Length';
;
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉 HTML 的 DPI'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail'
	, @level2type = N'COLUMN', @level2name = N'DPI';
;
GO	