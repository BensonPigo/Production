CREATE TABLE [dbo].ShippingMarkStamp(
	PackingListID	varchar(15)	NOT NULL ,
	AddDate			datetime	NULL ,
	AddName			varchar(10) NOT NULL CONSTRAINT [DF_ShippingMarkStamp_AddName] DEFAULT(''),
	EditDate		datetime	NULL ,
	EditName		varchar(10) NOT NULL CONSTRAINT [DF_ShippingMarkStamp_EditName] DEFAULT(''),
	CONSTRAINT [PK_ShippingMarkStamp] PRIMARY KEY CLUSTERED 
(
	PackingListID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
GO
	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝箱清單'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkStamp'
	, @level2type = N'COLUMN', @level2name = N'PackingListID';
;
GO