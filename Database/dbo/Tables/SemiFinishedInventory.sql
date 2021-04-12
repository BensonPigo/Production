CREATE TABLE [dbo].[SemiFinishedInventory]
(
	[POID] VARCHAR(13) NOT NULL, 
	[Refno] VARCHAR(21) NOT NULL, 
    [Roll] VARCHAR(8) NOT NULL, 
    [Dyelot] VARCHAR(8) NOT NULL, 
    [StockType] CHAR CONSTRAINT [DF_SemiFinishedInventory_StockType] DEFAULT ('') NOT NULL,
    [InQty] NUMERIC(11, 2) CONSTRAINT [DF_SemiFinishedInventory_InQty] DEFAULT (0) NOT NULL,
    [OutQty] NUMERIC(11, 2) CONSTRAINT [DF_SemiFinishedInventory_OutQty] DEFAULT (0) NOT NULL,
    [AdjustQty] NUMERIC(11, 2) CONSTRAINT [DF_SemiFinishedInventory_AdjustQty] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_SemiFinishedInventory] PRIMARY KEY CLUSTERED  ([POID], [Refno], [Roll], [Dyelot], [StockType] ASC)
)
