CREATE TABLE AttachmentMeasurement(
	ID varchar(13) NOT NULL ,
	Measurement nvarchar(200) NOT NULL CONSTRAINT [DF_AttachmentMeasurement_Type] DEFAULT '',
	Junk bit not NULL  CONSTRAINT [DF_AttachmentMeasurement_Junk] DEFAULT 0,	
	AddName varchar(10) NOT NULL CONSTRAINT [DF_AttachmentMeasurement_AddName] DEFAULT '',
	AddDate datetime NULL ,
	EditName varchar(10) NOT NULL CONSTRAINT [DF_AttachmentMeasurement_EditName] DEFAULT '',
	EditDate datetime NULL ,
		CONSTRAINT [PK_AttachmentMeasurement] PRIMARY KEY CLUSTERED 
	(
		ID ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO