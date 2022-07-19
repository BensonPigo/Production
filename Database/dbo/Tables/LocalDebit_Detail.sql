CREATE TABLE [dbo].[LocalDebit_Detail] (
    [ID]           VARCHAR (13)    CONSTRAINT [DF_LocalDebit_Detail_ID] DEFAULT ('') NOT NULL,
    [Orderid]      VARCHAR (13)    CONSTRAINT [DF_LocalDebit_Detail_Orderid] DEFAULT ('') NULL,
    [UnitID]       VARCHAR (8)     CONSTRAINT [DF_LocalDebit_Detail_UnitID] DEFAULT ('') NULL,
    [Qty]          NUMERIC (11, 2) CONSTRAINT [DF_LocalDebit_Detail_Qty] DEFAULT ((0)) NULL,
    [Amount]       NUMERIC (12, 2) CONSTRAINT [DF_LocalDebit_Detail_Amount] DEFAULT ((0)) NULL,
    [Addition]     NUMERIC (12, 2) CONSTRAINT [DF_LocalDebit_Detail_Addition] DEFAULT ((0)) NULL,
    [Reasonid]     VARCHAR (5)     CONSTRAINT [DF_LocalDebit_Detail_Reasonid] DEFAULT ('') NULL,
    [Description]  NVARCHAR (MAX)  CONSTRAINT [DF_LocalDebit_Detail_Description] DEFAULT ('') NULL,
    [TaipeiReason] VARCHAR (100)   CONSTRAINT [DF_LocalDebit_Detail_TaipeiReason] DEFAULT ('') NULL,
    [Ukey]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [TaipeiUkey]   BIGINT          CONSTRAINT [DF_LocalDebit_Detail_TaipeiUkey] DEFAULT ((0)) NOT NULL,
    [AddName]      VARCHAR (10)    CONSTRAINT [DF_LocalDebit_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME        NULL,
    [EditName]     VARCHAR (10)    CONSTRAINT [DF_LocalDebit_Detail_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME        NULL,
    CONSTRAINT [PK_LocalDebit_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Ukey] ASC, [TaipeiUkey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LocalDebit_Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'Orderid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'其他金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'Addition';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'Reasonid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北扣款原因內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'TaipeiReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TaipeiUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'TaipeiUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';

