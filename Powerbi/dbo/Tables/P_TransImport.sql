CREATE TABLE [dbo].[P_TransImport] (
    [GroupID]              INT            NOT NULL,
    [Seq]                  INT            NOT NULL,
    [Name]                 NVARCHAR (100) CONSTRAINT [DF_P_TransImport_Name] DEFAULT ('') NULL,
    [MailName]             NVARCHAR (100) CONSTRAINT [DF_P_TransImport_MailName] DEFAULT ('') NULL,
    [TSQL]                 NVARCHAR (500) CONSTRAINT [DF_P_TransImport_TSQL] DEFAULT ('') NULL,
    [ImportConnectionName] VARCHAR (50)   CONSTRAINT [DF_P_TransImport_ImportConnectionName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_P_TransImport] PRIMARY KEY CLUSTERED ([GroupID] ASC, [Seq] ASC)
);

