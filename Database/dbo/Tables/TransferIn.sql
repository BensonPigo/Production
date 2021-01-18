CREATE TABLE [dbo].[TransferIn] (
    [Id]          VARCHAR (13)   CONSTRAINT [DF_TransferIn_Id] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)    CONSTRAINT [DF_TransferIn_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]   VARCHAR (8)    NULL,
    [IssueDate]   DATE           NULL,
    [Status]      VARCHAR (15)   CONSTRAINT [DF_TransferIn_Status] DEFAULT ('') NULL,
    [Remark]      NVARCHAR (100) CONSTRAINT [DF_TransferIn_Remark] DEFAULT ('') NULL,
    [FromFtyID]   VARCHAR (8)    CONSTRAINT [DF_TransferIn_FromFtyID] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_TransferIn_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_TransferIn_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [InvNo] VARCHAR(25) NULL DEFAULT (''), 
    [Packages] NUMERIC(5) NULL, 
    CONSTRAINT [PK_TransferIn] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠入主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'FromFtyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferIn', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'包裹數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferIn',
    @level2type = N'COLUMN',
    @level2name = N'Packages'