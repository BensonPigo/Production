CREATE TABLE [dbo].[ThreadAllowanceScale] (
    [ID]         VARCHAR (3)    NOT NULL,
    [LowerBound] INT            NULL,
    [UpperBound] INT            NULL,
    [Allowance]  NUMERIC (5, 2) NULL,
    [Remark]     NVARCHAR (MAX) NULL,
    [AddName]    VARCHAR (10)   NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   NULL,
    [EditDate]   DATETIME       NULL,
    CONSTRAINT [PK_ThreadAllowanceScale] PRIMARY KEY CLUSTERED ([ID] ASC)
);

