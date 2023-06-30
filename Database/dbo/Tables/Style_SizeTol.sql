CREATE TABLE [dbo].[Style_SizeTol] (
    [StyleUkey] BIGINT       NOT NULL,
    [SizeGroup] VARCHAR (1)  NOT NULL,
    [SizeItem]  VARCHAR (3)  NOT NULL,
    [Lower]     VARCHAR (15) CONSTRAINT [DF_Style_SizeTol_Lower] DEFAULT ('') NOT NULL,
    [Upper]     VARCHAR (15) CONSTRAINT [DF_Style_SizeTol_Upper] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Style_SizeTol] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [SizeGroup] ASC, [SizeItem] ASC)
);


