CREATE TABLE [dbo].[ShippingMarkStamp_Detail] (
    [PackingListID]        VARCHAR (15)    NOT NULL,
    [SCICtnNo]             VARCHAR (16)    NOT NULL,
    [ShippingMarkTypeUkey] BIGINT          CONSTRAINT [DF_ShippingMarkStamp_Detail_ShippingMarkTypeUkey] DEFAULT ((0)) NOT NULL,
    [FilePath]             VARCHAR (150)   CONSTRAINT [DF_ShippingMarkStamp_Detail_FilePath] DEFAULT ('') NOT NULL,
    [FileName]             VARCHAR (30)    CONSTRAINT [DF_ShippingMarkStamp_Detail_FileName] DEFAULT ('') NOT NULL,
    [Image]                VARBINARY (MAX) NULL,
    [Side]                 VARCHAR (5)     CONSTRAINT [DF_ShippingMarkStamp_Detail_Side] DEFAULT ('') NOT NULL,
    [Seq]                  INT             CONSTRAINT [DF_ShippingMarkStamp_Detail_Seq] DEFAULT ((0)) NOT NULL,
    [FromRight]            NUMERIC (8, 2)  CONSTRAINT [DF_ShippingMarkStamp_Detail_FromRight] DEFAULT ((0)) NOT NULL,
    [FromBottom]           NUMERIC (8, 2)  CONSTRAINT [DF_ShippingMarkStamp_Detail_FromBottom] DEFAULT ((0)) NOT NULL,
    [Width]                INT             CONSTRAINT [DF_ShippingMarkStamp_Detail_Width] DEFAULT ((0)) NOT NULL,
    [Length]               INT             CONSTRAINT [DF_ShippingMarkStamp_Detail_Length] DEFAULT ((0)) NOT NULL,
    [CmdTime]              DATETIME        NOT NULL,
    [GenSongUpdated]       BIT             CONSTRAINT [DF_ShippingMarkStamp_Detail_GenSongUpdated] DEFAULT ((0)) NOT NULL,
    [Junk]                 BIT             CONSTRAINT [DF_ShippingMarkStamp_Detail_Junk] DEFAULT ((0)) NOT NULL,
    [DPI]                  INT             CONSTRAINT [DF_ShippingMarkStamp_Detail_DPI] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ShippingMarkStamp_Detail] PRIMARY KEY CLUSTERED ([PackingListID] ASC, [SCICtnNo] ASC, [ShippingMarkTypeUkey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉 HTML 的 DPI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'DPI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'判斷資料是否需要移除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GenSong是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'GenSongUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI寫入/更新此筆資料時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標籤長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'Length';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標籤寬度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離下面的位置(mm)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'FromBottom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離右邊的位置(mm)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'FromRight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼面, 上下左右前後', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BMP圖片二進位制資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'Image';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HTML 名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'FileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HTML 位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'FilePath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark 種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'ShippingMarkTypeUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'SCICtnNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝箱清單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp_Detail', @level2type = N'COLUMN', @level2name = N'PackingListID';

