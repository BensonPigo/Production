CREATE TABLE [dbo].[PackErrTransfer]
(
	[ID] BIGINT IDENTITY (1, 1) NOT NULL, 
    [TransferDate] DATE NULL, 
    [MDivisionID] VARCHAR(8) CONSTRAINT [DF_PackErrTransfer_MDivisionID] DEFAULT ('') NULL, 
    [OrderID] VARCHAR(13) CONSTRAINT [DF_PackErrTransfer_OrderID] DEFAULT ('') NULL, 
    [PackingListID] VARCHAR(13) CONSTRAINT [DF_PackErrTransfer_PackingListID] DEFAULT ('') NULL, 
    [CTNStartNo] VARCHAR(6) CONSTRAINT [DF_PackErrTransfer_CTNStartNo] DEFAULT ('') NULL,  
    [AddName] VARCHAR(10) CONSTRAINT [DF_PackErrTransfer_AddName] DEFAULT ('') NULL,  
    [AddDate] DATETIME NULL, 
    CONSTRAINT [PK_PackErrTransfer] PRIMARY KEY CLUSTERED ([ID] ASC)
)
