﻿CREATE TABLE [dbo].[TransferIn_Detail] (
    [ID]          VARCHAR (13)    CONSTRAINT [DF_TransferIn_Detail_ID] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)     CONSTRAINT [DF_TransferIn_Detail_MDivisionID] DEFAULT ('') NOT NULL,
    [POID]        VARCHAR (13)    CONSTRAINT [DF_TransferIn_Detail_POID] DEFAULT ('') NOT NULL,
    [Seq1]        VARCHAR (3)     CONSTRAINT [DF_TransferIn_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]        VARCHAR (2)     CONSTRAINT [DF_TransferIn_Detail_Seq2] DEFAULT ('') NOT NULL,
    [Roll]        VARCHAR (8)     CONSTRAINT [DF_TransferIn_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]      VARCHAR (4)     CONSTRAINT [DF_TransferIn_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [StockType]   VARCHAR (1)     CONSTRAINT [DF_TransferIn_Detail_StockType] DEFAULT ('') NOT NULL,
    [Location]    VARCHAR (60)    CONSTRAINT [DF_TransferIn_Detail_Location] DEFAULT ('') NULL,
    [Qty]         NUMERIC (10, 2) CONSTRAINT [DF_TransferIn_Detail_Qty] DEFAULT ((0)) NULL,
    [Ukey]        BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_TransferIn_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠入明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠入單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn_Detail', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn_Detail', @level2type = N'COLUMN', @level2name = N'Qty';

