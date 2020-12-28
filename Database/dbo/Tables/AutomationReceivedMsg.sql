CREATE TABLE [dbo].[AutomationReceivedMsg] (
    [Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [SuppID]        VARCHAR (6)    CONSTRAINT [DF_AutomationReceivedMsg_SuppID] DEFAULT ('') NOT NULL,
    [ModuleName]    VARCHAR (20)   CONSTRAINT [DF_AutomationReceivedMsg_ModuleName] DEFAULT ('') NOT NULL,
    [APIThread]     VARCHAR (50)   CONSTRAINT [DF_AutomationReceivedMsg_APIThread] DEFAULT ('') NULL,
    [SuppAPIThread] VARCHAR (100)  CONSTRAINT [DF_AutomationReceivedMsg_SuppAPIThread] DEFAULT ('') NULL,
    [ErrorCode]     VARCHAR (3)    CONSTRAINT [DF_AutomationReceivedMsg_ErrorCode] DEFAULT ('') NOT NULL,
    [ErrorMsg]      VARCHAR (1000) CONSTRAINT [DF_AutomationReceivedMsg_ErrorMsg] DEFAULT ('') NOT NULL,
    [JSON]          VARCHAR (MAX)  CONSTRAINT [DF_AutomationReceivedMsg_JSON] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_AutomationReceivedMsg_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME       NULL,
    [Success]       BIT            CONSTRAINT [DF_AutomationReceivedMsg_Success] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_AutomationReceivedMsg] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



