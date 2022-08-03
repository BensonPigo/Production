CREATE TABLE [dbo].[Order_Article] (
    [id]                VARCHAR (13) CONSTRAINT [DF_Order_Article_id] DEFAULT ('') NOT NULL,
    [Seq]               SMALLINT     CONSTRAINT [DF_Order_Article_Seq] DEFAULT ((0)) NULL,
    [Article]           VARCHAR (8)  CONSTRAINT [DF_Order_Article_Article] DEFAULT ('') NOT NULL,
    [TissuePaper]       BIT          CONSTRAINT [DF_Order_Article_TissuePaper] DEFAULT ((0)) NULL,
    [CertificateNumber] VARCHAR (50) CONSTRAINT [DF_Order_Article_CertificateNumber] DEFAULT ('') NOT NULL,
    [SecurityCode]      VARCHAR (50) CONSTRAINT [DF_Order_Article_SecurityCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Order_Article] PRIMARY KEY CLUSTERED ([id] ASC, [Article] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Article', @level2type = N'COLUMN', @level2name = N'id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Article', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Article', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'棉紙', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Article', @level2type = N'COLUMN', @level2name = N'TissuePaper';

