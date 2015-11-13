CREATE TABLE [dbo].[Stocktaking] (
    [ID]          VARCHAR (13)  CONSTRAINT [DF_Stocktaking_ID] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)   CONSTRAINT [DF_Stocktaking_MDivisionID] DEFAULT ('') NOT NULL,
    [IssueDate]   DATE          NOT NULL,
    [Remark]      NVARCHAR (60) CONSTRAINT [DF_Stocktaking_Remark] DEFAULT ('') NULL,
    [Status]      VARCHAR (15)  CONSTRAINT [DF_Stocktaking_Status] DEFAULT ('') NOT NULL,
    [Type]        VARCHAR (1)   CONSTRAINT [DF_Stocktaking_Type] DEFAULT ('') NOT NULL,
    [AdjustId]    VARCHAR (13)  CONSTRAINT [DF_Stocktaking_AdjustId] DEFAULT ('') NULL,
    [Stocktype]   VARCHAR (1)   CONSTRAINT [DF_Stocktaking_Stocktype] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_Stocktaking_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_Stocktaking_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_Stocktaking] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫盤點主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'Status';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'AdjustId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'Stocktype';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Stocktaking', @level2type = N'COLUMN', @level2name = N'MDivisionID';

