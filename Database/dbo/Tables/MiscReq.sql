CREATE TABLE [dbo].[MiscReq] (
    [ID]           VARCHAR (13)   CONSTRAINT [DF_MiscReq_ID] DEFAULT ('') NOT NULL,
    [cDate]        DATE           NULL,
    [FactoryID]    VARCHAR (8)    CONSTRAINT [DF_MiscReq_FactoryID] DEFAULT ('') NULL,
    [DepartmentID] VARCHAR (8)    CONSTRAINT [DF_MiscReq_DepartmentID] DEFAULT ('') NULL,
    [PurchaseFrom] VARCHAR (1)    CONSTRAINT [DF_MiscReq_PurchaseFrom] DEFAULT ('') NULL,
    [Handle]       VARCHAR (10)   CONSTRAINT [DF_MiscReq_Handle] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (MAX) CONSTRAINT [DF_MiscReq_Remark] DEFAULT ('') NULL,
    [Approve]      VARCHAR (10)   CONSTRAINT [DF_MiscReq_Approve] DEFAULT ('') NULL,
    [ApproveDate]  DATETIME       NULL,
    [Status]       VARCHAR (15)   CONSTRAINT [DF_MiscReq_Status] DEFAULT ('') NULL,
    [PurchaseType] VARCHAR (20)   CONSTRAINT [DF_MiscReq_PurchaseType] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_MiscReq_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_MiscReq_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL,
    CONSTRAINT [PK_MiscReq] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous Requisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'cDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部門別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'DepartmentID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購來源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'PurchaseFrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'Approve';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'ApproveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'PurchaseType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq', @level2type = N'COLUMN', @level2name = N'EditDate';

