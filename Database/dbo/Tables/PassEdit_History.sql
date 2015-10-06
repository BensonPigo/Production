CREATE TABLE [dbo].[PassEdit_History] (
    [PKey]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [ID]        VARCHAR (20)   CONSTRAINT [DF_PassEdit_History_ID] DEFAULT ('') NULL,
    [TableName] VARCHAR (20)   CONSTRAINT [DF_PassEdit_History_TableName] DEFAULT ('') NULL,
    [Remark]    NVARCHAR (500) CONSTRAINT [DF_PassEdit_History_Remark] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_PassEdit_History_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_PassEdit_History_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_PassEdit_History] PRIMARY KEY CLUSTERED ([PKey] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PassEdit_History]
    ON [dbo].[PassEdit_History]([PKey] ASC);

