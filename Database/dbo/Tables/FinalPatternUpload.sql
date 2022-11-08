CREATE TABLE [dbo].[FinalPatternUpload] (
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [StyleUkey]   BIGINT         CONSTRAINT [DF_FinalPatternUpload_StyleUkey] DEFAULT ((1)) NOT NULL,
    [SourceFile]  NVARCHAR (266) CONSTRAINT [DF_FinalPatternUpload_SourceFile] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (100) CONSTRAINT [DF_FinalPatternUpload_Description] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_FinalPatternUpload_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    CONSTRAINT [PK_FinalPatternUpload] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

