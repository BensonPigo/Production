CREATE TABLE [dbo].[WHClose] (
    [POID]           VARCHAR (13) NOT NULL,
    [WhseClose]      DATE         NULL,
    [CmdTime]        DATETIME     NOT NULL,
    [GenSongUpdated] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WHClose] PRIMARY KEY CLUSTERED ([POID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GenSong是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHClose', @level2type = N'COLUMN', @level2name = N'GenSongUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI寫入/更新此筆資料時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHClose', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'關倉日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHClose', @level2type = N'COLUMN', @level2name = N'WhseClose';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHClose', @level2type = N'COLUMN', @level2name = N'POID';

