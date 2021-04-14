CREATE TABLE [dbo].[Stocktaking_Detail] (
    [Id]               VARCHAR (13)    CONSTRAINT [DF_Stocktaking_Detail_Id] DEFAULT ('') NOT NULL,
    [FtyInventoryUkey] BIGINT          NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_Stocktaking_Detail_MDivisionID] DEFAULT ('') NULL,
    [POID]             VARCHAR (13)    CONSTRAINT [DF_Stocktaking_Detail_POID] DEFAULT ('') NULL,
    [Seq1]             VARCHAR (3)     CONSTRAINT [DF_Stocktaking_Detail_Seq1] DEFAULT ('') NULL,
    [Seq2]             VARCHAR (2)     CONSTRAINT [DF_Stocktaking_Detail_Seq2] DEFAULT ('') NULL,
    [Roll]             VARCHAR (8)     CONSTRAINT [DF_Stocktaking_Detail_Roll] DEFAULT ('') NULL,
    [Dyelot]           VARCHAR (8)     CONSTRAINT [DF_Stocktaking_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [StockType]        CHAR (1)        CONSTRAINT [DF_Stocktaking_Detail_StockType] DEFAULT ('') NULL,
    [QtyBefore]        NUMERIC (11, 2) CONSTRAINT [DF_Stocktaking_Detail_QtyBefore] DEFAULT ((0)) NULL,
    [QtyAfter]         NUMERIC (11, 2) CONSTRAINT [DF_Stocktaking_Detail_QtyAfter] DEFAULT ((0)) NULL,
    [UKey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompleteTime]     DATETIME        NULL,
    [SentToWMS]        BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Stocktaking_Detail] PRIMARY KEY CLUSTERED ([UKey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫盤點明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整前數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'QtyBefore';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'QtyAfter';

