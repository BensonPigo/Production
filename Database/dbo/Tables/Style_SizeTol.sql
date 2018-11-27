CREATE TABLE [dbo].[Style_SizeTol]
(
	[StyleUkey] BIGINT NOT NULL, 
    [SizeGroup] VARCHAR(1) NOT NULL, 
    [SizeItem] VARCHAR(3) NOT NULL, 
    [Lower] VARCHAR(15) NULL, 
    [Upper] VARCHAR(15) NULL,
	CONSTRAINT [PK_Style_SizeTol] PRIMARY KEY CLUSTERED ([StyleUkey],[SizeGroup],[SizeItem] ASC)
)
