CREATE TABLE [dbo].[Order_EachCons_PatternPanel] (
    [Id]                 VARCHAR (13) CONSTRAINT [DF_Order_EachCons_PatternPanel_Id] DEFAULT ('') NOT NULL,
    [PatternPanel]       VARCHAR (2)  CONSTRAINT [DF_Order_EachCons_PatternPanel_PatternPanel] DEFAULT ('') NOT NULL,
    [Order_EachConsUkey] BIGINT       CONSTRAINT [DF_Order_EachCons_PatternPanel_Order_EachConsUkey] DEFAULT ((0)) NOT NULL,
    [LectraCode]         VARCHAR (2)  CONSTRAINT [DF_Order_EachCons_PatternPanel_LectraCode] DEFAULT ('') NOT NULL,
    [AddName]            VARCHAR (10) CONSTRAINT [DF_Order_EachCons_PatternPanel_AddName] DEFAULT ('') NULL,
    [AddDate]            DATETIME     NULL,
    [EditName]           VARCHAR (10) CONSTRAINT [DF_Order_EachCons_PatternPanel_EditName] DEFAULT ('') NULL,
    [EditDate]           DATETIME     NULL,
    CONSTRAINT [PK_Order_EachCons_PatternPanel] PRIMARY KEY CLUSTERED ([PatternPanel] ASC, [Order_EachConsUkey] ASC, [LectraCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EachCons PatternPanel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Each Cons 的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel', @level2type = N'COLUMN', @level2name = N'Order_EachConsUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel', @level2type = N'COLUMN', @level2name = N'LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons_PatternPanel', @level2type = N'COLUMN', @level2name = N'EditDate';

