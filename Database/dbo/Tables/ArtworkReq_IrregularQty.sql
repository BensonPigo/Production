CREATE TABLE [dbo].[ArtworkReq_IrregularQty]
(
	[OrderID] VARCHAR(13) NOT NULL  DEFAULT (''), 
    [ArtworkTypeID] VARCHAR(20) NOT NULL DEFAULT (''), 
    [StandardQty] NUMERIC(6) NOT NULL DEFAULT ((0)), 
    [ReqQty] NUMERIC(6) NOT NULL DEFAULT ((0)), 
    [SubconReasonID] VARCHAR(5) NOT NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [AddDate] DATETIME NOT NULL, 
    [EditName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    PRIMARY KEY ([OrderID], [ArtworkTypeID])
)
