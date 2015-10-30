CREATE TABLE [dbo].[ThreadIncoming] (
    [ID]          VARCHAR (13)  CONSTRAINT [DF_ThreadIncoming_ID] DEFAULT ('') NOT NULL,
    [Cdate]       DATE          NOT NULL,
    [MDivisionid] VARCHAR (8)   CONSTRAINT [DF_ThreadIncoming_MDivisionid] DEFAULT ('') NOT NULL,
    [Remark]      NVARCHAR (60) CONSTRAINT [DF_ThreadIncoming_Remark] DEFAULT ('') NULL,
    [Status]      VARCHAR (15)  CONSTRAINT [DF_ThreadIncoming_Status] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_ThreadIncoming_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_ThreadIncoming_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_ThreadIncoming] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread In-coming', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線入庫單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming', @level2type = N'COLUMN', @level2name = N'Cdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming', @level2type = N'COLUMN', @level2name = N'EditDate';

