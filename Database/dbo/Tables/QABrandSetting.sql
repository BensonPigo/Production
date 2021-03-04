
	CREATE TABLE [dbo].QABrandSetting(
		BrandID varchar(8) NOT NULL,
		Junk Bit NOT NULL CONSTRAINT [DF_QABrandSetting_Junk] DEFAULT (0),
		PointRateOption varchar(1) NOT NULL CONSTRAINT [DF_QABrandSetting_PointRateOption] DEFAULT ('1'),
		AddDate Datetime NULL,
		AddName varchar(10) NOT NULL CONSTRAINT [DF_QABrandSetting_AddName] DEFAULT (''),
		EditDate Datetime NULL,
		EditName varchar(10) NOT NULL CONSTRAINT [DF_QABrandSetting_EditName] DEFAULT (''),
		CrockingTestOption tinyint  NOT NULL CONSTRAINT [DF_QABrandSetting_CrockingTestOption] DEFAULT (0),
		SkewnessOption Varchar(1) NOT NULL CONSTRAINT [DF_QABrandSetting_SkewnessOption] DEFAULT '1',
    CONSTRAINT [PK_QABrandSetting] PRIMARY KEY CLUSTERED 
	(
		BrandID ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

	EXEC sp_addextendedproperty @name = N'MS_Description',
		@value = N'QA計算瑕疵率的公式，Option 1：(Total Points / Act. Yds Inspected)*100；Option 2：(Total Points*360)/( Act. Yds Inspected*Actual width)',
		@level0type = N'SCHEMA',
		@level0name = N'dbo',
		@level1type = N'TABLE',
		@level1name = N'QABrandSetting',
		@level2type = N'COLUMN',
		@level2name = N'PointRateOption'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
		@value = N'此欄位影響摩擦測試項目
 0 : Only 1 for Wet and Dry
 1 : 2 kind (WEFT and WARP) of testing for Wet and Dry',
		@level0type = N'SCHEMA',
		@level0name = N'dbo',
		@level1type = N'TABLE',
		@level1name = N'QABrandSetting',
		@level2type = N'COLUMN',
		@level2name = N'CrockingTestOption'
GO