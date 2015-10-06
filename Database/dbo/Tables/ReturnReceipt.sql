CREATE TABLE [dbo].[ReturnReceipt] (
    [Id]           VARCHAR (13)  CONSTRAINT [DF_ReturnReceipt_Id] DEFAULT ('') NOT NULL,
    [IssueDate]    DATE          NULL,
    [FactoryId]    VARCHAR (8)   CONSTRAINT [DF_ReturnReceipt_FactoryId] DEFAULT ('') NULL,
    [Status]       VARCHAR (15)  CONSTRAINT [DF_ReturnReceipt_Status] DEFAULT ('') NULL,
    [WhseReasonId] VARCHAR (5)   CONSTRAINT [DF_ReturnReceipt_WhseReasonId] DEFAULT ('') NULL,
    [ActionID]     VARCHAR (5)   CONSTRAINT [DF_ReturnReceipt_ActionID] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (60) CONSTRAINT [DF_ReturnReceipt_Remark] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)  CONSTRAINT [DF_ReturnReceipt_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME      NULL,
    [EditName]     VARCHAR (10)  CONSTRAINT [DF_ReturnReceipt_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME      NULL,
    CONSTRAINT [PK_ReturnReceipt] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料退回主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退回原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'WhseReasonId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建議處置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'ActionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnReceipt', @level2type = N'COLUMN', @level2name = N'EditDate';

