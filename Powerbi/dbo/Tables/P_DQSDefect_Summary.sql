CREATE TABLE [dbo].[P_DQSDefect_Summary](
	[FirstInspectDate] [date] NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[BrandID] [varchar](8) NULL,
	[StyleID] [varchar](15) NULL,
	[POID] [varchar](30) NULL,
	[SPNO] [varchar](13) NULL,
	[Article] [varchar](8) NULL,
	[SizeCode] [varchar](8) NULL,
	[Destination] [varchar](30) NULL,
	[CDCode] [varchar](6) NULL,
	[ProductionFamilyID] [varchar](20) NULL,
	[Team] [varchar](10) NULL,
	[QCName] [varchar](10) NULL,
	[Shift] [varchar](5) NULL,
	[Line] [varchar](5) NULL,
	[Cell] [varchar](2) NULL,
	[InspectQty] [int] NULL,
	[RejectQty] [int] NULL,
	[WFT] [decimal](6, 3) NULL,
	[RFT] [decimal](6, 3) NULL,
	[CDCodeNew] [varchar](5) NULL,
	[ProductType] [nvarchar](500) NULL,
	[FabricType] [nvarchar](500) NULL,
	[Lining] [varchar](20) NULL,
	[Gender] [varchar](10) NULL,
	[Construction] [nvarchar](50) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_P_DQSDefect_Summary] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO