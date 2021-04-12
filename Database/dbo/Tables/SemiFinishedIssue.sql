CREATE TABLE [dbo].[SemiFinishedIssue]
(
	[ID] VARCHAR(13) NOT NULL , 
    [MDivisionID] VARCHAR(8) NOT NULL, 
    [FactoryID] VARCHAR(8) NOT NULL, 
    [IssueDate] DATE NULL, 
    [Remark] NVARCHAR(100) CONSTRAINT [DF_SemiFinishedIssue_Remark] DEFAULT ('') NOT NULL, 
    [Status] VARCHAR(15) NOT NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_SemiFinishedIssue_AddName] DEFAULT ('') NOT NULL, 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SemiFinishedIssue_EditName] DEFAULT ('') NOT NULL, 
    [EditDate] DATETIME NULL,
	CONSTRAINT [PK_SemiFinishedIssue] PRIMARY KEY CLUSTERED  ([ID] ASC)
)
