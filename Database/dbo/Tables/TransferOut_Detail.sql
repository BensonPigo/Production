CREATE TABLE [dbo].[TransferOut_Detail] (
    [ID]               VARCHAR (13)    CONSTRAINT [DF_TransferOut_Detail_ID] DEFAULT ('') NOT NULL,
    [FtyInventoryUkey] BIGINT          NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_TransferOut_Detail_MDivisionID] DEFAULT ('') NULL,
    [POID]             VARCHAR (13)    CONSTRAINT [DF_TransferOut_Detail_POID] DEFAULT ('') NULL,
    [Seq1]             VARCHAR (3)     CONSTRAINT [DF_TransferOut_Detail_Seq1] DEFAULT ('') NULL,
    [Seq2]             VARCHAR (2)     CONSTRAINT [DF_TransferOut_Detail_Seq2] DEFAULT ('') NULL,
    [Roll]             VARCHAR (8)     CONSTRAINT [DF_TransferOut_Detail_Roll] DEFAULT ('') NULL,
    [Dyelot]           VARCHAR (8)     CONSTRAINT [DF_TransferOut_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [StockType]        CHAR (1)        CONSTRAINT [DF_TransferOut_Detail_StockType] DEFAULT ('') NULL,
    [Qty]              NUMERIC (10, 2) CONSTRAINT [DF_TransferOut_Detail_Qty] DEFAULT ((0)) NULL,
    [Ukey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ToPOID] VARCHAR(13) NOT NULL CONSTRAINT [DF_TransferOut_Detail_ToPOID] DEFAULT ('') NULL,
    [ToSeq1] VARCHAR(3) NOT NULL CONSTRAINT [DF_TransferOut_Detail_ToSeq1] DEFAULT ('') NULL,
    [ToSeq12] VARCHAR(2) NOT NULL CONSTRAINT [DF_TransferOut_Detail_ToSeq2] DEFAULT ('') NULL,
    CONSTRAINT [PK_TransferOut_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠出明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠出單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[TransferOut_Detail]([ID] ASC);

