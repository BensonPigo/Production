
CREATE TABLE [dbo].LostFinalInspNotification
(   
	FactoryID Varchar(8)  NOT NULL , 
	ToAddress      nVarchar(500) NOT NULL CONSTRAINT [DF_LostFinalInspNotification_ToAddress] DEFAULT '',
	CcAddress      nVarchar(500) NOT NULL CONSTRAINT [DF_LostFinalInspNotification_CcAddress] DEFAULT '',
	StartTime    time(7)   NULL ,
	EndTime    time(7)   NULL ,
	Frequency   Varchar(20)    NOT NULL CONSTRAINT [DF_LostFinalInspNotification_Frequency] DEFAULT '',
	[Description]     NVarchar(500) NOT NULL CONSTRAINT [DF_LostFinalInspNotification_Description] DEFAULT '',
	AddDate datetime NULL,
	AddName Varchar(10) NOT NULL CONSTRAINT [DF_LostFinalInspNotification_AddName] DEFAULT '',
	EditDate datetime NULL,
	EditName Varchar(10) NOT NULL CONSTRAINT [DF_LostFinalInspNotification_EditName] DEFAULT '',
	LastExecuteTime datetime NULL,
	CONSTRAINT [PK_LostFinalInspNotification] PRIMARY KEY CLUSTERED 
	(
		FactoryID ASC
	)
)
GO