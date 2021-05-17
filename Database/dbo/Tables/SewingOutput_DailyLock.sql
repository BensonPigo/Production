CREATE TABLE [dbo].[SewingOutput_DailyLock] (
    [Ukey]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [FactoryID]    VARCHAR (8)  CONSTRAINT [DF_SewingOutput_DailyLock_FactoryID] DEFAULT ('') NOT NULL,
    [LockDate]     DATE         NOT NULL,
    [LastLockName] VARCHAR (10) CONSTRAINT [DF_SewingOutput_DailyLock_LastLockName] DEFAULT ('') NOT NULL,
    [LastLockDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_SewingOutput_DailyLock] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

