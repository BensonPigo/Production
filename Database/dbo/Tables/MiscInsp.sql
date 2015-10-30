CREATE TABLE [dbo].[MiscInsp] (
    [ID]          VARCHAR (13)   CONSTRAINT [DF_MiscInsp_ID] DEFAULT ('') NOT NULL,
    [MDivisionId] VARCHAR (8)    CONSTRAINT [DF_MiscInsp_Factoryid] DEFAULT ('') NULL,
    [cDate]       DATE           NULL,
    [Remark]      NVARCHAR (MAX) CONSTRAINT [DF_MiscInsp_Remark] DEFAULT ('') NULL,
    [Handle]      VARCHAR (10)   CONSTRAINT [DF_MiscInsp_Handle] DEFAULT ('') NULL,
    [Status]      VARCHAR (15)   CONSTRAINT [DF_MiscInsp_Status] DEFAULT ('') NOT NULL,
    [MiscInID]    VARCHAR (13)   CONSTRAINT [DF_MiscInsp_MiscInID] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_MiscInsp_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_MiscInsp_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_MiscInsp] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inspect In-coming Miscellaneous', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'cDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'點收單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'MiscInID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp', @level2type = N'COLUMN', @level2name = N'MDivisionId';

