CREATE TABLE [dbo].[ShippingMarkPicture]
(
	[BrandID] VARCHAR(8) NOT NULL , 
    [CustCD] VARCHAR(16) NOT NULL, 
    [CTNRefno] VARCHAR(21) NOT NULL, 
    [Side] VARCHAR(5) NOT NULL, 
    [Seq] INT  NOT NULL, 
    [FromLeft] NUMERIC(8, 2) CONSTRAINT [DF_ShippingMarkPicture_FromLeft] DEFAULT (0) NULL, 
    [FromTop] NUMERIC(8, 2) CONSTRAINT [DF_ShippingMarkPicture_FromTop] DEFAULT (0) NULL, 
    [PicLength] NUMERIC(8, 2) CONSTRAINT [DF_ShippingMarkPicture_PicLength] DEFAULT (0) NULL, 
    [PicWidth] NUMERIC(8, 2) CONSTRAINT [DF_ShippingMarkPicture_PicWidth] DEFAULT (0) NULL, 
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_ShippingMarkPicture_AddName] DEFAULT ('') NULL, 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_ShippingMarkPicture_EditName] DEFAULT ('') NULL,
	[IsRotate] bit NOT NULL DEFAULT ((0)),
    CONSTRAINT [PK_ShippingMarkPicture] PRIMARY KEY CLUSTERED ([BrandID],[CustCD],[CTNRefno],[Side],[Seq] ASC)
)

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上下左右前後', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'Side';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'Seq';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離左邊的位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'FromLeft';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離上面的位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'FromTop';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'噴碼長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'PicLength';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'噴碼寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'PicWidth';


