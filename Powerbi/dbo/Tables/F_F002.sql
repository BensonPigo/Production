CREATE TABLE [dbo].[F_F002] (
    [onboard]             NVARCHAR (8)    NULL,
    [InvoiceNo]           VARCHAR (25)    NULL,
    [PulloutDate]         DATE            NULL,
    [ETD]                 DATE            NULL,
    [Brand]               VARCHAR (8)     NULL,
    [Shipper]             VARCHAR (8)     NULL,
    [Currency]            VARCHAR (3)     NULL,
    [VoucherID]           VARCHAR (16)    NULL,
    [orgAmount]           NUMERIC (18, 2) NULL,
    [Amount]              NUMERIC (18, 2) NULL,
    [VoucherDate]         DATE            NULL,
    [VoucherMonth]        VARCHAR (10)    NULL,
    [Factory]             VARCHAR (8)     NULL,
    [MDivision]           VARCHAR (8)     NULL,
    [DifferenceDays]      INT             NULL,
    [DifferenceDaysGroup] VARCHAR (8)     NULL
);

