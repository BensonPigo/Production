
CREATE TABLE PerspirationFastness_Detail(
ID bigint NOT NULL  CONSTRAINT [DF_PerspirationFastness_Detail_ID] DEFAULT (0),		
PerspirationFastnessGroup varchar(2) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_PerspirationFastnessGroup] DEFAULT '',		
SEQ1 varchar(3) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_SEQ1] DEFAULT '',		
SEQ2 varchar(2) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_SEQ2] DEFAULT '',		
Roll varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_Roll] DEFAULT '',		
Dyelot varchar(8)NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_Dyelot] DEFAULT '',		
Result varchar(4)NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_Result] DEFAULT '',		
SubmitDate date NULL,

AlkalineChangeScale varchar(8)NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineChangeScale] DEFAULT '',		
AlkalineResultChange varchar(5)NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineResultChange] DEFAULT '',		
AlkalineAcetateScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineAcetateScale] DEFAULT '',		
AlkalineResultAcetate varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineResultAcetate] DEFAULT '',		
AlkalineCottonScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineCottonScale] DEFAULT '',		
AlkalineResultCotton varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineResultCotton] DEFAULT '',		
AlkalineNylonScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineNylonScale] DEFAULT '',		
AlkalineResultNylon varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineResultNylon] DEFAULT '',		
AlkalinePolyesterScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalinePolyesterScale] DEFAULT '',		
AlkalineResultPolyester varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineResultPolyester] DEFAULT '',		
AlkalineAcrylicScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineAcrylicScale] DEFAULT '',		
AlkalineResultAcrylic varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineResultAcrylic] DEFAULT '',		
AlkalineWoolScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineWoolScale] DEFAULT '',		
AlkalineResultWool varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AlkalineResultWool] DEFAULT '',	
		
AcidChangeScale varchar(8)NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidChangeScale] DEFAULT '',		
AcidResultChange varchar(5)NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidResultChange] DEFAULT '',		
AcidAcetateScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidAcetateScale] DEFAULT '',		
AcidResultAcetate varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidResultAcetate] DEFAULT '',		
AcidCottonScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidCottonScale] DEFAULT '',		
AcidResultCotton varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidResultCotton] DEFAULT '',		
AcidNylonScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidNylonScale] DEFAULT '',		
AcidResultNylon varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidResultNylon] DEFAULT '',		
AcidPolyesterScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidPolyesterScale] DEFAULT '',		
AcidResultPolyester varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidResultPolyester] DEFAULT '',		
AcidAcrylicScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidAcrylicScale] DEFAULT '',		
AcidResultAcrylic varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidResultAcrylic] DEFAULT '',		
AcidWoolScale varchar(8) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidWoolScale] DEFAULT '',		
AcidResultWool varchar(5) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AcidResultWool] DEFAULT '',	

Remark nvarchar(60)NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_Remark] DEFAULT '',		
AddName varchar(10)NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_AddName] DEFAULT '',		
AddDate datetime NULL,
EditName varchar(10) NOT NULL CONSTRAINT [DF_PerspirationFastness_Detail_EditName] DEFAULT '',		
EditDate datetime NULL,
	CONSTRAINT [PK_PerspirationFastness_Detail]  PRIMARY KEY CLUSTERED 
(
	ID ASC,
	PerspirationFastnessGroup ASC,
	SEQ1 ASC,
	SEQ2 ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO