CREATE TABLE [dbo].[P_CFAInline_Detail](
	[Action] [varchar](254) NULL,
	[Area] [varchar](60) NULL,
	[AuditDate] [date] NULL,
	[BrandID] [varchar](8) NULL,
	[BuyerDelivery] [date] NULL,
	[CfaName] [varchar](45) NULL,
	[DefectDesc] [nvarchar](100) NULL,
	[DefectQty] [int] NULL,
	[Destination] [varchar](30) NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[GarmentOutput] [decimal](5, 2) NULL,
	[InspectionStage] [varchar](30) NULL,
	[Line] [varchar](5) NULL,
	[NumberDefect] [numeric](5, 0) NULL,
	[OrderQty] [int] NULL,
	[POID] [varchar](30) NULL,
	[Remark] [nvarchar](254) NULL,
	[Result] [varchar](4) NULL,
	[InspectQty] [numeric](7, 0) NULL,
	[Shift] [varchar](15) NULL,
	[SPNO] [varchar](13) NULL,
	[SQR] [numeric](7, 3) NULL,
	[StyleID] [varchar](15) NULL,
	[Team] [varchar](10) NULL,
	[VASSHAS] [varchar](1) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_P_CFAInline_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO