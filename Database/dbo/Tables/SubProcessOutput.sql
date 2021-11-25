CREATE TABLE [dbo].[SubProcessOutput] (
    [ID]          VARCHAR (13)   NOT NULL,
    [TypeID]      VARCHAR (10)   NULL,
    [OutputDate]  DATE           NULL,
    [Shift]       VARCHAR (1)    NULL,
    [Team]        VARCHAR (5)    NULL,
    [QAQty]       INT            NULL,
    [ProdQty]     INT            NULL,
    [DefectQty]   INT            NULL,
    [Status]      VARCHAR (15)   NULL,
    [AddName]     VARCHAR (10)   NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   NULL,
    [EditDate]    DATETIME       NULL,
    [Manpower]    INT            NULL,
    [WHour]       NUMERIC (3, 1) NULL,
    [MdivisionID] VARCHAR (8)    NULL,
    [TotalCPU]    NUMERIC (8, 2) DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SubProcessOutput] PRIMARY KEY CLUSTERED ([ID] ASC)
);





