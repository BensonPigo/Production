CREATE TABLE [dbo].[UserActionLog] (
    [UserLogPKey]    BIGINT        NOT NULL,
    [PKey]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [LoginID]        VARCHAR (10)  CONSTRAINT [DF_UserActionLog_LoginID] DEFAULT ('') NOT NULL,
    [SubMenuName]    VARCHAR (30)  CONSTRAINT [DF_UserActionLog_ModuleName] DEFAULT ('') NOT NULL,
    [FMName]         VARCHAR (80)  NOT NULL,
    [FMCaption]      VARCHAR (200) NOT NULL,
    [KeyFieldValues] VARCHAR (50)  CONSTRAINT [DF_UserActionLog_ID] DEFAULT ('') NOT NULL,
    [ActionTime]     DATETIME      NULL,
    [Action]         VARCHAR (20)  CONSTRAINT [DF_UserActionLog_Action] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_UserActionLog] PRIMARY KEY CLUSTERED ([PKey] ASC)
);

