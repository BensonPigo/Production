CREATE TABLE [dbo].[ShippingMarkPicture] (
    [BrandID]       VARCHAR (8)  NOT NULL,
    [CTNRefno]      VARCHAR (21) NOT NULL,
    [Side]          VARCHAR (5)  NOT NULL,
    [Seq]           INT          NOT NULL,
    [AddDate]       DATETIME     NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_ShippingMarkPicture_AddName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_ShippingMarkPicture_EditName] DEFAULT ('') NULL,
    [Is2Side]       BIT          DEFAULT ((0)) NULL,
    [IsHorizontal]  BIT          DEFAULT ((0)) NOT NULL,
    [IsSSCC]        BIT          DEFAULT ((0)) NOT NULL,
    [FromRight]     INT          DEFAULT ((0)) NOT NULL,
    [FromBottom]    INT          DEFAULT ((0)) NOT NULL,
    [StickerSizeID] BIGINT       DEFAULT ((0)) NOT NULL,
    [Junk]          BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ShippingMarkPicture] PRIMARY KEY CLUSTERED ([BrandID] ASC, [CTNRefno] ASC, [Side] ASC, [Seq] ASC)
);



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�W�U���k�e��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'Side';



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Ǹ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'Seq';



GO


GO


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�O�_�ਤ�K', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPicture', @level2type = N'COLUMN', @level2name = N'Is2Side';

