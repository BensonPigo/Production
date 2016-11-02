CREATE TABLE [dbo].[TransRegion] (
    [Region]         VARCHAR (50)  NOT NULL,
    [DirName]        VARCHAR (500) NULL,
    [RarName]        VARCHAR (30)  NULL,
    [Is_Export]      BIT           NULL,
    [ConnectionName] VARCHAR (50)  NULL,
    [DBName]         VARCHAR (50)  NULL,
    [DBFileName]     VARCHAR (50)  NULL
);

