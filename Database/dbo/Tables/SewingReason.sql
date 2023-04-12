CREATE TABLE [dbo].[SewingReason]
(
	
	[ID]            VARCHAR(5)      CONSTRAINT [DF_SewingReason_ID]             DEFAULT ('')    NOT NULL, 
    [Type]          VARCHAR(2)      CONSTRAINT [DF_SewingReason_Type]           DEFAULT ('')    NOT NULL, 
    [Description]   NVARCHAR(60)    CONSTRAINT [DF_SewingReason_Description]    DEFAULT ('')    NOT NULL, 
    [Junk]          BIT             CONSTRAINT [DF_SewingReason_Junk]           NULL, 
    [AddName]       VARCHAR(10)     CONSTRAINT [DF_SewingReason_AddName]        DEFAULT ('')    NULL, 
    [AddDate]       DATETIME        CONSTRAINT [DF_SewingReason_AddDate]        NULL, 
    [EditName]      VARCHAR(10)     CONSTRAINT [DF_SewingReason_EditName]       DEFAULT ('')    Null, 
    [EditDate]  DATETIME        CONSTRAINT [DF_SewingReason_EditDate]           NULL, 
    [ForDQSCheck]   BIT             CONSTRAINT [DF_SewingReason_ForDQSCheck]    DEFAULT ((0))  NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'取消',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'Junk'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後編輯時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'