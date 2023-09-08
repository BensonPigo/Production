CREATE TABLE [dbo].[OperationRef]
(
	[Id] varchar(20) NOT NULL,
	[CodeType] varchar(5) NOT NULL,
	[CodeID] varchar(20) NOT NULL, 
    CONSTRAINT [PK_OperationRef] PRIMARY KEY ([Id], [CodeType], [CodeID]),
)
