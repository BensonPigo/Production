CREATE TABLE [dbo].[SubTransfer_Detail] (
    [ID]             VARCHAR (13) NOT NULL,
    [Type]           VARCHAR (1)  NOT NULL,
    [FromPoId]       VARCHAR (13) NOT NULL,
    [FromSeq1]       VARCHAR (3)  NOT NULL,
    [FromSeq2]       VARCHAR (2)  NOT NULL,
    [FromRoll]       VARCHAR (8)  NOT NULL,
    [FromDyelot]     VARCHAR (8)  NOT NULL,
    [FromStockType]  VARCHAR (1)  DEFAULT ('') NULL,
    [ToPoId]         VARCHAR (13) NOT NULL,
    [ToSeq1]         VARCHAR (3)  NOT NULL,
    [ToSeq2]         VARCHAR (2)  NOT NULL,
    [ToRoll]         VARCHAR (8)  NOT NULL,
    [ToDyelot]       VARCHAR (8)  NOT NULL,
    [ToStockType]    VARCHAR (1)  DEFAULT ('') NULL,
    [Barcode]        VARCHAR (13) NOT NULL,
    [Ukey]           BIGINT       NOT NULL,
    [CmdTime]        DATETIME     NOT NULL,
    [GenSongUpdated] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SubTransfer_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GenSong是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'GenSongUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI寫入/更新此筆資料時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布捲條碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'Barcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToStockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToDyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToRoll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新序號二', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToSeq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新序號一', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToSeq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ToPoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromStockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromDyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromRoll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原序號二', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromSeq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原序號一', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromSeq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'FromPoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉倉類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉倉單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ID';

