CREATE TABLE [dbo].[TransImport] (
    [GroupID] INT            NOT NULL,
    [Seq]     INT            NOT NULL,
    [Name]    NVARCHAR (100) NULL,
    [TSQL]    NVARCHAR (500) NULL,
    [ImportConnectionName] VARCHAR(50) NOT NULL CONSTRAINT [DF_TransImport_ImportConnectionName] DEFAULT (''), 
    CONSTRAINT [PK_TransImport] PRIMARY KEY CLUSTERED ([GroupID] ASC, [Seq] ASC)
);


GO
