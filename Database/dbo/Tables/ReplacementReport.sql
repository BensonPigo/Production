CREATE TABLE [dbo].[ReplacementReport] (
    [ID]                  VARCHAR (13)    CONSTRAINT [DF_ReplacementReport_ID] DEFAULT ('') NOT NULL,
    [CDate]               DATE            NOT NULL,
    [POID]                VARCHAR (13)    CONSTRAINT [DF_ReplacementReport_POID] DEFAULT ('') NOT NULL,
    [Type]                VARCHAR (1)     CONSTRAINT [DF_ReplacementReport_Type] DEFAULT ('') NOT NULL,
    [MDivisionID]         VARCHAR (8)     CONSTRAINT [DF_ReplacementReport_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]           VARCHAR (8)     CONSTRAINT [DF_ReplacementReport_FactoryID] DEFAULT ('') NOT NULL,
    [ApplyName]           VARCHAR (10)    CONSTRAINT [DF_ReplacementReport_ApplyName] DEFAULT ('') NOT NULL,
    [ApplyDate]           DATETIME        NULL,
    [ApvName]             VARCHAR (10)    CONSTRAINT [DF_ReplacementReport_ApvName] DEFAULT ('') NOT NULL,
    [ApvDate]             DATETIME        NULL,
    [TPECFMName]          VARCHAR (10)    CONSTRAINT [DF_ReplacementReport_TPECFMName] DEFAULT ('') NOT NULL,
    [TPECFMDate]          DATETIME        NULL,
    [Status]              VARCHAR (15)    CONSTRAINT [DF_ReplacementReport_Status] DEFAULT ('') NOT NULL,
    [ExportToTPE]         DATETIME        NULL,
    [AddName]             VARCHAR (10)    CONSTRAINT [DF_ReplacementReport_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME        NULL,
    [EditName]            VARCHAR (10)    CONSTRAINT [DF_ReplacementReport_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME        NULL,
    [TPEEditName]         VARCHAR (10)    CONSTRAINT [DF_ReplacementReport_TPEEditName] DEFAULT ('') NOT NULL,
    [TPEEditDate]         DATETIME        NULL,
    [Responsibility]      VARCHAR (1)     CONSTRAINT [DF_ReplacementReport_Responsibility] DEFAULT ('') NOT NULL,
    [SendToTrade]         BIT             CONSTRAINT [DF_ReplacementReport_SendToTrade] DEFAULT ((0)) NOT NULL,
    [RMtlAmt]             NUMERIC (11, 2) CONSTRAINT [DF__Replaceme__RMtlA__08683914] DEFAULT ((0)) NOT NULL,
    [ActFreight]          NUMERIC (10, 2) CONSTRAINT [DF__Replaceme__ActFr__095C5D4D] DEFAULT ((0)) NOT NULL,
    [EstFreight]          NUMERIC (10, 2) CONSTRAINT [DF__Replaceme__EstFr__0A508186] DEFAULT ((0)) NOT NULL,
    [SurchargeAmt]        NUMERIC (10, 2) CONSTRAINT [DF__Replaceme__Surch__0B44A5BF] DEFAULT ((0)) NOT NULL,
    [VoucherID]           VARCHAR (16)    CONSTRAINT [DF__Replaceme__Vouch__0C38C9F8] DEFAULT ('') NOT NULL,
    [VoucherDate]         DATE            NULL,
    [LockDate]            DATE            NULL,
    [CompleteDate]        DATE            NULL,
    [RespDeptConfirmDate] DATETIME        NULL,
    [RespDeptConfirmName] VARCHAR (10)    CONSTRAINT [DF__Replaceme__RespD__4393EBDA] DEFAULT ('') NOT NULL,
    [TransferName]        VARCHAR (10)    CONSTRAINT [DF_ReplacementReport_TransferName] DEFAULT ('') NOT NULL,
    [TransferDate]        DATETIME        NULL,
    [TransferResponsible] VARCHAR (1)     CONSTRAINT [DF__Replaceme__Trans__31403175] DEFAULT ('') NOT NULL,
    [TransferNo]          VARCHAR (13)    CONSTRAINT [DF_ReplacementReport_TransferNo] DEFAULT ('') NOT NULL,
    [CompleteName]        VARCHAR (10)    DEFAULT ('') NOT NULL,
    [isComplete]          BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReplacementReport] PRIMARY KEY CLUSTERED ([ID] ASC)
);


















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Replacement Report', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補料報告單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補料報告申請日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'CDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'ApplyName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'ApplyDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠核准人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠核准日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Taipei Confirm 人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'TPECFMName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Taipei Confirm 日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'TPECFMDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料轉出時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'ExportToTPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'TPEEditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'TPEEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport', @level2type = N'COLUMN', @level2name = N'MDivisionID';

