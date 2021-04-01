CREATE TABLE [dbo].[SemiFinishedInventory_Location]
(
	[POID] VARCHAR(13) NOT NULL, 
	[Refno] VARCHAR(21) NOT NULL, 
    [Roll] VARCHAR(8) NOT NULL, 
    [Dyelot] VARCHAR(8) NOT NULL, 
    [StockType] CHAR CONSTRAINT [DF_SemiFinishedInventory_Location_StockType] DEFAULT ('') NOT NULL,
	[MtlLocationID] VARCHAR(20)  NOT NULL,
	CONSTRAINT [PK_SemiFinishedInventory_Location] PRIMARY KEY CLUSTERED  ([POID], [Refno], [Roll], [Dyelot], [StockType], [MtlLocationID] ASC)
)
