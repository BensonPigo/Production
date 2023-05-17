CREATE TABLE [dbo].[Issue_Detail] (
    [ID]             VARCHAR (13)    NOT NULL,
    [Type]           VARCHAR (3)     NOT NULL,
    [CutplanID]      VARCHAR (13)    NOT NULL,
    [EstCutdate]     DATE            NULL,
    [SpreadingNoID]  VARCHAR (5)     NOT NULL,
    [PoId]           VARCHAR (13)    NOT NULL,
    [Seq1]           VARCHAR (3)     NOT NULL,
    [Seq2]           VARCHAR (2)     NOT NULL,
    [Roll]           VARCHAR (8)     NOT NULL,
    [Dyelot]         VARCHAR (8)     NOT NULL,
    [Barcode]        VARCHAR (13)    DEFAULT ('') NOT NULL,
    [Qty]            NUMERIC (11, 2) DEFAULT ((0)) NULL,
    [Ukey]           BIGINT          NOT NULL,
    [CmdTime]        DATETIME        NOT NULL,
    [GenSongUpdated] BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Issue_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GenSong是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'GenSongUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI寫入/更新此筆資料時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布捲條碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Barcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號二', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號一', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'PoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉布機桌號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'SpreadingNoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計裁剪日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'EstCutdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪計畫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'CutplanID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料來源P10 / P13 / P16', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Detail', @level2type = N'COLUMN', @level2name = N'ID';

