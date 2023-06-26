CREATE TABLE AttachmentType(
	ID varchar(13) NOT NULL,
	Type nvarchar(200) NOT NULL CONSTRAINT [DF_AttachmentType_Type] DEFAULT '',
	Junk bit not NULL  CONSTRAINT [DF_AttachmentType_Junk] DEFAULT 0,	
	AddName varchar(10) NULL CONSTRAINT [DF_AttachmentType_AddName] DEFAULT '',
	AddDate datetime NULL ,
	EditName varchar(10) NULL CONSTRAINT [DF_AttachmentType_EditName] DEFAULT '',
	EditDate datetime NULL ,
		CONSTRAINT [PK_AttachmentType] PRIMARY KEY CLUSTERED 
	(
		ID ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO