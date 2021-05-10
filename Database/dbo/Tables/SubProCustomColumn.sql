CREATE TABLE [dbo].[SubProCustomColumn] (
    [SubProcessID] VARCHAR (15) NOT NULL,
    [AssignColumn] VARCHAR (13) NOT NULL,
    [DisplayName]  VARCHAR (20) NOT NULL,
    [AddDate]      DATETIME     NULL,
    [AddName]      VARCHAR (10) NULL,
    [EditDate]     DATETIME     NULL,
    [Editname]     VARCHAR (10) NULL,
    [LocalDisplayName] NVARCHAR(100) NULL, 
    CONSTRAINT [PK_SubProCustomColumn] PRIMARY KEY CLUSTERED ([SubProcessID] ASC, [AssignColumn] ASC)
);

