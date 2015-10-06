CREATE TABLE [dbo].[Bundle_Detail_qty] (
    [ID]       BIGINT      CONSTRAINT [DF_Bundle_Detail_qty_ID] DEFAULT ((0)) NOT NULL,
    [SizeCode] VARCHAR (8) CONSTRAINT [DF_Bundle_Detail_qty_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]      NUMERIC (5) CONSTRAINT [DF_Bundle_Detail_qty_Qty] DEFAULT ((0)) NOT NULL,
    [Iden]     BIGINT      IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_Bundle_Detail_qty] PRIMARY KEY CLUSTERED ([Iden] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每一個Bundle Card 的Qty, Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_qty', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_qty', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_qty', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_qty', @level2type = N'COLUMN', @level2name = N'Iden';

