CREATE TABLE [dbo].[LocalItem_ThreadBuyerColorGroupPrice]
(
	[Refno] VARCHAR(21) NOT NULL, 
    [BuyerID] VARCHAR(8) NOT NULL, 
    [ThreadColorGroupID] VARCHAR(50) NOT NULL, 
    [Price] NUMERIC(12, 4) NULL,
	[AddName] VARCHAR(10) CONSTRAINT [DF_LocalItem_ThreadBuyerColorGroupPrice_AddName] DEFAULT ('') NULL,
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_LocalItem_ThreadBuyerColorGroupPrice_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME NULL,
    CONSTRAINT [PK_LocalItem_ThreadBuyerColorGroupPrice] PRIMARY KEY CLUSTERED (Refno,BuyerID,ThreadColorGroupID ASC)
)
