CREATE TABLE [dbo].[SewingTeam]
(
	[Id] VARCHAR(5) NOT NULL PRIMARY KEY, 
    [Description] VARCHAR(100) NULL DEFAULT (''), 
    [Junk] BIT NOT NULL, 
    [AddName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [EditDate] DATETIME NULL
)
