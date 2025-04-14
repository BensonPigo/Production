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
 [DefectQty] INT NOT NULL DEFAULT 0, 
    [InspectionDate] DATE NULL, 
    CONSTRAINT [PK_P_DQSDefect_Summary] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Defect數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_DQSDefect_Summary',
    @level2type = N'COLUMN',
    @level2name = N'DefectQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'實際產出日 (Last Inspection Date)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_DQSDefect_Summary',
    @level2type = N'COLUMN',
    @level2name = N'InspectionDate'