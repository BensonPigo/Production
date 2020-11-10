CREATE TABLE [dbo].[CustomizedReport_Detail] (
    [CustomizedIden]    BIGINT         NULL,
    [ExcelColumnIndex]  NVARCHAR (50)  NULL,
    [ExcelColumnHeader] NVARCHAR (100) NULL,
    [ExcelColumnIden]   BIGINT         NULL,
    [ID]                BIGINT         IDENTITY (1, 1) NOT NULL,
	IndexNum int default 0,
    CONSTRAINT [PK_CustomizedReport_Detail] PRIMARY KEY CLUSTERED ([ID] ASC)
);



