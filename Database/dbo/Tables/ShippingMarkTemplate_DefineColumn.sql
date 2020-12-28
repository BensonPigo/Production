CREATE TABLE [dbo].ShippingMarkTemplate_DefineColumn(
	ID varchar(60)		NOT NULL ,
	FromPMS bit			NOT NULL CONSTRAINT [DF_ShippingMarkTemplate_DefineColumn_FromPMS] DEFAULT(0),
	[Desc] varchar(200) NOT NULL CONSTRAINT [DF_ShippingMarkTemplate_DefineColumn_Desc] DEFAULT(''),
	ChkEmpty Bit NOT NULL CONSTRAINT [DF_ShippingMarkTemplate_DefineColumn_ChkEmpty] DEFAULT 0 ,
	CONSTRAINT [PK_ShippingMarkTemplate_DefineColumn] PRIMARY KEY CLUSTERED 
(
	ID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'設計模板軟體可使用欄位名稱'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkTemplate_DefineColumn'
	, @level2type = N'COLUMN', @level2name = N'ID';
;
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料來源是否為 PMS'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkTemplate_DefineColumn'
	, @level2type = N'COLUMN', @level2name = N'FromPMS';
;
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料來源'
	, @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'ShippingMarkTemplate_DefineColumn'
	, @level2type = N'COLUMN', @level2name = N'Desc';
;
GO

