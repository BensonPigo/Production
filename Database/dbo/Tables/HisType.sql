CREATE TABLE [dbo].[HisType] (
    [ModuleName]  VARCHAR (60)   CONSTRAINT [DF_HisType_ModuleName] DEFAULT ('') NOT NULL,
    [TableName]   VARCHAR (60)   CONSTRAINT [DF_HisType_TableName] DEFAULT ('') NOT NULL,
    [HisType]     NVARCHAR (60)  CONSTRAINT [DF_HisType_HisType] DEFAULT ('') NOT NULL,
    [IDName]      VARCHAR (30)   CONSTRAINT [DF_HisType_IDName] DEFAULT ('') NULL,
    [ColumnName]  VARCHAR (30)   CONSTRAINT [DF_HisType_ColumnName] DEFAULT ('') NULL,
    [ShowOld]     BIT            CONSTRAINT [DF_HisType_ShowOld] DEFAULT ((0)) NULL,
    [ShowReason]  BIT            CONSTRAINT [DF_HisType_ShowReason] DEFAULT ((0)) NULL,
    [ShowUpDate]  BIT            CONSTRAINT [DF_HisType_ShowUpDate] DEFAULT ((0)) NULL,
    [OldField]    NVARCHAR (50)  CONSTRAINT [DF_HisType_OldField] DEFAULT ('') NULL,
    [NewField]    NVARCHAR (50)  CONSTRAINT [DF_HisType_NewField] DEFAULT ('') NULL,
    [UpdateField] NVARCHAR (50)  CONSTRAINT [DF_HisType_UpdateField] DEFAULT ('') NULL,
    [Remark]      NVARCHAR (100) CONSTRAINT [DF_HisType_Remark] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [Editname]    VARCHAR (10)   CONSTRAINT [DF_HisType_Editname] DEFAULT ('') NULL,
    CONSTRAINT [PK_HisType] PRIMARY KEY CLUSTERED ([ModuleName] ASC, [TableName] ASC, [HisType] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修正人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'Editname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷史記錄畫面的設定.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'ModuleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料表名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'TableName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'HisType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID 欄位名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'IDName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寫入歷史區的欄位名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'ColumnName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顯示舊值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'ShowOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顯示原因代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'ShowReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顯示update', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'ShowUpDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'舊值顯示名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'OldField';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新值顯示名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'NewField';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'update 顯示名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'UpdateField';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修正日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HisType', @level2type = N'COLUMN', @level2name = N'EditDate';

