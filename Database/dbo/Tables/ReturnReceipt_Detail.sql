﻿CREATE TABLE [dbo].[ReturnReceipt_Detail] (
    [Id]               VARCHAR (13)    CONSTRAINT [DF_ReturnReceipt_Detail_Id] DEFAULT ('') NOT NULL,
    [FtyInventoryUkey] BIGINT          NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_ReturnReceipt_Detail_MDivsionID] DEFAULT ('') NULL,
    [POID]             VARCHAR (13)    CONSTRAINT [DF_ReturnReceipt_Detail_POID] DEFAULT ('') NULL,
    [Seq1]             VARCHAR (3)     CONSTRAINT [DF_ReturnReceipt_Detail_Seq1] DEFAULT ('') NULL,
    [Seq2]             VARCHAR (2)     CONSTRAINT [DF_ReturnReceipt_Detail_Seq2] DEFAULT ('') NULL,
    [Roll]             VARCHAR (8)     CONSTRAINT [DF_ReturnReceipt_Detail_Roll] DEFAULT ('') NULL,
    [Dyelot]           VARCHAR (4)     CONSTRAINT [DF_ReturnReceipt_Detail_Dyelot] DEFAULT ('') NULL,
    [StockType]        CHAR (1)        CONSTRAINT [DF_ReturnReceipt_Detail_StockType] DEFAULT ('') NULL,
    [Qty]              NUMERIC (10, 2) CONSTRAINT [DF_ReturnReceipt_Detail_Qty] DEFAULT ((0)) NULL,
    [Ukey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ReturnReceipt_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料退回明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退回數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO


