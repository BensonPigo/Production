CREATE TABLE [dbo].[SemiFinishedIssue_Detail]
(
	[ID] VARCHAR(13) NOT NULL,
	[POID] VARCHAR(13) NOT NULL, 
	[Refno] VARCHAR(21) NOT NULL, 
    [Roll] VARCHAR(8) CONSTRAINT [DF_SemiFinishedIssue_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot] VARCHAR(8) CONSTRAINT [DF_SemiFinishedIssue_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [StockType] CHAR CONSTRAINT [DF_SemiFinishedIssue_Detail_StockType] DEFAULT ('') NOT NULL,
	[Qty] NUMERIC(11, 2) CONSTRAINT [DF_SemiFinishedIssue_Detail_Qty] DEFAULT (0) NOT NULL, 
    CONSTRAINT [PK_SemiFinishedIssue_Detail] PRIMARY KEY CLUSTERED  ([ID], [POID], [Refno], [Roll], [Dyelot], [StockType] ASC)
)
