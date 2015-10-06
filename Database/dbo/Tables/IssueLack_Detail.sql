CREATE TABLE [dbo].[IssueLack_Detail] (
    [Id]        VARCHAR (13)    CONSTRAINT [DF_IssueLack_Detail_Id] DEFAULT ('') NOT NULL,
    [Poid]      VARCHAR (13)    CONSTRAINT [DF_IssueLack_Detail_Poid] DEFAULT ('') NOT NULL,
    [Seq1]      VARCHAR (3)     CONSTRAINT [DF_IssueLack_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]      VARCHAR (2)     CONSTRAINT [DF_IssueLack_Detail_Seq2] DEFAULT ('') NOT NULL,
    [Roll]      VARCHAR (8)     CONSTRAINT [DF_IssueLack_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]    VARCHAR (4)     CONSTRAINT [DF_IssueLack_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [Qty]       NUMERIC (10, 2) CONSTRAINT [DF_IssueLack_Detail_Qty] DEFAULT ((0)) NULL,
    [StockType] VARCHAR (1)     CONSTRAINT [DF_IssueLack_Detail_StockType] DEFAULT ('') NULL,
    CONSTRAINT [PK_IssueLack_Detail] PRIMARY KEY CLUSTERED ([Id] ASC, [Poid] ASC, [Seq1] ASC, [Seq2] ASC, [Roll] ASC, [Dyelot] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缺補料發料明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Poid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'StockType';

