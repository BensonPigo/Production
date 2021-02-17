CREATE TABLE [dbo].[AutomationTransRecord]
(
	[Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
	[CallFrom]        VARCHAR (80)    CONSTRAINT [DF_AutomationCreateRecord_CallFrom] DEFAULT ('') NOT NULL,
	[Activity]        VARCHAR (80)    CONSTRAINT [DF_AutomationCreateRecord_Activity] DEFAULT ('') NOT NULL,
    [SuppID]        VARCHAR (6)    CONSTRAINT [DF_AutomationCreateRecord_SuppID] DEFAULT ('') NOT NULL,
    [ModuleName]    VARCHAR (20)   CONSTRAINT [DF_AutomationCreateRecord_ModuleName] DEFAULT ('') NOT NULL,
    [SuppAPIThread] VARCHAR (50)   CONSTRAINT [DF_AutomationCreateRecord_SuppAPIThread] DEFAULT ('') NOT NULL,
    [JSON]          VARCHAR (MAX)  CONSTRAINT [DF_AutomationCreateRecord_JSON] DEFAULT ('') NOT NULL,
	[TransJson]          VARCHAR (MAX)  CONSTRAINT [DF_AutomationCreateRecord_TransJson] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_AutomationCreateRecord_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME       NULL,
    CONSTRAINT [PK_AutomationTransRecord] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)
