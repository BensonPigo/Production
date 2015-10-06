CREATE TABLE [dbo].[Scrap_Detail] (
    [Id]     VARCHAR (13)    CONSTRAINT [DF_Scrap_Detail_Id] DEFAULT ('') NOT NULL,
    [Poid]   VARCHAR (13)    CONSTRAINT [DF_Scrap_Detail_Poid] DEFAULT ('') NOT NULL,
    [Seq1]   VARCHAR (3)     CONSTRAINT [DF_Scrap_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]   VARCHAR (2)     CONSTRAINT [DF_Scrap_Detail_Seq2] DEFAULT ('') NOT NULL,
    [Roll]   VARCHAR (8)     CONSTRAINT [DF_Scrap_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot] VARCHAR (4)     CONSTRAINT [DF_Scrap_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [Qty]    NUMERIC (10, 2) CONSTRAINT [DF_Scrap_Detail_Qty] DEFAULT ((0)) NULL,
    [CtnNo]  VARCHAR (10)    CONSTRAINT [DF_Scrap_Detail_CtnNo] DEFAULT ('') NULL,
    CONSTRAINT [PK_Scrap_Detail] PRIMARY KEY CLUSTERED ([Id] ASC, [Poid] ASC, [Seq1] ASC, [Seq2] ASC, [Roll] ASC, [Dyelot] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉報廢明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報廢單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail', @level2type = N'COLUMN', @level2name = N'Poid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報廢箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scrap_Detail', @level2type = N'COLUMN', @level2name = N'CtnNo';

