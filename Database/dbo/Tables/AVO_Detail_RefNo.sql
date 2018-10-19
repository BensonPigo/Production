CREATE TABLE [dbo].[AVO_Detail_RefNo]
(
	[AVO_DetailUkey] BIGINT NOT NULL , 
    [RefNo] VARCHAR(20) NOT NULL, 
    PRIMARY KEY ([RefNo], [AVO_DetailUkey])
)
