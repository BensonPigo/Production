
CREATE TABLE PerspirationFastness(
	ID bigint NOT NULL IDENTITY(1,1),
	POID varchar(13) NULL CONSTRAINT [DF_PerspirationFastness_POID] DEFAULT '',
	TestNo numeric(2, 0) NULL CONSTRAINT [DF_PerspirationFastness_TestNo] DEFAULT 0,
	InspDate Date NULL ,
	Article varchar(8) NULL CONSTRAINT [DF_PerspirationFastness_Article] DEFAULT '',
	Result varchar(15) NULL CONSTRAINT [DF_PerspirationFastness_Result] DEFAULT '',
	Status varchar(15) NULL CONSTRAINT [DF_PerspirationFastness_Status] DEFAULT '',
	Inspector varchar(10) NULL CONSTRAINT [DF_PerspirationFastness_Inspector] DEFAULT '',
	Remark nvarchar(120) NULL CONSTRAINT [DF_PerspirationFastness_Remark] DEFAULT '',
	Temperature int NULL CONSTRAINT [DF_PerspirationFastness_Temperature] DEFAULT 0,
	Time int NULL CONSTRAINT [DF_PerspirationFastness_Time] DEFAULT 0,
	MetalContent nvarchar(120) NULL CONSTRAINT [DF_PerspirationFastness_MetalContent] DEFAULT '',
	AddName varchar(10) NULL CONSTRAINT [DF_PerspirationFastness_AddName] DEFAULT '',
	AddDate datetime NULL ,
	EditName varchar(10) NULL CONSTRAINT [DF_PerspirationFastness_EditName] DEFAULT '',
	EditDate datetime NULL ,
	ReportNo varchar(14) not null CONSTRAINT [DF_PerspirationFastness_ReportNo] default '',
	[Approver] VARCHAR(10) CONSTRAINT [DF_PerspirationFastness_Approver] NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_PerspirationFastness] PRIMARY KEY CLUSTERED 
	(
		ID ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Approver',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PerspirationFastness',
    @level2type = N'COLUMN',
    @level2name = N'Approver'