CREATE TABLE [dbo].[SubProcessOutput] (
    [ID]         VARCHAR (13) NOT NULL,
    [TypeID]     VARCHAR (10) NULL,
    [OutputDate] DATE         NULL,
    [Shift]      VARCHAR (1)  NULL,
    [Team]       VARCHAR (1)  NULL,
    [QAQty]      INT          NULL,
    [ProdQty]    INT          NULL,
    [DefectQty]  INT          NULL,
    [Status]     VARCHAR (15) NULL,
    [AddName]    VARCHAR (10) NULL,
    [AddDate]    DATETIME     NULL,
    [EditName]   VARCHAR (10) NULL,
    [EditDate]   DATETIME     NULL,
    CONSTRAINT [PK_SubProcessOutput] PRIMARY KEY CLUSTERED ([ID] ASC)
);



