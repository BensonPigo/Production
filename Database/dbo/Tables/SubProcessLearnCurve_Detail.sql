CREATE TABLE [dbo].[SubProcessLearnCurve_Detail]
(
	[ukey] BIGINT NOT NULL IDENTITY , 
    [Day] INT NOT NULL, 
    [Efficiency] INT NULL, 
    PRIMARY KEY ([ukey], [Day])
)
