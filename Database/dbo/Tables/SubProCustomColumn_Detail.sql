CREATE TABLE [dbo].[SubProCustomColumn_Detail] (
    [SubProcessID] VARCHAR (10)   NOT NULL,
    [AssignColumn] VARCHAR (13)   NOT NULL,
    [Description]  VARCHAR (50)   NOT NULL,
    [Remark]       NVARCHAR (500) NULL,
    [SubProCustomColumn_Detail] NVARCHAR(100) NULL, 
    CONSTRAINT [PK_SubProCustomColumn_Detail] PRIMARY KEY CLUSTERED ([SubProcessID] ASC, [AssignColumn] ASC, [Description] ASC)
);

