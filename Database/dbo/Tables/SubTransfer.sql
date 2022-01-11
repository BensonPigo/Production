CREATE TABLE [dbo].[SubTransfer] (
    [Id]          VARCHAR (13)   CONSTRAINT [DF_SubTransfer_Id] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)    CONSTRAINT [DF_SubTransfer_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]   VARCHAR (8)    CONSTRAINT [DF_SubTransfer_FactoryID] DEFAULT ('') NOT NULL,
    [Type]        VARCHAR (1)    CONSTRAINT [DF_SubTransfer_Type] DEFAULT ('') NOT NULL,
    [IssueDate]   DATE           NOT NULL,
    [Status]      VARCHAR (15)   CONSTRAINT [DF_SubTransfer_Status] DEFAULT ('') NOT NULL,
    [Remark]      NVARCHAR (100) CONSTRAINT [DF_SubTransfer_Remark] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_SubTransfer_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NOT NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_SubTransfer_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [POID]        VARCHAR (13)   DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SubTransfer] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉倉主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'Type';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
CREATE NONCLUSTERED INDEX [IDX_SubTransfer_IssueDate]
    ON [dbo].[SubTransfer]([IssueDate] ASC, [Status] ASC, [Type] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'關單的單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubTransfer', @level2type = N'COLUMN', @level2name = N'POID';

