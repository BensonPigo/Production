CREATE TABLE [dbo].[AutomationDisplay] (
    [Ukey]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [SuppID]        VARCHAR (6)  CONSTRAINT [DF_AutomationDisplay_SuppID] DEFAULT ('') NULL,
    [ModuleName]    VARCHAR (20) CONSTRAINT [DF_AutomationDisplay_ModuleName] DEFAULT ('') NULL,
    [SuppAPIThread] VARCHAR (50) NULL,
    CONSTRAINT [PK_AutomationDisplay] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用來顯示對應的Module 名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AutomationDisplay', @level2type = N'COLUMN', @level2name = N'ModuleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用來顯示對應的SUPP 名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AutomationDisplay', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N' 流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AutomationDisplay', @level2type = N'COLUMN', @level2name = N'Ukey';

