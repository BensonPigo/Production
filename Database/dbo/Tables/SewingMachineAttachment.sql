
CREATE TABLE [dbo].SewingMachineAttachment(
	Ukey bigint  NOT NULL  IDENTITY(1,1),
	SewingMachineAttachmentID nvarchar(200) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_SewingMachineAttachmentID] DEFAULT '',
	MoldID nvarchar(20) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_MoldID] DEFAULT '',
	Description nvarchar(200) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Description] DEFAULT '',
	DescriptionCN nvarchar(200) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_DescriptionCN] DEFAULT '',
	MachineMasterGroupID varchar(2) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_MachineMasterGroupID] DEFAULT '',
	AttachmentTypeID varchar(200) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_AttachmentTypeID] DEFAULT '',
	MeasurementID varchar(200) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_MeasurementID] DEFAULT '',
	FoldTypeID varchar(200) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_FoldTypeID] DEFAULT '',
	Supplier1PartNo nvarchar(60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Supplier1PartNo] DEFAULT '',
	Supplier1BrandID nvarchar(60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Supplier1BrandID] DEFAULT '',
	Supplier2PartNo nvarchar(60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Supplier2PartNo] DEFAULT '',
	Supplier2BrandID nvarchar(60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Supplier2BrandID] DEFAULT '',
	Supplier3PartNo nvarchar(60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Supplier3PartNo] DEFAULT '',
	Supplier3BrandID nvarchar(60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Supplier3BrandID] DEFAULT '',
	Remark nvarchar(max) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Remark] DEFAULT '',
	Picture1 nvarchar(60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Picture1] DEFAULT '',
	Picture2 nvarchar(60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Picture2] DEFAULT '',		
	AddName varchar(10) NOT NULL CONSTRAINT [DF_SewingMachineAttachmen_AddName] DEFAULT '',
	AddDate datetime NULL ,
	EditName varchar(10) NOT NULL CONSTRAINT [DF_SewingMachineAttachmen_EditName] DEFAULT '',
	EditDate datetime NULL ,
	CONSTRAINT [PK_SewingMachineAttachment] PRIMARY KEY CLUSTERED 
(
	Ukey ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO