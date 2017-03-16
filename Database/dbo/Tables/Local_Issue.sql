CREATE TABLE [dbo].[Local_Issue] (
    [ID]          VARCHAR (13)   NOT NULL,
    [MDivisionID] VARCHAR (8)    NOT NULL,
    [FactoryID]   VARCHAR (8)    NOT NULL,
    [IssueDate]   DATE           NOT NULL,
    [Status]      VARCHAR (15)   NULL,
    [AddName]     VARCHAR (10)   NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   NULL,
    [EditDate]    DATETIME       NULL,
    [Remark]      NVARCHAR (100) NULL,
    CONSTRAINT [PK_Local_Issue] PRIMARY KEY CLUSTERED ([ID] ASC)
);





