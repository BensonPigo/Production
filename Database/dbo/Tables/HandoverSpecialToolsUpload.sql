CREATE TABLE [dbo].[HandoverSpecialToolsUpload] (
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [StyleUkey]   BIGINT         CONSTRAINT [DF_HandoverSpecialToolsUpload_StyleUkey] DEFAULT ((1)) NOT NULL,
    [SourceFile]  NVARCHAR (266) CONSTRAINT [DF_HandoverSpecialToolsUpload_SourceFile] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (100) CONSTRAINT [DF_HandoverSpecialToolsUpload_Description] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_HandoverSpecialToolsUpload_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    CONSTRAINT [PK_HandoverSpecialToolsUpload] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

