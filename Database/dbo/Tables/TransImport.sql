CREATE TABLE [dbo].[TransImport] (
    [GroupID] INT            NOT NULL,
    [Seq]     INT            NOT NULL,
    [Name]    NVARCHAR (100) NULL,
    [TSQL]    NVARCHAR (500) NULL,
    CONSTRAINT [PK_TransImport] PRIMARY KEY CLUSTERED ([GroupID] ASC, [Seq] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TransImport]
    ON [dbo].[TransImport]([GroupID] ASC);

