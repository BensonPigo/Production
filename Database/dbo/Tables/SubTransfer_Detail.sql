CREATE TABLE [dbo].[SubTransfer_Detail] (
    [ID]         VARCHAR (13)    CONSTRAINT [DF_SubTransfer_Detail_ID] DEFAULT ('') NOT NULL,
    [FromPoId]   VARCHAR (13)    CONSTRAINT [DF_SubTransfer_Detail_FromPoId] DEFAULT ('') NOT NULL,
    [FromSeq1]   VARCHAR (3)     CONSTRAINT [DF_SubTransfer_Detail_FromSeq1] DEFAULT ('') NOT NULL,
    [FromSeq2]   VARCHAR (2)     CONSTRAINT [DF_SubTransfer_Detail_FromSeq2] DEFAULT ('') NOT NULL,
    [FromRoll]   VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_FromRoll] DEFAULT ('') NOT NULL,
    [FromDyelot] VARCHAR (4)     CONSTRAINT [DF_SubTransfer_Detail_FromDyelot] DEFAULT ('') NOT NULL,
    [FromStock]  VARCHAR (1)     CONSTRAINT [DF_SubTransfer_Detail_FromStock] DEFAULT ('') NOT NULL,
    [Qty]        NUMERIC (10, 2) CONSTRAINT [DF_SubTransfer_Detail_Qty] DEFAULT ((0)) NULL,
    [ToPoId]     VARCHAR (13)    CONSTRAINT [DF_SubTransfer_Detail_ToPoId] DEFAULT ('') NOT NULL,
    [ToSeq1]     VARCHAR (3)     CONSTRAINT [DF_SubTransfer_Detail_ToSeq1] DEFAULT ('') NOT NULL,
    [ToSeq2]     VARCHAR (2)     CONSTRAINT [DF_SubTransfer_Detail_ToSeq2] DEFAULT ('') NOT NULL,
    [ToStock]    VARCHAR (1)     CONSTRAINT [DF_SubTransfer_Detail_ToStock] DEFAULT ('') NOT NULL,
    [ToLocation] VARCHAR (60)    CONSTRAINT [DF_SubTransfer_Detail_ToLocation] DEFAULT ('') NOT NULL,
    [ToRoll]     VARCHAR (8)     CONSTRAINT [DF_SubTransfer_Detail_ToRoll] DEFAULT ('') NOT NULL,
    [ToDyelot]   VARCHAR (4)     CONSTRAINT [DF_SubTransfer_Detail_ToDyelot] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SubTransfer_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [FromPoId] ASC, [FromSeq1] ASC, [FromSeq2] ASC, [FromRoll] ASC, [FromDyelot] ASC, [FromStock] ASC, [ToPoId] ASC, [ToSeq1] ASC, [ToSeq2] ASC, [ToStock] ASC, [ToLocation] ASC, [ToRoll] ASC, [ToDyelot] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉倉明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源po#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromPoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromSeq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromSeq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromRoll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromDyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromStock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToPoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToSeq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToSeq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToStock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToRoll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToDyelot';

