CREATE TABLE [dbo].[Clip] (
    [PKey]        VARCHAR (12)  CONSTRAINT [DF_Clip_PKey] DEFAULT ('') NOT NULL,
    [TableName]   VARCHAR (50)  CONSTRAINT [DF_Clip_TableName] DEFAULT ('') NULL,
    [UniqueKey]   VARCHAR (80)  CONSTRAINT [DF_Clip_UniqueKey] DEFAULT ('') NULL,
    [SourceFile]  NVARCHAR (60) CONSTRAINT [DF_Clip_SourceFile] DEFAULT ('') NULL,
    [Description] NVARCHAR (60) CONSTRAINT [DF_Clip_Description] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_Clip_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    CONSTRAINT [PK_Clip] PRIMARY KEY CLUSTERED ([PKey] ASC)
);

