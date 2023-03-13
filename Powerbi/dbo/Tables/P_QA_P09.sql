﻿CREATE TABLE [dbo].[P_QA_P09](
	[WK#] [varchar](13) NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Invoice#] [varchar](max) NULL,
	[ATA] [date] NULL,
	[ETA] [date] NULL,
	[Season] [varchar](10) NULL,
	[SP#] [varchar](13) NULL,
	[Seq#] [varchar](10) NULL,
	[Brand] [varchar](8) NULL,
	[Supp] [varchar](6) NULL,
	[Supp Name] [varchar](12) NULL,
	[Ref#] [varchar](36) NULL,
	[Color] [nvarchar](150) NULL,
	[Qty] [numeric](12, 2) NULL,
	[Inspection Report_Fty Received Date] [date] NULL,
	[Inspection Report_Supp Sent Date] [date] NULL,
	[Test Report_Fty Received Date] [date] NULL,
	[Test Report_ Check Clima] [bit] NULL,
	[Test Report_Supp Sent Date] [date] NULL,
	[Continuity Card_Fty Received Date] [date] NULL,
	[Continuity Card_Supp Sent Date] [date] NULL,
	[Continuity Card_AWB#] [varchar](30) NULL,
	[1st Bulk Dyelot_Fty Received Date] [date] NULL,
	[1st Bulk Dyelot_Supp Sent Date] [varchar](100) NULL,
	[T2 Inspected Yards] [numeric](10, 2) NULL,
	[T2 Defect Points] [numeric](5, 0) NULL,
	[Grade] [varchar](1) NULL,
	[T1 Inspected Yards] [numeric](10, 2) NULL,
	[T1 Defect Points] [numeric](6, 0) NULL,
	[Fabric with clima] [bit] NULL,
	[Ukey] [bigint] NOT NULL,
	[Consignee] [varchar](8) NOT NULL,
 CONSTRAINT [PK_P_QA_P09] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_QA_P09] ADD  CONSTRAINT [DF_P_QA_P09_Consignee]  DEFAULT ('') FOR [Consignee]
GO