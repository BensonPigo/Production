
	CREATE TABLE WaterFastness_Detail(
		ID bigint NOT NULL  CONSTRAINT [DF_WaterFastness_Detail_ID] DEFAULT (0),		
		WaterFastnessGroup varchar(2) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_WaterFastnessGroup] DEFAULT '',		
		SEQ1 varchar(3) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_SEQ1] DEFAULT '',		
		SEQ2 varchar(2) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_SEQ2] DEFAULT '',		
		Roll varchar(8) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_Roll] DEFAULT '',		
		Dyelot varchar(8)NOT NULL CONSTRAINT [DF_WaterFastness_Detail_Dyelot] DEFAULT '',		
		Result varchar(4)NOT NULL CONSTRAINT [DF_WaterFastness_Detail_Result] DEFAULT '',		
		SubmitDate date NULL,
		ChangeScale varchar(8)NOT NULL CONSTRAINT [DF_WaterFastness_Detail_ChangeScale] DEFAULT '',		
		ResultChange varchar(5)NOT NULL CONSTRAINT [DF_WaterFastness_Detail_ResultChange] DEFAULT '',		
		AcetateScale varchar(8) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_AcetateScale] DEFAULT '',		
		ResultAcetate varchar(5) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_ResultAcetate] DEFAULT '',		
		CottonScale varchar(8) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_CottonScale] DEFAULT '',		
		ResultCotton varchar(5) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_ResultCotton] DEFAULT '',		
		NylonScale varchar(8) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_NylonScale] DEFAULT '',		
		ResultNylon varchar(5) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_ResultNylon] DEFAULT '',		
		PolyesterScale varchar(8) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_PolyesterScale] DEFAULT '',		
		ResultPolyester varchar(5) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_ResultPolyester] DEFAULT '',		
		AcrylicScale varchar(8) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_AcrylicScale] DEFAULT '',		
		ResultAcrylic varchar(5) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_ResultAcrylic] DEFAULT '',		
		WoolScale varchar(8) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_WoolScale] DEFAULT '',		
		ResultWool varchar(5) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_ResultWool] DEFAULT '',		
		Remark nvarchar(60)NOT NULL CONSTRAINT [DF_WaterFastness_Detail_Remark] DEFAULT '',		
		AddName varchar(10)NOT NULL CONSTRAINT [DF_WaterFastness_Detail_AddName] DEFAULT '',		
		AddDate datetime NULL,
		EditName varchar(10) NOT NULL CONSTRAINT [DF_WaterFastness_Detail_EditName] DEFAULT '',		
		EditDate datetime NULL,
		 CONSTRAINT [PK_WaterFastness_Detail]  PRIMARY KEY CLUSTERED 
		(
			ID ASC,
			WaterFastnessGroup ASC,
			SEQ1 ASC,
			SEQ2 ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
GO