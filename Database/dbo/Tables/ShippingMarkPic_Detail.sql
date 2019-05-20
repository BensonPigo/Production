CREATE TABLE [dbo].[ShippingMarkPic_Detail] (
    [ShippingMarkPicUkey] BIGINT         NOT NULL,
    [SCICtnNo]            VARCHAR (15)   NOT NULL,
    [FromLeft]            NUMERIC (8, 2) CONSTRAINT [DF_ShippingMarkPic_Detail_FromLeft] DEFAULT ((0)) NOT NULL,
    [FromTop]             NUMERIC (8, 2) CONSTRAINT [DF_ShippingMarkPic_Detail_FromTop] DEFAULT ((0)) NOT NULL,
    [PicLength]           NUMERIC (8, 2) CONSTRAINT [DF_ShippingMarkPic_Detail_PicLength] DEFAULT ((0)) NOT NULL,
    [PicWidth]            NUMERIC (8, 2) CONSTRAINT [DF_ShippingMarkPic_Detail_PicWidth] DEFAULT ((0)) NOT NULL,
    [Is2Side]             BIT            CONSTRAINT [DF_ShippingMarkPic_Detail_Is2Side] DEFAULT ((0)) NOT NULL,
    [FileName]            VARCHAR (30)   CONSTRAINT [DF_ShippingMarkPic_Detail_FileName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ShippingMarkPic_Detail] PRIMARY KEY CLUSTERED ([ShippingMarkPicUkey] ASC, [SCICtnNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCICtnNo+Seq+Side', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉角貼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'Is2Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼圖片寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'PicWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼圖片長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'PicLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離上面的位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FromTop';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離左邊的位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FromLeft';

