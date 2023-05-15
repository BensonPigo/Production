CREATE TABLE [dbo].[Receiving_Detail] (
    [ID]             VARCHAR (13)    NOT NULL,
    [InvNo]          VARCHAR (25)    NOT NULL,
    [PoId]           VARCHAR (13)    NOT NULL,
    [Seq1]           VARCHAR (3)     NOT NULL,
    [Seq2]           VARCHAR (2)     NOT NULL,
    [Roll]           VARCHAR (8)     NOT NULL,
    [Refno]          VARCHAR (20)    NULL,
    [ColorID]        VARCHAR (6)     NULL,
    [Dyelot]         VARCHAR (8)     NOT NULL,
    [PoUnit]         VARCHAR (8)     DEFAULT ('') NULL,
    [ShipQty]        NUMERIC (11, 2) DEFAULT ((0)) NULL,
    [Weight]         NUMERIC (7, 2)  DEFAULT ((0)) NULL,
    [StockType]      VARCHAR (1)     DEFAULT ('') NULL,
    [Ukey]           BIGINT          NOT NULL,
    [IsInspection]   BIT             DEFAULT ((0)) NOT NULL,
    [Junk]           BIT             DEFAULT ((0)) NOT NULL,
    [CmdTime]        DATETIME        NOT NULL,
    [GenSongUpdated] BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Receiving_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GenSong是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'GenSongUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI寫入/更新此筆資料時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'被移除的資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否檢驗', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'IsInspection';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Weight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'PoUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號二', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號一', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'PoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船務出貨單號/提單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'ID';

