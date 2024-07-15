CREATE TABLE [dbo].[P_DailyRTLStatusByLineByStyle] (
	TransferDate        DATE         NOT NULL,
	MDivisionID         VARCHAR(8)   CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_MDivisionID] DEFAULT ('') NOT NULL,
	FactoryID           VARCHAR(8)   CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_FactoryID] DEFAULT ('') NOT NULL,
	APSNo               INT          CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_APSNo] DEFAULT 0 NOT NULL,
	SewingLineID        VARCHAR(5)   CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_SewingLineID] DEFAULT ('') NOT NULL,
	BrandID             VARCHAR(8)   CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_BrandID] DEFAULT ('') NOT NULL,
	SeasonID            VARCHAR(10)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_SeasonID] DEFAULT ('') NOT NULL,
	StyleID             VARCHAR(15)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_StyleID] DEFAULT ('') NOT NULL,
	CurrentWIP			INT			 CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_CurrentWIP] DEFAULT ((0)) NOT NULL,
	StdQty              INT          CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_StdQty] DEFAULT 0 NOT NULL,
	WIP                 NUMERIC(5,2)          CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_WIP] DEFAULT 0 NOT NULL,
	nWIP                NUMERIC(5,2)          CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_nWIP] DEFAULT 0 NOT NULL,
	InLine              DATE         NOT NULL,
	OffLine             DATE         NOT NULL,
	NewCdCode           VARCHAR(5)   CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_NewCdCode] DEFAULT ('') NOT NULL,
	ProductType         NVARCHAR(500) CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_ProductType] DEFAULT ('') NOT NULL,
	FabricType          NVARCHAR(500) CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_FabricType] DEFAULT ('') NOT NULL,
	AlloQty             INT          CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_AlloQty] DEFAULT 0 NOT NULL,
	CONSTRAINT [PK_P_DailyRTLStatusByLineByStyle] PRIMARY KEY (TransferDate,FactoryID,APSNo)
)
GO