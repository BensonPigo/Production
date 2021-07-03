CREATE TABLE [dbo].[SemiFinishedIssue_Detail] (
    [ID]        VARCHAR (13)    NOT NULL,
    [POID]      VARCHAR (13)    NOT NULL,
    [Roll]      VARCHAR (8)     CONSTRAINT [DF_SemiFinishedIssue_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]    VARCHAR (8)     CONSTRAINT [DF_SemiFinishedIssue_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [StockType] CHAR (1)        CONSTRAINT [DF_SemiFinishedIssue_Detail_StockType] DEFAULT ('') NOT NULL,
    [Qty]       NUMERIC (11, 2) CONSTRAINT [DF_SemiFinishedIssue_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [Seq]       VARCHAR (6)     DEFAULT ('') NOT NULL,
    [Tone]      VARCHAR (8)     DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SemiFinishedIssue_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [POID] ASC, [Roll] ASC, [Dyelot] ASC, [StockType] ASC, [Seq] ASC, [Tone] ASC)
);



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedIssue_Detail', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedIssue_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedIssue_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedIssue_Detail', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedIssue_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedIssue_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品色 Tone', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedIssue_Detail', @level2type = N'COLUMN', @level2name = N'Tone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品項次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedIssue_Detail', @level2type = N'COLUMN', @level2name = N'Seq';

