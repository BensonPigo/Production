CREATE TABLE [dbo].[AVO_Detail_RefNo]
(
	[AVO_DetailUkey] BIGINT NOT NULL , 
    [RefNo] VARCHAR(36) NOT NULL, 
    PRIMARY KEY ([RefNo], [AVO_DetailUkey])
)
