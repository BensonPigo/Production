CREATE TABLE [dbo].[P_CFAInspectionRecord_Detail](
	[Action] [nvarchar](300) NULL,
	[AreaCodeDesc] [varchar](60) NULL,
	[AuditDate] [date] NULL,
	[BrandID] [varchar](8) NULL,
	[BuyerDelivery] [date] NULL,
	[CfaName] [varchar](45) NULL,
	[ClogReceivedPercentage] [numeric](3, 0) NULL,
	[DefectDesc] [nvarchar](100) NULL,
	[DefectQty] [int] NULL,
	[Destination] [varchar](2) NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Carton] [varchar](500) NULL,
	[InspectedCtn] [int] NULL,
	[InspectedPoQty] [int] NULL,
	[InspectionStage] [varchar](30) NULL,
	[SewingLineID] [varchar](100) NULL,
	[Mdivisionid] [varchar](8) NULL,
	[NoOfDefect] [int] NULL,
	[OrderQty] [int] NULL,
	[PONO] [varchar](30) NULL,
	[Remark] [nvarchar](500) NULL,
	[Result] [varchar](16) NULL,
	[SampleLot] [int] NULL,
	[Seq] [varchar](2) NULL,
	[Shift] [varchar](1) NULL,
	[SPNO] [varchar](13) NULL,
	[SQR] [numeric](7, 3) NULL,
	[Status] [varchar](17) NULL,
	[StyleID] [varchar](15) NULL,
	[Team] [varchar](5) NULL,
	[TtlCTN] [int] NULL,
	[VasShas] [varchar](1) NULL,
	[1st_Inspection] [varchar](1) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_P_CFAInspectionRecord_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO