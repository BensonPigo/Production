CREATE TABLE [dbo].[SemiFinishedReceiving_Detail]
(
	[ID] VARCHAR(13) NOT NULL,
	[POID] VARCHAR(13) NOT NULL, 
	[Refno] VARCHAR(21) NOT NULL, 
    [Roll] VARCHAR(8) CONSTRAINT [DF_SemiFinishedReceiving_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot] VARCHAR(8) CONSTRAINT [DF_SemiFinishedReceiving_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [StockType] CHAR CONSTRAINT [DF_SemiFinishedReceiving_Detail_StockType] DEFAULT ('') NOT NULL,
	[Qty] NUMERIC(11, 2)  CONSTRAINT [DF_SemiFinishedReceiving_Detail_Qty] DEFAULT (0) NOT NULL, 
    [Location] VARCHAR(60) CONSTRAINT [DF_SemiFinishedReceiving_Detail_Location] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SemiFinishedReceiving_Detail] PRIMARY KEY CLUSTERED  ([ID], [POID], [Refno], [Roll], [Dyelot], [StockType] ASC)
)
