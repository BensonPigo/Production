CREATE TABLE [dbo].[SubTransferLocal_Detail]
(
	[ID] VARCHAR(13) NOT NULL DEFAULT ('') , 
    [MDivisionID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [Poid] VARCHAR(13) NOT NULL DEFAULT (''), 
    [Refno] VARCHAR(21) NOT NULL DEFAULT (''), 
    [Color] VARCHAR(15) NOT NULL DEFAULT (''), 
    [FromLocation] VARCHAR(100) NOT NULL DEFAULT (''), 
    [ToLocation] VARCHAR(100) NOT NULL DEFAULT (''), 
    [Qty] NUMERIC(11, 2) NULL DEFAULT ((0)), 
    [Ukey] BIGINT NOT NULL, 
    CONSTRAINT [PK_SubTransferLocal_Detail] PRIMARY KEY ([Ukey])
)
