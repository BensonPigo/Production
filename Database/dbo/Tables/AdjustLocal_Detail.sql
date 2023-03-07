CREATE TABLE [dbo].[AdjustLocal_Detail]
(
	[Id] VARCHAR(13) NOT NULL DEFAULT ('') , 
    [MDivisionID] VARCHAR(8) NULL DEFAULT (''), 
    [POID] VARCHAR(13) NULL DEFAULT (''), 
    [Refno] VARCHAR(36) NOT NULL DEFAULT (''), 
    [Color] VARCHAR(15) NOT NULL DEFAULT (''), 
    [StockType] CHAR NULL DEFAULT (''), 
    [QtyBefore] NUMERIC(11, 2) NULL DEFAULT ((0)), 
    [QtyAfter] NUMERIC(11, 2) NULL DEFAULT ((0)), 
    [ReasonId] VARCHAR(5) NULL DEFAULT (''), 
    [Ukey] BIGINT NOT NULL IDENTITY, 
    CONSTRAINT [PK_AdjustLocal_Detail] PRIMARY KEY ([Ukey]) 
)
