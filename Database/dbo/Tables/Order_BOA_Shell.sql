CREATE TABLE [dbo].[Order_BOA_Shell] (
    [Id]            VARCHAR (13) CONSTRAINT [DF_Order_BOA_Shell_Id] DEFAULT ('') NOT NULL,
    [order_BOAUkey] BIGINT       CONSTRAINT [DF_Order_BOA_Shell_order_BOAUkey] DEFAULT ((0)) NOT NULL,
    [Refno]         VARCHAR (20) CONSTRAINT [DF_Order_BOA_Shell_Refno] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Order_BOA_Shell_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Order_BOA_Shell_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    [Ukey]          BIGINT       CONSTRAINT [DF_Order_BOA_Shell_Ukey] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_BOA_Shell] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style - Bill of Accessory- Shell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BOA的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell', @level2type = N'COLUMN', @level2name = N'order_BOAUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_Shell', @level2type = N'COLUMN', @level2name = N'Ukey';

