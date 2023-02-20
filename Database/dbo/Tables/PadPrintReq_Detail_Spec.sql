CREATE TABLE [dbo].[PadPrintReq_Detail_Spec](
	[ID] [varchar](13)　		CONSTRAINT [DF_PadPrintReq_Detail_Spec_ID]  DEFAULT ('') NOT NULL,
	[Seq2] [varchar](2)			CONSTRAINT [DF_PadPrintReq_Detail_Spec_Seq2]  DEFAULT ('')　NOT NULL,
	[PadPrint_Ukey] [bigint] NOT NULL,
	[MoldID] [varchar](10)　	CONSTRAINT [DF_PadPrintReq_Detail_Spec_MoldID]  DEFAULT ('') NOT NULL,
	[Side] [varchar](1)　		CONSTRAINT [DF_PadPrintReq_Detail_Spec_Side]  DEFAULT ('') NOT NULL,
	[AddName] [varchar](10)　	CONSTRAINT [DF_PadPrintReq_Detail_Spec_AddName]  DEFAULT ('') NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10)　	CONSTRAINT [DF_PadPrintReq_Detail_Spec_EditName]  DEFAULT ('') NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_PadPrintReq_Detail_Spec] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Seq2] ASC,
	[PadPrint_Ukey] ASC,
	[MoldID] ASC,
	[Side] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
