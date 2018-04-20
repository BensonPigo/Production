CREATE TABLE [dbo].[MockupOven]
(
	[ID] VARCHAR(13) NOT NULL PRIMARY KEY, 
    [BrandID] VARCHAR(8) NULL, 
    [StyleID] VARCHAR(15) NULL, 
    [SeasonID] VARCHAR(8) NULL, 
    [Article] VARCHAR(8) NULL, 
    [ReceivedDate] DATETIME NULL, 
    [ReleasedDate] DATETIME NULL, 
    [T1Subcon] VARCHAR(8) NULL, 
	[T2Supplier] VARCHAR(8) NULL, 
    [Remark] VARCHAR(300) NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT ('')
)