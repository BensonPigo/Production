CREATE TABLE [dbo].[StocktakingLocal_Detail]
(
	[ID] VARCHAR(13) NOT NULL DEFAULT ('') , 
    [FtyInventoryUkey] BIGINT NULL  , 
    [MDivisionID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [POID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [Refno] VARCHAR(36) NOT NULL DEFAULT (''), 
    [Color] VARCHAR(15) NOT NULL DEFAULT (''), 
    [StockType] VARCHAR NULL DEFAULT (''), 
    [QtyBefore] NUMERIC(11, 2) NULL DEFAULT ((0)), 
    [QtyAfter] NUMERIC(11, 2) NULL DEFAULT ((0)), 
    [UKey] BIGINT NOT NULL IDENTITY, 
    CONSTRAINT [PK_StocktakingLocal_Detail] PRIMARY KEY ([UKey], [ID]) 
)
