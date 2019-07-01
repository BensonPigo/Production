CREATE TABLE [dbo].[ExpenseMonthlyReport] (
    [WKID]          VARCHAR (13)    NULL,
    [CloseDate]     DATETIME        NULL,
    [Year]          VARCHAR (4)     NULL,
    [Month]         VARCHAR (2)     NULL,
    [MonthName]     VARCHAR (20)    NULL,
    [YM]            VARCHAR (7)     NULL,
    [LastYear]      VARCHAR (4)     NULL,
    [LYM]           VARCHAR (7)     NULL,
    [ShipTerm]      VARCHAR (5)     NULL,
    [ShipMode]      VARCHAR (10)    NULL,
    [ShipModeGroup] VARCHAR (20)    NULL,
    [Team]          VARCHAR (20)    NULL,
    [DutyID]        VARCHAR (13)    NULL,
    [Duty]          VARCHAR (30)    NULL,
    [KGS]           NUMERIC (16, 2) NULL,
    [Amount_US]     NUMERIC (16, 2) NULL,
    [From]          VARCHAR (5)     NULL,
    [To]            VARCHAR (5)     NULL,
    [ClientArea]    VARCHAR (30)    NULL,
    [Factory]       VARCHAR (8)     NULL,
    [FactoryGroup]  VARCHAR (30)    NULL
);

