CREATE TABLE [dbo].[AutomationCreateRecord] (
    [Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [SuppID]        VARCHAR (6)    CONSTRAINT [DF_AutomationCreateRecord_SuppID] DEFAULT ('') NOT NULL,
    [ModuleName]    VARCHAR (20)   CONSTRAINT [DF_AutomationCreateRecord_ModuleName] DEFAULT ('') NOT NULL,
    [APIThread]     VARCHAR (50)   CONSTRAINT [DF_AutomationCreateRecord_APIThread] DEFAULT ('') NULL,
    [SuppAPIThread] VARCHAR (50)   CONSTRAINT [DF_AutomationCreateRecord_SuppAPIThread] DEFAULT ('') NOT NULL,
    [JSON]          VARCHAR (MAX)  CONSTRAINT [DF_AutomationCreateRecord_JSON] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_AutomationCreateRecord_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_AutomationCreateRecord_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME       NULL,
    CONSTRAINT [PK_AutomationCreateRecord] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


