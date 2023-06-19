CREATE TABLE [dbo].GASAClip (
    [PKey]        VARCHAR (12)  CONSTRAINT [DF_GASAClip_PKey] DEFAULT ('') NOT NULL,
    [TableName]   VARCHAR (50)  CONSTRAINT [DF_GASAClip_TableName] DEFAULT ('') NULL,
    [UniqueKey]   NVARCHAR(200)  CONSTRAINT [DF_GASAClip_UniqueKey] DEFAULT ('') NOT NULL,
    [SourceFile]  NVARCHAR (266) CONSTRAINT [DF_GASAClip_SourceFile] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (60) CONSTRAINT [DF_GASAClip_Description] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_GASAClip_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL, 
    [FactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_GASAClip] PRIMARY KEY ([PKey], [UniqueKey])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'分辨是哪間工廠上傳',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GASAClip',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'