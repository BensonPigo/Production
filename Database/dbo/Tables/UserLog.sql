CREATE TABLE [dbo].[UserLog] (
    [PKey]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [LoginID]    VARCHAR (10)  CONSTRAINT [DF_UserLog_LoginID] DEFAULT ('') NULL,
    [ModuleName] VARCHAR (20)  CONSTRAINT [DF_UserLog_ModuleName] DEFAULT ('') NULL,
    [FMName]     VARCHAR (80)  CONSTRAINT [DF_UserLog_FMName] DEFAULT ('') NULL,
    [FMCaption]  NVARCHAR (50) CONSTRAINT [DF_UserLog_FMCaption] DEFAULT ('') NULL,
    [Opentime]   DATETIME      NULL,
    [Closetime]  DATETIME      NULL,
    [Usertimes]  INT           CONSTRAINT [DF_UserLog_Usertimes] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_UserLog] PRIMARY KEY CLUSTERED ([PKey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統基本設定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog', @level2type = N'COLUMN', @level2name = N'PKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog', @level2type = N'COLUMN', @level2name = N'LoginID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog', @level2type = N'COLUMN', @level2name = N'ModuleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog', @level2type = N'COLUMN', @level2name = N'FMName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog', @level2type = N'COLUMN', @level2name = N'FMCaption';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog', @level2type = N'COLUMN', @level2name = N'Opentime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog', @level2type = N'COLUMN', @level2name = N'Closetime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserLog', @level2type = N'COLUMN', @level2name = N'Usertimes';

