CREATE TABLE [dbo].[ShippingMark] (
    [ID]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [BrandID]        VARCHAR (8)  DEFAULT ('') NOT NULL,
    [CTNRefno]       VARCHAR (21) DEFAULT ('') NOT NULL,
    [Side]           VARCHAR (5)  DEFAULT ('') NOT NULL,
    [Seq]            INT          DEFAULT ((0)) NOT NULL,
    [Category]       VARCHAR (4)  DEFAULT ('') NOT NULL,
    [FromRight]      INT          DEFAULT ((0)) NOT NULL,
    [FromBottom]     INT          DEFAULT ((0)) NOT NULL,
    [StickerSizeID]  BIGINT       DEFAULT ((0)) NOT NULL,
    [Is2Side]        BIT          DEFAULT ((0)) NOT NULL,
    [FileName]       VARCHAR (25) DEFAULT ('') NOT NULL,
    [CmdTime]        DATETIME     NOT NULL,
    [SunriseUpdated] BIT          DEFAULT ((0)) NOT NULL,
    [GenSongUpdated] BIT          DEFAULT ((0)) NOT NULL,
    [IsHorizontal]   BIT          DEFAULT ((0)) NOT NULL,
    [FilePath]       VARCHAR (80) DEFAULT ('') NOT NULL,
    [IsSSCC]         BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ShippingMark] PRIMARY KEY CLUSTERED ([ID] ASC, [BrandID] ASC, [CTNRefno] ASC, [Side] ASC, [Seq] ASC, [Category] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GenSong是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'GenSongUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sunrise是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'SunriseUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI寫入/更新此筆資料時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HTML檔名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'FileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否轉角貼, (0,1)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'Is2Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸貼紙ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'StickerSizeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離下面的位置(mm)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'FromBottom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離右邊的位置(mm)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'FromRight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別, 噴碼/貼碼(HTML/PIC)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼面, 上下左右前後', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'CTNRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMark', @level2type = N'COLUMN', @level2name = N'ID';

