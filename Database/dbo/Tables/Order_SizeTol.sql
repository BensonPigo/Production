CREATE TABLE [dbo].[Order_SizeTol] (
    [Id]        VARCHAR (13) NOT NULL,
    [SizeGroup] VARCHAR (1)  NOT NULL,
    [SizeItem]  VARCHAR (3)  NOT NULL,
    [Lower]     VARCHAR (15) CONSTRAINT [DF_Order_SizeTol_Lower] DEFAULT ('') NOT NULL,
    [Upper]     VARCHAR (15) CONSTRAINT [DF_Order_SizeTol_Upper] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Order_SizeTol] PRIMARY KEY CLUSTERED ([Id] ASC, [SizeGroup] ASC, [SizeItem] ASC)
);


GO



GO



GO



GO



GO



GO



GO


