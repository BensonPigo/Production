CREATE TABLE [dbo].[FtyInventory_Barcode] (
    [Ukey]          BIGINT       NOT NULL,
    [TransactionID] VARCHAR (13) NOT NULL,
    [Barcode]       VARCHAR (16) NOT NULL,
    CONSTRAINT [PK_FtyInventory_Barcode] PRIMARY KEY CLUSTERED ([Ukey] ASC, [TransactionID] ASC, [Barcode] ASC)
);

