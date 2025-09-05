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
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_CFAInline_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_CFAInline_Detail] ADD  CONSTRAINT [DF_P_CFAInline_Detail_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAInline_Detail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAInline_Detail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO