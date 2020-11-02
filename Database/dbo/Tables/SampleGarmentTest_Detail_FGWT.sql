CREATE TABLE [dbo].[SampleGarmentTest_Detail_FGWT] (
	ID BIGINT NOT NULL,
	No int NOT NULL,
	Location	 varchar (1) NOT NULL,
	Type	  varchar (150) NOT NULL,
	TestDetail	 varchar (15) NOT NULL CONSTRAINT [DF_SampleGarmentTest_Detail_FGWT_TestDetail] DEFAULT(''),
	BeforeWash	 Numeric(11, 2) NULL ,
	SizeSpec	 Numeric(11, 2) NULL ,
	AfterWash	 Numeric(11, 2) NULL ,
	Shrinkage	 Numeric(11, 2) NULL ,
	Scale		 varchar (5) NULL ,
	[Criteria] NUMERIC(11, 2) NULL, 
    [Criteria2] NUMERIC(11, 2) NULL, 
	[SystemType]	  varchar (150) NOT NULL CONSTRAINT [DF_SampleGarmentTest_Detail_FGWT_SystemType] DEFAULT(''),
	[Seq]	  int NOT NULL CONSTRAINT [DF_SampleGarmentTest_Detail_FGWT_Seq] DEFAULT(0),
    CONSTRAINT [PK_SampleGarmentTest_Detail_FGWT] PRIMARY KEY CLUSTERED 
	( ID ASC ,No ASC ,Location ASC ,Type ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
);


GO


	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pass 標準；Criteria2 主要用在判斷一個區間', @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail_FGWT'
	, @level2type = N'COLUMN', @level2name = N'Criteria2';
	;

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用於呈現在系統畫面中的項目名稱', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail_FGWT'
, @level2type = N'COLUMN', @level2name = N'SystemType';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用於排序 FGWT 各項目', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'SampleGarmentTest_Detail_FGWT'
, @level2type = N'COLUMN', @level2name = N'Seq';
go