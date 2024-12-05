CREATE TABLE [dbo].[Clip] (
    [PKey]        VARCHAR (12)   CONSTRAINT [DF_Clip_PKey] DEFAULT ('') NOT NULL,
    [TableName]   VARCHAR (50)   CONSTRAINT [DF_Clip_TableName] DEFAULT ('') NOT NULL,
    [UniqueKey]   VARCHAR (200)   NOT NULL,
    [SourceFile]  NVARCHAR (532) CONSTRAINT [DF_Clip_SourceFile] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (120) CONSTRAINT [DF_Clip_Description] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_Clip_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    CONSTRAINT [PK_Clip] PRIMARY KEY CLUSTERED ([PKey] ASC, [UniqueKey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統基本設定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Clip';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Clip', @level2type = N'COLUMN', @level2name = N'UniqueKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Clip', @level2type = N'COLUMN', @level2name = N'TableName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Clip', @level2type = N'COLUMN', @level2name = N'SourceFile';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Clip', @level2type = N'COLUMN', @level2name = N'PKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Clip', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Clip', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Clip', @level2type = N'COLUMN', @level2name = N'AddDate';

