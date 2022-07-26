CREATE TABLE [dbo].[SewingOutputEfficiency_History]
(
	[Ukey] bigint NOT NULL IDENTITY, 
    [SewingOutputEfficiencyUkey] bigint NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_History_SewingOutputEfficiencyUkey] DEFAULT (0), 
    [Action] varchar(10) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_History_Action] DEFAULT (''), 
    [OldValue] varchar(10) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_History_OldValue] DEFAULT (''), 
    [NewValue] varchar(10) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_History_NewValue] DEFAULT (''), 
    [AddName] varchar(10) NOT NULL CONSTRAINT [DF_SewingOutputEfficiency_History_AddName] DEFAULT (''), 
    [AddDate] datetime NOT NULL
)
