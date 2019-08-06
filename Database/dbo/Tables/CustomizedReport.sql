CREATE TABLE [dbo].[CustomizedReport] (
    [UserID]         NVARCHAR (50) NULL,
    [ReportIden]     BIGINT        NOT NULL,
    [CustomizedIden] BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserVersion]    NVARCHAR (50) DEFAULT ('') NOT NULL,
    [UserDefault]    BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CustomizedIden] ASC)
);



