CREATE TABLE [dbo].[ShippingMarkPic_Detail] (
    [SCICtnNo]       VARCHAR (15) NOT NULL,
    [Side]           VARCHAR (5)  NOT NULL,
    [Seq]            INT          NOT NULL,
    [FilePath]       VARCHAR (80) NULL,
    [FileName]       VARCHAR (30) NULL,
    [CmdTime]        DATETIME     NOT NULL,
    [SunriseUpdated] BIT          DEFAULT ((0)) NOT NULL,
    [GenSongUpdated] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ShippingMarkPic_Detail] PRIMARY KEY CLUSTERED ([SCICtnNo] ASC, [Side] ASC, [Seq] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCICtnNo+Seq+Side', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'SCICtnNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sunrise是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'SunriseUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼面, 上下左右前後', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GenSong是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'GenSongUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖檔位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FilePath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI寫入/更新此筆資料時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'CmdTime';

