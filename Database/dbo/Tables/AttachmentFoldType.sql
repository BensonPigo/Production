
CREATE TABLE AttachmentFoldType(
ID varchar(13) NOT NULL ,
FoldType nvarchar(200) NOT NULL CONSTRAINT [DF_AttachmentFoldType_FoldType] DEFAULT '',
Description nvarchar(200) NOT NULL CONSTRAINT [DF_AttachmentFoldType_Description] DEFAULT '',
Junk bit not NULL  CONSTRAINT [DF_AttachmentFoldType_Junk] DEFAULT 0,	
AddName varchar(10) NOT NULL CONSTRAINT [DF_AttachmentFoldType_AddName] DEFAULT '',
AddDate datetime NULL ,
EditName varchar(10) NOT NULL CONSTRAINT [DF_AttachmentFoldType_EditName] DEFAULT '',
EditDate datetime NULL ,
	CONSTRAINT [PK_AttachmentFoldType] PRIMARY KEY CLUSTERED 
(
	ID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO