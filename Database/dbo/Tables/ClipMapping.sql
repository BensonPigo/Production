CREATE TABLE [dbo].[ClipMapping] (
    [NewTableName] VARCHAR (50) CONSTRAINT [DF_ClipMapping_NewTableName] DEFAULT ('') NOT NULL,
    [OldTableName] VARCHAR (30) CONSTRAINT [DF_ClipMapping_OldTableName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ClipMapping] PRIMARY KEY CLUSTERED ([NewTableName] ASC)
);

