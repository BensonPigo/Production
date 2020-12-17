CREATE TABLE [dbo].[Order_FabricCode] (
    [Id]              VARCHAR (13)    CONSTRAINT [DF_Order_FabricCode_Id] DEFAULT ('') NOT NULL,
    [PatternPanel]    VARCHAR (2)     CONSTRAINT [DF_Order_FabricCode_PatternPanel] DEFAULT ('') NOT NULL,
    [FabricCode]      VARCHAR (3)     CONSTRAINT [DF_Order_FabricCode_FabricCode] DEFAULT ('') NOT NULL,
    [FabricPanelCode] VARCHAR (2)     CONSTRAINT [DF_Order_FabricCode_FabricPanelCode] DEFAULT ('') NOT NULL,
    [AddName]         VARCHAR (10)    CONSTRAINT [DF_Order_FabricCode_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME        NULL,
    [EditName]        VARCHAR (10)    CONSTRAINT [DF_Order_FabricCode_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME        NULL,
    [ConsPC]          NUMERIC (12, 4) NULL,
    CONSTRAINT [PK_Order_FabricCode] PRIMARY KEY CLUSTERED ([Id] ASC, [FabricPanelCode] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : Fabric Code (Bill Of Farbic)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode', @level2type = N'COLUMN', @level2name = N'EditDate';

