CREATE TABLE [dbo].[SemiFinishedAdjust_Detail]
(
	[ID] VARCHAR(13) NOT NULL,
	[POID] VARCHAR(13) NOT NULL, 
	[Refno] VARCHAR(21) NOT NULL, 
    [Roll] VARCHAR(8) CONSTRAINT [DF_SemiFinishedAdjust_Detail_Roll] DEFAULT ('') NOT NULL, 
    [Dyelot] VARCHAR(8) CONSTRAINT [DF_SemiFinishedAdjust_Detail_Dyelot] DEFAULT ('') NOT NULL, 
    [StockType] CHAR CONSTRAINT [DF_SemiFinishedAdjust_Detail_StockType] DEFAULT ('') NOT NULL,
	[QtyBefore] NUMERIC(11, 2) CONSTRAINT [DF_SemiFinishedAdjust_Detail_QtyBefore] DEFAULT (0) NOT NULL, 
    [QtyAfter] NUMERIC(11, 2) CONSTRAINT [DF_SemiFinishedAdjust_Detail_QtyAfter] DEFAULT (0) NOT NULL, 
    CONSTRAINT [PK_SemiFinishedAdjust_Detail] PRIMARY KEY CLUSTERED  ([ID], [POID], [Refno], [Roll], [Dyelot], [StockType] ASC)
)
