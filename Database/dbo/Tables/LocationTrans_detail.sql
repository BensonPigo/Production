CREATE TABLE [dbo].[LocationTrans_detail] (
    [ID]               VARCHAR (13)    CONSTRAINT [DF_LocationTrans_detail_ID] DEFAULT ('') NOT NULL,
    [FtyInventoryUkey] BIGINT          NULL,
    [MDivisionID]      VARCHAR (8)     NULL,
    [Poid]             VARCHAR (13)    CONSTRAINT [DF_LocationTrans_detail_Poid] DEFAULT ('') NOT NULL,
    [Seq1]             VARCHAR (3)     CONSTRAINT [DF_LocationTrans_detail_Seq] DEFAULT ('') NOT NULL,
    [Seq2]             VARCHAR (2)     NOT NULL,
    [Roll]             VARCHAR (8)     CONSTRAINT [DF_LocationTrans_detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]           VARCHAR (8)     CONSTRAINT [DF_LocationTrans_detail_Dyelot] DEFAULT ('') NOT NULL,
    [FromLocation]     VARCHAR (5000)   CONSTRAINT [DF_LocationTrans_detail_FromLocation] DEFAULT ('') NULL,
    [ToLocation]       VARCHAR (5000)   CONSTRAINT [DF_LocationTrans_detail_ToLocation] DEFAULT ('') NOT NULL,
    [Qty]              NUMERIC (11, 2) CONSTRAINT [DF_LocationTrans_detail_Qty] DEFAULT ((0)) NOT NULL,
    [Ukey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [StockType]        VARCHAR (1)     CONSTRAINT [DF_LocationTrans_detail_StockType] DEFAULT ('') NOT NULL,
    [CompleteTime]     DATETIME        NULL,
    [SentToWMS] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_LocationTrans_detail_1] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位變更明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Poid';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'卷號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'FromLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'ToLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當下庫存數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
CREATE NONCLUSTERED INDEX [<Name2 of Missing Index, sysname,>]
    ON [dbo].[LocationTrans_detail]([MDivisionID] ASC, [Poid] ASC, [Seq1] ASC, [Seq2] ASC)
    INCLUDE([ID], [FromLocation], [ToLocation], [Qty]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[LocationTrans_detail]([ID] ASC, [MDivisionID] ASC, [Poid] ASC, [Seq1] ASC, [Seq2] ASC)
    INCLUDE([FromLocation], [ToLocation]);

