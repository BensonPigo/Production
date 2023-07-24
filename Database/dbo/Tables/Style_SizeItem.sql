CREATE TABLE [dbo].[Style_SizeItem] (
    [StyleUkey]     BIGINT        CONSTRAINT [DF_Style_SizeItem_StyleUkey] DEFAULT ((0)) NOT NULL,
    [StyleUkey_Old] VARCHAR (10)  CONSTRAINT [DF_Style_SizeItem_StyleUkey_Old] DEFAULT ('') NOT NULL,
    [SizeItem]      VARCHAR (3)   CONSTRAINT [DF_Style_SizeItem_SizeItem] DEFAULT ('') NOT NULL,
    [SizeUnit]      VARCHAR (8)   CONSTRAINT [DF_Style_SizeItem_SizeUnit] DEFAULT ('') NOT NULL,
    [Description]   VARCHAR (200) CONSTRAINT [DF_Style_SizeItem_Description] DEFAULT ('') NOT NULL,
    [Ukey]          BIGINT        NOT NULL,
    [TolMinus]      VARCHAR (15)  CONSTRAINT [DF_Style_SizeItem_TolMinus] DEFAULT ('') NOT NULL,
    [TolPlus]       VARCHAR (15)  CONSTRAINT [DF_Style_SizeItem_TolPlus] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Style_SizeItem] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);





