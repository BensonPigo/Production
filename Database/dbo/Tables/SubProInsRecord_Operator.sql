CREATE TABLE [dbo].[SubProInsRecord_Operator]
(
	[SubProInsRecordUkey] BIGINT NOT NULL, 
    [SubProOperatorEmployeeID] NVARCHAR(8) NOT NULL, 
    CONSTRAINT [PK_SubProInsRecord_Operator] PRIMARY KEY CLUSTERED ([SubProInsRecordUkey], [SubProOperatorEmployeeID] ASC)
)
