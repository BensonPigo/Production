CREATE TABLE [dbo].[P_TransLog] (
    [ukey]         BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FunctionName] NVARCHAR (100) CONSTRAINT [DF_P_TransLog_FunctionName] DEFAULT (N'('')') NULL,
    [Description]  NVARCHAR (MAX) CONSTRAINT [DF_P_TransLog_Description] DEFAULT ('') NULL,
    [StartTime]    DATETIME       NULL,
    [EndTime]      DATETIME       NULL,
    [RegionID]     VARCHAR (50)   CONSTRAINT [DF_P_TransLog_RegionID] DEFAULT ('') NULL,
    [TransCode]    INT            CONSTRAINT [DF_P_TransLog_Succeeded] DEFAULT ((0)) NULL,
    [GroupID]      INT            CONSTRAINT [DF_P_TransLog_GroupID] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TransLog] PRIMARY KEY CLUSTERED ([ukey] ASC)
);

