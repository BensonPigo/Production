CREATE TABLE [dbo].[ShippingMarkPic] (
    [PackingListID] VARCHAR (13) NOT NULL,
    [Seq]           INT          CONSTRAINT [DF_ShippingMarkPic_Seq] DEFAULT ((1)) NOT NULL,
    [Side]          VARCHAR (5)  NOT NULL,
    [Ukey]          BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ShippingMarkPic] PRIMARY KEY CLUSTERED ([PackingListID] ASC, [Seq] ASC, [Side] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上下左右前後', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic', @level2type = N'COLUMN', @level2name = N'Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'因一面可能貼到兩張,需Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic';

