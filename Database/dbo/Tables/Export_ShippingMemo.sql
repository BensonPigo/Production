CREATE TABLE [dbo].[Export_ShippingMemo]
(
	[Ukey] BIGINT NOT NULL IDENTITY, 
    [ID] VARCHAR(13) NOT NULL CONSTRAINT [DF_Export_ShippingMemo_ID] DEFAULT(''), 
    [ShippingExpense] BIT NOT NULL CONSTRAINT [DF_Export_ShippingMemo_ShippingExpense] DEFAULT(0), 
    [Subject] NVARCHAR(100) NOT NULL CONSTRAINT [DF_Export_ShippingMemo_Subject] DEFAULT(''), 
    [Description] NVARCHAR(500) NOT NULL CONSTRAINT [DF_Export_ShippingMemo_Description] DEFAULT(''), 
    [AddName] VARCHAR(10) NOT NULL CONSTRAINT [DF_Export_ShippingMemo_AddName] DEFAULT(''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL CONSTRAINT [DF_Export_ShippingMemo_EditName] DEFAULT(''), 
    [EditDate] DATETIME NULL,
    CONSTRAINT [PK_Export_ShippingMemo] PRIMARY KEY CLUSTERED (Ukey)
)
