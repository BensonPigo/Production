CREATE TABLE [dbo].[ArtworkReq_Detail]
(
	[uKey] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [OrderID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [ArtworkID] VARCHAR(20) NOT NULL DEFAULT (''), 
    [PatternCode] VARCHAR(20) NOT NULL DEFAULT (''), 
	[PatternDesc] VARCHAR(40) NOT NULL DEFAULT (''),
    [ReqQty] NUMERIC(6) NOT NULL DEFAULT ((0)), 
    [Stitch] NUMERIC(6) NOT NULL DEFAULT ((0)), 
    [QtyGarment] NUMERIC(2) NOT NULL DEFAULT ((0)), 
    [ExceedQty] NUMERIC(6) NOT NULL DEFAULT ((0)), 
    [ArtworkPOID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [Article] VARCHAR(8) NOT NULL CONSTRAINT [DF_ArtworkReq_Detail_Article] DEFAULT (''), 
    [SizeCode] VARCHAR(8) NOT NULL CONSTRAINT [DF_ArtworkReq_Detail_SizeCode] DEFAULT (''), 
    CONSTRAINT [PK_ArtworkReq_Detail] PRIMARY KEY CLUSTERED ([uKey]) 
)
