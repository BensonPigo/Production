CREATE TABLE [dbo].[Order_SizeSpec] (
    [Id]       VARCHAR (13) CONSTRAINT [DF_Order_SizeSpec_Id] DEFAULT ('') NOT NULL,
    [SizeItem] VARCHAR (3)  CONSTRAINT [DF_Order_SizeSpec_SizeItem] DEFAULT ('') NOT NULL,
    [SizeCode] VARCHAR (8)  CONSTRAINT [DF_Order_SizeSpec_SizeCode] DEFAULT ('') NOT NULL,
    [SizeSpec] VARCHAR (15) CONSTRAINT [DF_Order_SizeSpec_SizeSpec] DEFAULT ('') NULL,
    [Ukey]     BIGINT       NULL, 
    CONSTRAINT [PK_Order_SizeSpec] PRIMARY KEY ([Id], [SizeCode], [SizeItem])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的value', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeSpec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeSpec', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'量法順序(橫欄)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'應對尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeSpec';


GO
CREATE NONCLUSTERED INDEX [IDX_SizeSpec]
    ON [dbo].[Order_SizeSpec]([Id] ASC, [SizeItem] ASC, [SizeCode] ASC, [Ukey] ASC);

