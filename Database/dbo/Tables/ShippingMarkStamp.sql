CREATE TABLE [dbo].[ShippingMarkStamp] (
    [BrandID]       VARCHAR (8)  NOT NULL,
    [CTNRefno]      VARCHAR (21) NOT NULL,
    [Side]          VARCHAR (5)  NOT NULL,
    [StampLength]   INT          CONSTRAINT [DF_ShippingMarkStamp_StampLength] DEFAULT ((0)) NOT NULL,
    [StampWidth]    INT          CONSTRAINT [DF_ShippingMarkStamp_StampWidth] DEFAULT ((0)) NOT NULL,
    [FileName]      VARCHAR (40) CONSTRAINT [DF_ShippingMarkStamp_FileName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_ShippingMarkStamp_AddName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_ShippingMarkStamp_EditName] DEFAULT ('') NOT NULL,
    [FromRight]     INT          DEFAULT ((0)) NOT NULL,
    [FromBottom]    INT          DEFAULT ((0)) NOT NULL,
    [StickerSizeID] BIGINT       DEFAULT ((0)) NOT NULL,
    [Junk]          BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ShippingMarkStamp] PRIMARY KEY CLUSTERED ([BrandID] ASC, [CTNRefno] ASC, [Side] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Q�X�e', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp', @level2type = N'COLUMN', @level2name = N'StampWidth';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Q�X��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp', @level2type = N'COLUMN', @level2name = N'StampLength';




GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�W�U���k�e��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp', @level2type = N'COLUMN', @level2name = N'Side';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Q�X', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkStamp';



