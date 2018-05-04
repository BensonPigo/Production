CREATE TABLE [dbo].[MockupCrocking_Detail]
(
	[ID] VARCHAR(13) NOT NULL , 
    [ReportNo] VARCHAR(13) NOT NULL PRIMARY KEY, 
    [No] INT NOT NULL, 
    [SubmitDate] DATE NULL, 
    [CombineStyle] VARCHAR(120) NULL DEFAULT (''), 
    [Result] VARCHAR(4) NULL DEFAULT (''), 
    [ReceivedDate] DATETIME NULL, 
    [ReleasedDate] DATETIME NULL, 
    [Technician] VARCHAR(10) NULL DEFAULT (''), 
    [MR] VARCHAR(10) NULL DEFAULT (''),
	[AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT ('')
)
