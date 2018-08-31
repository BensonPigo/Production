CREATE TABLE [dbo].[Style_SizeItem] (
    [StyleUkey]     BIGINT         NULL,
    [StyleUkey_Old] VARCHAR (10)   NULL,
    [SizeItem]      VARCHAR (3)    NULL,
    [SizeUnit]      VARCHAR (8)    NULL,
    [Description]   NVARCHAR (100) NULL,
    [Ukey]          BIGINT         NOT NULL,
    [TolMinus]      VARCHAR (15)   NULL,
    [TolPlus]       VARCHAR (15)   NULL,
    CONSTRAINT [PK_Style_SizeItem] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



