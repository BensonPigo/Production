CREATE TABLE [dbo].[StandardReport_Detail] (
    [ExcelColumnIndex]  NVARCHAR (50)  NULL,
    [ExcelColumnHeader] NVARCHAR (100) NULL,
    [ExcelColumnIden]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReportIden]        BIGINT         NULL,
    CONSTRAINT [PK_StandardReport_Detail] PRIMARY KEY CLUSTERED ([ExcelColumnIden] ASC)
);



