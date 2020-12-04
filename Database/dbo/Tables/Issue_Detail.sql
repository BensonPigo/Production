CREATE TABLE [dbo].[Issue_Detail] (
    [Id]                VARCHAR (13)    CONSTRAINT [DF_Issue_Detail_Id] DEFAULT ('') NOT NULL,
    [Issue_SummaryUkey] BIGINT          CONSTRAINT [DF_Issue_Detail_Issue_SummaryUkey] DEFAULT ((0)) NOT NULL,
    [FtyInventoryUkey]  BIGINT          NULL,
    [Qty]               NUMERIC (11, 2) CONSTRAINT [DF_Issue_Detail_Qty] DEFAULT ((0)) NULL,
    [MDivisionID]       VARCHAR (8)     CONSTRAINT [DF_Issue_Detail_MDivisionID] DEFAULT ('') NULL,
    [POID]              VARCHAR (13)    CONSTRAINT [DF_Issue_Detail_POID] DEFAULT ('') NULL,
    [Seq1]              VARCHAR (3)     CONSTRAINT [DF_Issue_Detail_Seq1] DEFAULT ('') NULL,
    [Seq2]              VARCHAR (2)     CONSTRAINT [DF_Issue_Detail_Seq2] DEFAULT ('') NULL,
    [Roll]              VARCHAR (8)     CONSTRAINT [DF_Issue_Detail_Roll] DEFAULT ('') NULL,
    [Dyelot]            VARCHAR (8)     CONSTRAINT [DF_Issue_Detail_Dyelot] DEFAULT ('') NULL,
    [StockType]         CHAR (1)        CONSTRAINT [DF_Issue_Detail_StockType] DEFAULT ('') NULL,
    [ukey]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompleteTime] DATETIME NULL,
    [IsQMS] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Issue_Detail] PRIMARY KEY CLUSTERED ([ukey] ASC),
    CONSTRAINT [FK_Issue_Detail_Issue_Detail] FOREIGN KEY ([ukey]) REFERENCES [dbo].[Issue_Detail] ([ukey])
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Issue_SummaryUkey';


GO
CREATE NONCLUSTERED INDEX [IX_Issue_Detail]
    ON [dbo].[Issue_Detail]([Id] ASC, [POID] ASC, [Seq1] ASC, [Seq2] ASC);


GO
CREATE NONCLUSTERED INDEX [<Name2 of Missing Index, sysname,>]
    ON [dbo].[Issue_Detail]([POID] ASC, [Seq1] ASC, [Seq2] ASC)
    INCLUDE([Id], [Qty], [Roll], [Dyelot], [StockType]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index2, sysname,>]
    ON [dbo].[Issue_Detail]([MDivisionID] ASC, [POID] ASC, [Seq1] ASC, [Seq2] ASC)
    INCLUDE([Id], [Qty]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Issue_Detail]([FtyInventoryUkey] ASC)
    INCLUDE([Id], [Qty]);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20180413-155915]
    ON [dbo].[Issue_Detail]([Issue_SummaryUkey] ASC);

