CREATE TABLE [dbo].[Issue_Detail] (
    [Id]                  VARCHAR (13)    CONSTRAINT [DF_Issue_Detail_Id] DEFAULT ('') NOT NULL,
    [Issue_SummaryUkey]   BIGINT          CONSTRAINT [DF_Issue_Detail_Issue_SummaryUkey] DEFAULT ((0)) NOT NULL,
    [FtyInventoryUkey]    BIGINT          NULL,
    [Qty]                 NUMERIC (11, 2) CONSTRAINT [DF_Issue_Detail_Qty] DEFAULT ((0)) NULL,
    [MDivisionID]         VARCHAR (8)     CONSTRAINT [DF_Issue_Detail_MDivisionID] DEFAULT ('') NULL,
    [POID]                VARCHAR (13)    CONSTRAINT [DF_Issue_Detail_POID] DEFAULT ('') NULL,
    [Seq1]                VARCHAR (3)     CONSTRAINT [DF_Issue_Detail_Seq1] DEFAULT ('') NULL,
    [Seq2]                VARCHAR (2)     CONSTRAINT [DF_Issue_Detail_Seq2] DEFAULT ('') NULL,
    [Roll]                VARCHAR (8)     CONSTRAINT [DF_Issue_Detail_Roll] DEFAULT ('') NULL,
    [Dyelot]              VARCHAR (8)     CONSTRAINT [DF_Issue_Detail_Dyelot] DEFAULT ('') NULL,
    [StockType]           CHAR (1)        CONSTRAINT [DF_Issue_Detail_StockType] DEFAULT ('') NULL,
    [ukey]                BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BarcodeNo]           VARCHAR (13)    NULL,
    [OriQty]              NUMERIC (11, 2) NULL,
    [CompleteTime]        DATETIME        NULL,
    [IsQMS]               BIT             DEFAULT ((0)) NOT NULL,
    [SentToWMS]           BIT             DEFAULT ((0)) NOT NULL,
    [MINDReleaser]        VARCHAR (10)    CONSTRAINT [DF_Issue_Detail_MINDReleaser] DEFAULT ('') NOT NULL,
    [MINDReleaseDate]     DATETIME        NULL,
    [NeedUnroll]          BIT             CONSTRAINT [DF_Issue_Detail_NeedUnroll] DEFAULT ((0)) NOT NULL,
    [UnrollStatus]        VARCHAR (10)    CONSTRAINT [DF_Issue_Detail_UnrollStatus] DEFAULT ('') NOT NULL,
    [UnrollStartTime]     DATETIME        NULL,
    [UnrollEndTime]       DATETIME        NULL,
    [RelaxationStartTime] DATETIME        NULL,
    [RelaxationEndTime]   DATETIME        NULL,
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


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MIND��ڵo�ƤH��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'MINDReleaser';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MIND�o�Ʈɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'MINDReleaseDate';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�O�_��QMS�ͦ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'IsQMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫攤開布捲狀態
    Ongoing, Done', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'UnrollStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫攤開布捲開始時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'UnrollStartTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫攤開布捲完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'UnrollEndTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫鬆布開始時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'RelaxationStartTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫鬆布完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'RelaxationEndTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Confirm 單據時，立即確認該物料是否需要攤開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'NeedUnroll';

