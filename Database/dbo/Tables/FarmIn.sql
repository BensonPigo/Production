CREATE TABLE [dbo].[FarmIn] (
    [ID]            VARCHAR (13)  CONSTRAINT [DF_FarmIn_ID] DEFAULT ('') NOT NULL,
    [FactoryId]     VARCHAR (8)   CONSTRAINT [DF_FarmIn_FactoryId] DEFAULT ('') NOT NULL,
    [IssueDate]     DATE          NOT NULL,
    [Handle]        VARCHAR (10)  CONSTRAINT [DF_FarmIn_Handle] DEFAULT ('') NOT NULL,
    [Status]        VARCHAR (15)  CONSTRAINT [DF_FarmIn_Status] DEFAULT ('') NULL,
    [Remark]        NVARCHAR (60) CONSTRAINT [DF_FarmIn_Remark] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_FarmIn_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_FarmIn_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [ArtworkTypeId] VARCHAR (20)  CONSTRAINT [DF_FarmIn_ArtworkTypeId] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_FarmIn] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工返回主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'返回單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmIn', @level2type = N'COLUMN', @level2name = N'ArtworkTypeId';

