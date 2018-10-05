CREATE TABLE [dbo].[AVO]
(
	[ID] BIGINT NOT NULL PRIMARY KEY, 
    [cDate] DATE NOT NULL, 
    [MDivisionID] VARCHAR(8) NOT NULL, 
    [Handle] VARCHAR(10) NOT NULL, 
    [Remark] VARCHAR(100) NOT NULL DEFAULT (''), 
    [SupApvName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [SupApvDate] DATETIME NULL, 
    [PPDApvName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [PPDApvDate] DATETIME NULL, 
    [ProdApvName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [ProdApvDate] DATETIME NULL, 
    [Status] VARCHAR(15) NOT NULL, 
    [AddName] VARCHAR(10) NOT NULL, 
    [AddDate] DATETIME NOT NULL, 
    [EditName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [EditDate] DATETIME NULL
)
