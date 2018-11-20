CREATE TABLE [dbo].[AVO] (
    [ID]             VARCHAR (13)  NOT NULL,
    [cDate]          DATE          NOT NULL,
    [MDivisionID]    VARCHAR (8)   NOT NULL,
    [Handle]         VARCHAR (10)  NOT NULL,
    [Remark]         VARCHAR (100) DEFAULT ('') NOT NULL,
    [PPICSupApvName] VARCHAR (10)  DEFAULT ('') NOT NULL,
    [PPICSupApvDate] DATETIME      NULL,
    [PPDApvName]     VARCHAR (10)  DEFAULT ('') NOT NULL,
    [PPDApvDate]     DATETIME      NULL,
    [ProdApvName]    VARCHAR (10)  DEFAULT ('') NOT NULL,
    [ProdApvDate]    DATETIME      NULL,
    [Status]         VARCHAR (15)  NOT NULL,
    [AddName]        VARCHAR (10)  NOT NULL,
    [AddDate]        DATETIME      NOT NULL,
    [EditName]       VARCHAR (10)  DEFAULT ('') NOT NULL,
    [EditDate]       DATETIME      NULL,
    [WHSupApvName]   VARCHAR (10)  CONSTRAINT [DF_AVO_WHSupApvName] DEFAULT ('') NOT NULL,
    [WHSupApvDate]   DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


