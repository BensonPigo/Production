
CREATE TABLE FIR_Laboratory_Iron (
	ID bigint NOT NULL  CONSTRAINT [DF_FIR_Laboratory_Iron_ID] DEFAULT 0,
	Roll varchar(8)NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_Roll] DEFAULT '',
	Dyelot varchar(8) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_Dyelot] DEFAULT '',
	Inspdate date NULL,
	Inspector varchar(10) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_Inspector] DEFAULT '',
	Result varchar(5) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_Result] DEFAULT '',
	Remark nvarchar(100) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_Remark] DEFAULT '',
	AddName varchar(10) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_AddName] DEFAULT '',
	AddDate datetime NULL,
	EditName varchar(10) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_EditName] DEFAULT '',
	EditDate datetime NULL,
	HorizontalRate numeric(5, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_HorizontalRate] DEFAULT 0,
	HorizontalOriginal numeric(4, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_HorizontalOriginal] DEFAULT 0,
	HorizontalTest1 numeric(4, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_HorizontalTest1] DEFAULT 0,
	HorizontalTest2 numeric(4, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_HorizontalTest2] DEFAULT 0,
	HorizontalTest3 numeric(4, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_HorizontalTest3] DEFAULT 0,

	VerticalRate numeric(5, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_VerticalRate] DEFAULT 0,
	VerticalOriginal numeric(4, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_VerticalOriginal] DEFAULT 0,
	VerticalTest1 numeric(4, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_VerticalTest1] DEFAULT 0,
	VerticalTest2 numeric(4, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_VerticalTest2] DEFAULT 0,
	VerticalTest3 numeric(4, 2) NOT NULL CONSTRAINT [DF_FIR_Laboratory_Iron_VerticalTest3] DEFAULT 0,
	CONSTRAINT [PK_FIR_Laboratory_Iron] PRIMARY KEY CLUSTERED 
	(
		ID ASC,Roll ASC ,Dyelot ASC
	)
);
GO