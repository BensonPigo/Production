CREATE TABLE [dbo].[CustBarCode] (
    [BrandID]  VARCHAR (8)  NOT NULL,
    [CustPONo] VARCHAR (30) NOT NULL,
    [StyleID]  VARCHAR (15) NOT NULL,
    [Article]  VARCHAR (8)  NOT NULL,
    [SizeCode] VARCHAR (8)  NOT NULL,
    [BarCode]  VARCHAR (30) NULL,
    [EditName] VARCHAR (10) NULL,
    [EditDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_CustBarCode] PRIMARY KEY CLUSTERED ([BrandID] ASC, [CustPONo] ASC, [StyleID] ASC, [Article] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustBarCode', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustBarCode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣條碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustBarCode', @level2type = N'COLUMN', @level2name = N'BarCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustBarCode', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustBarCode', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustBarCode', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustBarCode', @level2type = N'COLUMN', @level2name = N'CustPONo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustBarCode', @level2type = N'COLUMN', @level2name = N'BrandID';

