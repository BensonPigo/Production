CREATE TABLE [dbo].[F_F003] (
    [PulloutMonth]        NVARCHAR (8)    NULL,
    [CDate]               DATE            NULL,
    [InvoiceNo]           VARCHAR (25)    NULL,
    [ETD]                 DATE            NULL,
    [OrderID]             VARCHAR (13)    NULL,
    [Currency]            VARCHAR (3)     NULL,
    [Rate]                NUMERIC (18, 2) NULL,
    [orginAmount]         NUMERIC (18, 2) NULL,
    [Amount]              NUMERIC (18, 2) NULL,
    [OnboardMonth]        NVARCHAR (8)    NULL,
    [Factory]             VARCHAR (8)     NULL,
    [MDivision]           VARCHAR (8)     NULL,
    [DifferenceDays]      INT             NULL,
    [DifferenceDaysGroup] VARCHAR (8)     NULL,
    [Brand]               VARCHAR (8)     NULL
);

