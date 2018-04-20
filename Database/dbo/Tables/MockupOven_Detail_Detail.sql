CREATE TABLE [dbo].[MockupOven_Detail_Detail]
(
	[ID] VARCHAR(13) NOT NULL , 
    [ReportNo] VARCHAR(13) NOT NULL, 
    [Ukey] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ArtworkTypeID] VARCHAR(20) NULL DEFAULT (''), 
    [ArtworkColor] VARCHAR(6) NULL DEFAULT (''), 
    [FabricRefNo] VARCHAR(30) NULL DEFAULT (''), 
    [FabricColor] VARCHAR(6) NULL DEFAULT (''), 
    [Result] VARCHAR(4) NULL DEFAULT (''), 
    [Remark] VARCHAR(300) NULL DEFAULT (''),
	[AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT ('')
)
