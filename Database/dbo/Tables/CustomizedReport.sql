CREATE TABLE [dbo].[CustomizedReport] (
    [UserID]         NVARCHAR (50) NULL,
    [ReportIden]     BIGINT        NOT NULL,
    [CustomizedIden] BIGINT        IDENTITY (1, 1) NOT NULL
);

