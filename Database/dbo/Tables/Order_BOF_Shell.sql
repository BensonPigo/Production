CREATE TABLE [dbo].[Order_BOF_Shell] (
    [Id]         VARCHAR (13) CONSTRAINT [DF_Order_BOF_Shell_Id] DEFAULT ('') NOT NULL,
	[Order_BOFUkey] BIGINT NOT NULL DEFAULT ((0)), 
    [BOFUkey_Shell] BIGINT NOT NULL DEFAULT ((0)), 
    [FabricCode] VARCHAR (3)  CONSTRAINT [DF_Order_BOF_Shell_FabricCode] DEFAULT ('') NOT NULL,
    [Refno]      VARCHAR (20) CONSTRAINT [DF_Order_BOF_Shell_Refno] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_Order_BOF_Shell_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_Order_BOF_Shell_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME     NULL,
    CONSTRAINT [PK_Order_BOF_Shell] PRIMARY KEY CLUSTERED ([Id], [BOFUkey_Shell], [Order_BOFUkey])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order- Bill of Fabric - Shell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Shell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Shell', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Shell', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Shell', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Shell', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Shell', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Shell', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF_Shell', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'BOF的唯一值',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_BOF_Shell',
    @level2type = N'COLUMN',
    @level2name = N'Order_BOFUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'對應的BOF Key',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_BOF_Shell',
    @level2type = N'COLUMN',
    @level2name = N'BOFUkey_Shell'