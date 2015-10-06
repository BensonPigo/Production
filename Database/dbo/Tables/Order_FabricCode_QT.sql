CREATE TABLE [dbo].[Order_FabricCode_QT] (
    [Id]           VARCHAR (13) CONSTRAINT [DF_Order_FabricCode_QT_Id] DEFAULT ('') NOT NULL,
    [FabricCode]   VARCHAR (3)  CONSTRAINT [DF_Order_FabricCode_QT_FabricCode] DEFAULT ('') NOT NULL,
    [LectraCode]   VARCHAR (2)  CONSTRAINT [DF_Order_FabricCode_QT_LectraCode] DEFAULT ('') NOT NULL,
    [SeqNO]        VARCHAR (2)  CONSTRAINT [DF_Order_FabricCode_QT_SeqNO] DEFAULT ('') NOT NULL,
    [QTFabricCode] VARCHAR (3)  CONSTRAINT [DF_Order_FabricCode_QT_QTFabricCode] DEFAULT ('') NOT NULL,
    [QTLectraCode] VARCHAR (2)  CONSTRAINT [DF_Order_FabricCode_QT_QTLectraCode] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10) CONSTRAINT [DF_Order_FabricCode_QT_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME     NULL,
    [EditName]     VARCHAR (10) CONSTRAINT [DF_Order_FabricCode_QT_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME     NULL,
    CONSTRAINT [PK_Order_FabricCode_QT] PRIMARY KEY CLUSTERED ([Id] ASC, [LectraCode] ASC, [SeqNO] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : 部位別的QT組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+布種的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'SeqNO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QT 布別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'QTFabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QT 布別+布種的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'QTLectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'EditDate';

