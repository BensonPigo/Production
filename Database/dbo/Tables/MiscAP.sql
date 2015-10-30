CREATE TABLE [dbo].[MiscAP] (
    [ID]          VARCHAR (13)    CONSTRAINT [DF_MiscAP_ID] DEFAULT ('') NOT NULL,
    [MDivisionid] VARCHAR (8)     CONSTRAINT [DF_MiscAP_MDivisionid] DEFAULT ('') NOT NULL,
    [FactoryID]   VARCHAR (8)     CONSTRAINT [DF_MiscAP_FactoryID] DEFAULT ('') NULL,
    [cDate]       DATE            NULL,
    [LocalSuppid] VARCHAR (6)     CONSTRAINT [DF_MiscAP_LocalSuppid] DEFAULT ('') NULL,
    [Currencyid]  VARCHAR (3)     CONSTRAINT [DF_MiscAP_Currencyid] DEFAULT ('') NULL,
    [Amount]      NUMERIC (12, 2) CONSTRAINT [DF_MiscAP_Amount] DEFAULT ((0)) NULL,
    [Vatrate]     NUMERIC (3, 1)  CONSTRAINT [DF_MiscAP_Vatrate] DEFAULT ((0)) NULL,
    [Vat]         NUMERIC (11, 2) CONSTRAINT [DF_MiscAP_Vat] DEFAULT ((0)) NULL,
    [PayTermID]   VARCHAR (8)     CONSTRAINT [DF_MiscAP_PayTermID] DEFAULT ('') NULL,
    [Invoice]     VARCHAR (100)   CONSTRAINT [DF_MiscAP_Invoice] DEFAULT ('') NULL,
    [Handle]      VARCHAR (10)    CONSTRAINT [DF_MiscAP_Handle] DEFAULT ('') NULL,
    [ApproveDate] DATE            NULL,
    [Approve]     VARCHAR (10)    CONSTRAINT [DF_MiscAP_Approve] DEFAULT ('') NULL,
    [TransID]     VARCHAR (16)    CONSTRAINT [DF_MiscAP_TransID] DEFAULT ('') NULL,
    [Remark]      NVARCHAR (60)   CONSTRAINT [DF_MiscAP_Remark] DEFAULT ('') NULL,
    [TransDate]   DATE            NULL,
    [Status]      VARCHAR (15)    CONSTRAINT [DF_MiscAP_Status] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)    CONSTRAINT [DF_MiscAP_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME        NULL,
    [EditName]    VARCHAR (10)    CONSTRAINT [DF_MiscAP_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME        NULL,
    CONSTRAINT [PK_MiscAP] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous Account Payable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'cDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'LocalSuppid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Currencyid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Vatrate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Vat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款條件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'PayTermID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Invoice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve 時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'ApproveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve 人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Approve';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'TransID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉傳票日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'TransDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAP', @level2type = N'COLUMN', @level2name = N'EditName';

