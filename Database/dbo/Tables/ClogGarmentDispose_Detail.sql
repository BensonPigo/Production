CREATE TABLE [dbo].[ClogGarmentDispose_Detail]
(
	[ID] VARCHAR(13) NOT NULL , 
    [PackingListID] VARCHAR(13) NOT NULL, 
    [CTNStartNO] VARCHAR(6) NOT NULL, 
    CONSTRAINT [PK_ClogGarmentDispose_Detail] PRIMARY KEY CLUSTERED ([ID],[PackingListID],[CTNStartNO] ASC)
)
