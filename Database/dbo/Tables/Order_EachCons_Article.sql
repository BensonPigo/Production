CREATE TABLE [dbo].[Order_EachCons_Article] (
    [Id]                 VARCHAR (13) CONSTRAINT [DF_Order_EachCons_Article_Id] DEFAULT ('') NOT NULL,
    [Order_EachConsUkey] BIGINT       CONSTRAINT [DF_Order_EachCons_Article_Order_EachConsUkey] DEFAULT ((0)) NOT NULL,
    [Article]            VARCHAR (8)  CONSTRAINT [DF_Order_EachCons_Article_Article] DEFAULT ('') NOT NULL,
    [AddName]            VARCHAR (10) CONSTRAINT [DF_Order_EachCons_Article_AddName] DEFAULT ('') NULL,
    [AddDate]            DATETIME     NULL,
    [EditName]           VARCHAR (10) CONSTRAINT [DF_Order_EachCons_Article_EditName] DEFAULT ('') NULL,
    [EditDate]           DATETIME     NULL,
    CONSTRAINT [PK_Order_EachCons_Article] PRIMARY KEY CLUSTERED ([Order_EachConsUkey] ASC, [Article] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EachCons PatternPanel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Article', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Each Cons 的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Article', @level2type = N'COLUMN', @level2name = N'Order_EachConsUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'　', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Article', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Article', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Article', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Article', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_Article', @level2type = N'COLUMN', @level2name = N'EditDate';

