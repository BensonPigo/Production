CREATE TABLE [dbo].[ExpressDuty_Functions]
(
	[ExpressDutyID] VARCHAR NOT NULL, 
    [FunctionID] VARCHAR(50) NOT NULL, 
    [AddName] VARCHAR(10) not null CONSTRAINT [CONSTRAINT_ExpressDuty_Functions_AddName] DEFAULT (''),
    [AddDate] DATETIME NULL, 
    CONSTRAINT [PK_ExpressDuty_Functions] PRIMARY KEY CLUSTERED ([ExpressDutyID], [FunctionID] ASC)
)
