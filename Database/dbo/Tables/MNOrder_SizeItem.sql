CREATE TABLE [dbo].[MNOrder_SizeItem] (
    [ID]       VARCHAR (13)   CONSTRAINT [DF_MNOrder_SizeItem_ID] DEFAULT ('') NOT NULL,
    [SizeItem] VARCHAR (3)    CONSTRAINT [DF_MNOrder_SizeItem_SizeItem] DEFAULT ('') NOT NULL,
    [SizeDesc] NVARCHAR (100) CONSTRAINT [DF_MNOrder_SizeItem_SizeDesc] DEFAULT ('') NULL,
    [SizeUnit] VARCHAR (6)    CONSTRAINT [DF_MNOrder_SizeItem_SizeUnit] DEFAULT ('') NULL,
    CONSTRAINT [PK_MNOrder_SizeItem] PRIMARY KEY CLUSTERED ([ID] ASC, [SizeItem] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的左邊標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeItem', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'項目編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'項目描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeItem', @level2type = N'COLUMN', @level2name = N'SizeUnit';

