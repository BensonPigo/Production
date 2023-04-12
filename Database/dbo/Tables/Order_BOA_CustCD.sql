CREATE TABLE [dbo].[Order_BOA_CustCD] (
    [Id]            VARCHAR (13) CONSTRAINT [DF_Order_BOA_CustCD_Id] DEFAULT ('') NOT NULL,
    [Order_BOAUkey] BIGINT       CONSTRAINT [DF_Order_BOA_CustCD_Order_BOAUkey] DEFAULT ((0)) NOT NULL,
    [CustCDID]      VARCHAR (16) CONSTRAINT [DF_Order_BOA_CustCD_CustCDID] DEFAULT ('') NOT NULL,
    [Refno]         VARCHAR (36) CONSTRAINT [DF_Order_BOA_CustCD_Refno] DEFAULT ('') NOT NULL,
    [SCIRefno]      VARCHAR (30) CONSTRAINT [DF_Order_BOA_CustCD_SCIRefno] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Order_BOA_CustCD_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Order_BOA_CustCD_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
	[ColumnValue]   VARCHAR (50) NULL
    CONSTRAINT [PK_Order_BOA_CustCD] PRIMARY KEY CLUSTERED ([Order_BOAUkey] ASC, [ColumnValue] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : Bill of Accessory For CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BOA的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'Order_BOAUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶指定出口代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'CustCDID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'EditDate';

