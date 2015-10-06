CREATE TABLE [dbo].[Stocktaking_Detail] (
    [Id]        VARCHAR (13)    CONSTRAINT [DF_Stocktaking_Detail_Id] DEFAULT ('') NOT NULL,
    [PoId]      VARCHAR (13)    CONSTRAINT [DF_Stocktaking_Detail_PoId] DEFAULT ('') NOT NULL,
    [Seq1]      VARCHAR (3)     CONSTRAINT [DF_Stocktaking_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]      VARCHAR (2)     CONSTRAINT [DF_Stocktaking_Detail_Seq2] DEFAULT ('') NOT NULL,
    [Roll]      VARCHAR (8)     CONSTRAINT [DF_Stocktaking_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]    VARCHAR (4)     CONSTRAINT [DF_Stocktaking_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [QtyBefore] NUMERIC (10, 2) CONSTRAINT [DF_Stocktaking_Detail_QtyBefore] DEFAULT ((0)) NULL,
    [QtyAfter]  NUMERIC (10, 2) CONSTRAINT [DF_Stocktaking_Detail_QtyAfter] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Stocktaking_Detail] PRIMARY KEY CLUSTERED ([Id] ASC, [PoId] ASC, [Seq1] ASC, [Seq2] ASC, [Roll] ASC, [Dyelot] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫盤點明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'PoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整前數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'QtyBefore';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking_Detail', @level2type = N'COLUMN', @level2name = N'QtyAfter';

