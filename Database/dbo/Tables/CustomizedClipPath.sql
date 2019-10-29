CREATE TABLE [dbo].[CustomizedClipPath] (
    [TableName] VARCHAR (50)  NOT NULL,
    [Path]      VARCHAR (100) NULL,
    [AddName]   VARCHAR (10)  NULL,
    [AddDate]   DATETIME      NULL,
    [EditName]  VARCHAR (10)  NULL,
    [EditDate]  DATETIME      NULL,
    CONSTRAINT [PK_CustomizedClipPath] PRIMARY KEY CLUSTERED ([TableName] ASC)
);

