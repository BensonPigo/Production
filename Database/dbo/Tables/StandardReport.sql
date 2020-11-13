CREATE TABLE [dbo].[StandardReport] (
    [ReportName]        NVARCHAR (50) NULL,
    [ReportType]        NVARCHAR (50) NULL,
    [ReportHeaderIndex] INT           NULL,
    [ReportIden]        BIGINT        IDENTITY (1, 1) NOT NULL,
	ExcelTitle nvarchar(50) NULL,
	OnlyforPublic bit NOT NULL default 0,
    CONSTRAINT [PK_StandardReport] PRIMARY KEY CLUSTERED ([ReportIden] ASC)
);

