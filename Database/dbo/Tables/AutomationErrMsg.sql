CREATE TABLE [dbo].[AutomationErrMsg] (
    [Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [SuppID]        VARCHAR (6)    CONSTRAINT [DF_AutomationErrMsg_SuppID] DEFAULT ('') NOT NULL,
    [ModuleName]    VARCHAR (20)   CONSTRAINT [DF_AutomationErrMsg_ModuleName] DEFAULT ('') NOT NULL,
    [APIThread]     VARCHAR (50)   CONSTRAINT [DF_AutomationErrMsg_APIThread] DEFAULT ('') NULL,
    [SuppAPIThread] VARCHAR (50)   CONSTRAINT [DF_AutomationErrMsg_SuppAPIThread] DEFAULT ('') NOT NULL,
    [ErrorCode]     VARCHAR (3)    CONSTRAINT [DF_AutomationErrMsg_ErrorCode] DEFAULT ('') NOT NULL,
    [ErrorMsg]      VARCHAR (5000) CONSTRAINT [DF_AutomationErrMsg_ErrorMsg] DEFAULT ('') NULL,
    [JSON]          VARCHAR (MAX)  CONSTRAINT [DF_AutomationErrMsg_JSON] DEFAULT ('') NOT NULL,
    [ReSented]      BIT            CONSTRAINT [DF_AutomationErrMsg_ReSented] DEFAULT ((0)) NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_AutomationErrMsg_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_AutomationErrMsg_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME       NULL,
    CONSTRAINT [PK_AutomationErrMsg] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


