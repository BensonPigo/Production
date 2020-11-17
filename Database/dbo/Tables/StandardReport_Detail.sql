CREATE TABLE [dbo].[StandardReport_Detail] (
    [ExcelColumnIndex]  NVARCHAR (50)  NULL,
    [ExcelColumnHeader] NVARCHAR (100) NULL,
    [ExcelColumnIden]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReportIden]        BIGINT         NULL,
	ColumnName nvarchar(100) default '',
	CellFormat nvarchar(50)   default '',
	CellFontName nvarchar(50)  default '',
	CellFontSize int default 0,
	CellFontBold bit default 0,
    CONSTRAINT [PK_StandardReport_Detail] PRIMARY KEY CLUSTERED ([ExcelColumnIden] ASC)
);



