CREATE TABLE [dbo].[SubProLocation] (
    [ID]                      VARCHAR (20)  NOT NULL,
    [SubProcessID] VARCHAR(10)  NOT NULL,
    [Junk] BIT NULL CONSTRAINT [DF_SubProLocation_Junk] DEFAULT (0), 
    [Description] NVARCHAR(500) NULL, 
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) NULL, 
    [EditDate] DATETIME NULL, 
    [Editname] VARCHAR(10) NULL, 
    CONSTRAINT [PK_SubProLocation] PRIMARY KEY CLUSTERED ([Id],[SubProcessID] ASC)
);


