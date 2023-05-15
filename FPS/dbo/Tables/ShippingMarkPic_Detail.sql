CREATE TABLE [dbo].[ShippingMarkPic_Detail] (
    [PackingListID]        VARCHAR (15)    NOT NULL,
    [SCICtnNo]             VARCHAR (16)    NOT NULL,
    [Side]                 VARCHAR (5)     NOT NULL,
    [GensongUpdateTime]    VARCHAR (50)    CONSTRAINT [DF_ShippingMarkPic_Detail_GensongUpdateTime] DEFAULT ((0)) NOT NULL,
    [Seq]                  INT             NOT NULL,
    [FilePath]             VARCHAR (150)   NULL,
    [FileName]             VARCHAR (30)    NULL,
    [CmdTime]              DATETIME        NOT NULL,
    [SunriseUpdated]       BIT             CONSTRAINT [DF__ShippingM__Sunri__68D28DBC] DEFAULT ((0)) NOT NULL,
    [GenSongUpdated]       BIT             CONSTRAINT [DF__ShippingM__GenSo__69C6B1F5] DEFAULT ((0)) NOT NULL,
    [Image]                VARBINARY (MAX) NULL,
    [ShippingMarkTypeUkey] BIGINT          CONSTRAINT [DF__ShippingM__Shipp__6ABAD62E] DEFAULT ((0)) NOT NULL,
    [Is2Side]              BIT             CONSTRAINT [DF__ShippingM__Is2Si__6BAEFA67] DEFAULT ((0)) NOT NULL,
    [IsHorizontal]         BIT             CONSTRAINT [DF__ShippingM__IsHor__6CA31EA0] DEFAULT ((0)) NOT NULL,
    [IsSSCC]               BIT             CONSTRAINT [DF__ShippingM__IsSSC__6D9742D9] DEFAULT ((0)) NOT NULL,
    [FromRight]            NUMERIC (8, 2)  CONSTRAINT [DF__ShippingM__FromR__6E8B6712] DEFAULT ((0)) NOT NULL,
    [FromBottom]           NUMERIC (8, 2)  CONSTRAINT [DF__ShippingM__FromB__6F7F8B4B] DEFAULT ((0)) NOT NULL,
    [Width]                INT             CONSTRAINT [DF__ShippingM__Width__7073AF84] DEFAULT ((0)) NOT NULL,
    [Length]               INT             CONSTRAINT [DF__ShippingM__Lengt__7167D3BD] DEFAULT ((0)) NOT NULL,
    [Junk]                 BIT             CONSTRAINT [DF__ShippingMa__Junk__725BF7F6] DEFAULT ((0)) NOT NULL,
    [DPI]                  INT             CONSTRAINT [DF_ShippingMarkPic_Detail_DPI] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ShippingMarkPic_Detail] PRIMARY KEY CLUSTERED ([SCICtnNo] ASC, [PackingListID] ASC, [ShippingMarkTypeUkey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉 HTML 的 DPI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'DPI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片二進位制資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'Image';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GenSong是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'GenSongUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sunrise是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'SunriseUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI寫入/更新此筆資料時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖檔名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖檔位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FilePath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼面, 上下左右前後', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'SCICtnNo';

