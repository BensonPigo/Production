CREATE TABLE [dbo].[FarmOut] (
    [Id]            VARCHAR (13)  CONSTRAINT [DF_FarmOut_Id] DEFAULT ('') NOT NULL,
    [MDivisionID]   VARCHAR (8)   CONSTRAINT [DF_FarmOut_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryId]     VARCHAR (8)   CONSTRAINT [DF_FarmOut_FactoryId] DEFAULT ('') NOT NULL,
    [IssueDate]     DATE          NOT NULL,
    [Remark]        NVARCHAR (60) CONSTRAINT [DF_FarmOut_Remark] DEFAULT ('') NULL,
    [Status]        VARCHAR (15)  CONSTRAINT [DF_FarmOut_Status] DEFAULT ('') NULL,
    [Handle]        VARCHAR (10)  CONSTRAINT [DF_FarmOut_Handle] DEFAULT ('') NOT NULL,
    [TotalQty]      NUMERIC (7)   CONSTRAINT [DF_FarmOut_TotalQty] DEFAULT ((0)) NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_FarmOut_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_FarmOut_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [ArtworkTypeId] VARCHAR (20)  CONSTRAINT [DF_FarmOut_ArtworkTypeId] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_FarmOut] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工發放主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發放單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發放總數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'TotalQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'ArtworkTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FarmOut', @level2type = N'COLUMN', @level2name = N'MDivisionID';

