CREATE TABLE [dbo].[BorrowBack] (
    [Id]           VARCHAR (13)   CONSTRAINT [DF_BorrowBack_Id] DEFAULT ('') NOT NULL,
    [MDivisionID]  VARCHAR (8)    CONSTRAINT [DF_BorrowBack_MDivisionID] DEFAULT ('') NULL,
    [FactoryID]    VARCHAR (8)    NULL,
    [Type]         VARCHAR (1)    CONSTRAINT [DF_BorrowBack_Type] DEFAULT ('') NULL,
    [EstBackDate]  DATE           NULL,
    [BackDate]     DATE           NULL,
    [BorrowId]     VARCHAR (13)   CONSTRAINT [DF_BorrowBack_BorrowId] DEFAULT ('') NULL,
    [IssueDate]    DATE           NOT NULL,
    [Status]       VARCHAR (15)   CONSTRAINT [DF_BorrowBack_Status] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (100) CONSTRAINT [DF_BorrowBack_Remark] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_BorrowBack_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_BorrowBack_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL,
    [SewingLineID] VARCHAR (5)    CONSTRAINT [DF_BorrowBack_SewingLineID] DEFAULT ('') NOT NULL,
    [DepartmentID] VARCHAR (8)    CONSTRAINT [DF_BorrowBack_DepartmentID] DEFAULT ('') NOT NULL,
    [Shift]        VARCHAR (1)    CONSTRAINT [DF_BorrowBack_Shift] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_BorrowBack] PRIMARY KEY CLUSTERED ([Id] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'借還料主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'借還單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'Type';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計歸還日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'EstBackDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際歸還日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'BackDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'借料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'BorrowId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BorrowBack', @level2type = N'COLUMN', @level2name = N'MDivisionID';

