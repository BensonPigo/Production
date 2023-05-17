CREATE TABLE [dbo].GASAClip (
    [PKey]        VARCHAR (12)  CONSTRAINT [DF_GASAClip_PKey] DEFAULT ('') NOT NULL,
    [TableName]   VARCHAR (50)  CONSTRAINT [DF_GASAClip_TableName] DEFAULT ('') NULL,
    [UniqueKey]   VARCHAR (80)  CONSTRAINT [DF_GASAClip_UniqueKey] DEFAULT ('') NOT NULL,
    [SourceFile]  NVARCHAR (266) CONSTRAINT [DF_GASAClip_SourceFile] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (60) CONSTRAINT [DF_GASAClip_Description] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_GASAClip_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL, 
    CONSTRAINT [PK_GASAClip] PRIMARY KEY ([PKey], [UniqueKey])
);

