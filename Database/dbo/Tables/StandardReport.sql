CREATE TABLE [dbo].[StandardReport] (
    [ReportName]        NVARCHAR (50) NULL,
    [ReportType]        NVARCHAR (50) NULL,
    [ReportHeaderIndex] INT           NULL,
    [ReportIden]        BIGINT        IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_StandardReport] PRIMARY KEY CLUSTERED ([ReportIden] ASC)
);

