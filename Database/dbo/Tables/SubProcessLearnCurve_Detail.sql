CREATE TABLE [dbo].[SubProcessLearnCurve_Detail]
(
	[ukey] INT NOT NULL , 
    [Day] NCHAR(10) NOT NULL, 
    [Efficiency] NCHAR(10) NULL, 
    PRIMARY KEY ([ukey], [Day])
)
