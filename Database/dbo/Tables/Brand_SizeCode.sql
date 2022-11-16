CREATE TABLE [dbo].[Brand_SizeCode] (
    [BrandID]    VARCHAR (8)   CONSTRAINT [DF_Brand_SizeCode_BrandID] DEFAULT ('') NOT NULL,
    [SizeCode]   VARCHAR (8)   CONSTRAINT [DF_Brand_SizeCode_SizeCode] DEFAULT ('') NOT NULL,
    [AgeGroupID] NVARCHAR (10) CONSTRAINT [DF_Brand_SizeCode_AgeGroupID] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (50)  CONSTRAINT [DF_Brand_SizeCode_AddName] DEFAULT ('') NOT NULL,
    [AddDate]    DATETIME      NULL,
    [EditName]   VARCHAR (10)  NOT NULL,
    [EditDate]   DATETIME      CONSTRAINT [DF_Brand_SizeCode_EditDate] DEFAULT ('') NULL,
    CONSTRAINT [PK_Brand_SizeCode] PRIMARY KEY CLUSTERED ([BrandID] ASC, [SizeCode] ASC)
);

