CREATE TABLE [dbo].[AutomationCheckMsg] (
    [Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [SuppID]        VARCHAR (6)    CONSTRAINT [DF_AutomationCheckMsg_SuppID] DEFAULT ('') NOT NULL,
    [ModuleName]    VARCHAR (20)   CONSTRAINT [DF_AutomationCheckMsg_ModuleName] DEFAULT ('') NOT NULL,
    [APIThread]     VARCHAR (50)   CONSTRAINT [DF_AutomationCheckMsg_APIThread] DEFAULT ('') NULL,
    [SuppAPIThread] VARCHAR (50)   CONSTRAINT [DF_AutomationCheckMsg_SuppAPIThread] DEFAULT ('') NOT NULL,
    [ErrorCode]     VARCHAR (3)    CONSTRAINT [DF_AutomationCheckMsg_ErrorCode] DEFAULT ('') NOT NULL,
    [ErrorMsg]      VARCHAR (1000) CONSTRAINT [DF_AutomationCheckMsg_ErrorMsg] DEFAULT ('') NOT NULL,
    [JSON]          VARCHAR (MAX)  CONSTRAINT [DF_AutomationCheckMsg_JSON] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_AutomationCheckMsg_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_AutomationCheckMsg_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME       NULL,
    CONSTRAINT [PK_AutomationCheckMsg] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

