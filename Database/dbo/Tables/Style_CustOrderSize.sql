CREATE TABLE [dbo].[Style_CustOrderSize] (
    [StyleUkey] BIGINT       NOT NULL,
    [SizeCode]  VARCHAR (8)  CONSTRAINT [DF_Style_CustOrderSize_SizeCode] DEFAULT ('') NOT NULL,
    [SizeSpec]  VARCHAR (15) CONSTRAINT [DF_Style_CustOrderSize_SizeSpec] DEFAULT ('') NOT NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_Style_CustOrderSize_AddName] DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_Style_CustOrderSize_EditName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME     NULL
);

