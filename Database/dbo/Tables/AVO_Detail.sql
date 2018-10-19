CREATE TABLE [dbo].[AVO_Detail]
(
	[ID] VARCHAR(13) NOT NULL , 
    [OrderID] VARCHAR(13) NOT NULL, 
    [OrderShipmodeSeq] VARCHAR(2) NOT NULL, 
    [ShipModeID] VARCHAR(10) NOT NULL, 
    [Ukey] BIGINT NOT NULL IDENTITY, 
    CONSTRAINT [PK_AVO_Detail] PRIMARY KEY ([Ukey])
)
