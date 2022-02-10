CREATE TABLE [dbo].[SemiFinishedLocationTrans_Detail] (
    [ID]           VARCHAR (13)    NOT NULL,
    [POID]         VARCHAR (13)    NOT NULL,
    [Seq]          VARCHAR (6)     NOT NULL,
    [Roll]         VARCHAR (8)     NOT NULL,
    [Dyelot]       VARCHAR (8)     NOT NULL,
    [Tone]         VARCHAR (8)     NOT NULL,
    [StockType]    CHAR (1)        NOT NULL,
    [Qty]          NUMERIC (11, 2) CONSTRAINT [DF_SemiFinishedLocationTrans_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [FromLocation] VARCHAR (60)    CONSTRAINT [DF_SemiFinishedLocationTrans_Detail_FromLocation] DEFAULT ('') NULL,
    [ToLocation]   VARCHAR (60)    CONSTRAINT [DF_SemiFinishedLocationTrans_Detail_ToLocation] DEFAULT ('') NULL,
    CONSTRAINT [PK_SemiFinishedLocationTrans_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [POID] ASC, [Seq] ASC, [Roll] ASC, [Dyelot] ASC, [Tone] ASC, [StockType] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整前庫存位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'ToLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整前庫存位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'FromLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別 (I, B, O)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色 Tone', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'Tone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'項次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位調整單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedLocationTrans_Detail', @level2type = N'COLUMN', @level2name = N'ID';

