CREATE TABLE [dbo].[P_ICRAnalysis](
	[ICRNo] [varchar](13) NOT NULL,
	[Status] [varchar](15) NOT NULL,
	[Mdivision] [varchar](8) NOT NULL,
	[ResponsibilityFTY] [nvarchar](40) NOT NULL,
	[FTY] [varchar](8) NOT NULL,
	[SDPKPICode] [varchar](8) NOT NULL,
	[SPNo] [varchar](13) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[TotalQty] [int] NOT NULL,
	[POHandle] [varchar](45) NOT NULL,
	[POSMR] [varchar](45) NOT NULL,
	[MR] [varchar](45) NOT NULL,
	[SMR] [varchar](45) NOT NULL,
	[IssueSubject] [nvarchar](506) NOT NULL,
	[ResponsibilityAndExplaination] [nvarchar](max) NOT NULL,
	[RMtlAmtUSD] [numeric](10, 2) NOT NULL,
	[OtherAmtUSD] [numeric](10, 2) NOT NULL,
	[ActFreightAmtUSD] [numeric](10, 2) NOT NULL,
	[TotalUSD] [numeric](11, 2) NOT NULL,
	[Createdate] [date] NULL,
	[Confirmeddate] [date] NULL,
	[VoucherNo] [varchar](16) NOT NULL,
	[VoucherDate] [date] NULL,
	[Seq] [varchar](6) NOT NULL,
	[SourceType] [nvarchar](100) NOT NULL,
	[WeaveType] [varchar](20) NOT NULL,
	[IrregularMtlType] [varchar](20) NOT NULL,
	[IrregularQty] [numeric](8, 2) NOT NULL,
	[IrregularFOC] [numeric](8, 2) NOT NULL,
	[IrregularPriceUSD] [numeric](16, 2) NOT NULL,
	[IrregularAmtUSD] [numeric](24, 5) NOT NULL,
 CONSTRAINT [PK_P_ICRReportList] PRIMARY KEY CLUSTERED 
(
	[ICRNo] ASC,
	[Seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_ICRNo]  DEFAULT ('') FOR [ICRNo]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_Status]  DEFAULT ('') FOR [Status]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_Mdivision]  DEFAULT ('') FOR [Mdivision]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_ResponsibilityFTY]  DEFAULT ('') FOR [ResponsibilityFTY]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_FTY]  DEFAULT ('') FOR [FTY]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_SDPKPICode]  DEFAULT ('') FOR [SDPKPICode]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_SPNo]  DEFAULT ('') FOR [SPNo]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_TotalQty]  DEFAULT ((0)) FOR [TotalQty]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_POHandle]  DEFAULT ('') FOR [POHandle]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_POSMR]  DEFAULT ('') FOR [POSMR]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_MR]  DEFAULT ('') FOR [MR]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_SMR]  DEFAULT ('') FOR [SMR]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_IssueSubject]  DEFAULT ('') FOR [IssueSubject]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_ResponsibilityAndExplaination]  DEFAULT ('') FOR [ResponsibilityAndExplaination]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_RMtlAmtUSD]  DEFAULT ((0)) FOR [RMtlAmtUSD]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_OtherAmtUSD]  DEFAULT ((0)) FOR [OtherAmtUSD]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_ActFreightAmtUSD]  DEFAULT ((0)) FOR [ActFreightAmtUSD]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_TotalUSD]  DEFAULT ((0)) FOR [TotalUSD]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_VoucherNo]  DEFAULT ('') FOR [VoucherNo]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_SourceType]  DEFAULT ('') FOR [SourceType]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_WeaveType]  DEFAULT ('') FOR [WeaveType]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_IrregularMtlType]  DEFAULT ('') FOR [IrregularMtlType]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_IrregularQty]  DEFAULT ((0)) FOR [IrregularQty]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_IrregularFOC]  DEFAULT ((0)) FOR [IrregularFOC]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_IrregularPriceUSD]  DEFAULT ((0)) FOR [IrregularPriceUSD]
GO

ALTER TABLE [dbo].[P_ICRAnalysis] ADD  CONSTRAINT [DF_P_ICRReportList_IrregularAmtUSD]  DEFAULT ((0)) FOR [IrregularAmtUSD]
GO


