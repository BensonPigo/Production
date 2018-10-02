CREATE TABLE [dbo].[ThreadIssue] (
    [id]          VARCHAR (13)  CONSTRAINT [DF_ThreadIssue_id] DEFAULT ('') NOT NULL,
    [MDivisionId] VARCHAR (8)   CONSTRAINT [DF_ThreadIssue_MDivisionId] DEFAULT ('') NOT NULL,
    [CDate]       DATE          NOT NULL,
    [Remark]      NVARCHAR (60) CONSTRAINT [DF_ThreadIssue_Remark] DEFAULT ('') NULL,
    [Status]      VARCHAR (15)  CONSTRAINT [DF_ThreadIssue_Status] DEFAULT ('') NULL,
	[RequestID]      VARCHAR (13)  CONSTRAINT [DF_ThreadIssue_RequestID] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_ThreadIssue_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_ThreadIssue_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_ThreadIssue] PRIMARY KEY CLUSTERED ([id] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Issue', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue', @level2type = N'COLUMN', @level2name = N'CDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue', @level2type = N'COLUMN', @level2name = N'EditDate';

