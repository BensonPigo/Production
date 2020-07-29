CREATE TABLE [dbo].[BorrowBack_Detail] (
    [ID]                   VARCHAR (13)    CONSTRAINT [DF_BorrowBack_Detail_ID] DEFAULT ('') NOT NULL,
    [FromFtyInventoryUkey] BIGINT          NULL,
    [FromMDivisionID]      VARCHAR (8)     CONSTRAINT [DF_BorrowBack_Detail_FromMDivisionID] DEFAULT ('') NULL,
    [FromFactoryID]        VARCHAR (8)     NULL,
    [FromPOID]             VARCHAR (13)    CONSTRAINT [DF_BorrowBack_Detail_FromPOID] DEFAULT ('') NOT NULL,
    [FromSeq1]             VARCHAR (3)     CONSTRAINT [DF_BorrowBack_Detail_FromSeq1] DEFAULT ('') NOT NULL,
    [FromSeq2]             VARCHAR (2)     CONSTRAINT [DF_BorrowBack_Detail_FromSeq2] DEFAULT ('') NOT NULL,
    [FromRoll]             VARCHAR (8)     CONSTRAINT [DF_BorrowBack_Detail_FromRoll] DEFAULT ('') NULL,
    [FromDyelot]           VARCHAR (8)     CONSTRAINT [DF_BorrowBack_Detail_FromDyelot] DEFAULT ('') NULL,
    [FromStockType]        CHAR (1)        CONSTRAINT [DF_BorrowBack_Detail_FromStockType] DEFAULT ('') NOT NULL,
    [ToMDivisionID]        VARCHAR (8)     CONSTRAINT [DF_BorrowBack_Detail_ToMDivisionID] DEFAULT ('') NULL,
    [ToFactoryID]          VARCHAR (8)     NULL,
    [ToPOID]               VARCHAR (13)    CONSTRAINT [DF_BorrowBack_Detail_ToPOID] DEFAULT ('') NOT NULL,
    [ToSeq1]               VARCHAR (3)     CONSTRAINT [DF_BorrowBack_Detail_ToSeq1] DEFAULT ('') NOT NULL,
    [ToSeq2]               VARCHAR (2)     CONSTRAINT [DF_BorrowBack_Detail_ToSeq2] DEFAULT ('') NOT NULL,
    [ToRoll]               VARCHAR (8)     CONSTRAINT [DF_BorrowBack_Detail_ToRoll] DEFAULT ('') NULL,
    [ToStockType]          CHAR (1)        CONSTRAINT [DF_BorrowBack_Detail_ToStock] DEFAULT ('') NOT NULL,
    [ToDyelot]             VARCHAR (8)     CONSTRAINT [DF_BorrowBack_Detail_ToDyelot] DEFAULT ('') NULL,
    [Qty]                  NUMERIC (10, 2) CONSTRAINT [DF_BorrowBack_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [Ukey]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompleteTime] DATETIME NULL, 
    CONSTRAINT [PK_BorrowBack_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'借還料明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'借還料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'借還量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack_Detail', @level2type = N'COLUMN', @level2name = N'ToRoll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack_Detail', @level2type = N'COLUMN', @level2name = N'ToDyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack_Detail', @level2type = N'COLUMN', @level2name = N'ToStockType';

