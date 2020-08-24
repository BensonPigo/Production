CREATE TABLE [dbo].[GarmentTest_Detail_FGWT] (
	ID BIGINT NOT NULL,
	No int NOT NULL,
	Location	 varchar (1) NOT NULL,
	Type	  varchar (150) NOT NULL,
	TestDetail	 varchar (10) NOT NULL CONSTRAINT [DF_GarmentTest_Detail_FGWT_TestDetail] DEFAULT(''),
	BeforeWash	 Numeric(11, 2) NULL ,
	SizeSpec	 Numeric(11, 2) NULL ,
	AfterWash	 Numeric(11, 2) NULL ,
	Shrinkage	 Numeric(11, 2) NULL ,
	Scale		 varchar (5) NULL ,
	[Criteria] NUMERIC(11, 2) NULL, 
    CONSTRAINT [PK_GarmentTest_Detail_FGWT] PRIMARY KEY CLUSTERED 
	( ID ASC ,No ASC ,Location ASC ,Type ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
);


