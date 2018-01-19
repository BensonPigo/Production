CREATE TABLE [dbo].[AdjustGMT] (
    [ID]          VARCHAR (13) NOT NULL,
    [MDivisionID] VARCHAR (8)  NULL,
    [IssueDate]   DATE         NULL,
    [Remark]      NCHAR (200)  NULL,
    [Status]      VARCHAR (15) NULL,
    [AddName]     VARCHAR (10) NOT NULL,
    [AddDate]     DATETIME     NOT NULL,
    [EditName]    VARCHAR (10) NULL,
    [EditDate]    DATETIME     NULL,
    CONSTRAINT [PK_AdjustGMT] PRIMARY KEY CLUSTERED ([ID] ASC)
);

