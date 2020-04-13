CREATE TABLE [dbo].[ICR_ResponsibilityDept] (
    [ID]           VARCHAR (13)    DEFAULT ('') NOT NULL,
    [FactoryID]    VARCHAR (8)     DEFAULT ('') NOT NULL,
    [DepartmentID] VARCHAR (8)     DEFAULT ('') NOT NULL,
    [Percentage]   NUMERIC (5, 2)  DEFAULT ((0)) NOT NULL,
    [Amount]       NUMERIC (11, 2) DEFAULT ((0)) NOT NULL,
    [EditName] VARCHAR(10) NOT NULL CONSTRAINT [DF_ICR_ResponsibilityDept_EditName] DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    CONSTRAINT [PK_ICR_ResponsibilityDept] PRIMARY KEY CLUSTERED ([ID] ASC, [FactoryID] ASC, [DepartmentID] ASC)
);

