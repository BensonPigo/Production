CREATE TABLE [dbo].[SemiFinishedAdjust]
(
	[ID] VARCHAR(13) NOT NULL , 
    [MDivisionID] VARCHAR(8) NOT NULL, 
    [FactoryID] VARCHAR(8) NOT NULL, 
    [IssueDate] DATE NULL, 
    [Remark] NVARCHAR(100) CONSTRAINT [DF_SemiFinishedAdjust_Remark] DEFAULT ('') NOT NULL, 
    [Status] VARCHAR(15) NOT NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_SemiFinishedAdjust_AddName] DEFAULT ('') NOT NULL, 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SemiFinishedAdjust_EditName] DEFAULT ('') NOT NULL, 
    [EditDate] DATETIME NULL,
	CONSTRAINT [PK_SemiFinishedAdjust] PRIMARY KEY CLUSTERED  ([ID] ASC)
)
