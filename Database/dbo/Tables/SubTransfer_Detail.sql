CREATE TABLE [dbo].[SubTransfer_Detail] (
    [ID]                   VARCHAR (13)    CONSTRAINT [DF_SubTransfer_Detail_ID] DEFAULT ('') NOT NULL,
    [FromFtyInventoryUkey] BIGINT          NULL,
    [FromMDivisionID]      VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_FromMDivisionID] DEFAULT ('') NULL,
    [FromFactoryID]        VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_FromFactoryID] DEFAULT ('') NULL,
    [FromPOID]             VARCHAR (13)    CONSTRAINT [DF_SubTransfer_Detail_FromPOID] DEFAULT ('') NOT NULL,
    [FromSeq1]             VARCHAR (3)     CONSTRAINT [DF_SubTransfer_Detail_FromSeq1] DEFAULT ('') NOT NULL,
    [FromSeq2]             VARCHAR (2)     CONSTRAINT [DF_SubTransfer_Detail_FromSeq2] DEFAULT ('') NOT NULL,
    [FromRoll]             VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_FromRoll] DEFAULT ('') NULL,
    [FromStockType]        VARCHAR (1)     CONSTRAINT [DF_SubTransfer_Detail_FromStockType] DEFAULT ('') NOT NULL,
    [FromDyelot]           VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_FromDyelot] DEFAULT ('') NULL,
    [ToMDivisionID]        VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_ToMDivisionID] DEFAULT ('') NULL,
    [ToFactoryID]          VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_ToFactoryID] DEFAULT ('') NULL,
    [ToPOID]               VARCHAR (13)    CONSTRAINT [DF_SubTransfer_Detail_ToPOID] DEFAULT ('') NOT NULL,
    [ToSeq1]               VARCHAR (3)     CONSTRAINT [DF_SubTransfer_Detail_ToSeq1] DEFAULT ('') NOT NULL,
    [ToSeq2]               VARCHAR (2)     CONSTRAINT [DF_SubTransfer_Detail_ToSeq2] DEFAULT ('') NOT NULL,
    [ToRoll]               VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_ToRoll] DEFAULT ('') NULL,
    [ToStockType]          VARCHAR (1)     CONSTRAINT [DF_SubTransfer_Detail_ToStockType] DEFAULT ('') NOT NULL,
    [ToDyelot]             VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_ToDyelot] DEFAULT ('') NULL,
    [Qty]                  NUMERIC (11, 2) CONSTRAINT [DF_SubTransfer_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [ToLocation]           VARCHAR (500)    CONSTRAINT [DF_SubTransfer_Detail_ToLocation] DEFAULT ('') NULL,
    [ToCTNNo]              VARCHAR (10)    NULL,
    [Ukey]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompleteTime] DATETIME NULL, 
    [SentToWMS] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_SubTransfer_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉倉明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToLocation';


GO



GO
CREATE NONCLUSTERED INDEX [<Name2 of Missing Index, sysname,>]
    ON [dbo].[SubTransfer_Detail]([FromPOID] ASC, [FromSeq1] ASC, [FromSeq2] ASC)
    INCLUDE([ID], [FromRoll], [FromStockType], [FromDyelot], [Qty]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index3, sysname,>]
    ON [dbo].[SubTransfer_Detail]([ToPOID] ASC, [ToSeq1] ASC, [ToSeq2] ASC, [ToLocation] ASC)
    INCLUDE([ID]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index2, sysname,>]
    ON [dbo].[SubTransfer_Detail]([ToMDivisionID] ASC, [ToPOID] ASC, [ToSeq1] ASC, [ToSeq2] ASC)
    INCLUDE([ID], [Qty]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[SubTransfer_Detail]([FromMDivisionID] ASC, [FromPOID] ASC, [FromSeq1] ASC, [FromSeq2] ASC)
    INCLUDE([ID], [Qty]);


GO

CREATE INDEX [ID_TOSEQ1_TOSEQ2] ON [dbo].[SubTransfer_Detail] ([ID],[ToSeq1],[ToSeq2])
