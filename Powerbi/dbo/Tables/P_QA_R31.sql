CREATE TABLE [dbo].[P_QA_R31](
	[Stage] [varchar](20) NULL,
	[InspResult] [varchar](10) NULL,
	[NotYetInspCtn#] [varchar](max) NULL,
	[NotYetInspCtn] [int] NULL,
	[NotYetInspQty] [int] NULL,
	[FailCtn#] [varchar](5000) NULL,
	[FailCtn] [int] NULL,
	[FailQty] [int] NULL,
	[MDivisionID] [varchar](8) NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[BrandID] [varchar](8) NULL,
	[OrderID] [varchar](13) NULL,
	[Category] [varchar](15) NULL,
	[OrderTypeID] [varchar](20) NULL,
	[CustPoNo] [varchar](30) NULL,
	[StyleID] [varchar](15) NULL,
	[StyleName] [nvarchar](50) NULL,
	[SeasonID] [varchar](10) NULL,
	[Dest] [varchar](30) NULL,
	[Customize1] [varchar](30) NULL,
	[CustCDID] [varchar](16) NULL,
	[Seq] [varchar](6) NULL,
	[ShipModeID] [varchar](10) NULL,
	[ColorWay] [varchar](200) NULL,
	[SewLine] [varchar](60) NULL,
	[TtlCtn] [varchar](15) NULL,
	[StaggeredCtn] [varchar](15) NULL,
	[ClogCtn] [varchar](15) NULL,
	[ClogCtn%] [varchar](15) NULL,
	[LastCartonReceivedDate] [date] NULL,
	[CFAFinalInspectDate] [date] NULL,
	[CFA3rdInspectDate] [date] NULL,
	[CFARemark] [nvarchar](500) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_QA_R31] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_QA_R31] ADD  CONSTRAINT [DF_P_QA_R31_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_R31', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_R31', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
