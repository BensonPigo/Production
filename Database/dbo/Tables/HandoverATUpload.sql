CREATE TABLE [dbo].[HandoverATUpload] (
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [StyleUkey]   BIGINT         CONSTRAINT [DF_HandoverATUpload_StyleUkey] DEFAULT ((1)) NOT NULL,
    [SourceFile]  NVARCHAR (266) CONSTRAINT [DF_HandoverATUpload_SourceFile] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (100) CONSTRAINT [DF_HandoverATUpload_Description] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_HandoverATUpload_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    CONSTRAINT [PK_HandoverATUpload] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

