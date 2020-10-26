CREATE TABLE [dbo].[SubProResponseTeam] (
    [ID]           VARCHAR (50)   NOT NULL,
    [SubProcessID] VARCHAR (10)   NOT NULL,
    [Junk]         BIT            CONSTRAINT [DF_SubProResponseTeam_Junk] DEFAULT ((0)) NOT NULL,
    [Description]  NVARCHAR (500) NULL,
    [AddDate]      DATETIME       NULL,
    [AddName]      VARCHAR (10)   NULL,
    [EditDate]     DATETIME       NULL,
    [Editname]     VARCHAR (10)   NULL,
    CONSTRAINT [PK_SubProResponseTeam] PRIMARY KEY CLUSTERED ([ID] ASC, [SubProcessID] ASC)
);

