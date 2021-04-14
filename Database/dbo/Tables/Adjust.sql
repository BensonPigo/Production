CREATE TABLE [dbo].[Adjust] (
    [ID]            VARCHAR (13)  CONSTRAINT [DF_Adjust_ID] DEFAULT ('') NOT NULL,
    [MDivisionID]   VARCHAR (8)   CONSTRAINT [DF_Adjust_MDivisionID] DEFAULT ('') NULL,
    [FactoryID]     VARCHAR (8)   CONSTRAINT [DF_Adjust_FactoryID] DEFAULT ('') NULL,
    [IssueDate]     DATE          NOT NULL,
    [Remark]        NVARCHAR (60) CONSTRAINT [DF_Adjust_Remark] DEFAULT ('') NULL,
    [Status]        VARCHAR (15)  CONSTRAINT [DF_Adjust_Status] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_Adjust_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME      NOT NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_Adjust_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [Type]          VARCHAR (1)   CONSTRAINT [DF_Adjust_Type] DEFAULT ('') NOT NULL,
    [StocktakingID] VARCHAR (13)  CONSTRAINT [DF_Adjust_StocktakingID] DEFAULT ('') NULL,
    [IsFromWMS]     BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Adjust] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存調整主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'Status';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'盤點單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'StocktakingID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust', @level2type = N'COLUMN', @level2name = N'MDivisionID';

