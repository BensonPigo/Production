
	CREATE TABLE WaterFastness(
		ID bigint NOT NULL IDENTITY(1,1),
		POID varchar(13) NULL CONSTRAINT [DF_WaterFastness_POID] DEFAULT '',
		TestNo numeric(2, 0) NULL CONSTRAINT [DF_WaterFastness_TestNo] DEFAULT 0,
		InspDate Date NULL ,
		Article varchar(8) NULL CONSTRAINT [DF_WaterFastness_Article] DEFAULT '',
		Result varchar(15) NULL CONSTRAINT [DF_WaterFastness_Result] DEFAULT '',
		Status varchar(15) NULL CONSTRAINT [DF_WaterFastness_Status] DEFAULT '',
		Inspector varchar(10) NULL CONSTRAINT [DF_WaterFastness_Inspector] DEFAULT '',
		Remark nvarchar(120) NULL CONSTRAINT [DF_WaterFastness_Remark] DEFAULT '',
		Temperature int NULL CONSTRAINT [DF_WaterFastness_Temperature] DEFAULT 0,
		Time int NULL CONSTRAINT [DF_WaterFastness_Time] DEFAULT 0,
		--TestBeforePicture varbinary(MAX) NULL,
		--TestAfterPicture varbinary(MAX) NULL,		
		AddName varchar(10) NULL CONSTRAINT [DF_WaterFastness_AddName] DEFAULT '',
		AddDate datetime NULL ,
		EditName varchar(10) NULL CONSTRAINT [DF_WaterFastness_EditName] DEFAULT '',
		EditDate datetime NULL ,
		ReportNo varchar(14) not null CONSTRAINT [DF_WaterFastness_ReportNo] default '',
		 CONSTRAINT [PK_WaterFastness] PRIMARY KEY CLUSTERED 
		(
			ID ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
GO
