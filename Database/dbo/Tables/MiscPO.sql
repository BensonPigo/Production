CREATE TABLE [dbo].[MiscPO] (
    [ID]           VARCHAR (13)    CONSTRAINT [DF_MiscPO_ID] DEFAULT ('') NOT NULL,
    [cDate]        DATE            NULL,
    [Factoryid]    VARCHAR (8)     CONSTRAINT [DF_MiscPO_Factoryid] DEFAULT ('') NULL,
    [PurchaseFrom] VARCHAR (1)     CONSTRAINT [DF_MiscPO_PurchaseFrom] DEFAULT ('') NULL,
    [Currencyid]   VARCHAR (3)     CONSTRAINT [DF_MiscPO_Currencyid] DEFAULT ('') NULL,
    [LocalSuppid]  VARCHAR (6)     CONSTRAINT [DF_MiscPO_LocalSuppid] DEFAULT ('') NULL,
    [Amount]       NUMERIC (12, 2) CONSTRAINT [DF_MiscPO_Amount] DEFAULT ((0)) NULL,
    [Vatrate]      NUMERIC (3, 1)  CONSTRAINT [DF_MiscPO_Vatrate] DEFAULT ((0)) NULL,
    [Vat]          NUMERIC (11, 2) CONSTRAINT [DF_MiscPO_Vat] DEFAULT ((0)) NULL,
    [Remark]       NVARCHAR (MAX)  CONSTRAINT [DF_MiscPO_Remark] DEFAULT ('') NULL,
    [Handle]       VARCHAR (10)    CONSTRAINT [DF_MiscPO_Handle] DEFAULT ('') NULL,
    [TranstoTPE]   DATETIME        NULL,
    [Approve]      VARCHAR (10)    CONSTRAINT [DF_MiscPO_Approve] DEFAULT ('') NULL,
    [ApproveDate]  DATETIME        NULL,
    [Status]       VARCHAR (15)    CONSTRAINT [DF_MiscPO_Status] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)    CONSTRAINT [DF_MiscPO_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME        NULL,
    [EditName]     VARCHAR (10)    CONSTRAINT [DF_MiscPO_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME        NULL,
    CONSTRAINT [PK_MiscPO] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous Purchase Order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'cDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Factoryid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購來源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'PurchaseFrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Currencyid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地採購廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'LocalSuppid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Vatrate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Vat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉回台北日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'TranstoTPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Approve';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'ApproveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO', @level2type = N'COLUMN', @level2name = N'EditDate';

