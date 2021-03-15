CREATE TABLE [dbo].[Order_SizeTol](
	[Id] [varchar](13) NOT NULL,
	[SizeGroup] [varchar](1) NOT NULL,
	[SizeItem] [varchar](3) NOT NULL,
	[Lower] [varchar](15) NULL,
	[Upper] [varchar](15) NULL,
 CONSTRAINT [PK_Order_SizeTol] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[SizeGroup] ASC,
	[SizeItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的左邊標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'項目編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸表單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'項目描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeDesc';


GO
CREATE NONCLUSTERED INDEX [IDX_SizeItem]
    ON [dbo].[Order_SizeItem]([Id] ASC, [SizeItem] ASC, [Ukey] ASC);

