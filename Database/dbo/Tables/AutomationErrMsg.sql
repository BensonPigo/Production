CREATE TABLE [dbo].[AutomationErrMsg]
(
	[Ukey] BIGINT NOT NULL IDENTITY, 
    [SuppID] VARCHAR(6) NOT NULL CONSTRAINT [DF_AutomationErrMsg_SuppID] DEFAULT (''), 
	[ModuleName] VARCHAR(20) NOT NULL CONSTRAINT [DF_AutomationErrMsg_ModuleName] DEFAULT (''), 
    [APIThread] VARCHAR(30) NOT NULL CONSTRAINT [DF_AutomationErrMsg_APIThread] DEFAULT (''), 
	[SuppAPIThread] VARCHAR(50) NOT NULL CONSTRAINT [DF_AutomationErrMsg_SuppAPIThread] DEFAULT (''), 
    [ErrorCode] VARCHAR(3) NOT NULL CONSTRAINT [DF_AutomationErrMsg_ErrorCode] DEFAULT (''), 
    [ErrorMsg] VARCHAR(1000) NOT NULL CONSTRAINT [DF_AutomationErrMsg_ErrorMsg] DEFAULT (''), 
    [JSON] VARCHAR(MAX) NOT NULL CONSTRAINT [DF_AutomationErrMsg_JSON] DEFAULT (''), 
    [ReSented] BIT NOT NULL CONSTRAINT [DF_AutomationErrMsg_ReSented] DEFAULT (0), 
    [AddName] VARCHAR(10) NOT NULL CONSTRAINT [DF_AutomationErrMsg_AddName] DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL CONSTRAINT [DF_AutomationErrMsg_EditName] DEFAULT (''), 
    [EditDate] DATETIME NULL,
	CONSTRAINT [PK_AutomationErrMsg] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)
