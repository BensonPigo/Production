CREATE TABLE [dbo].[Order_MarkerList_Article] (
    [Id]                   VARCHAR (13) CONSTRAINT [DF_Order_MarkerList_Article_Id] DEFAULT ('') NOT NULL,
    [Order_MarkerlistUkey] BIGINT       CONSTRAINT [DF_Order_MarkerList_Article_Order_MarkerlistUkey] DEFAULT ((0)) NOT NULL,
    [Article]              VARCHAR (8)  CONSTRAINT [DF_Order_MarkerList_Article_Article] DEFAULT ('') NOT NULL,
    [AddName]              VARCHAR (10) CONSTRAINT [DF_Order_MarkerList_Article_AddName] DEFAULT ('') NULL,
    [AddDate]              DATETIME     NULL,
    [EditName]             VARCHAR (10) CONSTRAINT [DF_Order_MarkerList_Article_EditName] DEFAULT ('') NULL,
    [EditDate]             DATETIME     NULL,
    CONSTRAINT [PK_Order_MarkerList_Article] PRIMARY KEY CLUSTERED ([Order_MarkerlistUkey] ASC, [Article] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MarkerList PatternPanel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_Article', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_Article', @level2type = N'COLUMN', @level2name = N'Order_MarkerlistUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_Article', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_Article', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_Article', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_Article', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList_Article', @level2type = N'COLUMN', @level2name = N'EditDate';

