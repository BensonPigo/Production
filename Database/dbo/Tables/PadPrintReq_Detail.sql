CREATE TABLE [dbo].[PadPrintReq_Detail](
	[ID] [varchar](13)　		CONSTRAINT [DF_PadPrintReq_Detail_ID]  DEFAULT ('') NOT NULL,
	[Seq2] [varchar](2)　		CONSTRAINT [DF_PadPrintReq_Detail_Seq2]  DEFAULT ('') NOT NULL,
	[PadPrint_Ukey] [bigint]　 NOT NULL,
	[Refno] [varchar](36)　		CONSTRAINT [DF_PadPrintReq_Detail_Refno]  DEFAULT ('') NOT NULL,
	[MoldID] [varchar](10)　	CONSTRAINT [DF_PadPrintReq_Detail_MoldID]  DEFAULT ('') NOT NULL,
	[SourceID] [varchar](13)　	CONSTRAINT [DF_PadPrintReq_Detail_SourceID]  DEFAULT ('') NOT NULL,
	[Price] [numeric](10, 4)　	CONSTRAINT [DF_PadPrintReq_Detail_Price]  DEFAULT ((0)) NOT NULL,
	[Qty] [numeric](10, 0)　	CONSTRAINT [DF_PadPrintReq_Detail_Qty]  DEFAULT ((0)) NOT NULL,
	[Foc] [numeric](10, 0)　	CONSTRAINT [DF_PadPrintReq_Detail_Foc]  DEFAULT ((0)) NOT NULL,
	[ShipModeID] [varchar](10)　CONSTRAINT [DF_PadPrintReq_Detail_ShipModeID]  DEFAULT ('') NOT NULL,
	[SuppID] [varchar](6)　		CONSTRAINT [DF_PadPrintReq_Detail_SuppID]  DEFAULT ('') NOT NULL,
	[CurrencyID] [varchar](3)　	CONSTRAINT [DF_PadPrintReq_Detail_CurrencyID]  DEFAULT ('') NOT NULL,
	[Junk] [bit] 　				CONSTRAINT [DF_PadPrintReq_Detail_Junk]  DEFAULT ((0))　NOT NULL,
	[Remark] [nvarchar](1000)　	CONSTRAINT [DF_PadPrintReq_Detail_Remark]  DEFAULT ('') NOT NULL,
	[POID] [varchar](13)　		CONSTRAINT [DF_PadPrintReq_Detail_POID]  DEFAULT ('') NOT NULL,
	[AddName] [varchar](10)　	CONSTRAINT [DF_PadPrintReq_Detail_AddName]  DEFAULT ('') NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10)　	CONSTRAINT [DF_PadPrintReq_Detail_EditName]  DEFAULT ('') NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_PadPrintReq_Detail_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Seq2] ASC,
	[PadPrint_Ukey] ASC,
	[MoldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
