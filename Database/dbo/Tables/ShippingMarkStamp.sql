CREATE TABLE [dbo].[ShippingMarkStamp] (
    [BrandID]     VARCHAR (8)    NOT NULL,
    [CustCD]      VARCHAR (16)   NOT NULL,
    [CTNRefno]    VARCHAR (21)   NOT NULL,
    [Side]        VARCHAR (5)    NOT NULL,
    [FromLeft]    NUMERIC (8, 2) CONSTRAINT [DF_ShippingMarkStamp_FromLeft] DEFAULT ((0)) NOT NULL,
    [FromTop]     NUMERIC (8, 2) CONSTRAINT [DF_ShippingMarkStamp_FromTop] DEFAULT ((0)) NOT NULL,
    [StampLength] NUMERIC (8, 2) CONSTRAINT [DF_ShippingMarkStamp_StampLength] DEFAULT ((0)) NOT NULL,
    [StampWidth]  NUMERIC (8, 2) CONSTRAINT [DF_ShippingMarkStamp_StampWidth] DEFAULT ((0)) NOT NULL,
    [FileName]    VARCHAR (25)   CONSTRAINT [DF_ShippingMarkStamp_FileName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ShippingMarkStamp] PRIMARY KEY CLUSTERED ([BrandID] ASC, [CustCD] ASC, [CTNRefno] ASC, [Side] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'噴碼寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp', @level2type = N'COLUMN', @level2name = N'StampWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'噴碼長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp', @level2type = N'COLUMN', @level2name = N'StampLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離上面的位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp', @level2type = N'COLUMN', @level2name = N'FromTop';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離左邊的位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp', @level2type = N'COLUMN', @level2name = N'FromLeft';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上下左右前後', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp', @level2type = N'COLUMN', @level2name = N'Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'噴碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp';

