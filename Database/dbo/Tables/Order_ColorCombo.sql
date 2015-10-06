CREATE TABLE [dbo].[Order_ColorCombo] (
    [Id]           VARCHAR (13) CONSTRAINT [DF_Order_ColorCombo_Id] DEFAULT ('') NOT NULL,
    [Article]      VARCHAR (8)  CONSTRAINT [DF_Order_ColorCombo_Article] DEFAULT ('') NOT NULL,
    [ColorID]      VARCHAR (6)  CONSTRAINT [DF_Order_ColorCombo_ColorID] DEFAULT ('') NULL,
    [FabricCode]   VARCHAR (3)  CONSTRAINT [DF_Order_ColorCombo_FabricCode] DEFAULT ('') NULL,
    [LectraCode]   VARCHAR (2)  CONSTRAINT [DF_Order_ColorCombo_LectraCode] DEFAULT ('') NOT NULL,
    [PatternPanel] VARCHAR (2)  CONSTRAINT [DF_Order_ColorCombo_PatternPanel] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10) CONSTRAINT [DF_Order_ColorCombo_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME     NULL,
    [EditName]     VARCHAR (10) CONSTRAINT [DF_Order_ColorCombo_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME     NULL,
    CONSTRAINT [PK_Order_ColorCombo] PRIMARY KEY CLUSTERED ([Id] ASC, [Article] ASC, [LectraCode] ASC)
);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Order_ColorCombo]([PatternPanel] ASC)
    INCLUDE([Id], [Article], [ColorID], [LectraCode]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單- 配色表 Color Comb. (主料+副料-配色表)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_ColorCombo', @level2type = N'COLUMN', @level2name = N'EditDate';

